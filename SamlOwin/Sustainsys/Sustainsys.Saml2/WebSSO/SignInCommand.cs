using Sustainsys.Saml2.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Sustainsys.Saml2.Internal;
using Sustainsys.Saml2.Metadata;

namespace Sustainsys.Saml2.WebSso
{
    /// <summary>
    /// Represents the sign in command behaviour.
    /// Instances of this class can be created directly or by using the factory method
    /// CommandFactory.GetCommand(CommandFactory.SignInCommandName).
    /// </summary>
    public class SignInCommand : ICommand
    {
        /// <summary>
        /// Run the command, initiating the sign in sequence.
        /// </summary>
        /// <param name="request">Request data.</param>
        /// <param name="options">Options</param>
        /// <returns>CommandResult</returns>
        public CommandResult Run(HttpRequestData request, IOptions options)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            
            var result = Run(
                new EntityId(request.QueryString["idp"].FirstOrDefault()),
                request,
                options,
                request.StoredRequestState?.RelayData);

            if (request.RelayState != null)
            {
                result.ClearCookieName = StoredRequestState.CookieNameBase + request.RelayState;
            }

            return result;
        }
        
 

        /// <summary>
        /// Initiate the sign in sequence.
        /// </summary>
        /// <param name="idpEntityId">Entity id of idp to sign in to, or
        /// null to use default (discovery service if configured)</param>
        /// <param name="request">The incoming http request.</param>
        /// <param name="options">Options.</param>
        /// <param name="relayData">Data to store and make available when the
        /// ACS command has processed the response.</param>
        /// <returns>Command Result</returns>
        public static CommandResult Run(
            EntityId idpEntityId,
            HttpRequestData request,
            IOptions options,
            IDictionary<string, string> relayData)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var urls = new Saml2Urls(request, options);

            IdentityProvider idp = options.Notifications.SelectIdentityProvider(idpEntityId, relayData);
            if (idp == null)
            {
                var idpEntityIdString = idpEntityId?.Id;
                if (idpEntityIdString == null)
                {
                    if (options.SPOptions.DiscoveryServiceUrl != null)
                    {
                        var commandResult = RedirectToDiscoveryService(null, options.SPOptions, urls, relayData);
                        options.Notifications.SignInCommandResultCreated(commandResult, relayData);
                        options.SPOptions.Logger.WriteInformation("Redirecting to Discovery Service to select Idp.");
                        return commandResult;
                    }

                    idp = options.IdentityProviders.Default;
                    options.SPOptions.Logger.WriteVerbose(
                        "No specific idp requested and no Discovery Service configured. " +
                        "Falling back to use configured default Idp " + idp.EntityId.Id);
                }
                else
                {
                    if (!options.IdentityProviders.TryGetValue(idpEntityId, out idp))
                    {
                        throw new InvalidOperationException("Unknown idp " + idpEntityIdString);
                    }
                }
            }

            var returnUrl = request.QueryString["returnUrl"].FirstOrDefault();
            var errorUrl = request.QueryString["errorUrl"].FirstOrDefault();
            var unauthorizedUrl = request.QueryString["unauthorizedUrl"].FirstOrDefault();
                
            if (returnUrl != null)
            {
                // some hacks
                options.SPOptions.ReturnUrl = new Uri($"{options.SPOptions.ReturnUrl.GetLeftPart(UriPartial.Path)}?returnUrl={returnUrl}&errorUrl={errorUrl}unauthorizedUrl={unauthorizedUrl}");
            }

            options.SPOptions.Logger.WriteInformation("Initiating login to " + idp.EntityId.Id);
            return InitiateLoginToIdp(options, relayData, urls, idp);
        }

        private static CommandResult InitiateLoginToIdp(IOptions options, IDictionary<string, string> relayData,
            Saml2Urls urls, IdentityProvider idp)
        {
            var authnRequest = idp.CreateAuthenticateRequest(urls);

            options.Notifications.AuthenticationRequestCreated(authnRequest, idp, relayData);

            var commandResult = idp.Bind(authnRequest);

            commandResult.RequestState = new StoredRequestState(
                idp.EntityId, null, authnRequest.Id, relayData);
            commandResult.SetCookieName = StoredRequestState.CookieNameBase + authnRequest.RelayState;

            options.Notifications.SignInCommandResultCreated(commandResult, relayData);

            return commandResult;
        }

        private static CommandResult RedirectToDiscoveryService(
            string returnPath,
            SPOptions spOptions,
            Saml2Urls saml2Urls,
            IDictionary<string, string> relayData)
        {
            string returnUrl = saml2Urls.SignInUrl.OriginalString;

            var relayState = SecureKeyGenerator.CreateRelayState();

            returnUrl += "?RelayState=" + Uri.EscapeDataString(relayState);

            var redirectLocation = string.Format(
                CultureInfo.InvariantCulture,
                "{0}?entityID={1}&return={2}&returnIDParam=idp",
                spOptions.DiscoveryServiceUrl,
                Uri.EscapeDataString(spOptions.EntityId.Id),
                Uri.EscapeDataString(returnUrl));

            var requestState = new StoredRequestState(
                null,
                returnPath == null ? null : new Uri(returnPath, UriKind.RelativeOrAbsolute),
                null,
                relayData);

            return new CommandResult()
            {
                HttpStatusCode = HttpStatusCode.SeeOther,
                Location = new Uri(redirectLocation),
                RequestState = requestState,
                SetCookieName = StoredRequestState.CookieNameBase + relayState
            };
        }
    }
}
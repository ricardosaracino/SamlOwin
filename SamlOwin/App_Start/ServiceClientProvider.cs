using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Xrm.Tooling.Connector;

namespace SamlOwin
{
    public class ServiceClientProvider
    {
        private static CrmServiceClient _client;

        public static CrmServiceClient Create(string serviceUrl, string redirectUri, string clientId, string secret)
        {
            if (_client?.IsReady != null) return _client;

            // Define organization Url
            var orgUrl = new Uri(serviceUrl);

            var adClass = new AzureAuthHookWrapperLoginHook();

            // Call your existing authentication implementation
            var accessToken = AzureAuthHookWrapperLoginHook.GetAccessTokenFromAzureAd(orgUrl, redirectUri, clientId, secret);

            // Create instance of your hook
            var hook = new AzureAuthHookWrapperLoginHook(redirectUri, clientId, secret);
            // Add token to your hook
            hook.AddAccessToken(orgUrl, accessToken);

            // Register the hook with the CrmServiceClient
            CrmServiceClient.AuthOverrideHook = hook;

            // Create a new instance of CrmServiceClient, pass your organization url and make sure useUniqueInstance = true!
            _client = new CrmServiceClient(orgUrl, true);

            // todo what about if IsReady is false


            return _client;
        }
    }

    public class AzureAuthHookWrapperLoginHook : IOverrideAuthHookWrapper
    {
        // In memory cache of access tokens
        private readonly Dictionary<string, AuthenticationResult> _accessTokens =
            new Dictionary<string, AuthenticationResult>();

        private readonly string _clientId;
        private readonly string _redirectUri;
        private readonly string _secret;

        public AzureAuthHookWrapperLoginHook(string redirectUri, string clientId, string secret)
        {
            _redirectUri = redirectUri;
            _clientId = clientId;
            _secret = secret;
        }

        public AzureAuthHookWrapperLoginHook()
        {
        }


        /// <summary>
        ///     Default Interface Implementation
        /// </summary>
        /// <param name="connectedUri"></param>
        /// <returns></returns>
        public string GetAuthToken(Uri connectedUri)
        {
            // Check if you have an access token for this host
            if (_accessTokens.ContainsKey(connectedUri.Host) &&
                _accessTokens[connectedUri.Host].ExpiresOn > DateTime.Now)
                return _accessTokens[connectedUri.Host].AccessToken;
            _accessTokens[connectedUri.Host] =
                GetAccessTokenFromAzureAd(connectedUri, _redirectUri, _clientId, _secret);
            return null;
        }

        public void AddAccessToken(Uri orgUri, AuthenticationResult accessToken)
        {
            // Access tokens can be matched on the hostname,
            // different endpoints in the same organization can use the same access token
            _accessTokens[orgUri.Host] = accessToken;
        }

        public static AuthenticationResult GetAccessTokenFromAzureAd(Uri orgUrl, string redirectUri = "", string clientId = "",
            string secret = "")
        {
            if (clientId == string.Empty || secret == string.Empty || redirectUri == string.Empty) return null;

            var serviceUrl = orgUrl.ToString();

            var authContext = new AuthenticationContext(redirectUri, false);
            var credential = new ClientCredential(clientId, secret);
            
            var task = Task.Run(async () => await authContext.AcquireTokenAsync(serviceUrl, credential));

            var result = task.Result;

            return result;
        }
    }
}
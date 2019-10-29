using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Web.Hosting;
using CrmEarlyBound;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using SamlOwin.ActionFilters;
using SamlOwin.Identity;
using SamlOwin.Models;
using SamlOwin.Providers;
using Sustainsys.Saml2;
using Sustainsys.Saml2.Configuration;
using Sustainsys.Saml2.Metadata;
using Sustainsys.Saml2.Owin;
using Sustainsys.Saml2.Saml2P;

namespace SamlOwin
{
    public partial class Startup
    {
        private static void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(CrmServiceContextProvider.Create);

            app.CreatePerOwinContext<ApplicationUserManager>((options, context) =>
                new ApplicationUserManager(new PortalUserStore<ApplicationUser>(context.Get<CrmServiceContext>()))
                {
                    ClaimsIdentityFactory = new ApplicationClaimsIdentityFactory()
                });

            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                CookieSecure = CookieSecureOption.SameAsRequest,
                ExpireTimeSpan =
                    TimeSpan.FromMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["SessionTimeInMinutes"])),
                SlidingExpiration = true,
                CookieName = ConfigurationManager.AppSettings["ApplicationCookieName"],
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = ApplicationCookieValidateIdentityContext.ApplicationValidateIdentity
                }
            });

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseSaml2Authentication(CreateSaml2Options());

            //app.UseOpenIdConnectAuthentication(CreateAzureAuthenticationOptions());
        }


        /*private static OpenIdConnectAuthenticationOptions CreateAzureAuthenticationOptions()
        {
            
            // The Client ID is used by the application to uniquely identify itself to Azure AD.
            var clientId = System.Configuration.ConfigurationManager.AppSettings["ClientId"];

            // RedirectUri is the URL where the user will be redirected to after they sign in.
            var redirectUri = System.Configuration.ConfigurationManager.AppSettings["RedirectUri"];

            // Tenant is the tenant ID (e.g. contoso.onmicrosoft.com, or 'common' for multi-tenant)
            var tenant = System.Configuration.ConfigurationManager.AppSettings["Tenant"];

            // Authority is the URL for authority, composed by Azure Active Directory v2.0 endpoint and the tenant name (e.g. https://login.microsoftonline.com/contoso.onmicrosoft.com/v2.0)
            var authority = string.Format(System.Globalization.CultureInfo.InvariantCulture, ConfigurationManager.AppSettings["Authority"], tenant);

            
            return new OpenIdConnectAuthenticationOptions
            {
                // Sets the ClientId, authority, RedirectUri as obtained from web.config
                ClientId = clientId,
                Authority = authority,
                RedirectUri = redirectUri,
                // PostLogoutRedirectUri is the page that users will be redirected to after sign-out. In this case, it is using the home page
                PostLogoutRedirectUri = redirectUri,
                Scope = OpenIdConnectScope.OpenIdProfile,
                // ResponseType is set to request the id_token - which contains basic information about the signed-in user
                ResponseType = OpenIdConnectResponseType.IdToken,
                // ValidateIssuer set to false to allow personal and work accounts from any organization to sign in to your application
                // To only allow users from a single organizations, set ValidateIssuer to true and 'tenant' setting in web.config to the tenant name
                // To allow users from only a list of specific organizations, set ValidateIssuer to true and use ValidIssuers parameter
                TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false // This is a simplification
                },
                // OpenIdConnectAuthenticationNotifications configures OWIN to send notification of failed authentications to OnAuthenticationFailed method
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    AuthenticationFailed = context =>
                    {
                        context.HandleResponse();
                        context.Response.Redirect("/?errormessage=" + context.Exception.Message);
                        return Task.FromResult(0);
                    }
                }
            };
        }*/


        private static Saml2AuthenticationOptions CreateSaml2Options()
        {
            var spOptions = CreateSpOptions();

            var saml2Options = new Saml2AuthenticationOptions(false)
            {
                SPOptions = spOptions
            };


            var idp5 = new IdentityProvider(
                new EntityId("http://idp5.canadacentral.cloudapp.azure.com:80/opensso"), spOptions)
            {
                MetadataLocation = HostingEnvironment.MapPath("~/App_Data/idp5-metadata.xml"),
                AllowUnsolicitedAuthnResponse = true
            };

            // Key from IDP COT
            idp5.SigningKeys.AddConfiguredKey(new X509Certificate2(
                HostingEnvironment.MapPath("~/App_Data/idp5.canadacentral.cloudapp.azure.com.cer")));


            var cbs = new IdentityProvider(
                new EntityId("https://cbs-uat-cbs.securekey.com"), spOptions)
            {
                MetadataLocation = HostingEnvironment.MapPath("~/App_Data/cbs-metadata-signed.xml")
            };


            var gckey = new IdentityProvider(
                new EntityId("https://te.clegc-gckey.gc.ca"), spOptions)
            {
                MetadataLocation = HostingEnvironment.MapPath("~/App_Data/gckey-metadata-signed.xml")
            };

            saml2Options.Notifications = new Saml2Notifications
            {
                GetBinding = SessionActionFilter.GetSaml2Binding()
            };

            saml2Options.IdentityProviders.Add(idp5);
            saml2Options.IdentityProviders.Add(cbs);
            saml2Options.IdentityProviders.Add(gckey);

            return saml2Options;
        }

        private static SPOptions CreateSpOptions()
        {
            var spOptions = new SPOptions
            {
                // remove from metadata: xmlns:saml2="urn:oasis:names:tc:SAML:2.0:assertion"
                EntityId = new EntityId("https://dev-ep-pe.csc-scc.gc.ca"),

                // 	proxied 
                PublicOrigin = new Uri("https://dev-ep-pe.csc-scc.gc.ca"),

                // Indicates the base path of the Saml2 endpoints. Defaults to /Saml2 if not specified.
                ModulePath = "api/saml",

                // 
                ReturnUrl = new Uri("https://dev-ep-pe.csc-scc.gc.ca/api/auth/loginCallback"),

                // add to metadata: <X509SubjectName>CN=dev-ep-pe,OU=csc-scc,O=GC,C=CA</X509SubjectName>
                WantAssertionsSigned = true,
                AuthenticateRequestSigningBehavior = SigningBehavior.Always,
                MinIncomingSigningAlgorithm = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256",

                // remove from metadata: cacheDuration="PT1H" 
                MetadataCacheDuration = new XsdDuration(years: 1),

                // add to metadata: <NameIDFormat>urn:oasis:names:tc:SAML:2.0:nameid-format:persistent</NameIDFormat>
                NameIdPolicy = new Saml2NameIdPolicy(true, NameIdFormat.Persistent)
            };

            // add to metadata: <EncryptionMethod Algorithm="http://www.w3.org/2001/04/xmlenc#aes256-cbc"/>
            spOptions.ServiceCertificates.Add(new ServiceCertificate
            {
                Certificate = GetEncryptionCertificate(),
                Use = CertificateUse.Encryption
            });

            spOptions.ServiceCertificates.Add(new ServiceCertificate
            {
                Certificate = GetSigninCertificate(),
                Use = CertificateUse.Signing,
            });

            return spOptions;
        }

        private static X509Certificate2 GetEncryptionCertificate()
        {
            return FindFirstByFriendlyName("GCCF Encryption", "1CA-AC1");
        }

        private static X509Certificate2 GetSigninCertificate()
        {
            return FindFirstByFriendlyName("GCCF Verification", "1CA-AC1");
        }

        private static X509Certificate2 FindFirstByFriendlyName(string friendlyName, string issuerName = null)
        {
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);

            store.Open(OpenFlags.ReadOnly);

            var certificates = issuerName == null
                ? store.Certificates
                : store.Certificates.Find(X509FindType.FindByIssuerName, issuerName, false);

            foreach (var certificate in certificates)
            {
                if (certificate.FriendlyName != friendlyName) continue;
                store.Close();
                return certificate;
            }

            store.Close();
            return null;
        }
    }
}
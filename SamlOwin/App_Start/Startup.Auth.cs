using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Description;
using System.Web.Hosting;
using CrmEarlyBound;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Owin;
using SamlOwin.ActionFilters;
using SamlOwin.Identity;
using SamlOwin.Models;
using Sustainsys.Saml2;
using Sustainsys.Saml2.Configuration;
using Sustainsys.Saml2.Metadata;
using Sustainsys.Saml2.Owin;
using Sustainsys.Saml2.Saml2P;
using XrmFramework;
using IdentityProvider = Sustainsys.Saml2.IdentityProvider;

namespace SamlOwin
{
    public partial class Startup
    {
        private static void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(() =>
                {
                    var credentials = new ClientCredentials()
                    {
                        UserName =
                        {
                            UserName = "CRM365.service@053gc.onmicrosoft.com", Password = ""
                        }
                    };

                    var serviceProxy = new OrganizationServiceProxy(
                        new Uri("https://dev-csc-scc.crm3.dynamics.com/XRMServices/2011/Organization.svc"), null, credentials,
                        null);
                    
                    serviceProxy.EnableProxyTypes();

                    return new CrmServiceContext((IOrganizationService) serviceProxy);
                }
            );

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
        }

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
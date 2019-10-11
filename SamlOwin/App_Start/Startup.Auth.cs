using System;
using System.Security.Cryptography.X509Certificates;
using System.Web.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using SamlOwin.Managers;
using SamlOwin.Models;
using Sustainsys.Saml2;
using Sustainsys.Saml2.Configuration;
using Sustainsys.Saml2.Metadata;
using Sustainsys.Saml2.Owin;
using Sustainsys.Saml2.WebSso;

namespace SamlOwin
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                CookieName = "AspNetAuthorize",
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity =
                        SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                            validateInterval: TimeSpan.FromMinutes(30),
                            regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseSaml2Authentication(CreateSaml2Options());

            app.UseSaml2Authentication(CreateSaml2Options());
        }

        private static Saml2AuthenticationOptions CreateSaml2Options()
        {
            var spOptions = CreateSpOptions();
            var saml2Options = new Saml2AuthenticationOptions(false)
            {
                SPOptions = spOptions
            };

            var idp = new IdentityProvider(new EntityId("http://idp5.canadacentral.cloudapp.azure.com:80/opensso"),
                spOptions)
            {
                WantAuthnRequestsSigned = false,
                OutboundSigningAlgorithm = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256",
                AllowUnsolicitedAuthnResponse = true,
                Binding = Saml2BindingType.HttpRedirect,
                SingleSignOnServiceUrl =
                    new Uri("http://idp5.canadacentral.cloudapp.azure.com:80/opensso/SSORedirect/metaAlias/idp")
            };

            idp.SigningKeys.AddConfiguredKey(
                new X509Certificate2(HostingEnvironment.MapPath("~/App_Data/stubidp.sustainsys.com.cer")));

            saml2Options.IdentityProviders.Add(idp);

            new Federation("~/App_Data/idp-metadata.xml", false, saml2Options);

            return saml2Options;
        }

        private static SPOptions CreateSpOptions()
        {
            var spOptions = new SPOptions
            {
                EntityId = new EntityId("https://localhost:44325/Saml2"),
                ReturnUrl = new Uri("https://localhost:44325/api/auth/loginCallback"),
                MinIncomingSigningAlgorithm = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256",
                WantAssertionsSigned = false,
                AuthenticateRequestSigningBehavior = SigningBehavior.Always
            };

            spOptions.ServiceCertificates.Add(new ServiceCertificate()
            {
                Certificate = new X509Certificate2(HostingEnvironment.MapPath("~/App_Data/Sustainsys.Saml2.Tests.pfx")),
                Use = CertificateUse.Encryption
            });

            spOptions.ServiceCertificates.Add(new ServiceCertificate()
            {
                Certificate = new X509Certificate2(HostingEnvironment.MapPath("~/App_Data/Sustainsys.Saml2.Tests.pfx")),
                Use = CertificateUse.Signing
            });

            return spOptions;
        }
    }
}
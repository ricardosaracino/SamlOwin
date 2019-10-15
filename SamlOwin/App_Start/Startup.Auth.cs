using System;
using System.Security.Cryptography.X509Certificates;
using System.Web.Hosting;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using SamlOwin.Identity;
using SamlOwin.Models;
using Sustainsys.Saml2;
using Sustainsys.Saml2.Configuration;
using Sustainsys.Saml2.Metadata;
using Sustainsys.Saml2.Owin;
using Sustainsys.Saml2.WebSso;
using XrmFramework;

namespace SamlOwin
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        private static void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => XrmService.Create(
            ));

            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                ExpireTimeSpan = TimeSpan.FromMinutes(1),
                // TODO LoginPath = new PathString("/Saml2/Signin"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser, Guid>(
                        TimeSpan.FromMinutes(1),
                        (manager, user) => user.GenerateUserIdentityAsync(manager),
                        (user) => Guid.Parse(user.GetUserId()))
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
                new X509Certificate2(
                    HostingEnvironment.MapPath("~/App_Data/idp5.canadacentral.cloudapp.azure.com.cer")));

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
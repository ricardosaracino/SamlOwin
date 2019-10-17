using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using System.Web;
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
using Sustainsys.Saml2.Saml2P;
using XrmFramework;

namespace SamlOwin
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        private static void ConfigureAuth(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => XrmService.Create(ConfigurationManager.AppSettings["CrmConnectionString"]));
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                ExpireTimeSpan = TimeSpan.FromMinutes(1),
                SlidingExpiration = false,
                // LogoutPath = new PathString("/saml/logout"),
                // TODO LoginPath = new PathString("/Saml2/Signin"),
                Provider = new CookieAuthenticationProvider
                {
                    // check still active
                    OnValidateIdentity = SecurityStampValidator
                        .OnValidateIdentity<ApplicationUserManager, ApplicationUser, Guid>(
                            TimeSpan.FromMinutes(1),
                            (manager, user) => user.GenerateUserIdentityAsync(manager),
                            user => Guid.Parse(user.GetUserId()))
                }
            });
            
            // todo signout on expire
            // equest.GetOwinContext().Authentication.SignOut();

            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            app.UseSaml2Authentication(CreateSaml2Options());
        }

        private static Saml2AuthenticationOptions CreateSaml2Options()
        {
            // var spOptions = CreateLocalSpOptions();
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


            saml2Options.IdentityProviders.Add(idp5);
            saml2Options.IdentityProviders.Add(cbs);
            saml2Options.IdentityProviders.Add(gckey);


           /// new Federation("~/App_Data/idp-metadata.xml", false, saml2Options);

            return saml2Options;
        }

        private static SPOptions CreateSpOptions()
        {
            var spOptions = new SPOptions
            {
                EntityId = new EntityId("https://dev-ep-pe.csc-scc.gc.ca"),

                // 	proxied 
                PublicOrigin = new Uri("https://dev-ep-pe.csc-scc.gc.ca"),

                // Indicates the base path of the Saml2 endpoints. Defaults to /Saml2 if not specified.
                ModulePath = "api/saml",

                // 
                ReturnUrl = new Uri("https://dev-ep-pe.csc-scc.gc.ca/api/auth/loginCallback"),
                
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
                Certificate = new X509Certificate2(HostingEnvironment.MapPath("~/App_Data/Sustainsys.Saml2.Tests.pfx")),
                Use = CertificateUse.Encryption
            });

            spOptions.ServiceCertificates.Add(new ServiceCertificate
            {
                Certificate = new X509Certificate2(HostingEnvironment.MapPath("~/App_Data/Sustainsys.Saml2.Tests.pfx")),
                Use = CertificateUse.Signing
            });

            return spOptions;
        }
    }
}
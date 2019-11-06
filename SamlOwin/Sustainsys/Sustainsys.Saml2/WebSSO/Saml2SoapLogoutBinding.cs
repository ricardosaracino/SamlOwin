﻿using Sustainsys.Saml2.Configuration;
using Sustainsys.Saml2.Saml2P;

namespace Sustainsys.Saml2.WebSso
{
    public class Saml2SoapLogoutBinding : Saml2Binding
    {
        public HttpRequestData Request { get; set; }


        /**
        <samlp:LogoutResponse xmlns:samlp="urn:oasis:names:tc:SAML:2.0:protocol" xmlns:saml="urn:oasis:names:tc:SAML:2.0:assertion" ID="_6c3737282f007720e736f0f4028feed8cb9b40291c" Version="2.0" IssueInstant="2014-07-18T01:13:06Z" Destination="http://sp.example.com/demo1/index.php?acs" InResponseTo="ONELOGIN_21df91a89767879fc0f7df6a1490c6000c81644d">
        <saml:Issuer>http://idp.example.com/metadata.php</saml:Issuer>
        <samlp:Status>
        <samlp:StatusCode Value="urn:oasis:names:tc:SAML:2.0:status:Success"/>
            </samlp:Status>
        </samlp:LogoutResponse>
        */

        protected internal override bool CanUnbind(HttpRequestData request)
        {
            return false;
        }
    }
}
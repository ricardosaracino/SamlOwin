using System;
using System.Linq;
using System.Xml;
using Sustainsys.Saml2.Saml2P;

namespace Sustainsys.Saml2.WebSso
{
    public class Saml2SoapLogoutBinding : Saml2Binding
    {
        private XmlElement _doc;

        public Saml2SoapLogoutBinding(HttpRequestData request)
        {
            /* example doc
              <samlp:LogoutRequest xmlns:samlp="urn:oasis:names:tc:SAML:2.0:protocol" ID="s2bdda3668d4fe6c44cf2f70978f83ab2f1aa52061"
              Version="2.0" IssueInstant="2019-11-06T15:53:20Z"
              Destination="https://dev-ep-pe.csc-scc.gc.ca/api/saml/Logout" NotOnOrAfter="2019-11-06T16:03:20Z">
              <saml:Issuer xmlns:saml="urn:oasis:names:tc:SAML:2.0:assertion">
              http://idp5.canadacentral.cloudapp.azure.com:80/opensso
              </saml:Issuer>
              <saml:NameID xmlns:saml="urn:oasis:names:tc:SAML:2.0:assertion"
              NameQualifier="http://idp5.canadacentral.cloudapp.azure.com:80/opensso"
              SPNameQualifier="https://dev-ep-pe.csc-scc.gc.ca"
              Format="urn:oasis:names:tc:SAML:2.0:nameid-format:persistent">ivXuQGiYLvDQLVZkN60pBwESi8fy
                  </saml:NameID>
              <samlp:SessionIndex xmlns:samlp="urn:oasis:names:tc:SAML:2.0:protocol">s2fd8384805728227f014a2cedace57124acaa2a01
                  </samlp:SessionIndex>
              </samlp:LogoutRequest>
              */
            
            var soapString = request.Form.Keys.First() + "=" + request.Form.Values.First();

            try
            {
                _doc = Saml2SoapBinding.ExtractBody(soapString);
            }
            catch (Exception e)
            {
                // ignored
            }
        }
        
        public string GetSessionIndex()
        {
            return _doc?.GetElementsByTagName("samlp:SessionIndex").Item(0)?.InnerText;
        }
        
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
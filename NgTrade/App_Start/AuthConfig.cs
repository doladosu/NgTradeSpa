using Microsoft.Web.WebPages.OAuth;

namespace NgTrade
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            OAuthWebSecurity.RegisterTwitterClient(consumerKey: "waLKThzsu8HxsPPnPTS2vQ", consumerSecret: "oc2C3p9dnBBSWjlo7MlLmBJGWMR1RJiXYGYFY7Rhos");

            OAuthWebSecurity.RegisterFacebookClient(appId: "185502631637619", appSecret: "abe912fa4061330ce88103bf9f8f9210");

            OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}

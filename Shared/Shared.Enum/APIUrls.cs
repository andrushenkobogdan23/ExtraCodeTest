namespace Shared.Enum
{
    public sealed class APIUrls
    {
        // Auth
        public const string IdentityServer = "http://localhost:6000";//"https://templatenetcore2identity.azurewebsites.net/";// "https://localhost:44374/";// 
        public const string IdentityServerConnectToken = "http://localhost:6000/connect/token";//"https://templatenetcore2identity.azurewebsites.net/connect/token";//

        // UI
        public const string MVC = "http://localhost:6001";//"https://templatenetcore2mvc.azurewebsites.net"; //
        public const string MVCAppSignin = "http://localhost:6001/signin-oidc";//"https://templatenetcore2mvc.azurewebsites.net/signin-oidc"; //
        public const string MVCAppSignout = "http://localhost:6001/signout-callback-oidc";//"https://templatenetcore2mvc.azurewebsites.net/signout-callback-oidc"; //


        // microservices
        public const string Todos = "http://localhost:6300";//"https://templatenetcore2read.azurewebsites.net"; //
        public const string TodosCmd = "http://localhost:6301";//"https://templatenetcore2command.azurewebsites.net"; //
        public const string TodosMsg = "http://localhost:6302";

        // versions
        public const string Todos_V10 = "1";
        public const string Todos_V11 = "1.1";
        public const string TodosCmd_V10 = "1";
        public const string TodosCmd_V11 = "1.1";
        public const string TodosMsg_V10 = "1.0";


        // rabbitmq
        public const string Rabbit = "host=localhost";
    }
}

using Microsoft.Owin.Security.OAuth;
using SmarteWaste_API.Contracts;
using SmartWaste_API.Library.Security;
using SmartWaste_API.Services.Interfaces;
using System.Threading.Tasks;

namespace SmartWaste_API.Security
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly ISecurityService _securityService;
        
        public SimpleAuthorizationServerProvider(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        // NOTE: Generating client token.
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var result = _securityService.SignIn(context.UserName, context.Password);

            if (!result.Success)
            {
                context.SetError("invalid_grant", result.GetMessage(true));
                return;
            }

            result.Result.AuthenticationType = context.Options.AuthenticationType;
            result.Result.Login = context.UserName;

            var identity = ClaimsParser.Create<IdentityContract>(result.Result);

            context.Validated(identity);
        }
    }
}
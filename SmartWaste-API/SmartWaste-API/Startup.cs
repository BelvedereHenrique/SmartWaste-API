using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using SmartWaste_API.Security;

[assembly: OwinStartup(typeof(SmartWaste_API.Startup))]

namespace SmartWaste_API
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            GlobalConfiguration.Configure(WebApiConfig.Register);

            ConfigureAuth(app);
        }

        public void ConfigureAuth(IAppBuilder app)
        {            
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = App_Start.NinjectWebCommon.CreateInstance<SimpleAuthorizationServerProvider>()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}

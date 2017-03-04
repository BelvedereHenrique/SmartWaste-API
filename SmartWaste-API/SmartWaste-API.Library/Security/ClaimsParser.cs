using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Library.Security
{
    public static class ClaimsParser
    {
        private const string DATA_KEY = "data";

        public static ClaimsIdentity Create<T>(T model) where T : IIdentityModel
        {
            var claims = new ClaimsIdentity(model.AuthenticationType);
            claims.AddClaim(new Claim("sub", model.Login));
            claims.AddClaim(new Claim(DATA_KEY, JsonConvert.SerializeObject(model)));

            return claims;
        }

        public static T Parse<T>(ClaimsIdentity claims) where T : IIdentityModel, new() 
        {
            var model =  JsonConvert.DeserializeObject<T>(GetClaim<string>(claims, DATA_KEY));

            model.IsAuthenticated = claims.IsAuthenticated;

            return model;
        }

        private static T GetClaim<T>(ClaimsIdentity claims, String claimName)
        {
            var claim = claims.Claims.FirstOrDefault(x => x.Type == claimName);

            if (claim == null)
                return default(T);

            var converter = TypeDescriptor.GetConverter(typeof(T));

            if (!converter.IsValid(claim.Value))
                return default(T);

            return (T)converter.ConvertFromInvariantString(claim.Value);
        }
    }
}
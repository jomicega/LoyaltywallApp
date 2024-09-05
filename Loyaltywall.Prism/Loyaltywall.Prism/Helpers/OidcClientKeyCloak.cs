using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;
using Loyaltywall.Prism.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Loyaltywall.Prism.Helpers
{
    public class OidcClientKeyCloak
    {

        private OidcClient _oidcClient;
        private UserInfo newPerson;        

        public OidcClientKeyCloak(bool isLogout)
        {
            var options = new OidcClientOptions
            {
                Authority = "https://dev.loyaltywall.com/realms/loyaltywall-client-test",
                ClientId = "FrontEnd APP Movil",
                RedirectUri = "https://frontend-client.loyaltywall.com/*",
                Scope = "openid profile",            
                Browser = isLogout ? (IBrowser) new SystemBrowser() : new LogoutBrowser()
            };

            _oidcClient = new OidcClient(options);

        }

        public async Task<UserInfo> LoginAsync()
        {
            try
            {
                var result = await _oidcClient.LoginAsync(new LoginRequest());

                if (!result.IsError)
                {

                    var identityToken = new JwtSecurityToken(result.IdentityToken);
                    var identityClaims = identityToken.Claims;

                    var accessToken = new JwtSecurityToken(result.AccessToken);
                    var accessClaims = accessToken.Claims;

                    var nameClaim = identityClaims.FirstOrDefault(c => c.Type == "name");
                    var emailClaim = identityClaims.FirstOrDefault(c => c.Type == "email");
                    var subClain = identityClaims.FirstOrDefault(c => c.Type == "sub");

                    newPerson = new UserInfo
                    {
                        IsSuccess = result.IsError,
                        Token = result.AccessToken,
                        TokenExpiresIn = result.TokenResponse.ExpiresIn,
                        Name = nameClaim.Value,
                        Email = emailClaim.Value,
                        Id = subClain.Value,
                    };

                }
                else
                {
                    newPerson = new UserInfo
                    {
                        IsSuccess = result.IsError,
                    };
                }
            }
            catch (Exception ex)
            {

                throw;
            }

               
            return newPerson;

        }

        public async Task<bool> LogoutAsync()
        {
            var result = await _oidcClient.LogoutAsync(new LogoutRequest());

            if (result.IsError)
            {
                Console.WriteLine($"Logout error: {result.Error}");
            }
            return true;
        }

    }
}




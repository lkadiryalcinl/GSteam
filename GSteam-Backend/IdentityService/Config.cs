using Duende.IdentityServer.Models;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        [
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        ];

    public static IEnumerable<ApiScope> ApiScopes =>
        [
            new ApiScope("microserviceApp","microService full access"),
        ];

    public static IEnumerable<Client> Clients =>
        [
            new Client
            {
                ClientId = "postman",
                ClientName = "postman",
                AllowedGrantTypes = {GrantType.ResourceOwnerPassword} ,
                ClientSecrets = [new Secret("SoBigSecret".Sha256())],
                RedirectUris={"https://www.getpostman.com"},
                AllowedScopes = { "openid","profile","microserviceApp" }

            },

        ];
}

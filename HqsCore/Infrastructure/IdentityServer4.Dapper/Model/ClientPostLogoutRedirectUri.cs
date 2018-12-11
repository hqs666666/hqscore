

namespace IdentityServer4.Dapper.Model
{
    public class ClientPostLogoutRedirectUri
    {
        public int Id { get; set; }
        public string PostLogoutRedirectUri { get; set; }
        public Client Client { get; set; }
    }
}
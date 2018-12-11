

namespace IdentityServer4.Dapper.Model
{
    public class ClientSecret : Secret
    {
        public Client Client { get; set; }
    }
}
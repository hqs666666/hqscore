

namespace IdentityServer4.Dapper.Model
{
    public abstract class UserClaim
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }
}
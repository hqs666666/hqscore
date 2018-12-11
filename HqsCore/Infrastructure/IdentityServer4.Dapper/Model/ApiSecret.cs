namespace IdentityServer4.Dapper.Model
{
    public class ApiSecret : Secret
    {
        public ApiResource ApiResource { get; set; }
    }
}
﻿

namespace Hqs.Dto.Users
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Mobilephone { get; set; }
        public string Email { get; set; }
        public int Status { get; set; }
        public int Gender { get; set; }
        public string Country { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string HeadImgUrl { get; set; }
    }

    public class UserLogin
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}

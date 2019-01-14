using System;
using System.Collections.Generic;
using System.Text;

namespace Hqs.Model.Users
{
    public class UserProfile : BaseEntity<string>
    {
        public DateTime CTime { get; set; }
        public DateTime MTime { get; set; }
        public string CreateBy { get; set; }
        public string ModifyBy { get; set; }
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

        public UserProfile()
        {
            CTime = DateTime.Now;
            MTime = CTime;
            Status = 1;
        }
    }
}

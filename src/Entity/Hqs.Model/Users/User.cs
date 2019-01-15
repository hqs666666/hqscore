using System;
using System.Collections.Generic;
using System.Text;

namespace Hqs.Model.Users
{
    public class User : BaseEntity<string>
    {
        public DateTime CTime { get; set; }
        public DateTime MTime { get; set; }
        public string CreateBy { get; set; }
        public string ModifyBy { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Mobilephone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Status { get; set; }

        public User()
        {
            CTime = DateTime.Now;
            MTime = CTime;
            Status = 1;
        }
    }
}

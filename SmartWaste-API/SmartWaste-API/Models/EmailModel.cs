using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartWaste_API.Models
{
    public class EmailModel
    {
        public string email { get; set; }
    }
    public class EnterpriseRequestTokenModel
    {
        public string email { get; set; }
        public bool check { get; set; }
    }

    public class EnterprisePermissionModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public string  token { get; set; }
    }
}
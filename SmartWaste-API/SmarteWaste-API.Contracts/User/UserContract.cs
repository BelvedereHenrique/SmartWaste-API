using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.User
{
    public class UserContract
    {
        public UserContract()
        {
            Roles = new List<Role.RoleContract>();
        }

        public Guid ID { get; set; }
        public String Login { get; set; }
        public String Password { get; set; }
        public String RecoveryToken { get; set; }
        public DateTime? RecoveredOn { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public List<Role.RoleContract> Roles { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.User
{
    public class UserFilterContract
    {
        public Guid? ID { get; set; }
        public string Login { get; set; }
    }
}

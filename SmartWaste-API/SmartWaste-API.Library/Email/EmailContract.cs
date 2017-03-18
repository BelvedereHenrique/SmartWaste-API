using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Library.Email
{
    public class EmailContract
    {
        public SenderInformationsContract Informations { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}

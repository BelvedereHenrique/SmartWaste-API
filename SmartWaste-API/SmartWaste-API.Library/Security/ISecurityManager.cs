using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Library.Security
{
    public interface ISecurityManager<T> where T : IIdentityModel
    {
        T User { get; }
        bool IsInRole(string roleName);
    }
}

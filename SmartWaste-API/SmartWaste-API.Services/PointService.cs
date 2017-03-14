using SmartWaste_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmarteWaste_API.Contracts.Point;
using SmartWaste_API.Business.Interfaces;
using SmartWaste_API.Library.Security;
using SmarteWaste_API.Contracts;
using SmartWaste_API.Services.Security;

namespace SmartWaste_API.Services
{
    public class PointService : IPointService
    {
        private readonly IPointRepository _pointRepository;
        private readonly ISecurityManager<IdentityContract> _user;

        public PointService(IPointRepository pointRepository, ISecurityManager<IdentityContract> user)
        {
            _pointRepository = pointRepository;
            _user = user;
        }

        public List<PointDetailedContract> GetDetailedList(PointFilterContract filter)
        {
            return _pointRepository.GetDetailedList(filter);            
        }

        public List<PointContract> GetList(PointFilterContract filter)
        {
            return _pointRepository.GetList(CheckFilter(filter));
        }

        private PointFilterContract CheckFilter(PointFilterContract filter)
        {
            if (!_user.User.IsAuthenticated)
                filter.Type = PointTypeEnum.CompanyTrashCan;
            else if (_user.IsInRole(RolesName.USER))
                filter.PersonID = _user.User.Person.ID;

            return filter;
        }
    }
}
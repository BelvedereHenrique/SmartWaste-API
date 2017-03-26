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

namespace SmartWaste_API.Services
{
    public class PointHistoryService : IPointHistoryService
    {
        private readonly IPointHistoryRepository _pointHistoryRepository;
        private readonly ISecurityManager<IdentityContract> _user;

        public PointHistoryService(IPointHistoryRepository pointHistoryRepository,
                                   ISecurityManager<IdentityContract> user)
        {
            _pointHistoryRepository = pointHistoryRepository;
            _user = user;
        }

        public List<PointHistoryContract> GetList(PointHistoryFilterContract filter)
        {
            if (!_user.User.IsAuthenticated)
                return new List<PointHistoryContract>();
            else if (_user.User.Person.CompanyID == null)
                filter.PersonID = _user.User.Person.ID;
            else
                filter.CompanyID = _user.User.Person.CompanyID;

            return _pointHistoryRepository.GetList(filter);
        }
    }
}

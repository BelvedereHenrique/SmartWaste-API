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
using SmarteWaste_API.Contracts.OperationResult;

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

        public PointDetailedContract GetDetailed(PointFilterContract filter)
        {
            return _pointRepository.GetDetailed(filter);
        }

        public List<PointDetailedContract> GetDetailedList(PointFilterContract filter)
        {
            return _pointRepository.GetDetailedList(filter);
        }

        public List<PointContract> GetList(PointFilterContract filter)
        {
            return _pointRepository.GetList(CheckFilter(filter));
        }
        
        public OperationResult SetAsFull()
        {
            var result = new OperationResult();

            result.Merge(UserHasCompany());
            if (!result.Success) return result;

            var point = GetDetailed(new PointFilterContract()
            {
                PersonID = _user.User.Person.ID
            });

            result.Merge(PointIsntNull(point));
            if (!result.Success) return result;

            result.Merge(PointIsFull(point));            
            if (!result.Success) return result;

            point.Status = PointStatusEnum.Full;

            var histories = new List<PointHistoryContract>() {
                GetHistoryForFullPoint(point)
            };

            _pointRepository.Edit(point, histories);

            result.AddWarning("Your trashcan was setted as full, now, wait for a truck to collect it.");

            return result;
        }

        public void Edit(PointContract point) {
            _pointRepository.Edit(point);
        }

        private PointFilterContract CheckFilter(PointFilterContract filter)
        {
            if (!_user.User.IsAuthenticated)
                filter.Type = PointTypeEnum.CompanyTrashCan;
            else if (_user.IsInRole(RolesName.USER))
                filter.PersonID = _user.User.Person.ID;

            return filter;
        }

        private OperationResult UserHasCompany()
        {
            var result = new OperationResult();

            if (_user.User.Person.CompanyID != null)
                result.AddError("Company users cannot set their trashcan as full.");

            return result;
        }

        private OperationResult PointIsntNull(PointDetailedContract point)
        {
            var result = new OperationResult();

            if (point == null)
                result.AddError("You don't have any trashcan to set as full.");

            return result;
        }

        private OperationResult PointIsFull(PointDetailedContract point)
        {
            var result = new OperationResult();

            if (point.Status == PointStatusEnum.Full)
                result.AddError("Your trashcan is already full, wait for a truck to collect it.");

            return result;
        }

        private PointHistoryContract GetHistoryForFullPoint(PointDetailedContract point)
        {
            return new PointHistoryContract()
            {
                ID = Guid.NewGuid(),
                Date = DateTime.Now,
                Person = new SmarteWaste_API.Contracts.Person.PersonContract()
                {
                    ID = _user.User.Person.ID
                },
                PointID = point.ID,
                Reason = "Point setted as full.",
                Status = PointStatusEnum.Full
            };
        }
    }
}
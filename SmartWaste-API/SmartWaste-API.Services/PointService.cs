using SmartWaste_API.Services.Interfaces;
using System;
using System.Collections.Generic;
using SmarteWaste_API.Contracts.Point;
using SmartWaste_API.Business.Interfaces;
using SmartWaste_API.Library.Security;
using SmarteWaste_API.Contracts;
using SmartWaste_API.Services.Security;
using SmarteWaste_API.Contracts.OperationResult;
using SmarteWaste_API.Contracts.Address;
using SmarteWaste_API.Contracts.Person;
using SmarteWaste_API.Contracts.Device;

namespace SmartWaste_API.Services
{
    public class PointService : IPointService
    {
        private readonly IPointRepository _pointRepository;
        private readonly IDeviceService _deviceService;
        private readonly ISecurityManager<IdentityContract> _user;
        private readonly IAddressService _addressService;

        public PointService(IPointRepository pointRepository,
                            ISecurityManager<IdentityContract> user,
                            IDeviceService deviceService)
        {
            _pointRepository = pointRepository;
            _deviceService = deviceService;
            _user = user;
        }

        public PointDetailedContract GetDetailed(PointFilterContract filter)
        {
            if (!_user.User.IsAuthenticated)
                throw new UnauthorizedAccessException();
            else if (_user.User.Person.CompanyID.HasValue)
                return _pointRepository.GetCompanyDetailed(_user.User.Person.CompanyID.Value, filter);
            else
                return _pointRepository.GetUserDetailed(_user.User.Person.ID, filter);
        }

        public List<PointDetailedContract> GetDetailedList(PointFilterContract filter)
        {
            if (!_user.User.IsAuthenticated)
                throw new UnauthorizedAccessException();
            else if (_user.User.Person.CompanyID.HasValue)
                return _pointRepository.GetCompanyDetailedList(_user.User.Person.CompanyID.Value, filter);
            else
                return _pointRepository.GetUserDetailedList(_user.User.Person.ID, filter);
        }

        public List<PointContract> GetList(PointFilterContract filter)
        {
            if (!_user.User.IsAuthenticated)
                return _pointRepository.GetPublicList(filter);
            else if (_user.User.Person.CompanyID.HasValue)
                return _pointRepository.GetCompanyList(_user.User.Person.CompanyID.Value, filter);
            else
                return _pointRepository.GetUserList(_user.User.Person.ID, filter);
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

            return SetAsFull(point);
        }

        private OperationResult SetAsFull(PointDetailedContract point)
        {
            var result = new OperationResult();

            result.Merge(PointIsntNull(point));
            if (!result.Success) return result;

            result.Merge(PointIsFull(point));
            if (!result.Success) return result;

            point.Status = PointStatusEnum.Full;

            var histories = new List<PointHistoryContract>() {
                GetHistoryForFullPoint(point, _user.User.IsAuthenticated ? _user.User.Person.ID : point.PersonID.Value)
            };

            _pointRepository.Edit(point, histories);

            result.AddWarning("Your trashcan was setted as full, now, wait for a truck to collect it.");

            return result;
        }

        public OperationResult SetAsFull(DeviceEventContract deviceEvent)
        {
            var result = new OperationResult();

            var device = _deviceService.Get(new DeviceFilterContract()
            {
                InternalID = deviceEvent.SerialNumber
            });

            if (device == null)
                result.AddError("Device does not exist.");

            if (device != null && device.Status == DeviceStatusEnum.Deactivated)
                result.AddError("Device is deactivated.");

            if (!result.Success)
                return result;

            var point = _pointRepository.GetDetailed(device.ID);

            result = SetAsFull(point);

            device.BatteryVoltage = int.Parse(deviceEvent.BatteryVoltage.ToLower().Replace("vw", string.Empty));
            _deviceService.Edit(device);

            return result;
        }

        public void Edit(PointContract point)
        {
            _pointRepository.Edit(point,null);
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

        private PointHistoryContract GetHistoryForFullPoint(PointDetailedContract point, Guid personID)
        {
            return new PointHistoryContract()
            {
                ID = Guid.NewGuid(),
                Date = DateTime.Now,
                Person = new SmarteWaste_API.Contracts.Person.PersonContract()
                {
                    ID = personID
                },
                PointID = point.ID,
                Reason = "Point setted as full.",
                Status = PointStatusEnum.Full
            };
        }
        public PointContract GetPointByDeviceID(Guid deviceID)
        {
            return _pointRepository.GetPointByDeviceID(deviceID);
        }

        public void RegisterPoint(AddressContract address)
        {
            var personID = _user.User.Person.CompanyID;

            if (personID == null)
                throw new ArgumentException("The specified user is not a company.");

            var pointContract = new PointContract()
            {
                AddressID = Guid.NewGuid(),
                ID = Guid.NewGuid(),
                Status = PointStatusEnum.Empty,
                Type = PointTypeEnum.CompanyTrashCan,
                PointRouteStatus = PointRouteStatusEnum.Free
            };

            address.ID = pointContract.AddressID;

            _addressService.Add(address);

            _pointRepository.AddCompanyPoint(pointContract,personID.Value);

        }
    }
}
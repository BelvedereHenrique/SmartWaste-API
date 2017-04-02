using SmarteWaste_API.Contracts.Point;
using SmarteWaste_API.Contracts.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business.ContractParser
{
    public static class RoutePointContractParser
    {
        public static RoutePointContract ToContract(this Data.vw_RoutePointsDetailed entitie)
        {
            if (entitie == null)
                return null;

            return new RoutePointContract() {
                CollectedBy = entitie.CollectedBy,
                CollectedOn = entitie.CollectedOn,
                ID = entitie.RoutePointID,
                IsCollected = entitie.IsCollected,
                Reason = entitie.Reason,
                Point= new SmarteWaste_API.Contracts.Point.PointDetailedContract() {
                    ID = entitie.ID,
                    Status = (PointStatusEnum)entitie.StatusID,
                    Type = (PointTypeEnum)entitie.TypeID,
                    Latitude = entitie.Latitude,
                    Longitude = entitie.Longitude,
                    DeviceID = entitie.DeviceID,
                    PersonID = entitie.PersonID,                    
                    CountryAlias = entitie.CountryAlias,
                    CountryName = entitie.CountryName,
                    Line1 = entitie.Line1,
                    Line2 = entitie.Line2,
                    Name = entitie.Name,
                    Neighborhood = entitie.Neighborhood,
                    StateAlias = entitie.StateAlias,
                    StateName = entitie.StateName,
                    ZipCode = entitie.ZipCode,
                    PointRouteStatus = (PointRouteStatusEnum)entitie.PointRouteStatusID,
                    AddressID = entitie.AddressID,
                    CityID = entitie.CityID,
                    CityName = entitie.CityName,
                    StateID = entitie.StateID,
                    CountryID = entitie.CountryID
                }
            };
        }

        public static List<RoutePointContract> ToContracts(this List<Data.vw_RoutePointsDetailed> entities)
        {
            return entities.Select(x => x.ToContract()).ToList();
        }
    }
}

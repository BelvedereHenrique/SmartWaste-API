using SmarteWaste_API.Contracts.Route;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWaste_API.Business.ContractParser
{
    public static class RouteContractParser
    {
        public static RouteDetailedContract ToContract(this Data.Route entitie)
        {
            if (entitie == null) return null;

            return new RouteDetailedContract() {
                AssignedTo = entitie.Person.ToContract(),
                ClosedOn = entitie.ClosedOn,
                CreatedBy = entitie.Person1.ToContract(),
                CreatedOn = entitie.CreatedOn,
                Histories = entitie.RouteHistories.ToList().ToContracts(),
                Points = entitie.RoutePoints.Select(x => new SmarteWaste_API.Contracts.Point.PointDetailedContract() { ID = x.PointID }).ToList(),
                ID = entitie.ID,                
                Status = (RouteStatusEnum) entitie.StatusID,
                CompanyID = entitie.CompanyID,
                ExpectedKilometers = entitie.ExpectedKilometers,
                ExpectedMinutes = entitie.ExpectedMinutes
            };
        }

        public static List<RouteDetailedContract> ToContracts(this List<Data.Route> entities)
        {
            return entities.Select(x => x.ToContract()).ToList();
        }

        public static Data.Route ToEntitie(this RouteDetailedContract contract)
        {
            if (contract == null) return null;

            var entitie = new Data.Route()
            {
                ID = contract.ID,                
                StatusID = (int)contract.Status,
                ClosedOn = contract.ClosedOn,              
                CreatedOn = contract.CreatedOn,
                CompanyID = contract.CompanyID,
                ExpectedKilometers = contract.ExpectedKilometers,
                ExpectedMinutes = contract.ExpectedMinutes
            };

            if (contract.AssignedTo != null)
                entitie.AssignedTo = contract.AssignedTo.ID;

            if (contract.CreatedBy != null)
                entitie.CreatedBy = contract.CreatedBy.ID;

            return entitie;
        }

        public static RouteContract ToContract(this Data.vw_GetRoutes entitie)
        {
            if (entitie == null) return null;

            var contract = new RouteContract()
            {

                ClosedOn = entitie.ClosedOn,
                CreatedBy = new SmarteWaste_API.Contracts.Person.PersonContract() {
                    ID = entitie.CreatedByID,
                    Name = entitie.CreatedByName
                },
                CreatedOn = entitie.CreatedOn,
                ID = entitie.ID,
                Status = (RouteStatusEnum)entitie.StatusID,
                CompanyID = entitie.CompanyID,
                ExpectedKilometers = entitie.ExpectedKilometers,
                ExpectedMinutes = entitie.ExpectedMinutes
            };

            if (entitie.AssignedToID.HasValue)
                contract.AssignedTo = new SmarteWaste_API.Contracts.Person.PersonContract() {
                    ID = entitie.AssignedToID.Value,
                    Name = entitie.AssignedToName
                };

            return contract;
        }

        public static List<RouteContract> ToContracts(this List<Data.vw_GetRoutes> entities)
        {
            return entities.Select(x => x.ToContract()).ToList();
        }
    }
}

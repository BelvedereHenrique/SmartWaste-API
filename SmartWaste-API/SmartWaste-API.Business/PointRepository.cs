using SmartWaste_API.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmarteWaste_API.Contracts.Point;
using SmartWaste_API.Business.ContractParser;

namespace SmartWaste_API.Business
{
    public class PointRepository : IPointRepository
    {
        public List<PointContract> GetList(PointFilterContract filter)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                return GetPointsQuery(context, filter).ToList().ToContracts();
            }
        }

        public IQueryable<Data.vw_GetPoints> GetPointsQuery(Data.SmartWasteDatabaseConnection context, PointFilterContract filter)
        {
            return context.vw_GetPoints.Where(x =>
                (x.Latitude <= filter.Northwest.Latitude && x.Latitude >= filter.Southeast.Latitude) &&
                (x.Latitude >= filter.Northwest.Longitude && x.Longitude <= filter.Southeast.Longitude)
            ).AsQueryable();
        }
    }
}

using System;
using SmarteWaste_API.Contracts.Device;
using SmartWaste_API.Business.Data;
using SmartWaste_API.Business.Interfaces;

namespace SmartWaste_API.Business
{
    public class DeviceHistoryRepository : IDeviceHistoryRepository
    {
        public void Save(DeviceHistoryContract deviceHistory)
        {
            using (var context = new Data.SmartWasteDatabaseConnection())
            {
                var history = new DeviceHistory()
                {
                    ID = deviceHistory.ID,
                    DeviceID = deviceHistory.DeviceID,
                    StatusID = (int)deviceHistory.Status,
                    PersonID = deviceHistory.PersonID != null ? deviceHistory.PersonID.Value : Guid.Empty,
                    Reason = deviceHistory.Reason,
                    Date = deviceHistory.Date,
                };
                context.DeviceHistories.Add(history);
                context.SaveChanges();
            }
        }
    }
}

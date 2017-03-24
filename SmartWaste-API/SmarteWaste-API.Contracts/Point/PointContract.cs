using System;

namespace SmarteWaste_API.Contracts.Point
{
    public class PointContract
    {
        public Guid ID { get; set; }
        public PointStatusEnum Status { get; set; }
        public PointRouteStatusEnum PointRouteStatus { get; set; }
        public PointTypeEnum Type { get; set; }
        public Guid? DeviceID { get; set; }
        public Guid AddressID { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public System.Guid PersonID { get; set; }
        public System.Guid UserID { get; set; }
    }
}

using SmarteWaste_API.Contracts.Point;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartWaste_API.Models
{
    public class PointDetailedHistoriesModel : PointDetailedContract
    {
        public PointDetailedHistoriesModel(PointDetailedContract point, List<PointHistoryContract> histories)
        {
            this.AddressID = point.AddressID;
            this.CityID = point.CityID;
            this.CityName = point.CityName;
            this.CountryAlias = point.CountryAlias;
            this.CountryID = point.CountryID;
            this.CountryName = point.CountryName;
            this.DeviceID = point.DeviceID;
            this.ID = point.ID;
            this.Latitude = point.Latitude;
            this.Line1 = point.Line1;
            this.Line2 = point.Line2;
            this.Longitude = point.Longitude;
            this.Name = point.Name;
            this.Neighborhood = point.Neighborhood;
            this.PersonID = point.PersonID;
            this.PointRouteStatus = point.PointRouteStatus;
            this.StateAlias = point.StateAlias;
            this.StateID = point.StateID;
            this.StateName = point.StateName;
            this.Status = point.Status;
            this.Type = point.Type;
            this.UserID = point.UserID;
            this.ZipCode = point.ZipCode;

            this.Histories = histories;
        }

        public List<PointHistoryContract> Histories { get; set; }
    }
}
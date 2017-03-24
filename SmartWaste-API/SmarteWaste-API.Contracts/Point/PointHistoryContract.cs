using SmarteWaste_API.Contracts.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Point
{
    public class PointHistoryContract
    {
        public PointHistoryContract()
        {
            Person = new PersonContract();
        }

        public Guid ID { get; set; }
        public DateTime Date { get; set; }
        public PointStatusEnum Status { get; set; }
        public PersonContract Person { get; set; }
        public string Reason { get; set; }
        public Guid PointID { get; set; }
    }
}

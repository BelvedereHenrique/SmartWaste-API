using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmarteWaste_API.Contracts.Point
{
    public class PointFilterContract
    {
        public PointFilterContract()
        {
            this.IDs = new List<Guid>();
            this.NotIDs = new List<Guid>();
            this.AlwaysIDs = new List<Guid>();
            this.Northwest = new PointCoordinatorContract();
            this.Southeast = new PointCoordinatorContract();
        }

        public PointCoordinatorContract Northwest { get; set; }
        public PointCoordinatorContract Southeast { get; set; }
        public Guid? PersonID { get; set; }
        public PointStatusEnum? Status { get; set; }
        public PointTypeEnum? Type { get; set; }
        public PointRouteStatusEnum? PointRouteStatus { get; set; }
        // NOTE: AlwaysIDs => Get theses points even if the other filters don't match.
        public List<Guid> AlwaysIDs { get; set; }
        public List<Guid> IDs { get; set; }
        public List<Guid> NotIDs { get; set; }
    }
}

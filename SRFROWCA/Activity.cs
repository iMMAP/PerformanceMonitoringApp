//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SRFROWCA
{
    using System;
    using System.Collections.Generic;
    
    public partial class Activity
    {
        public Activity()
        {
            this.ActivityDetails = new HashSet<ActivityDetail>();
            this.Indicators = new HashSet<Indicator>();
        }
    
        public int ActivityId { get; set; }
        public int EmergencyLocationId { get; set; }
        public int EmergencyClusterId { get; set; }
        public int EmergencyObjectiveId { get; set; }
        public int YearId { get; set; }
        public bool IsMigrated { get; set; }
        public Nullable<int> OldActivityId { get; set; }
        public bool IsActive { get; set; }
        public System.Guid CreatedById { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.Guid> UpdatedById { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> tempnumber { get; set; }
        public Nullable<int> EmgergencySecClusterId { get; set; }
    
        public virtual EmergencyCluster EmergencyCluster { get; set; }
        public virtual EmergencyLocation EmergencyLocation { get; set; }
        public virtual EmergencyObjective EmergencyObjective { get; set; }
        public virtual ICollection<ActivityDetail> ActivityDetails { get; set; }
        public virtual ICollection<Indicator> Indicators { get; set; }
    }
}

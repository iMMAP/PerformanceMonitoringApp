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
    
    public partial class ClusterIndicatorTarget
    {
        public int ClusterIndicatorTargetId { get; set; }
        public int EmergencyLocationId { get; set; }
        public int EmergencyClusterId { get; set; }
        public int ActivityDataId { get; set; }
        public int YearId { get; set; }
        public Nullable<int> TargetMidYear { get; set; }
        public Nullable<int> TargetFullYear { get; set; }
        public System.Guid CreatedById { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.Guid> UpdatedById { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual EmergencyLocation EmergencyLocation { get; set; }
    }
}

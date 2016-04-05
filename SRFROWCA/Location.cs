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
    
    public partial class Location
    {
        public Location()
        {
            this.EmergencyLocations = new HashSet<EmergencyLocation>();
            this.IndicatorClusterTargets = new HashSet<IndicatorClusterTarget>();
            this.ProjectIndicatorAnnualTargets = new HashSet<ProjectIndicatorAnnualTarget>();
            this.ReportLocations = new HashSet<ReportLocation>();
        }
    
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int LocationTypeId { get; set; }
        public Nullable<int> LocationParentId { get; set; }
        public string LocationPCode { get; set; }
        public Nullable<double> Latitude { get; set; }
        public Nullable<double> Longitude { get; set; }
        public Nullable<bool> IsAccurateLatLng { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.Guid> CreatedById { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedById { get; set; }
        public Nullable<int> EstimatedPopulation { get; set; }
        public Nullable<int> TempId { get; set; }
        public int LocationCategoryId { get; set; }
    
        public virtual ICollection<EmergencyLocation> EmergencyLocations { get; set; }
        public virtual ICollection<IndicatorClusterTarget> IndicatorClusterTargets { get; set; }
        public virtual LocationType LocationType { get; set; }
        public virtual ICollection<ProjectIndicatorAnnualTarget> ProjectIndicatorAnnualTargets { get; set; }
        public virtual ICollection<ReportLocation> ReportLocations { get; set; }
    }
}
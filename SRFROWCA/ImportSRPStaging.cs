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
    
    public partial class ImportSRPStaging
    {
        public int Id { get; set; }
        public Nullable<int> tid { get; set; }
        public Nullable<int> EmergencyId { get; set; }
        public Nullable<int> EmergencyLocationId { get; set; }
        public Nullable<int> EmergencyClusterId { get; set; }
        public Nullable<int> EmergencyObjectiveId { get; set; }
        public string Objective { get; set; }
        public Nullable<int> ObjectiveId { get; set; }
        public string Activity_En { get; set; }
        public string Activity_Fr { get; set; }
        public Nullable<int> ActivityId { get; set; }
        public string Indicator_En { get; set; }
        public string Indicator_Fr { get; set; }
        public Nullable<int> IndicatorId { get; set; }
        public string Unit_En { get; set; }
        public string Unit_Fr { get; set; }
        public Nullable<int> UnitId { get; set; }
        public string Location { get; set; }
        public Nullable<int> LocationId { get; set; }
        public Nullable<int> Data { get; set; }
        public Nullable<bool> IsTargetExists { get; set; }
    }
}

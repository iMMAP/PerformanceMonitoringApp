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
    
    public partial class StagingTableLogFrame
    {
        public int Id { get; set; }
        public Nullable<int> EmergencyId { get; set; }
        public string Cluster_En { get; set; }
        public string Cluster_Fr { get; set; }
        public Nullable<int> ClusterId { get; set; }
        public Nullable<int> EmergencyClusterId { get; set; }
        public string Objective_En { get; set; }
        public string Objective_Fr { get; set; }
        public Nullable<int> ObjectiveId { get; set; }
        public Nullable<int> ClusterObjectiveId { get; set; }
        public string Priority_En { get; set; }
        public string Priority_Fr { get; set; }
        public Nullable<int> PriorityId { get; set; }
        public Nullable<int> ObjectivePriorityId { get; set; }
        public string Activity_En { get; set; }
        public string Activity_Fr { get; set; }
        public Nullable<int> PriorityActivityId { get; set; }
        public string Data_En { get; set; }
        public string Data_Fr { get; set; }
        public Nullable<int> ActivityDataId { get; set; }
        public string Unit_En { get; set; }
        public string Unit_Fr { get; set; }
        public Nullable<int> UnitId { get; set; }
        public Nullable<System.Guid> UserId { get; set; }
        public string SecondaryCluster_En { get; set; }
        public string SecondaryCluster_Fr { get; set; }
        public Nullable<int> SecondaryClusterId { get; set; }
    }
}

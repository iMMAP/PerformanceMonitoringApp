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
    
    public partial class ClusterReport
    {
        public ClusterReport()
        {
            this.ClusterReportDetails = new HashSet<ClusterReportDetail>();
        }
    
        public int ClusterReportID { get; set; }
        public int EmergencyLocationID { get; set; }
        public int EmergencyClusterID { get; set; }
        public int YearID { get; set; }
        public int MonthID { get; set; }
        public Nullable<System.Guid> CreatedByID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> UpdatedByID { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual ICollection<ClusterReportDetail> ClusterReportDetails { get; set; }
        public virtual ClusterReport ClusterReports1 { get; set; }
        public virtual ClusterReport ClusterReport1 { get; set; }
    }
}

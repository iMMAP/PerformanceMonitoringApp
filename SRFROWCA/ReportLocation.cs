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
    
    public partial class ReportLocation
    {
        public ReportLocation()
        {
            this.ReportDetails = new HashSet<ReportDetail>();
        }
    
        public int ReportLocationId { get; set; }
        public int ReportId { get; set; }
        public int LocationId { get; set; }
    
        public virtual Location Location { get; set; }
        public virtual ICollection<ReportDetail> ReportDetails { get; set; }
        public virtual Report Report { get; set; }
    }
}

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
    
    public partial class OPSReportLocation
    {
        public int OPSReportLocationId { get; set; }
        public int OPSReportId { get; set; }
        public int LocationId { get; set; }
        public int EmergencyClusterId { get; set; }
    
        public virtual OPSReport OPSReport { get; set; }
    }
}

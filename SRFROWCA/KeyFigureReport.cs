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
    
    public partial class KeyFigureReport
    {
        public KeyFigureReport()
        {
            this.KeyFigureReportDetails = new HashSet<KeyFigureReportDetail>();
        }
    
        public int KeyFigureReportId { get; set; }
        public int KeyFigureSubCategoryId { get; set; }
        public int CountryId { get; set; }
        public Nullable<int> KeyFigureIndicatorId { get; set; }
        public System.DateTime KeyFigureReportedDate { get; set; }
        public bool IsActive { get; set; }
        public string KeyFigureReportName { get; set; }
        public System.Guid CreatedById { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.Guid> UpdatedById { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual KeyFigureIndicator KeyFigureIndicator { get; set; }
        public virtual ICollection<KeyFigureReportDetail> KeyFigureReportDetails { get; set; }
    }
}

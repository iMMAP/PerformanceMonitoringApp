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
    
    public partial class HumanitarianPriority
    {
        public int PriorityIdentityId { get; set; }
        public int HumanitarianPriorityId { get; set; }
        public byte SiteLanguageId { get; set; }
        public string HumanitarianPriority1 { get; set; }
        public Nullable<bool> IsLogFramePriority { get; set; }
        public string ShortPriorityText { get; set; }
        public System.DateTime DateCreated { get; set; }
        public Nullable<System.Guid> CreatedById { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public Nullable<System.DateTime> UpdatedById { get; set; }
    }
}
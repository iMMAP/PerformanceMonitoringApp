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
    
    public partial class ImportCLDataStagingTable
    {
        public int id { get; set; }
        public Nullable<int> tid { get; set; }
        public string Month { get; set; }
        public Nullable<int> MonthId { get; set; }
        public string ProjectCode { get; set; }
        public Nullable<int> ProjectId { get; set; }
        public string Objective { get; set; }
        public string Priority { get; set; }
        public string Activity { get; set; }
        public Nullable<int> Indicator_Id { get; set; }
        public string Indicator { get; set; }
        public string Accumulative { get; set; }
        public string Unit { get; set; }
        public Nullable<int> Mid_Year_Target { get; set; }
        public Nullable<int> Full_Year_Target { get; set; }
        public Nullable<int> AnnualTarget { get; set; }
        public string Location { get; set; }
        public Nullable<int> Data { get; set; }
        public Nullable<int> LocationId { get; set; }
        public Nullable<int> OrganizationId { get; set; }
        public Nullable<int> ReportId { get; set; }
        public Nullable<int> ReportLocationId { get; set; }
        public Nullable<bool> IsAccum { get; set; }
        public Nullable<int> IsReportExists { get; set; }
        public Nullable<int> IsReportLocationExists { get; set; }
        public Nullable<int> IndicatorExists { get; set; }
        public Nullable<int> MidTargetExists { get; set; }
        public Nullable<int> FullTargetExists { get; set; }
        public Nullable<int> ProjIndExists { get; set; }
        public Nullable<int> ProjectIndicatorId { get; set; }
    }
}
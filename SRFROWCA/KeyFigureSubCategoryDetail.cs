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
    
    public partial class KeyFigureSubCategoryDetail
    {
        public int KeyFigureSubCategoryDetailId { get; set; }
        public int KeyFigureSubCategoryId { get; set; }
        public int SiteLanguageId { get; set; }
        public string KeyFigureSubCategory { get; set; }
    
        public virtual KeyFigureSubCategory KeyFigureSubCategory1 { get; set; }
    }
}

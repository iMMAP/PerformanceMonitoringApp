using SRFROWCA.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace SRFROWCA.Configurations
{
    public class AdminTargetSettingItems
    {
        public bool IsTarget { get; set; }

        public RC.AdminLevels AdminLevel { get; set; }

        public RC.LocationCategory Category { get; set; }

        public bool IsMandatory { get; set; }
    }
}
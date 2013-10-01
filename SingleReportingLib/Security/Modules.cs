using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleReporting.Security
{
    internal sealed class Modules
    {
        private int moduleId;
        private string moduleName;
        private DateTime createdDate;
        private ModuleType moduleType;

        #region Properties

        public int ModuleId
        {
            set { moduleId = value; }
            get { return moduleId; }
        }

        public string ModuleName
        {
            set { moduleName = value; }
            get { return moduleName; }
        }
        public ModuleType ModuleType
        {
            set { moduleType = value; }
            get { return moduleType; }
        }





        #endregion
    }
}

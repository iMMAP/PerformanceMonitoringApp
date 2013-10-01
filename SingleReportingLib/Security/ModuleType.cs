using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SingleReporting.Security
{
    class ModuleType
    {
        private int moduleTypeId;
        private string moduleTypeName;
        private DateTime createdDate;
        private bool isActive;


        #region Properties

        public int ModuleTypeId
        {
            set { moduleTypeId = value; }
            get { return moduleTypeId; }
        }

        public string ModuleTypeName
        {
            set { moduleTypeName = value; }
            get { return moduleTypeName; }
        }
        public DateTime CreatedDate
        {
            set { createdDate = value; }
            get { return createdDate; }
        }
        public bool IsActive
        {
            set { isActive = value; }
            get { return isActive; }
        }
        #endregion

    }
}

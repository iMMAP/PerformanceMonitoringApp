using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Data.Services.Common;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;

namespace SRFROWCA
{
    public class BooksWcfDataService2 : DataService<rowcaEntities>
    {
        // This method is called only once to initialize service-wide policies.
        public static void InitializeService(DataServiceConfiguration config)
        {
            //// TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
            //// Examples:
            //// config.SetEntitySetAccessRule("MyEntityset", EntitySetRights.AllRead);
            //// config.SetServiceOperationAccessRule("MyServiceOperation", ServiceOperationRights.All);
            //config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;

            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;

        }
    }
}

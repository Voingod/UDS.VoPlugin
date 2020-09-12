using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KostenVoranSchlagConsoleParser.Helpers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using UDS.VoPlugin;

namespace ConsoleCRMApp
{
    class Program
    {
        static void Main(string[] args)
        {
            OrganizationServiceProxy serviceProxy = ConnectHelper.CrmService;
            var service = (IOrganizationService)serviceProxy;
            Entity someEnt = service.Retrieve("new_mainentity", new Guid("A91350CA-F5BA-E711-80FB-00155D05FA01"), new ColumnSet(true));
            ServiceClass serviceClass = new ServiceClass(service);
            serviceClass.MainMethod();
            string var = null;

            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrmEarlyBound;
using KostenVoranSchlagConsoleParser.Helpers;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using UDS.VoPlugin;
using UDS.VoPlugin.Repository;

namespace ConsoleCRMApp
{
    class Program
    {
        static void Main(string[] args)
        {

            OrganizationServiceProxy serviceProxy = ConnectHelper.CrmService;
            var service = (IOrganizationService)serviceProxy;
            serviceProxy.ServiceConfiguration.CurrentServiceEndpoint.Behaviors.Add(new ProxyTypesBehavior());
            //VoService voService = new VoService(service);
            //voService.Deactivate();
            //voService.EarlyBoundDeactivate();

            ///////////////////////////////////////////////////////////////////////////////////////////////////

            //CrmServiceContext context = new CrmServiceContext(service);



            VoMainScriptDeactivateRepository voMainScript = new VoMainScriptDeactivateRepository(service);
            var e = voMainScript.EarlyBoundGetEntities(new Guid("E71698A5-1BEA-EA11-8129-00155D06F203"));

            Console.ReadLine();

        }
        
    }

}

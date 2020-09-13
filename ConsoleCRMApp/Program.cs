using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            VoService voService = new VoService(service);
            voService.Deactivate();

            Console.ReadLine();

        }
        
    }

}

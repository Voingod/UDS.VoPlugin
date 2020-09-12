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

            var links = new VoService(service).GetLinkEntities();
            var groupLinks = links.GroupBy(g => g.Id)
                .ToDictionary(k => k.Key, i => i.Select(z => z.Attributes).ToList());

            foreach (var item in groupLinks)
            {
                int countRecords = 0;
                int allRecordsCount = item.Value.Count;

                for (int i = 0; i < item.Value.Count; i++)
                {
                    if (!item.Value[i].Contains("new_vo_two_main2.new_account"))
                    {
                        break;
                    }
                    EntityReference VoMainScriptAccount = (EntityReference)item.Value[i]["new_account"];
                    AliasedValue aliesNewAccount = (AliasedValue)item.Value[i]["new_vo_two_main2.new_account"];
                    EntityReference VoTwoMainAccount = (EntityReference)aliesNewAccount.Value;
                    var VoMainScriptAccountId = VoMainScriptAccount.Id;
                    var VoTwoMainAccountId = VoTwoMainAccount.Id;
                    if (VoMainScriptAccountId!= VoTwoMainAccountId)
                    {
                        continue;
                    }
                    countRecords++;
                }

                if (countRecords==allRecordsCount)
                {
                    var record = item.Value[countRecords - 1];
                    Console.WriteLine(record["new_name"]+": "+record["new_vo_main_scriptid"]);
                    DeactivateRecord("new_vo_main_script", (Guid)record["new_vo_main_scriptid"], service);

                }
            }
            
            Console.ReadLine();

            
        }
        public static void DeactivateRecord(string entityName, Guid recordId, IOrganizationService organizationService)
        {
            var cols = new ColumnSet(new[] { "statecode", "statuscode" });

            //Check if it is Active or not
            var entity = organizationService.Retrieve(entityName, recordId, cols);

            if (entity != null && entity.GetAttributeValue<OptionSetValue>("statecode").Value == 0)
            {
                //StateCode = 1 and StatusCode = 2 for deactivating Account or Contact
                SetStateRequest setStateRequest = new SetStateRequest()
                {
                    EntityMoniker = new EntityReference
                    {
                        Id = recordId,
                        LogicalName = entityName,
                    },
                    State = new OptionSetValue(1),
                    Status = new OptionSetValue(2)
                };
                organizationService.Execute(setStateRequest);
            }
        }
    }

}

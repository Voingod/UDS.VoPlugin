using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using UDS.VoPlugin.Repository;
using System.Diagnostics;
using CrmEarlyBound;

namespace UDS.VoPlugin
{
    public class VoService
    {
        private IOrganizationService _service;
        private CrmServiceContext _context;
        DeactivateService deactivateService = new DeactivateService();
        public VoService(IOrganizationService service)
        {
            _service = service;
            _context = new CrmServiceContext(_service);
        }

        public void Deactivate()
        {
            var entities = new VoMainScriptRepository(_service).GetEntities();
            var groupsEntities = entities.GroupBy(g => g.Id)
                .ToDictionary(k => k.Key, i => i.Select(z => z.Attributes).ToList());

            foreach (var item in groupsEntities)
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
                    if (VoMainScriptAccountId != VoTwoMainAccountId)
                    {
                        continue;
                    }
                    countRecords++;
                }

                if (countRecords == allRecordsCount)
                {

                    var record = item.Value[countRecords - 1];
                    OptionSetValue state = (OptionSetValue)record["statecode"];
                    if (state.Value == 0)
                    {
                        var output = record["new_name"] + ": " + record["new_vo_main_scriptid"] + " WAS DEACTIVATE";
                        Trace.WriteLine(output);
                        Console.WriteLine(output);
                        deactivateService.DeactivateRecord("new_vo_main_script", (Guid)record["new_vo_main_scriptid"], _service);
                    }
                    else
                    {
                        Console.WriteLine(record["new_name"] + ": already inactive!!!");
                    }
                }
            }
        }

        public void EarlyBoundDeactivate()
        {
            var entities = new VoMainScriptRepository(_service).EarlyBoundGetEntities(_context).ToList();
            var groupsEntities = entities.GroupBy(g => g.VoScriptId)
                .ToDictionary(k => k.Key, i => i.ToList());

            foreach (var item in groupsEntities)
            {
                int countRecords = 0;
                int allRecordsCount = item.Value.Count;
                var date = DateTime.Now.Subtract(item.Value[0].VoScriptData.Value).Days;

                for (int i = 0; i < item.Value.Count; i++)
                {
                    if (item.Value[i].VoTwoAcc == null || date < 2)
                    {
                        break;
                    }
                    EntityReference VoMainScriptAccount = item.Value[i].VoScriptAcc;
                    EntityReference VoTwoMainAccount = item.Value[i].VoTwoAcc;
                    var VoMainScriptAccountId = VoMainScriptAccount.Id;
                    var VoTwoMainAccountId = VoTwoMainAccount.Id;
                    
                    if (VoMainScriptAccountId != VoTwoMainAccountId)
                    {
                        continue;
                    }
                    countRecords++;
                }

                if (countRecords == allRecordsCount)
                {
                    var record = item.Value[countRecords - 1];
                    if (record.VoSriptState == 0)
                    {
                        var output = record.VoScriptName + ": " + record.VoScriptId + " WAS DEACTIVATE";
                        Trace.WriteLine(output);
                        Console.WriteLine(output);
                        deactivateService.DeactivateRecord("new_vo_main_script", (Guid)record.VoScriptId, _service);
                    }
                    else
                    {
                        Console.WriteLine(record.VoScriptName + ": already inactive!!!");
                    }


                }
            }
        }
    }
}

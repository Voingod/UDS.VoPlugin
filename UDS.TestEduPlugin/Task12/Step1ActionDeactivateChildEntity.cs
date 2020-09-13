using CrmEarlyBound;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UDS.VoPlugin.Repository;

namespace UDS.VoPlugin.Task12
{
    public class Step1ActionDeactivateChildEntity : Plugin
    {
        public Step1ActionDeactivateChildEntity() : base(typeof(Step1ActionDeactivateChildEntity))
        {
            //new_vo_main_script
            //vo_actiontask12 Update
            base.RegisteredEvents
                .Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PostOperation, "vo_actiontask12",
                    "new_vo_main_script",
                    DeactivateChildEntities));
        }
        protected void DeactivateChildEntities(LocalPluginContext localContext)
        {
            IOrganizationService service = localContext.OrganizationService;
            VoMainScriptDeactivateRepository voMainScript = new VoMainScriptDeactivateRepository(service);
            DeactivateService deactivateService = new DeactivateService();

            var recordId = localContext.PluginExecutionContext.PrimaryEntityId;

            var entities = voMainScript.EarlyBoundGetEntities(recordId);
            foreach (var item in entities)
            {
                deactivateService.DeactivateRecord(item.LogicalName, (Guid)item.new_vo_child_script_firstId, service);
            }
            //Entity entity = new Entity("new_vo_main_script");
            //service.Update(entity);
        }
    }
}

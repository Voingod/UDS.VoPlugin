using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDS.VoPlugin.Task9
{
    public class Step2SetState : Plugin
    {
        public Step2SetState() : base(typeof(Step2SetState))
        {
            base.RegisteredEvents
                .Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PreOperation, "SetState",
                    "contact",
                    CheckEmailAndWriteToField));
        }
        protected void CheckEmailAndWriteToField(LocalPluginContext localContext)
        {

            if (!localContext.PluginExecutionContext.PreEntityImages.Contains("PreImages"))
            {
                return;
            }
            Entity image = localContext.PluginExecutionContext.PreEntityImages["PreImages"];
            EntityReference voMainScript = image.GetAttributeValue<EntityReference>("new_vomainscript");
            string email = image.GetAttributeValue<string>("emailaddress1");

            if (voMainScript == null)
            {
                localContext.Trace("Doesn't have new_vomainscript");
                return;
            }
            IOrganizationService service = localContext.OrganizationService;
            Entity entityVoMainScript = service.Retrieve(voMainScript.LogicalName, voMainScript.Id, new ColumnSet("new_textfromcontact"));

            if (!entityVoMainScript.Attributes.ContainsKey("new_textfromcontact"))
            {
                localContext.Trace("Field not has a value or doesn't exist");
                return;
            }
            entityVoMainScript["new_textfromcontact"] += "в системе был деактивирован контакт с e-mail: " + email;
        }
    }
}

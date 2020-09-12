using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDS.VoPlugin.Task9
{
    public class Step2SetStateDynamicEntity : Plugin
    {
        public Step2SetStateDynamicEntity() : base(typeof(Step2SetStateDynamicEntity))
        {
            base.RegisteredEvents
                .Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PreOperation, "SetStateDynamicEntity",
                    "contact",
                    CheckEmailAndWriteToField));
        }
        protected void CheckEmailAndWriteToField(LocalPluginContext localContext)
        {
            OptionSetValue state = (OptionSetValue)localContext.PluginExecutionContext.InputParameters["State"];

            if (!localContext.PluginExecutionContext.PreEntityImages.Contains("PreImages") || state.Value == 0)
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
            string text = entityVoMainScript["new_textfromcontact"].ToString();
            entityVoMainScript["new_textfromcontact"] = "";
            entityVoMainScript["new_textfromcontact"] += "\nв системе был деактивирован контакт с e-mail: " + email+ "\n" + text;
            service.Update(entityVoMainScript);
        }
    }
}

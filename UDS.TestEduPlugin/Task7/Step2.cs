using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using Microsoft.Xrm.Client;

namespace UDS.VoPlugin.Task7
{
    public class Step2 : Plugin
    {
        public Step2() : base(typeof(Step1))
        {
            base.RegisteredEvents
                .Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PreOperation, "Update",
                    "new_vo_main_script",
                    CheckAccountEmail));
        }
        protected void CheckAccountEmail(LocalPluginContext localContext)
        {
            if (!localContext.PluginExecutionContext.InputParameters.Contains("Target"))
            {
                return;
            }
            localContext.Trace("Старт логирования: ");

            Entity target = (Entity)localContext.PluginExecutionContext.InputParameters["Target"];
            EntityReference company = target.GetAttributeValue<EntityReference>("new_account");
            if (company.Id == null)
            {
                return;
            }
            IOrganizationService service = localContext.OrganizationService;
            //OrganizationService organizationService = new OrganizationService(service);
            
            Entity account = service.Retrieve(company.LogicalName, company.Id, new ColumnSet("emailaddress1"));
            if (account.Attributes.ContainsKey("emailaddress1"))
            {
                target["new_emailfromplugin"] = account.GetAttributeValue<string>("emailaddress1");
                localContext.Trace("Конец логирования: ");
            }
            else
            {
                throw new InvalidPluginExecutionException("Doesn't have E-mail");
            }
            

        }
    }

}

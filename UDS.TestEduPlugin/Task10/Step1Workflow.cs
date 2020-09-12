using System;
using System.Activities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;

namespace UDS.VoPlugin.Task10
{
    public class Step1Workflow : CodeActivity
    {
        #region Workflow parameters

        [RequiredArgument]
        [Input("TextToAdd")]
        public InArgument<string> TextParam { get; set; }

        [RequiredArgument]
        [Input("IntToAdd")]
        public InArgument<int> IntParam { get; set; }
        
        [RequiredArgument]
        [ReferenceTarget("account")]
        [Input("Account")]
        public InArgument<EntityReference> Account { get; set; }

        [RequiredArgument]
        [Output("OuterString")]
        public OutArgument<string> OutParam { get; set; }
        #endregion

        protected override void Execute(CodeActivityContext executionContext)
        {
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);

            string textPar = TextParam.Get<string>(executionContext);
            int intPar = IntParam.Get<int>(executionContext);
            EntityReference company = Account.Get<EntityReference>(executionContext);

            Entity updatedEntity = new Entity(context.PrimaryEntityName)
            {
                Id = context.PrimaryEntityId
            };
            updatedEntity["new_text"] = textPar;
            updatedEntity["new_noun"] = intPar;

            if (company == null)
            {
                OutParam.Set(executionContext, "Record doesn't have account");
            }
            else
            {
                Entity account = service.Retrieve(company.LogicalName, company.Id, new ColumnSet("emailaddress1"));
                if (account.Attributes.ContainsKey("emailaddress1"))
                {
                    OutParam.Set(executionContext, account.GetAttributeValue<string>("emailaddress1"));
                }
                else
                {
                    OutParam.Set(executionContext, "Record's account doesn't have Email");
                }
            }

            service.Update(updatedEntity);
        }
    }
}
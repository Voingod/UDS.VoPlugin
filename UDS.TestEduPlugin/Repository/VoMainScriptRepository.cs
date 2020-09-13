using CrmEarlyBound;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace UDS.VoPlugin.Repository
{
    public class VoMainScriptRepository
    {
        private IOrganizationService _service;


        private const string FirstEntityName = "new_vo_main_script";
        private const string SecondEntityName = "new_vo_two_main";
        private const string LinkEntityName = "new_new_vo_two_main_new_vo_main_script";
        public VoMainScriptRepository(IOrganizationService service)
        {
            _service = service;
        }
        public DataCollection<Entity> GetEntities()
        {
            var query = new QueryExpression(FirstEntityName)
            {
                ColumnSet = new ColumnSet("new_name", "createdon", "new_account", "statecode"),
                Criteria = new FilterExpression()
                {
                    Conditions =
                  {
                      new ConditionExpression("createdon",ConditionOperator.OlderThanXDays,2)
                  }
                },
                LinkEntities =
                {
                  new LinkEntity(FirstEntityName, LinkEntityName, "new_vo_main_scriptid", "new_vo_main_scriptid", JoinOperator.Inner)
                  {
                      LinkEntities =
                      {
                        new LinkEntity(LinkEntityName, SecondEntityName, "new_vo_two_mainid", "new_vo_two_mainid", JoinOperator.Inner)
                        {
                            Columns = new ColumnSet("new_name", "createdon", "new_account"),
                        }
                      }
                  }
                }
            };
            var records = _service.RetrieveMultiple(query);
            return records.Entities;
        }


        public IQueryable<CustomEntity> EarlyBoundGetEntities(CrmServiceContext context)
        {
            var entities = (
                from vo_script in context.new_vo_main_scriptSet
                join vo_scriptTovo_two in context.new_new_vo_two_main_new_vo_main_scriptSet
                on vo_script.new_vo_main_scriptId equals vo_scriptTovo_two.new_vo_main_scriptid
                join vo_two in context.new_vo_two_mainSet
                on vo_scriptTovo_two.new_vo_two_mainid equals vo_two.new_vo_two_mainId
                //where DateTime.Now.Subtract(vo_script.CreatedOn.Value).Days > 2
                //where vo_script.StateCode.Value == 0
                select new CustomEntity
                {
                    VoSriptState = (int)vo_script.StateCode.Value,
                    VoScriptData = vo_script.CreatedOn,
                    VoScriptId = vo_script.new_vo_main_scriptId,
                    VoScriptAcc = vo_script.new_account,
                    VoScriptName = vo_script.new_name,
                    VoTwoId = vo_two.new_vo_two_mainId,
                    VoTwoAcc = vo_two.new_account,
                    VoTwoIdName = vo_two.new_name,
                });
            return entities;
        }

    }
    public class CustomEntity
    {
        public int VoSriptState { get; set; }
        public DateTime? VoScriptData { get; set; }
        public Guid? VoScriptId { get; set; }
        public EntityReference VoScriptAcc { get; set; }
        public string VoScriptName { get; set; }
        public Guid? VoTwoId { get; set; }
        public EntityReference VoTwoAcc { get; set; }
        public string VoTwoIdName { get; set; }
    }
}

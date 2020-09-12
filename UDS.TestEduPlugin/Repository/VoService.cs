﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDS.VoPlugin.Repository
{
    public class VoService
    {
        private IOrganizationService _service;
        private const string FirstEntityName = "new_vo_main_script";
        private const string SecondEntityName = "new_vo_two_main";
        private const string LinkEntityName = "new_new_vo_two_main_new_vo_main_script";
        public VoService(IOrganizationService service)
        {
            _service = service;
        }

        public DataCollection<Entity> GetLinkEntities()
        {
            var query = new QueryExpression(FirstEntityName)
            {
                ColumnSet = new ColumnSet("new_name", "createdon", "new_account"),
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
            var a = records.Entities.GroupBy(g => g.Id);
            return records.Entities;
        }
        public static IEnumerable<object> GetValues<T>(IEnumerable<T> items, string propertyName)
        {
            Type type = typeof(T);
            var prop = type.GetProperty(propertyName);
            foreach (var item in items)
                yield return prop.GetValue(item, null);
        }
    }


    
}

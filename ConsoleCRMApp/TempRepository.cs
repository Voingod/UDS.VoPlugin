using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleCRMApp
{
    public class TempRepository
    {
        private IOrganizationService _service;
        private const string EntityName = "new_vo_main_script";
        public TempRepository(IOrganizationService service)
        {
            _service = service;
        }

        public EntityCollection GetVoMainScriptRecors()
        {
            QueryExpression qe = new QueryExpression(EntityName)
            {
                ColumnSet = new ColumnSet("new_name"),

            };
            EntityCollection records = _service.RetrieveMultiple(qe);
            if (records.Entities.Count > 0)
            {
                return records;
            }
            return null;
        }
    }
}


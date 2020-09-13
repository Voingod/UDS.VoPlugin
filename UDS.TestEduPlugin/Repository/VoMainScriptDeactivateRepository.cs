using CrmEarlyBound;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDS.VoPlugin.Repository
{
    public class VoMainScriptDeactivateRepository
    {
        private IOrganizationService _service;
        private CrmServiceContext _context;
        public VoMainScriptDeactivateRepository(IOrganizationService service)
        {
            _service = service;
            _context = new CrmServiceContext(_service);
        }
        public List<new_vo_child_script_first> EarlyBoundGetEntities(Guid recordId)
        {
            var entities = (
                from vo_script in _context.new_vo_main_scriptSet
                join vo_child in _context.new_vo_child_script_firstSet
                on vo_script.new_vo_main_scriptId equals vo_child
                .new_new_vo_main_script_new_vo_child_script_first_lookup_to_vo_script_f
                .new_vo_main_scriptId
                where vo_script.new_vo_main_scriptId == recordId
                select vo_child).ToList();
            return entities;
        }
    }
}

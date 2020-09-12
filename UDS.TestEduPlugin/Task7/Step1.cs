using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDS.VoPlugin.Task7
{
    public class Step1 : Plugin
    {
        public Step1() : base(typeof(Step1))
        {
            base.RegisteredEvents
                .Add(new Tuple<int, string, string, Action<LocalPluginContext>>((int)PluginStage.PreOperation, "Create",
                    "new_vo_main_script",
                    RecordingNumberToStringField));
        }
        protected void RecordingNumberToStringField(LocalPluginContext localContext)
        {
            if (!localContext.PluginExecutionContext.InputParameters.Contains("Target"))
            {
                return;
            }
            Entity target = (Entity)localContext.PluginExecutionContext.InputParameters["Target"];
            Random random = new Random();
            int rnd = random.Next(-100, 100);
            target["new_simplestring"] = rnd.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.JSLHelpers
{
    internal class MiscQueryHandler(AppConfigManager appConfigManager)
    {
        public List<Result> HandleQuery(IEnumerable<string> query)
        {
            string modeQuery = query.FirstOrDefault("");

            if (string.Equals(modeQuery, "reload", StringComparison.CurrentCultureIgnoreCase))
            {
                return [
                    new Result()
                    {
                        QueryTextDisplay = "Reload",
                        Title = "Reload config project",
                        SubTitle = $"{appConfigManager.Config.ToolConfigFile}",
                        Action = _ => {
                            appConfigManager.HandleConfigFile();
                            return true;
                        }
                    }
                ];
            }

            return [];
            
        }
    }
}

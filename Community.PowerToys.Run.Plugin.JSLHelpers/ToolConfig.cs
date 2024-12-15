using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Community.PowerToys.Run.Plugin.JSLHelpers
{
    enum JSLTools
    {
        None,
        MT,
        MRCCT,
        HMI
    }

    internal class ToolConfig
    {
        internal static Dictionary<JSLTools, ToolConfig> tools = new();

        internal required JSLTools id;
        internal required string shortName;
        internal required string name;
        internal required bool useHttps;
        internal required int port;
        internal required string exePath;
    }
}

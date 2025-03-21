﻿using Community.PowerToys.Run.Plugin.JSLHelpers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wox.Plugin;

namespace Community.PowerToys.Run.Plugin.JSLHelpers.QueryHandler
{
    internal interface BaseQueryHandler
    {
        public List<Result>? HandleQuery(IEnumerable<string> query, AppConfig config);
        public List<ContextMenuResult>? LoadContextMenus(Result selectedResult, AppConfig config, string pluginName);

        /// <summary>
        /// Get all available querys of the handler
        /// </summary>
        /// <returns>List of queriess</returns>
        public List<Result> GetQueryOptions();
    }
}

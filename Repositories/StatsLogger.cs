using System;
using System.Collections;
using System.Collections.Generic;
using Opendata.Recalls.Commands;
using Opendata.Recalls.Stats;

namespace Opendata.Recalls.Repository
{
    public class StatsLogger : IStatsLogger
    {
        public void Log(BaseStats stats)
        {
            IDictionary dstore = new Dictionary<string, BaseStats>();
            dstore.Add(Guid.NewGuid().ToString(),stats);
        }
    }

}
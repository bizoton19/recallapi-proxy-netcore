using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Opendata.Recalls.Models;
using Opendata.Recalls.Repository;
using Opendata.Recalls.Stats;

namespace Opendata.Recalls.Controllers
{
    [Route("api/[controller]")]
    public class StatsController : Controller
    {
        private readonly IStatsLogger _statsLogger;

        public StatsController(IStatsLogger statsLogger) => _statsLogger = statsLogger;

        // POST api/stats
        [HttpPost]
        [Route("installnfo")]
        public HttpResponseMessage Post([FromBody]InstallInfo value)
        {

            _statsLogger.Log(value);
            var msg = new HttpResponseMessage();
            msg.Headers.Add("installLog", "success");
            return msg;
        }


        [HttpPost]
        [Route("error")]
        public HttpResponseMessage Post([FromBody]ErrorInfo value)
        {
            _statsLogger.Log(value);
            var msg = new HttpResponseMessage();
            msg.Headers.Add("errorLog", "success");
            return msg;
        }

    }
}

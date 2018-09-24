using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Opendata.Recalls.Commands;
using Opendata.Recalls.Models;
using Opendata.Recalls.Repository;
using Microsoft.Extensions.Logging;

namespace Opendata.Recalls.Controllers
{
    [Route("api/[controller]")]
    public class RecallController : Controller
    {
        private readonly IRecallApiProxyRepository _recallRepository;
        private readonly ILogger _logger;

        public RecallController(IRecallApiProxyRepository recallRepository,ILogger logger)
        {
            _recallRepository = recallRepository;
            _logger = logger;
        }
        // GET api/values
        [HttpGet]
        public async Task<HttpResponseMessage> Get(HttpRequestMessage req)
        {
            var recalls = await _recallRepository.RetrieveRecall(
               new SearchCommand(){
                   RecallDateStart=DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd")
               }
            );
            HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Headers.Add("count",recalls.Count.ToString());
            return resp;

        }

        // POST api/recall
        [HttpPost]
        public async Task<IEnumerable<Recall>> Post([FromBody]SearchCommand value) => await _recallRepository.RetrieveRecall(value);

    }
}

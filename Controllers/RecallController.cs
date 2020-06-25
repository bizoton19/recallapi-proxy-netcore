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

        public RecallController(IRecallApiProxyRepository recallRepository,ILogger<RecallController> logger)
        {
            _recallRepository = recallRepository;
           _logger = logger;
        }
        // GET api/values
        [HttpGet]
        [Route("/recall/latest")]
        public async Task<IActionResult> Get(HttpRequestMessage req)
        {
            var recalls = await _recallRepository.RetrieveRecall(
              new SearchCommand(){
                   Data = new SearchCommand.Params(){
                      RecallDateStart =DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd")
                   }
               }
            );
            var limit = 7;
            HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Headers.Add("count",recalls.Count.ToString());
            
            return Ok(new {
                resultCount = limit,
                recalls = recalls.Take(limit).ToList()
            });

        }
         [HttpGet]
        [Route("/recall/categories")]
        public async Task<IActionResult> Get()
        {
            var recalls = await _recallRepository.RetrieveRecall(
               new SearchCommand(){
                   Data = new SearchCommand.Params(){
                      RecallDateStart =DateTime.Now.AddMonths(-6).ToString("yyyy-MM-dd")
                   }
               }
            );
            var limit = 7;
            HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Headers.Add("count",recalls.Count.ToString());
            
            return Ok(new {
                resultCount = limit,
                recalls = recalls.Take(limit).ToList()
            });

        }

        // POST api/recall
        [HttpPost]
        [Route("/recall")]
        public async Task<IActionResult> Post([FromBody] SearchCommand cmd) {
           
        if (!ModelState.IsValid)
          return BadRequest(cmd);
         
          var results = await _recallRepository.RetrieveRecall(cmd);
          if(results==null){
              return  BadRequest("Results is null");
          }
          return  Ok( new {
                resultCount = results.Count,
                recalls = results
            });
        } 

    }
}

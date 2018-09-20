using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Opendata.Recalls.Models;
using Opendata.Recalls.Repository;
namespace Opendata.Recalls.Controllers
{
    [Route("api/[controller]")]
    public class RecallController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            //var repo = new RecallRepository();
            //return await repo.RetrieveRecall(
              // "atv",
              // "","","","","",""
            //);
            return new string[]{"test", "test2"};
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

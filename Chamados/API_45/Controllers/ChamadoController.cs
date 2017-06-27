using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API_45.Controllers
{
    public class ChamadoController : ApiController
    {
        // GET: api/Chamado
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Chamado/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Chamado
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Chamado/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Chamado/5
        public void Delete(int id)
        {
        }
    }
}

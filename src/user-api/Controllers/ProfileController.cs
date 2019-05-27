using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ChitChatAPI.UserAPI.ViewModel;

namespace ChitChatAPI.UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IConfiguration configuration;

        public ProfileController(IConfiguration configuration) {
            this.configuration = configuration;
        }

        // GET: api/Profile
        [HttpGet]
        public ActionResult<UserProfileViewModel> Get()
        {
            var conn = this.configuration.GetConnectionString("PostgresLocal");
            return new UserProfileViewModel("John", "Doe");
        }

        // GET: api/Profile/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST: api/Profile
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Profile/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

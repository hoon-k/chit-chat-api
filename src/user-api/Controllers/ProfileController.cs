using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ChitChatAPI.UserAPI.ViewModel;

using Dapper;
using Npgsql;

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
        [HttpGet("{uuid}")]
        public async Task<ActionResult<UserProfileViewModel>> Profile(string uuid)
        {
            using (var connection = new NpgsqlConnection(this.configuration["ConnectionString"]))
            {
                var sql = $"SELECT * FROM users WHERE uuid='{uuid}'";
                var result = await connection.QueryAsync<UserProfileViewModel>(sql);
                return result.First();
            }
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

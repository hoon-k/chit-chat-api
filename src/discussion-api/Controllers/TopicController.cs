using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;
using Npgsql;

namespace ChitChatAPI.DiscussionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly IConfiguration config;

        public TopicController(IConfiguration configuration) {
            this.config = configuration;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<object>>> GetAllTopics(int topicId)
        {
            using (var connection = new NpgsqlConnection(this.config["ConnectionString"]))
            {
                var sql = $"SELECT * FROM get_all_topics()";
                var result = await connection.QueryAsync<object>(sql);
                return Ok(result.ToList());
            }
        }

        // POST: api/Topic
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Topic/5
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

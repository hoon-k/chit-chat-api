using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Dapper;
using Npgsql;
using ChitChatAPI.DiscussionAPI.DataModel;

namespace ChitChatAPI.DiscussionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IConfiguration config;

        public PostController(IConfiguration configuration) {
            this.config = configuration;
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<object>> Create([FromBody] PostRequest reqObj)
        {
            using (var connection = new NpgsqlConnection(this.config["ConnectionString"]))
            {
                var sql = $"CALL create_post('{reqObj.Body}', '{reqObj.AuthorID}', '{reqObj.TopicID}')";
                if (String.IsNullOrEmpty(reqObj.TopicID))
                {
                    sql = $"CALL create_starter_post('{reqObj.Body}', '{reqObj.AuthorID}', '{reqObj.TopicTitle}')";
                }

                return Ok(await connection.ExecuteAsync(sql));
            }
        }

        [HttpGet()]
        [Route("all/for/{topicId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetAll(string topicId)
        {
            using (var connection = new NpgsqlConnection(this.config["ConnectionString"]))
            {
                var sql = $"SELECT * FROM get_posts('{topicId}')";

                var result = await connection.QueryAsync<object>(sql);
                return Ok(result.ToList());
            }
        }

        [HttpGet()]
        [Route("{postId}")]
        public async Task<ActionResult<object>> GetSinglePost(string topicId)
        {
            using (var connection = new NpgsqlConnection(this.config["ConnectionString"]))
            {
                var sql = $"SELECT * FROM get_posts('{topicId}')";

                var result = await connection.QueryAsync<object>(sql);
                return Ok(result.FirstOrDefault());
            }
        }

        [HttpDelete]
        [Route("{postId}/topic/{topicId}")]
        public async Task<ActionResult<object>> DeleteSinglePost(string postId, string topicId)
        {
            using (var connection = new NpgsqlConnection(this.config["ConnectionString"]))
            {
                var sql = $"CALL delete_post({topicId}, {postId})";
                return Ok(await connection.ExecuteAsync(sql));
            }
        }

        // Keep as examples
        // // GET: api/Post
        // [HttpGet("{id}")]
        // public IEnumerable<string> Posts(string id)
        // {
        //     return new string[] { "value1", "value2" };
        // }

        // // POST: api/Post
        // [HttpPost]
        // public void Post([FromBody] string value)
        // {
        // }

        // // PUT: api/Post/5
        // [HttpPut("{id}")]
        // public void Put(int id, [FromBody] string value)
        // {
        // }

        // // DELETE: api/ApiWithActions/5
        // [HttpDelete("{id}")]
        // public void Delete(int id)
        // {
        // }
    }
}

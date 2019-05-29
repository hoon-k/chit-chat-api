using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ChitChatAPI.UserAPI.DataModel;
using Dapper;
using Npgsql;

namespace ChitChatAPI.UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration config;

        public AccountController(IConfiguration configuration) {
            this.config = configuration;
        }

        // GET: api/Account
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<Object>> Create([FromBody] CreateAccountRequest reqObj) {
            using (var connection = new NpgsqlConnection(this.config["ConnectionString"]))
            {
                connection.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandText = $"CALL create_member_user('{reqObj.Username}', '{reqObj.Password}', '{reqObj.FirstName}', '{reqObj.LastName}')";

                    try
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch (System.Data.Common.DbException e)
                    {
                        // Bad
                        return e.Message;
                    }
                }
            }

            return reqObj;
        }

        // GET: api/Account/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST: api/Account
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Account/5
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

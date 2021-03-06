using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ChitChatAPI.UserAPI.DataModel;
using ChitChatAPI.UserAPI.IntegrationsEvents.Events;
using ChitChatAPI.Common.Event;
using Dapper;
using Npgsql;

namespace ChitChatAPI.UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration config;
        private readonly IIntegrationEventService integrationEventService;

        public AccountController(IConfiguration configuration, IIntegrationEventService integrationEventService) {
            this.config = configuration;
            this.integrationEventService = integrationEventService;
        }

        // GET: api/Account
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<object>> Create([FromBody] CreateAccountRequest reqObj) {
            using (var connection = new NpgsqlConnection(this.config["ConnectionString"]))
            {
                var sql = $"CALL create_member_user('{reqObj.Username}', '{reqObj.Password}', '{reqObj.FirstName}', '{reqObj.LastName}')";
                var result = await connection.ExecuteAsync(sql);

                // TODO: Find out why the following is throwing error.
                // var queryParameters = new DynamicParameters();
                // queryParameters.Add("@un", reqObj.Username, dbType: DbType.AnsiString);
                // queryParameters.Add("@pw", reqObj.Password, dbType: DbType.AnsiString);
                // queryParameters.Add("@fn", reqObj.FirstName, dbType: DbType.AnsiString);
                // queryParameters.Add("@ln", reqObj.LastName, dbType: DbType.AnsiString);

                // return await connection.ExecuteAsync(
                //     "create_member_user",
                //     queryParameters,
                //     commandType: CommandType.StoredProcedure);

                var evt = new NewUserCreatedEvent(reqObj.FirstName, reqObj.LastName, "some_uuid", reqObj.Username, "member");
                this.integrationEventService.PublishThroughEventBus(evt);

                return result;
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginRequest reqObj) {
            using (var connection = new NpgsqlConnection(this.config["ConnectionString"]))
            {
                var sql = $"SELECT * FROM authenticate_user('{reqObj.Username}', '{reqObj.Password}')";
                var result = await connection.QueryAsync<object>(sql);
                return result.First();
            }
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

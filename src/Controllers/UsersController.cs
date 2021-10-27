using System;
using Microsoft.AspNetCore.Mvc;

namespace ToDoApp.Controllers
{
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[Controller]")]
    public class UserController
    {
        [HttpGet]
        public void Get()
        {
            throw new NotImplementedException();
        }
    }
}
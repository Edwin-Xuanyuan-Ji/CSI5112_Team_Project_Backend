using CSI5112BackEndApi.Models;
using CSI5112BackEndApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
[Authorize] // can be added to class
// Controller for testing token validation
[Route("api/[controller]")]
    public class TestController : Controller
    {
        // GET api/values
        [HttpGet]
        // [Authorize] // can be added to methods
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
         // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
}
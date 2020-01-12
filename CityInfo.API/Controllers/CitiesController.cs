using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CityInfo.API.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        //[HttpGet("api/cities")]
        [HttpGet()]
        public JsonResult GetCities()
        {
            return new JsonResult(new List<object>()
            {
                new { id=1,Name="Colombo"},
                new { id=2,Name="Kurunegala"}
            });
        }
    }
}

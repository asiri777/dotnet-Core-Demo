using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CityInfo.API.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        //[HttpGet("api/cities")]
        //[HttpGet()]
        //public JsonResult GetCities()
        //{
        //    return new JsonResult(new List<object>()
        //    {
        //        new { id=1,Name="Colombo"},
        //        new { id=2,Name="Kurunegala"}
        //    });
        //}

        //[HttpGet()]
        //public JsonResult GetCities()
        //{
        //    return new JsonResult(CitiesDataStore.Current.Cities);
        //}

        //[HttpGet("{id}")]
        //public JsonResult GetCity(int id)
        //{
        //    return new JsonResult(CitiesDataStore.Current.Cities.Find(x => x.Id == id));
        //}

        //[HttpGet()]
        //public JsonResult GetCities()
        //{
        //    var temp = new JsonResult(CitiesDataStore.Current.Cities);
        //    temp.StatusCode = 200;
        //    return temp;
        //}

        [HttpGet()]
        public IActionResult GetCities()
        {
            return Ok(CitiesDataStore.Current.Cities);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            // Find City
            var cityToReturn = CitiesDataStore.Current.Cities.Find(x => x.Id == id);
            if(cityToReturn == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(cityToReturn);
            }
        }
    }
}

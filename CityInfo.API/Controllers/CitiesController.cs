using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CityInfo.API.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository;
            _mapper = mapper;
        }

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
            //return Ok(CitiesDataStore.Current.Cities);

            var cityEntities = _cityInfoRepository.GetCities();
            //var results = new List<CityWithoutPointsOfInterestDto>();
            //foreach (var cityEntity in cityEntities)
            //{
            //    results.Add(new CityWithoutPointsOfInterestDto()
            //    {
            //        Id = cityEntity.Id,
            //        Description = cityEntity.Description,
            //        Name = cityEntity.Name
            //    });
            //}
            var results = _mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities);
            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool incluedPointsOfInterest = false)
        {
            //// Find City
            //var cityToReturn = CitiesDataStore.Current.Cities.Find(x => x.Id == id);
            //if(cityToReturn == null)
            //{
            //    return NotFound();
            //}
            //else
            //{
            //    return Ok(cityToReturn);
            //}

            var city = _cityInfoRepository.GetCity(id, incluedPointsOfInterest);
            if (city == null)
            {
                return NotFound();
            }
            if (incluedPointsOfInterest)
            {
                //var cityResult = new CityDto()
                //{
                //    Id = city.Id,
                //    Name = city.Name,
                //    Description = city.Description
                //};

                //foreach (var poi in city.PointsOfInterest)
                //{
                //    cityResult.PointsOfInterest.Add(
                //        new PointOfInterestDto()
                //        {
                //            Id = poi.Id,
                //            Name = poi.Name,
                //            Description = poi.Description
                //        });
                //}
                var cityResult = _mapper.Map<CityDto>(city);
                return Ok(cityResult);
            }
            //var cityWithoutPointsOfInterestResult =
            //    new CityWithoutPointsOfInterestDto()
            //    {
            //        Id = city.Id,
            //        Name = city.Name,
            //        Description = city.Description
            //    };
            var cityWithoutPointsOfInterestResult = _mapper.Map<CityWithoutPointsOfInterestDto>(city);
            return Ok(cityWithoutPointsOfInterestResult);
        }
    }
}

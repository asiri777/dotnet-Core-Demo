﻿using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {

        private ILogger<PointsOfInterestController> _logger;
        private readonly IMapper _mapper;
        private IMailService _mailService;
        private ICityInfoRepository _cityInfoRepository;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger,
            IMailService mailService,
            ICityInfoRepository cityInfoRepository,
            IMapper mapper)
        {
            _logger = logger;
            _mailService = mailService;
            _cityInfoRepository = cityInfoRepository;
            _mapper = mapper;
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                //var city = CitiesDataStore.Current.Cities.Find(c => c.Id == cityId);
                //if (city == null)
                //{
                //    _logger.LogInformation($"City with id {cityId} wasn't found");
                //    return NotFound();
                //}
                //return Ok(city.PointsOfInterest);
                if (!_cityInfoRepository.CityExists(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                    return NotFound();
                }
                var pointsOfInterestForCity = _cityInfoRepository.GetPointsOfInterestForCity(cityId);
                //var pointsOfInterestForCityResults = new List<PointOfInterestDto>();
                //foreach(var poi in pointsOfInterestForCity)
                //{
                //    pointsOfInterestForCityResults.Add(new PointOfInterestDto()
                //    {
                //        Id = poi.Id,
                //        Name = poi.Name,
                //        Description = poi.Description
                //    });
                //}
                var pointsOfInterestForCityResults = _mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity);
                return Ok(pointsOfInterestForCityResults);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}.", ex);
                return StatusCode(500, "A problem happened while handling your request");
            }
            
        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            //var city = CitiesDataStore.Current.Cities.Find(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            //var pointsOfInterest = city.PointsOfInterest.Find(p => p.Id == id);
            //if (pointsOfInterest == null)
            //{
            //    return NotFound();
            //}
            //return Ok(pointsOfInterest);

            if (!_cityInfoRepository.CityExists(cityId))
            {
                _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                return NotFound();
            }
            var pointOfInterest = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if(pointOfInterest == null)
            {
                return NotFound();
            }
            //var pointOfInterestResult = new PointOfInterestDto()
            //{
            //    Id = pointOfInterest.Id,
            //    Name = pointOfInterest.Name,
            //    Description = pointOfInterest.Description
            //};
            var pointOfInterestResult = _mapper.Map<PointOfInterestDto>(pointOfInterest);
            return Ok(pointOfInterestResult);
        }

        [HttpPost("{cityId}/pointsOfInterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            //if (pointOfInterest == null)
            //{
            //    return BadRequest();
            //}

            //if (pointOfInterest.Description == pointOfInterest.Name)
            //{
            //    ModelState.AddModelError("Description", "The provided description should be different from the name");
            //}

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            //var city = CitiesDataStore.Current.Cities.Find(c => c.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            //// Demo purposes - to be improved
            //var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

            //var finalPointOfInterest = new PointOfInterestDto()
            //{
            //    Id = ++maxPointOfInterestId,
            //    Name = pointOfInterest.Name,
            //    Description = pointOfInterest.Description
            //};
            //city.PointsOfInterest.Add(finalPointOfInterest);

            //return CreatedAtRoute("GetPointOfInterest", new
            //{ cityId = cityId, id = finalPointOfInterest.Id }, finalPointOfInterest);

            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_cityInfoRepository.CityExists(cityId))
            {
                _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                return NotFound();
            }
            var finalPointOfInterest = _mapper.Map<PointOfInterest>(pointOfInterest);
            _cityInfoRepository.AddPointOfInterestForCity(cityId, finalPointOfInterest);

            if (!_cityInfoRepository.Save())
            {
                return StatusCode(500, "A problem happend while handling your request");
            }
            var createdPointOfInterestToReturn = _mapper.Map<PointOfInterestDto>(finalPointOfInterest);
            return CreatedAtRoute("GetPointOfInterest", new
            { cityId = cityId, id = createdPointOfInterestToReturn.Id }, createdPointOfInterestToReturn);
        }

        [HttpPut("{cityId}/pointsOfInterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id,
            [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var city = CitiesDataStore.Current.Cities.Find(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if(pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;

            return NoContent();
        }

        [HttpPatch("{cityId}/pointsOfInterest/{id}")]
        public IActionResult PatiallyUpdatePointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            if(patchDocument == null)
            {
                return NotFound();
            }

            var city = CitiesDataStore.Current.Cities.Find(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
            {
                Name = pointOfInterestFromStore.Name,
                Description = pointOfInterestFromStore.Description
            };

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different form the name");
            }

            TryValidateModel(pointOfInterestToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent();

        }

        [HttpDelete("{cityId}/pointsOfInterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities.Find(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(pointOfInterestFromStore);

            _mailService.Send("Point of Interest deleted",
                $"Point of Interest {pointOfInterestFromStore.Name} with id {pointOfInterestFromStore.Id} was deleted.");

            return NoContent();
        }

    }
}

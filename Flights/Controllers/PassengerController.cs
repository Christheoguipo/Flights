using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Flights.Dtos;
using Flights.ReadModels;
using System.ComponentModel.DataAnnotations;
using Flights.Domain.Entities;
using Flights.Data;

namespace Flights.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class PassengerController : ControllerBase
    {
        private readonly Entities _entities;

        public PassengerController(Entities entities)
        {
            _entities = entities;
        }

        [HttpPost]//for saving or updating to db
        [ProducesResponseType(201)]//new created
        [ProducesResponseType(400)]//error on client side
        [ProducesResponseType(500)]//error on server side
        public IActionResult Register(NewPassengerDto dto) 
        { 
            _entities.Passengers.Add(new Passenger(
                dto.Email,
                dto.FirstName,
                dto.LastName,
                dto.Gender
                ));

            _entities.SaveChanges();

            return CreatedAtAction(nameof( Find), new { email = dto.Email});

        }

        [HttpGet("{email}")]
        public ActionResult<PassengerRm> Find(string email)
        { 
            var passenger = _entities.Passengers.SingleOrDefault(p => p.Email == email);

            if(passenger == null)
                return NotFound();

            var rm = new PassengerRm(
                passenger.Email,
                passenger.FirstName,
                passenger.LastName,
                passenger.Gender
                );

            return Ok(rm);
        }

    }
}

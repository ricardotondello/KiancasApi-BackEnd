﻿using KiancaAPI.Models;
using KiancaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace KiancaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : Controller
    {
        private readonly PersonService _personService;

        public PersonsController(PersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        public ActionResult<List<Person>> Get() =>
            _personService.Get();

        [HttpGet("{id:length(24)}", Name = "GetPerson")]
        public ActionResult<Person> Get(string id)
        {
            var person = _personService.Get(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        [HttpPost]
        public ActionResult<Person> Create(Person person)
        {
            _personService.Create(person);

            return CreatedAtRoute("GetPerson", new { id = person.Id.ToString() }, person);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Person personIn)
        {
            var person = _personService.Get(id);

            if (person == null)
            {
                return NotFound();
            }

            _personService.Update(id, personIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var person = _personService.Get(id);

            if (person == null)
            {
                return NotFound();
            }

            _personService.Remove(person.Id);

            return NoContent();
        }
    }
}
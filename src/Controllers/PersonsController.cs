using KiancaAPI.Models;
using KiancaAPI.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KiancaAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonController : Controller
    {
        //private readonly PersonService _personService;
        private readonly IPersonRepository _repo;

        public PersonController(IPersonRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Person>>> GetAll()
        {
            return new ObjectResult(await _repo.Get());
        }

        [HttpGet("{id:length(24)}", Name = "GetPerson")]
        public async Task<ActionResult<Person>> Get(string id)
        {
            var person = await _repo.Get(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        [HttpPost]
        public async Task<ActionResult<Person>> Post(Person person)
        {
            await _repo.Create(person);

            return CreatedAtRoute("GetPerson", new { id = person.Id }, person);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Put(string id, Person personIn)
        {
            var person = await _repo.Get(id);

            if (person == null)
            {
                return NotFound();
            }

            await _repo.Update(personIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var person = await _repo.Get(id);

            if (person == null)
            {
                return NotFound();
            }

            await _repo.Delete(person.Id);

            return NoContent();
        }
    }
}
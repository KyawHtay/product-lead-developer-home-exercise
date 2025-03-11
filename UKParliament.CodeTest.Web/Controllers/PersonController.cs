using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.ViewModels;

namespace UKParliament.CodeTest.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _personService;
        private readonly IMapper _mapper;

        public PersonController(IPersonService personService, IMapper mapper)
        {
            _personService = personService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PersonViewModel>>> GetAll()
        {
            var persons = await _personService.GetAllAsync();
            var personViewModels = _mapper.Map<IEnumerable<PersonViewModel>>(persons);
            return Ok(personViewModels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonViewModel>> GetById(int id)
        {
            var person = await _personService.GetByIdAsync(id);
            if (person == null)
            {
                return NotFound();
            }
            var personViewModel = _mapper.Map<PersonViewModel>(person);
            return Ok(personViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PersonViewModel personViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var person = _mapper.Map<Person>(personViewModel);
            await _personService.AddAsync(person);
            var createdPersonViewModel = _mapper.Map<PersonViewModel>(person);

            return CreatedAtAction(nameof(GetById), new { id = person.Id }, createdPersonViewModel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PersonViewModel personViewModel)
        {
            if (id != personViewModel.Id)
            {
                return BadRequest();
            }

            var existingPerson = await _personService.GetByIdAsync(id);
            if (existingPerson == null)
            {
                return NotFound();
            }

            _mapper.Map(personViewModel, existingPerson);
            await _personService.UpdateAsync(existingPerson);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingPerson = await _personService.GetByIdAsync(id);
            if (existingPerson == null)
            {
                return NotFound();
            }

            await _personService.DeleteAsync(id);
            return NoContent();
        }
    }
}

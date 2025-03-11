using Microsoft.AspNetCore.Mvc;
using Moq;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Controllers;
using UKParliament.CodeTest.Web.ViewModels;
using Xunit;
using FluentAssertions;

namespace UKParliament.CodeTest.Tests
{
    public class PersonControllerTests
    {
        private readonly Mock<IPersonService> _mockService;
        private readonly Mock<IMapper> _mockMapper;
        private readonly PersonController _controller;

        public PersonControllerTests()
        {
            _mockService = new Mock<IPersonService>();
            _mockMapper = new Mock<IMapper>();

            _controller = new PersonController(_mockService.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithListOfPersons()
        {
            var persons = new List<Person>
            {
                new Person { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", DepartmentId = 1 },
                new Person { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane@example.com", DepartmentId = 2 }
            };

            var personViewModels = new List<PersonViewModel>
            {
                new PersonViewModel { Id=1, FirstName = "John", LastName = "Doe", Email = "john@example.com", 
                DateOfBirth = DateOnly.FromDateTime(new DateTime(1990, 1, 1)) },
               new PersonViewModel { Id=1, FirstName = "Jane", LastName = "Jane", Email = "jane@example.com", 
                DateOfBirth = DateOnly.FromDateTime(new DateTime(1990, 1, 1)) }
            };

            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(persons);
            _mockMapper.Setup(m => m.Map<IEnumerable<PersonViewModel>>(persons)).Returns(personViewModels);

            var result = await _controller.GetAll();

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var returnedPersons = okResult.Value as List<PersonViewModel>;
            returnedPersons.Should().NotBeNull();
            returnedPersons.Should().HaveCount(2);
            returnedPersons![0].FirstName.Should().Be("John");
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenPersonExists()
        {
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com",   
                        DateOfBirth = DateOnly.FromDateTime(new DateTime(1990, 1, 1)),DepartmentId = 1 };
            var personViewModel =new PersonViewModel { Id=1, FirstName = "John", LastName = "Doe", Email = "john@example.com", 
                DateOfBirth = DateOnly.FromDateTime(new DateTime(1990, 1, 1)) ,DepartmentId = 2};

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(person);
            _mockMapper.Setup(m => m.Map<PersonViewModel>(person)).Returns(personViewModel);

            var result = await _controller.GetById(1);

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);

            var returnedPerson = okResult.Value as PersonViewModel;
            returnedPerson.Should().NotBeNull();
            returnedPerson!.FirstName.Should().Be("John");
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenPersonDoesNotExist()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((Person)null!);

            var result = await _controller.GetById(99);

            var notFoundResult = result.Result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenValidPerson()
        {
            var model = new PersonViewModel { FirstName = "John", LastName = "Doe", Email = "john@example.com", DateOfBirth = DateOnly.FromDateTime(new DateTime(1990, 1, 1)) };
            var person = new Person { FirstName = "John", LastName = "Doe", Email = "john@example.com", DateOfBirth = DateOnly.FromDateTime(new DateTime(1990, 1, 1)) };

            _mockMapper.Setup(m => m.Map<Person>(model)).Returns(person);
            _mockService.Setup(s => s.AddAsync(person)).Returns(Task.CompletedTask);
            _mockMapper.Setup(m => m.Map<PersonViewModel>(person)).Returns(model);

            var result = await _controller.Create(model);

            var createdAtActionResult = result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult!.StatusCode.Should().Be(201);
            createdAtActionResult.ActionName.Should().Be(nameof(PersonController.GetById));
            createdAtActionResult.RouteValues["id"].Should().Be(person.Id);
            createdAtActionResult.Value.Should().BeEquivalentTo(model);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");

            var model = new PersonViewModel { Id=1, FirstName = "John", LastName = "Doe", Email = "john@example.com", 
                DateOfBirth = DateOnly.FromDateTime(new DateTime(1990, 1, 1)) };
            var result = await _controller.Create(model);

            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenSuccessful()
        {
            var model = new PersonViewModel { Id=1, FirstName = "John", LastName = "Doe", Email = "john@example.com", 
                DateOfBirth = DateOnly.FromDateTime(new DateTime(1990, 1, 1)) };
            var person = new Person { Id = 1, FirstName = "OldName", LastName = "OldLast", Email = "old@example.com" };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(person);
            _mockMapper.Setup(m => m.Map(model, person)).Returns(person); 
            _mockService.Setup(s => s.UpdateAsync(person)).Returns(Task.CompletedTask);

            var result = await _controller.Update(1, model);

            result.Should().BeOfType<NoContentResult>();
            (result as NoContentResult)!.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenNotSuccessful()
        {
            var model = new PersonViewModel { Id=1, FirstName = "John", LastName = "Doe", Email = "john@example.com", 
                    DateOfBirth = DateOnly.FromDateTime(new DateTime(1990, 1, 1)) };
            var person = new Person { Id = 1, FirstName = "OldName", LastName = "OldLast", Email = "old@example.com" ,
                        DateOfBirth = DateOnly.FromDateTime(new DateTime(1990, 1, 1)) };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(person);
            _mockMapper.Setup(m => m.Map(model, person)).Returns(person); 
            _mockService.Setup(s => s.UpdateAsync(person)).Returns(Task.CompletedTask);

            var result = await _controller.Update(1, model);

            result.Should().BeOfType<NoContentResult>();
            (result as NoContentResult)!.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenSuccessful()
        {
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" };

            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(person);
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult!.StatusCode.Should().Be(204);
        }
    }
}

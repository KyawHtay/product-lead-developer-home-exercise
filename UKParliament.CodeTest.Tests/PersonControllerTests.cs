using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Controllers;
using Xunit;
using FluentAssertions;

namespace UKParliament.CodeTest.Tests
{
    public class PersonControllerTests
    {
        private readonly Mock<IPersonService> _mockService;
        private readonly PersonController _controller;

        public PersonControllerTests()
        {
            _mockService = new Mock<IPersonService>();
            _controller = new PersonController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithListOfPersons()
        {
            var persons = new List<Person>
            {
                new Person { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", DateOfBirth = DateOnly.FromDateTime(new System.DateTime(1990, 1, 1)), DepartmentId = 1 },
                new Person { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane@example.com", DateOfBirth = DateOnly.FromDateTime(new System.DateTime(1995, 5, 10)), DepartmentId = 2 }
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(persons);

            var result = await _controller.GetAll();

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(persons);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenPersonExists()
        {
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", DateOfBirth = DateOnly.FromDateTime(new System.DateTime(1990, 1, 1)), DepartmentId = 1 };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(person);

            var result = await _controller.GetById(1);

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(person);
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
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", DateOfBirth = DateOnly.FromDateTime(new System.DateTime(1990, 1, 1)), DepartmentId = 1 };
            _mockService.Setup(s => s.AddAsync(person)).Returns(Task.CompletedTask);

            var result = await _controller.Create(person);

            var createdAtActionResult = result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult!.StatusCode.Should().Be(201);
            createdAtActionResult.ActionName.Should().Be(nameof(PersonController.GetById));
            createdAtActionResult.RouteValues["id"].Should().Be(person.Id);
            createdAtActionResult.Value.Should().BeEquivalentTo(person);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            _controller.ModelState.AddModelError("FirstName", "Required");
            var person = new Person { Id = 1, FirstName = "", LastName = "Doe", Email = "john@example.com", DateOfBirth = DateOnly.FromDateTime(new System.DateTime(1990, 1, 1)), DepartmentId = 1 };

            var result = await _controller.Create(person);

            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenSuccessful()
        {
            var person = new Person { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", DateOfBirth = DateOnly.FromDateTime(new System.DateTime(1990, 1, 1)), DepartmentId = 1 };
            _mockService.Setup(s => s.UpdateAsync(person)).Returns(Task.CompletedTask);

            var result = await _controller.Update(1, person);

            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult!.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenIdMismatch()
        {
            var person = new Person { Id = 2, FirstName = "John", LastName = "Doe", Email = "john@example.com", DateOfBirth = DateOnly.FromDateTime(new System.DateTime(1990, 1, 1)), DepartmentId = 1 };

            var result = await _controller.Update(1, person);

            var badRequestResult = result as BadRequestResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenSuccessful()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult!.StatusCode.Should().Be(204);
        }
    }
}

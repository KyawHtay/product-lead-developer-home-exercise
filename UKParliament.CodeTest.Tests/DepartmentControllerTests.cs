using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;
using UKParliament.CodeTest.Web.Controllers;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace UKParliament.CodeTest.Tests
{
    public class DepartmentControllerTests
    {
        private readonly Mock<IDepartmentService> _mockService;
        private readonly DepartmentController _controller;

        public DepartmentControllerTests()
        {
            _mockService = new Mock<IDepartmentService>();
            _controller = new DepartmentController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WithListOfDepartments()
        {
            var departments = new List<Department>
            {
                new Department { Id = 1, Name = "HR" },
                new Department { Id = 2, Name = "Finance" }
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(departments);

            var result = await _controller.GetAll();

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(departments);
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenDepartmentExists()
        {
            var department = new Department { Id = 1, Name = "HR" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(department);

            var result = await _controller.GetById(1);

            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(department);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenDepartmentDoesNotExist()
        {
            _mockService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((Department)null!);

            var result = await _controller.GetById(99);

            var notFoundResult = result.Result as NotFoundResult;
            notFoundResult.Should().NotBeNull();
            notFoundResult!.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task Create_ShouldReturnCreatedAtAction_WhenValidDepartment()
        {
            var department = new Department { Id = 1, Name = "HR" };
            _mockService.Setup(s => s.AddAsync(department)).Returns(Task.CompletedTask);

            var result = await _controller.Create(department);

            var createdAtActionResult = result as CreatedAtActionResult;
            createdAtActionResult.Should().NotBeNull();
            createdAtActionResult!.StatusCode.Should().Be(201);
            createdAtActionResult.ActionName.Should().Be(nameof(DepartmentController.GetById));
            createdAtActionResult?.RouteValues?["id"].Should().Be(department.Id);
            createdAtActionResult?.Value.Should().BeEquivalentTo(department);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenModelStateIsInvalid()
        {
            _controller.ModelState.AddModelError("Name", "Required");
            var department = new Department { Id = 1, Name = "" };

            var result = await _controller.Create(department);

            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult!.StatusCode.Should().Be(400);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContent_WhenSuccessful()
        {
            var department = new Department { Id = 1, Name = "HR" };
            _mockService.Setup(s => s.UpdateAsync(department)).Returns(Task.CompletedTask);

            var result = await _controller.Update(1, department);

            var noContentResult = result as NoContentResult;
            noContentResult.Should().NotBeNull();
            noContentResult!.StatusCode.Should().Be(204);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequest_WhenIdMismatch()
        {
            var department = new Department { Id = 2, Name = "HR" };

            var result = await _controller.Update(1, department);

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

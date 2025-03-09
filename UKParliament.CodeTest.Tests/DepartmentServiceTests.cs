using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;
using Xunit;

namespace UKParliament.CodeTest.Tests
{
    public class DepartmentServiceTests
    {
        private readonly Mock<IDepartmentRepository> _mockRepo;
        private readonly DepartmentService _service;

        public DepartmentServiceTests()
        {
            _mockRepo = new Mock<IDepartmentRepository>();
            _service = new DepartmentService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllDepartments()
        {
            // Arrange
            var departments = new List<Department>
            {
                new Department { Id = 1, Name = "HR" },
                new Department { Id = 2, Name = "IT" }
            };

            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(departments);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDepartment_WhenExists()
        {
            // Arrange
            var department = new Department { Id = 1, Name = "Finance" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(department);

            // Act
            var result = await _service.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result!.Name.Should().Be("Finance");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetByIdAsync(99)).ReturnsAsync((Department?)null);

            // Act
            var result = await _service.GetByIdAsync(99);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_ShouldCallRepositoryMethod()
        {
            // Arrange
            var newDepartment = new Department { Id = 3, Name = "Sales" };

            // Act
            await _service.AddAsync(newDepartment);

            // Assert
            _mockRepo.Verify(repo => repo.AddAsync(newDepartment), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallRepositoryMethod()
        {
            // Arrange
            var updatedDepartment = new Department { Id = 1, Name = "Marketing" };

            // Act
            await _service.UpdateAsync(updatedDepartment);

            // Assert
            _mockRepo.Verify(repo => repo.UpdateAsync(updatedDepartment), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryMethod()
        {
            // Arrange
            int departmentId = 1;

            // Act
            await _service.DeleteAsync(departmentId);

            // Assert
            _mockRepo.Verify(repo => repo.DeleteAsync(departmentId), Times.Once);
        }
    }
}

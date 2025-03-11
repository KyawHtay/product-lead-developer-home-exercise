using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Xunit;
using FluentAssertions;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;

namespace UKParliament.CodeTest.Tests
{
    public class PersonServiceTests
    {
        private readonly Mock<IPersonRepository> _mockRepo;
        private readonly PersonService _service;

        public PersonServiceTests()
        {
            _mockRepo = new Mock<IPersonRepository>();
            _service = new PersonService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllPersons()
        {
            var mockPeople = new List<Person>
            {
                new Person { Id = 1, FirstName = "Alice", LastName = "Smith", Email = "alice@example.com", DateOfBirth = DateOnly.FromDateTime(new DateTime(1990, 5, 20)), DepartmentId = 1 },
                new Person { Id = 2, FirstName = "Bob", LastName = "Johnson", Email = "bob@example.com", DateOfBirth = DateOnly.FromDateTime(new DateTime(1985, 8, 15)), DepartmentId = 2 }
            };

            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(mockPeople);

            var result = await _service.GetAllAsync();

            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(2);
            result.Should().ContainEquivalentOf(mockPeople[0]);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPerson_WhenPersonExists()
        {
            var mockPerson = new Person
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                DateOfBirth = DateOnly.FromDateTime(new DateTime(1990, 1, 1)),
                DepartmentId = 1
            };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(mockPerson);

            var result = await _service.GetByIdAsync(1);

            result.Should().NotBeNull();
            result.FirstName.Should().Be("John");
            result.Email.Should().Be("john@example.com");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenPersonDoesNotExist()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync(default(Person));

            var result = await _service.GetByIdAsync(99);

            result.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_ShouldCallRepositoryMethod()
        {
            var newPerson = new Person
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane@example.com",
                DateOfBirth = DateOnly.FromDateTime(new DateTime(1995, 3, 10)),
                DepartmentId = 2
            };

            await _service.AddAsync(newPerson);

            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Person>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldCallRepositoryMethod()
        {
            var existingPerson = new Person
            {
                Id = 1,
                FirstName = "UpdatedName",
                LastName = "Doe",
                Email = "updated@example.com",
                DateOfBirth = DateOnly.FromDateTime(new DateTime(1992, 6, 15)),
                DepartmentId = 1
            };

            await _service.UpdateAsync(existingPerson);

            _mockRepo.Verify(r => r.UpdateAsync(existingPerson), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallRepositoryMethod()
        {
            int personId = 1;

            await _service.DeleteAsync(personId);

            _mockRepo.Verify(r => r.DeleteAsync(personId), Times.Once);
        }

         [Fact]
        public async Task GetByEmailAsync_ShouldCallRepositoryMethod()
        {
             var email = "jane@example.com";

            await _service.GetByEmailAsync(email);

            _mockRepo.Verify(r => r.GetByEmailAsync(email), Times.Once);
        }
    }
}

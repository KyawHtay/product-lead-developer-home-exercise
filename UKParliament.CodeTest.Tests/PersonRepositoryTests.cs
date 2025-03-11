using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;
using Xunit;

namespace UKParliament.CodeTest.Tests
{
    public class PersonRepositoryTests
    {
        private readonly PersonManagerContext _context;
        private readonly PersonRepository _repository;

        public PersonRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<PersonManagerContext>()
                .UseInMemoryDatabase(databaseName: $"PersonManager_TestDb_{Guid.NewGuid()}") 
                .Options;

            _context = new PersonManagerContext(options);

            _context.Database.EnsureDeleted();  
            _context.Database.EnsureCreated();

            SeedDatabase(); 
            _repository = new PersonRepository(_context);
        }

        private void SeedDatabase()
        {
       
            _context.People.RemoveRange(_context.People);
            _context.Departments.RemoveRange(_context.Departments);
            _context.SaveChanges();


            var department = new Department { Id = 1, Name = "IT" };
            _context.Departments.Add(department);
            _context.SaveChanges();


            var person1 = new Person { FirstName = "John", LastName = "Doe", Email = "john@example.com", DateOfBirth = DateOnly.FromDateTime(new DateTime(1990, 1, 1)), DepartmentId = 1 };
            var person2 = new Person { FirstName = "Jane", LastName = "Doe", Email = "jane@example.com", DateOfBirth = DateOnly.FromDateTime(new DateTime(1992, 5, 5)), DepartmentId = 1 };

            _context.People.AddRange(person1, person2);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllPersons()
        {
            // Act
            var persons = await _repository.GetAllAsync();

            // Assert
            persons.Should().NotBeNull();
            persons.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPerson_WhenExists()
        {
            // Arrange
            var person = await _repository.GetByEmailAsync("john@example.com");

            // Act
            var foundPerson = await _repository.GetByIdAsync(person.Id);

            // Assert
            foundPerson.Should().NotBeNull();
            foundPerson!.FirstName.Should().Be("John");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            // Act
            var person = await _repository.GetByIdAsync(99); 

            // Assert
            person.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_ShouldAddPersonToDatabase()
        {
            // Arrange
            var newPerson = new Person
            {
                FirstName = "Alice",
                LastName = "Smith",
                Email = "alice@example.com",
                DateOfBirth = DateOnly.FromDateTime(new DateTime(1995, 3, 10)),
                DepartmentId = 1
            };

            // Act
            await _repository.AddAsync(newPerson);
            var person = await _repository.GetByEmailAsync("alice@example.com");

            // Assert
            person.Should().NotBeNull();
            person!.Email.Should().Be("alice@example.com");
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemovePersonFromDatabase()
        {
            // Arrange
            var person = await _repository.GetByEmailAsync("john@example.com");

            // Act
            await _repository.DeleteAsync(person.Id);
            var deletedPerson = await _repository.GetByIdAsync(person.Id);

            // Assert
            deletedPerson.Should().BeNull();
        }
    }
}

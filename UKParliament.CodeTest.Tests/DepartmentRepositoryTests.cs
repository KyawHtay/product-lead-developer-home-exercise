using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data;
using UKParliament.CodeTest.Services;
using Xunit;

namespace UKParliament.CodeTest.Tests
{
    public class DepartmentRepositoryTests
    {
        private readonly DbContextOptions<PersonManagerContext> _options;

        public DepartmentRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<PersonManagerContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
        }

        private PersonManagerContext CreateContext()
        {
            var context = new PersonManagerContext(_options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllDepartments()
        {
            using var context = CreateContext();
            var repository = new DepartmentRepository(context);

            var departments = new List<Department>
            {
                new Department { Id = 11, Name = "HR" },
                new Department { Id = 12, Name = "IT" },
                new Department { Id = 13, Name = "Finance" }
            };

            context.Departments.AddRange(departments);
            await context.SaveChangesAsync();

            var result = await repository.GetAllAsync();

            result.Should().HaveCount(7);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnDepartment_WhenExists()
        {
            using var context = CreateContext();
            var repository = new DepartmentRepository(context);

            var department = new Department { Id = 111, Name = "HR1" };
            context.Departments.Add(department);
            await context.SaveChangesAsync();

            var result = await repository.GetByIdAsync(111);

            result.Should().NotBeNull();
            result!.Name.Should().Be("HR1");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            using var context = CreateContext();
            var repository = new DepartmentRepository(context);

            var result = await repository.GetByIdAsync(99);

            result.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_ShouldAddDepartment()
        {
            using var context = CreateContext();
            var repository = new DepartmentRepository(context);

            var newDepartment = new Department { Id = 41, Name = "Marketing1" };

            await repository.AddAsync(newDepartment);
            var result = await repository.GetByIdAsync(41);

            result.Should().NotBeNull();
            result!.Name.Should().Be("Marketing1");
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyExistingDepartment()
        {
            using var context = CreateContext();
            var repository = new DepartmentRepository(context);

            var department = new Department { Id = 151, Name = "HR1" };
            context.Departments.Add(department);
            await context.SaveChangesAsync();

            department.Name = "Human Resources";

            await repository.UpdateAsync(department);
            var updatedDepartment = await repository.GetByIdAsync(151);

            updatedDepartment.Should().NotBeNull();
            updatedDepartment!.Name.Should().Be("Human Resources");
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveDepartment_WhenExists()
        {
            using var context = CreateContext();
            var repository = new DepartmentRepository(context);

            var department = new Department { Id = 111, Name = "HR2" };
            context.Departments.Add(department);
            await context.SaveChangesAsync();

            await repository.DeleteAsync(111);
            var result = await repository.GetByIdAsync(111);

            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_ShouldDoNothing_WhenDepartmentDoesNotExist()
        {
            using var context = CreateContext();
            var repository = new DepartmentRepository(context);

            await repository.DeleteAsync(99);

            var count = context.Departments.Count();
            count.Should().Be(4);
        }
    }
}

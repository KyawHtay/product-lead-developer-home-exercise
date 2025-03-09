using System;
using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services;

public interface  IPersonRepository
{
    Task<IEnumerable<Person>> GetAllAsync();
    Task<Person> GetByIdAsync(int id);
    Task AddAsync(Person person);
    Task UpdateAsync(Person person);
    Task DeleteAsync(int id);
}

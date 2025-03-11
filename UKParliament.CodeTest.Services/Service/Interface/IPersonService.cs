using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services
{
    public interface IPersonService
    {
        Task<IEnumerable<Person>> GetAllAsync();
        Task<Person> GetByIdAsync(int id);
        Task AddAsync(Person person);
        Task UpdateAsync(Person person);
        Task DeleteAsync(int id);
    }
}

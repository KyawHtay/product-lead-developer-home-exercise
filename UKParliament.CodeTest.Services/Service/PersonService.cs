using System.Collections.Generic;
using System.Threading.Tasks;
using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _repository;

        public PersonService(IPersonRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(Person person)
        {
            await _repository.AddAsync(person);
        }

        public async Task UpdateAsync(Person person)
        {
            await _repository.UpdateAsync(person);
        }

        public async Task<Person> GetByEmailAsync(string email){
            return await _repository.GetByEmailAsync(email);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}

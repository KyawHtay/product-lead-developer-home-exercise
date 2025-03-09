using System;
using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data;

namespace UKParliament.CodeTest.Services;

public class PersonRepository : IPersonRepository
{
    private readonly PersonManagerContext _context;

    public PersonRepository(PersonManagerContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Person>> GetAllAsync()
    {
        return await _context.People.Include(p => p.Department).ToListAsync();
    }

    public async Task<Person> GetByIdAsync(int id)
    {
        return await _context.People.Include(p => p.Department).FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Person person)
    {
        if (await _context.People.AnyAsync(p => p.Email == person.Email))
        {
            throw new InvalidOperationException("A person with this email already exists.");
        }
        await _context.People.AddAsync(person);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Person person)
    {
        _context.People.Update(person);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var person = await _context.People.FindAsync(id);
        if (person != null)
        {
            _context.People.Remove(person);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Person> GetByEmailAsync(string email)
    {
        return await _context.People.FirstOrDefaultAsync(p => p.Email == email);
    }
}

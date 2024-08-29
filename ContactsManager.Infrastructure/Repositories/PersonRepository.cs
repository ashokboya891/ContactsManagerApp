using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using System;
using System.Linq.Expressions;

namespace Repositories
{
    public class PersonRepository : IPersonRespository
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<PersonRepository> _logger;
        public PersonRepository(ApplicationDbContext db,ILogger<PersonRepository> log)
        {
            _db = db;
            _logger = log;
        }

        public async Task<Person> AddPerson(Person person)
        {
            _db.Persons.Add(person);
            await _db.SaveChangesAsync();
            return person;
        }

        public async Task<bool> DeletePersonByPersonID(Guid personId)
        {
            _db.Persons.RemoveRange(_db.Persons.Where(t=>t.PersonId == personId));
           int deletedrows =await _db.SaveChangesAsync();
            return deletedrows>0;
        }

        public async Task<List<Person>> GetAllPersons()
        {
          return await  _db.Persons.Include("Country").ToListAsync();
        }

        public async  Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            return await _db.Persons.Include("Country")
                .Where(predicate)
                .ToListAsync();


        }

        public async Task<Person?> GetPersonByPersonId(Guid id)
        {
            return await _db.Persons.Include("Country")
                            .Where(t=>t.PersonId==id)
                            .FirstOrDefaultAsync();
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            Person? matchingperson = await _db.Persons.FirstOrDefaultAsync(t => t.PersonId == person.PersonId); 
            if(matchingperson==null) { return person; }

            matchingperson.PersonName = person.PersonName;
            matchingperson.Email = person.Email;
            matchingperson.Gender = person.Gender;
            matchingperson.CountryId = person.CountryId;
            matchingperson.Address = person.Address;
            matchingperson.ReceiveLetters = person.ReceiveLetters;
            matchingperson.DateOfBirth = person.DateOfBirth;

           int countupdated=await _db.SaveChangesAsync();
            return matchingperson;
        }
    }
}

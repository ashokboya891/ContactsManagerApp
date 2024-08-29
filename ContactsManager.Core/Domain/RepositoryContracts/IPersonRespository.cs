using Entities;
using System.Linq.Expressions;

namespace RepositoryContracts
{        /// <summary>
         /// represents data access logic for managing person entity
         /// </summary>
         /// <returns></returns>
    public interface IPersonRespository
    {
        /// <summary>
        /// Adds apersons object to the data store
        /// </summary>
        /// <param name="person">person object to add</param>
        /// <returns>return the person object after addding it to the table</returns>
        Task<Person> AddPerson(Person person);

        /// <summary>
        /// return the list of person from data object
        /// </summary>
        /// <returns>list of person will get from table</returns>
        Task<List<Person>> GetAllPersons();

        /// <summary>
        /// retunr the matching person corresponding to givne id
        /// </summary>
        /// <param name="id">person guid id for search</param>
        /// <returns>given mactching prson or null</returns>
        Task<Person?> GetPersonByPersonId(Guid id);

        /// <summary>
        /// return all person object based on the given expression
        /// </summary>
        /// <param name="predicate">LINQ rxpression to check</param>
        /// <returns>all matching person with given conditions</returns>
        Task<List<Person>>  GetFilteredPersons(Expression<Func<Person,bool>> predicate);

        /// <summary>
        /// Deletes a person object based on the person id
        /// </summary>
        /// <param name="personId">personid guid to search</param>
        /// <returns>deletes the mtaching person guid object from data</returns>
        Task<bool> DeletePersonByPersonID(Guid personId);

        /// <summary>
        /// updates a object (person name and other details) based on the given person id
        /// </summary>
        /// <param name="person">person object to update</param>
        /// <returns>return the updated person object</returns>
        Task<Person> UpdatePerson(Person person);

    }
}

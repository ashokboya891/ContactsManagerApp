using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IPersonGetterService
    {
        

        /// <summary>
        /// return all persons
        /// </summary>
        /// <returns>returs a list of object of personreponse type</returns>
       Task< List<PersonResponse>> GetAllPersons();
        
        /// <summary>
        /// returns the person object based on the given person id 
        /// </summary>
        /// <param name="personId">person id to search</param>
        /// <returns>return matching person object</returns>
      Task< PersonResponse?> GetPersonByPersonId(Guid? personId);

        /// <summary>
        /// returns all person objects that matches with given search field string
        /// </summary>
        /// <param name="serachBy"> serach filed to search</param>
        /// <param name="searchstring">search string to search</param>
        /// <returns>returns the all method persons base on the given search filed  and search string</returns>
        Task<List<PersonResponse>> GetFilteredPersons(string serachBy,string? searchstring);

       
 

        /// <summary>
        /// return the persons as csv
        /// </summary>
        /// <returns>return the memory stream with csv data </returns>
        //memory stream always represents stream of bytes
         Task<MemoryStream> GetPersonsCSV();
        /// <summary>
        /// return the persons as Excel
        /// </summary>
        /// <returns>return the memory stream with Excel data </returns>
        //memory stream always represents stream of bytes
        Task<MemoryStream> GetPersonsExc();

    }
}

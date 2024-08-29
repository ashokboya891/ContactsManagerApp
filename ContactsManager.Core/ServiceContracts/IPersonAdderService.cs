using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IPersonAdderService
    {
        /// <summary>
        /// adds a new perosn into the list of persons
        /// </summary>
        /// <param name="personAddRequest">person to add</param>
        /// <returns>return the same person details along with newly granted perosn id </returns>
      Task<PersonResponse>  AddPerson(PersonAddRequest? personAddRequest);
    }
}

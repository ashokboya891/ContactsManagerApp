using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IPersonDeleterService
    {
        /// <summary>
        /// deletes a person based on the give person id
        /// </summary>
        /// <param name="PersonID">personid to delete</param>
        /// <returns>return true if the deletion is successfull otherwise false\</returns>
        Task<bool> DeletePerson(Guid? paersonID);
    }
}

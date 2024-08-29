using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IPersonUpdaterServices
    {
       
        /// <summary>
        /// updated the specified person details based on the given person id
        /// </summary>
        /// <param name="personUpdateRequest">Person details</param>
        /// <returns>returns the person object after updation</returns>
       Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);
      

    }
}

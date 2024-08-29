using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IPersonSorterService
    {
 
        /// <summary>
        ///returns the sortedl list of persons
        /// </summary>
        /// <param name="allpersons">represenst list of persons to sort</param>
        /// <param name="sortBy">Name of the property key based on which the persons should be sorted </param>
        /// <param name="sortorder">ASC or DESC</param>
        /// <returns>return sorted person as personresponse list</returns>
        Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse>? allpersons, string sortBy, SortOrderOptions sortorder);
        
    

    }
}

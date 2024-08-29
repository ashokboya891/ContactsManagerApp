using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// represent the dto class that conatians person details to update 
    /// </summary>
    public class PersonUpdateRequest
    {
        [Required(ErrorMessage ="person id cannot be black")]
        public Guid PersonID { set; get; }

        [Required(ErrorMessage = "Person name canot be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email  canot be blank")]
        [EmailAddress(ErrorMessage = "mail should be in format ")]
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }


        public GenderOptions? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Address { get; set; }
        public bool ReceiveLetters { get; set; }
        /// <summary>
        /// converts the current object of personadd request into new objedt of peron type
        /// </summary>
        /// <returns>return person object</returns>
        public Person ToPerson()
        {
            return new Person
            {
                PersonId= PersonID,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                Address = Address,
                CountryId = CountryId,
                ReceiveLetters = ReceiveLetters
            };
        }
    }
}

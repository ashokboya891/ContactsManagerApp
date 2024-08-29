using Entities;
using ServiceContracts.Enums;
using System.ComponentModel.DataAnnotations;
namespace ServiceContracts.DTO
{
    /// <summary>
    /// acts as dto for inserting a new person
    /// </summary>
    public class PersonAddRequest
    {
        [Required(ErrorMessage ="Person name canot be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email  canot be blank")]
        [EmailAddress(ErrorMessage ="mail should be in format ")]
        [DataType(DataType.EmailAddress)]    
        public string? Email { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage ="Gender cannot be blank")]
        public GenderOptions? Gender { get; set; }
        [Required(ErrorMessage ="Please select country")]
        public Guid? CountryId { get; set; }
        public string? Address { get; set; }
        public bool ReceiveLetters { get; set; }
        /// <summary>
        /// converts the current object of personadd request into new objedt of peron type
        /// </summary>
        /// <returns></returns>
        public Person ToPerson()
        {
            return new Person { PersonName = PersonName, Email = Email, DateOfBirth = DateOfBirth, Gender=Gender.ToString()
            ,Address=Address,CountryId=CountryId,ReceiveLetters=ReceiveLetters}; 
        }
    }
}

using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    /// <summary>
    ///represents dto class it used as return type of most methods of persons service
    /// </summary>
    public class PersonResponse
    {
        public Guid PersonId { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public string? Country { get; set; }

        public string? Address { get; set; }
        public bool ReceiveLetters { get; set; }
        public double? Age { get; set; }

        /// <summary>
        /// compares the current object data with parameter object
        /// </summary>
        /// <param name="obj">the personresponse object to compare </param>
        /// <returns> true or false idication whthere all person details are matched with the specified paramter
        /// </returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(PersonResponse)) return false;
            PersonResponse other = (PersonResponse)obj;
            return this.PersonId == other.PersonId && this.PersonName == other.PersonName && this.Email == other.Email
                && this.DateOfBirth == other.DateOfBirth && this.Gender == other.Gender && this.CountryId == other.CountryId &&
                 this.Country == other.Country;

        }


        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return $"Person Id:{PersonId}, person name:{PersonName}, Age : {Age},Email :{Email},Date of birth :{DateOfBirth.Value.ToString("dd mm yyyy")},gender : {Gender}" +
                $",country ; {Country} ,country id:{CountryId},address:{Address} received letters :{ReceiveLetters} ";
        }
        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonID = PersonId,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth=DateOfBirth,
                CountryId = CountryId,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true),
                Address=Address,
                ReceiveLetters = ReceiveLetters,
                };
            
           
        }
    }
    public static class PersonResponseExtensions
    {
        /// <summary>
        /// an extension method to convert objet of person class into personresponse class
        /// </summary>
        /// <param name="person">
        /// Return the converted personresponse object
        /// </param>
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonId = person.PersonId,
                PersonName = person.PersonName,
                Email = person.Email,
                Gender = person.Gender,
                Address = person.Address,
                ReceiveLetters = person.ReceiveLetters,
                CountryId = person.CountryId,
                DateOfBirth = person.DateOfBirth,
                Age = (person.DateOfBirth != null) ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25) : null,Country=person.Country?.CountryName
            };
        }

    }

}

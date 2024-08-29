using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class Person
    {
        [Key]
        public Guid PersonId { get; set; }

        [StringLength(40)] //nvarchar(40) for EF
        public string? PersonName{ get; set; }

        [StringLength(40)] //nvarchar(40) for EF
        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(10)] //nvarchar(10) for EF
        public string? Gender { get; set; }

        //uniqueid 
        public Guid? CountryId { get; set; }

        [StringLength(200)] //nvarchar(200) for EF
        public string? Address { get; set; }

        public bool ReceiveLetters { get; set; }

        public string? TIN { get; set; }

        [ForeignKey("CountryId")]  //instead of writing code in onmodel for relation we can use this direclty
        public Country? Country { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Country
    {
        [Key]
        public Guid CountryId { get; set; }

        public string? CountryName { get; set; }        //if ? is not there in return type means EFC will consider it as not null column or ="" also can adjust problem
        public virtual ICollection<Person>? Persons { set; get; } //this line added while adding creation code in onmodel class
    }
}

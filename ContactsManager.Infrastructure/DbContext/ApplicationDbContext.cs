using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<Person> Persons { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");
            //fluent API below line is example for bussines usage of models in DB wise
            modelBuilder.Entity<Person>().Property(temp => temp.TIN)
            .HasColumnName("TaxIdentificationNumber").HasColumnType("varchar(10)").HasDefaultValue("AB1354323");

            //FLUENT API PART2 FOR unique key adding not excuted cause above default valu we are using
            //  modelBuilder.Entity<Person>().HasIndex(temp => temp.TIN).IsUnique();

            //fluent api constraint checking adding here it will be checked for every insertion and updation
            //  modelBuilder.Entity<Person>().HasCheckConstraint("CHK_TIN", "len([TaxIdentificationNumber])=10");
           // modelBuilder.Entity<Person>().HasCheckConstraint("CHK_TIN", "LEN([TaxIdentificationNumber]) = 10");

            //seed to countries
            //modelBuilder.Entity<Country>().HasData(new Country()
            //{
            //    CountryId=Guid.NewGuid(),CountryName="sample0" //but also we can seed data through json format file
            //});

           
            //seeeding thrpugh json files
            string countriesJson = System.IO.File.ReadAllText("countries.json");
            List<Country>? countries= System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);

            foreach (var item in countries)
            {
                modelBuilder.Entity<Country>().HasData(item);
               
            }

            string personsJson = System.IO.File.ReadAllText("persons.json");
            List<Person> persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);

            foreach (var item in persons)
            {
                modelBuilder.Entity<Person>().HasData(item);

            }

            //creating relation between two tables below code is also optional we can use [foreignkey] above the field in model class
            //msg every country has set of persons & this country is from person model class & last persons is from countries model class
            //modelBuilder.Entity<Person>().HasOne<Country>(c => c.Country).WithMany(p=>p.Persons).HasForeignKey(p=>p.CountryId); 

        }

        public List<Person> sp_GetAllPersons()
        {
           return  Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }
        public int sp_InsertPerson(Person person)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
            //(@PersonId,@PersonName,@Email,@DateOfBirth,@Gender,@CountryId,@Adddress,@ReceiveLetters);

                new SqlParameter("@PersonId",person.PersonId),
                new SqlParameter("@PersonName", person.PersonName),
                new SqlParameter("@Email", person.Email),
                new SqlParameter("@DateOfBirth", person.DateOfBirth),
                new SqlParameter("@Gender", person.Gender),
                new SqlParameter("@CountryId", person.CountryId),
                new SqlParameter("@Address", person.Address),
                new SqlParameter("@ReceiveLetters", person.ReceiveLetters),
            };
          return  Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] @PersonId,@PersonName,@Email,@DateOfBirth,@Gender,@CountryId,@Address,@ReceiveLetters", parameters);
        }
    }
}

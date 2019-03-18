using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace School_Models 
{
    public class Person
    {
        //Id, Name,Gender, Email, Birthday, Password en een ImageUrl

        public int Id { get; set; }

        [Display(Name = "GeboorteDatum")]
        [DataType(DataType.Date, ErrorMessage ="kies een dag")]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString ="{0:dd/MM/yyyy}")]
        public DateTime? Birthday { get; set; }

        [Display(Name = "email adres")]
        [EmailAddress(ErrorMessage = "Ongeldig email adress")]
        [DefaultValue("")]
        public string Email { get; set; }

        [Required(ErrorMessage = "gelieve naam in te vullen")]
        [Display(Name="Naam")]
        [MaxLength(15)]
        public string Name { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [ScaffoldColumn(false)]
        public string ImageUrl { get; set; }

        public enum GenderType
        {
            Male = 0,
            Female = 1
        }

        [Display(Name = "Geslacht")]
        [EnumDataType(typeof(GenderType), ErrorMessage ="keuze maken")]
        public GenderType? Gender { get; set; }



    }
}

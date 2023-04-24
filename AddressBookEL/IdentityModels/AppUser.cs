using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookEL.IdentityModels
{
    public class AppUser:IdentityUser
    {
        //IdentityUser classının içindeki propertylerden farklı eklemek istediğin özellikler var ise IdentityUser classından kalıtım alarak tabloyu genişletebiliriz
        public DateTime CreatedDate { get; set; }

        [Required]
        [StringLength(50,MinimumLength =2)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }
        public DateTime? Birthdate { get; set; }
        public bool IsPassive { get; set; }

        //05396796650
        [Required]
        [StringLength(11, MinimumLength = 11)]
        [RegularExpression("^[0-9]*", ErrorMessage = "Telefon rakamlardan oluşmalıdır")]
        public override string PhoneNumber { get; set; }



    }
}

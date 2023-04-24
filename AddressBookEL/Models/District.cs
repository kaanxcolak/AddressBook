using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookEL.Models
{
    [Table("Districts")]
    public class District:BaseNumeric
    {
        [Required]  //Girilmesi şart
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
        
        public int CityId { get; set; }
        [ForeignKey("CityId")] //CityId 'ye yazdığım int değerinin City tablosunda karşılığı olmak zorunda
        public virtual City City { get; set; } //City.ıd propertysi Foregin Key olacağı için burada City Tablosuyla ilişkisi kuruldu.
        //Dipnot: İlişkiler burada kurulacağı gibi MYCONTEXT class'ı içindeki OnModelCreating metodu ezilerek (override) kurulabilir.
    }
}

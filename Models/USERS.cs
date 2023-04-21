using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PRUEBA.Models
{
    public class USERS
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        
        [Key]
        public int Id_User{get;set;}
        public String? Name{get;set;}
        public String? Email{get;set;}
        public String? Password{get;set;}
        public String? Address{get;set;}
        public int PhoneNumber{get;set;}

        public List<ORDER>? ORDERs{get;set;}
        public CART? CARTs{get; set;}
    }

}
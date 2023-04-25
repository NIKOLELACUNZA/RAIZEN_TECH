using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PRUEBA.Models
{
    public class CART
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]

        [Key]
        public int Id_Cart { get; set; }
       
        public DateTime Created_Date { get; set; }
        public DateTime Last_Updated_Date { get; set; }
        public USERS? USERs {get; set;}

       public List<CART_ITEM>? CART_ITEMs { get; set; }
       public List<ORDER>? ORDERs { get; set; }     


    }
}
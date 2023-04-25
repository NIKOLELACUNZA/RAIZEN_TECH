using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PRUEBA.Models
{
    [Table("Cart")]
    public class CART
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }   
        public USERS? Users { get; set; }    
        public DateTime Created_Date { get; set; }
        public DateTime Last_Updated_Date { get; set; }

    }
}
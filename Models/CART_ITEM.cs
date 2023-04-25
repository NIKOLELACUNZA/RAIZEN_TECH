using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PRUEBA.Models
{
    [Table("CartItem")]
    public class CART_ITEM
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }
        public CART? Cart { get; set; }
        public PRODUCT? Product { get; set; }
        public int Quantity { get; set; }
        


    }
}
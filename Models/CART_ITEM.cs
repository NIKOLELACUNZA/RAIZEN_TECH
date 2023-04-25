using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PRUEBA.Models
{
    public class CART_ITEM
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]

        [Key]
        public int Id_CartItem { get; set; }
        public int Quantity { get; set; }

        public CART? CARTs { get; set; }
        public PRODUCT? PRODUCTs { get; set; }
         public ORDER_ITEM? ORDER_ITEMs{get; set;}
    }
}
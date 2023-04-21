using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PRUEBA.Models
{
    public class ORDER_ITEM
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]

        [Key]
        public int Id_OrderItem { get; set; }
        [ForeignKey("ORDER")]
        public int Id_Order {get; set;}
        [ForeignKey("PRODUCT")]
        public int Id_Product {get; set;}
        public int Quantity {get; set;}
        public Decimal Unit_Price {get; set; }


        /* public List<ORDER>? ORDERs{get;set;} */
        public ORDER? ORDERs{get; set;}
        public PRODUCT? PRODUCTs{get; set;}

        public CART_ITEM? CART_ITEMs{get; set;}
         
        

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PRUEBA.Models
{
     [Table("OrderItem")]
    public class ORDER_ITEM
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }
        public ORDER? Order {get; set; }
        public PRODUCT? Product {get; set; }
        public int Quantity {get; set;}
        public Decimal Unit_Price {get; set; }

    }
}
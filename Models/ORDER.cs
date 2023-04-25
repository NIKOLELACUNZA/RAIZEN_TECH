using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PRUEBA.Models
{
    [Table("Order")]
    public class ORDER
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }
        public USERS? Users { get; set; }
        public CART? Cart { get; set; }
        public DateTime OrderDate { get; set; }
        public Decimal Total_Amount { get; set; }
        public String? Shipping_Address { get; set; }
        public String? Status { get; set; } = "PENDIENTE";

    }
}
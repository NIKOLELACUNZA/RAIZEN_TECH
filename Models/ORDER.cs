using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PRUEBA.Models
{
    public class ORDER
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]

        [Key]
        public int Id_Order { get; set; }
        [ForeignKey("USER")]
        public int Id_User { get; set; }
        [ForeignKey("CART")]
        public int Id_Cart { get; set; }
        public DateTime OrderDate { get; set; }
        public Decimal Total_Amount { get; set; }
        public String? Shipping_Address { get; set; }


       /*  public ORDER_ITEM? ORDER_ITEM { get; set; } */
        public USERS? USERs { get; set; }
        public CART? CARTs { get; set; }

        public List<ORDER_ITEM>? ORDER_ITEMs { get; set; }
    }
}
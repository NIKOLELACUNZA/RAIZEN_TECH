using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRUEBA.Models
{
    public class PRODUCT
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        [Key]
        public int Id_Product { get; set; }
        [ForeignKey("CATEGORY")]
        public int ID_CATEGORY { get; set; }
        public string? Name { get; set; }

        public string? Descripcion { get; set; }

        public Decimal Price { get; set; }

        public int Quantity { get; set; }

        public string? Image_URL { get; set; }

        public List<CART_ITEM>? CART_ITEMs { get; set;}
        public CATEGORY? CATEGORYs { get; set;}
        public List<ORDER_ITEM>? ORDER_ITEMs { get; set;}
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PRUEBA.Models
{
   [Table("Products")]
    public class PRODUCT
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }
        public CATEGORY? Category { get; set; }
        public string? Name { get; set; }
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El precio debe ser un número decimal con hasta dos decimales")]
        public Decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? Image_URL { get; set; }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PRUEBA.Models
{
    [Table("Category")]
        public class CATEGORY
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]

        public int id { get; set; }
        public String? Name { get; set; }
    }
}
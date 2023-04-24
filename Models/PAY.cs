using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PRUEBA.Models
{
  [Table("Pay")]
    public class PAY
    {        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int id { get; set; }
        public ORDER? Order {get; set; }
        public DateTime PaymentDate { get; set; }
        public string? NameCard { get; set; }        
        public string? NumberCard { get; set; }
        public string? CardExpireDateMMYY { get; set; }
        public string? Cvv { get; set; }
        public Decimal TotalAmount { get; set; }
        public string? UserID{ get; set; }
        public string? PaymentStatus{get; set; }
    
    }
}
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace Aleksandra_Petrovic_Test.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:120)]
        public string Name { get; set; }
        [Required]
        [Range(1910, 2025)]// dopuni ovo
        public int  Year { get; set; }
    }
}

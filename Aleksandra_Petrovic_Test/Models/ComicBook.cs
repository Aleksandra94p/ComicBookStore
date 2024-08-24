using System.ComponentModel.DataAnnotations;

namespace Aleksandra_Petrovic_Test.Models
{
    public class ComicBook
    {
        public int Id   { get; set; }
        [Required]
        [StringLength(30, MinimumLength =2)]
        public string  Genre { get; set; }
        [Required]
        [StringLength(120, MinimumLength =3)]
        public string Name { get; set; }
        [Required]
        [Range(typeof(decimal), "300.00", "10000.00")]
        public decimal Price { get; set; }
        [Required]
        [Range(minimum: 1, maximum:5000)]
        public int AvailableQuantity { get; set; }
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }

    }
}

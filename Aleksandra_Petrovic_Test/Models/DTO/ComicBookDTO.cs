using System;
using System.ComponentModel.DataAnnotations;

namespace Aleksandra_Petrovic_Test.Models.DTO
{
    public class ComicBookDTO
    {
         public int Id   { get; set; }
      
        public string  Genre { get; set; }
        
        public string Name { get; set; }
     
        public decimal Price { get; set; }
        public int AvailableQuantity { get; set; }
        public int PublisherId { get; set; }
        public string PublisherName { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ComicBookDTO dTO &&
                   PublisherName == dTO.PublisherName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PublisherName);
        }
    }
}

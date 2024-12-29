using System.ComponentModel.DataAnnotations;

namespace Catalog.API.Models {
    public class Product {
        [Key]
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public List<string> Categories { get; set; } = new();

        public required string Description { get; set; }

        public required string ImageFile { get; set; }

        public decimal Price { get; set; }
    }
}

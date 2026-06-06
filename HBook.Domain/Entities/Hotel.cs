using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HBook.Domain.Entities
{
    public class Hotel
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public required string Name { get; set; }
        public string? Description { get; set; }
        [Display(Name = "Price per night")]
        [Range(10,10000)]
        public double Price { get; set; }
        public int Sqft { get; set; }
        [Range(1,10)]
        public int Occupancy { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }

        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}

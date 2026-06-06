using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HBook.Domain.Entities
{
    public class Room
    {
        [Display(Name = "Room Number")]
        public int RoomNumber { get; set; }
        [ForeignKey("Hotel")]
        public int HotelId { get; set; }
        [ValidateNever]
        public Hotel Hotel { get; set; }
        public string? SpecialDetails { get; set; }
    }
}

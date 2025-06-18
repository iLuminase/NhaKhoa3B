using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyMvcApp.Areas.Admin.Models
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public required string UserId { get; set; }

        [ForeignKey("UserId")]
        public required ApplicationUser User { get; set; }
    }
} 
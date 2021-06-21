using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace PhotoBoom.Models
{
    public class Gallery
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter title")]
        public string Title { get; set; }
        public string Tags { get; set; }
        public string ImagePath { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Please choose photo")]
        public IFormFile ImageFile { get; set; }
    }
}
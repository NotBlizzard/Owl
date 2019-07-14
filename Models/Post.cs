using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Owl.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [Required]
        [StringLength(1000, MinimumLength = 10)]
        public string PostTitle { get; set; }
        [Required]
        [StringLength(100000, MinimumLength = 20)]
        public string PostData { get; set; }
        [DataType(DataType.Date)]
        public DateTime PostDate { get; set; }

        public List<Message> PostMessages { get; set; }
        public string UserEmail { get; internal set; }
    }
}

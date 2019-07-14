using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Owl.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
        [Required]
        [StringLength(100000, MinimumLength = 10)]
        public string MessageData { get; set; }
        [DataType(DataType.Date)]
        public DateTime MessageDate { get; set; }
        public string UserEmail { get; internal set; }
    }
}

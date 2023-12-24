using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Phone_Shop.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }

        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsReaded {  get; set; }

        public IdentityUser User { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GamingForum.Models
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Descrierea este obligatorie!")]
        [StringLength(500, ErrorMessage = "Descrierea nu poate avea mai mult de 500 de caractere!")]
        public string About { get; set; }

        public byte[] Image { get; set; }

        public virtual ApplicationUser User{ get; set; }

    }
}
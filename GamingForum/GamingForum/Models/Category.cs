using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GamingForum.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required(ErrorMessage ="Numele categoriei este obligatoriu.")]
        [StringLength(50,ErrorMessage = "Numele categoriei are peste 50 de caractere.")]
        public string CategoryName { get; set; }

        public virtual ICollection<Topic> Topics { get; set; }
    }

    
}
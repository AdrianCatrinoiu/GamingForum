using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GamingForum.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        [Required(ErrorMessage ="Comentariul nu poate fi gol.")]
        [StringLength(500,ErrorMessage = "Comentariul depaseste limita de caractere(500).")]
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int TopicId { get; set; }


        public virtual Topic Topic { get; set; }
    }

}
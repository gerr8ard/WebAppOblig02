using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogCentralVersion2.Models
{
    public class BlogPost
    {
        public int BlogPostId { get; set; }
        [Required]
        [Display(Name = "Tittel")]
        public String BlogPostTitle { get; set; }
        [Display(Name = "Forfatter")]
        public String BlogPostAuthor { get; set; }
        [Required]
        [Display(Name = "Innlegg")]
        [DataType(DataType.MultilineText)]
        public String BlogPostPost { get; set; }
        [Display(Name = "Opprettet")]
        public DateTime DateCreated { get; set; }
        [Display(Name = "Åpen for kommentar?")]
        public bool isOpen { get; set; }

        public virtual Blog Blog { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual ApplicationUser OwnerOfBlogPost { get; set; }//Setter eier på innlegget
    }
}
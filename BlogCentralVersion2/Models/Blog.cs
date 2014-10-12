using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogCentralVersion2.Models
{
    public class Blog
    {
        
        public int BlogId { get; set; }
        [Required(ErrorMessage = "Vennligst tast inn ett navn")]
        [Display(Name = "Blogg Navn")]
        public String BlogTitle { get; set; }
        [Display(Name = "Eier av blogg")]
        public String BlogOwner { get; set; }
        [Display(Name = "Opprettet")]
        public DateTime DateCreated { get; set; }

        public virtual List<BlogPost> Posts { get; set; }
        public virtual ApplicationUser OwnerOfBlog { get; set; }//Setter eier på bloggen.
    }
}
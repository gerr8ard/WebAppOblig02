using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BlogCentralVersion2.Models
{
    public class Comment
    {
       
        public int CommentId { get; set; }
        [Display(Name = "Kommentert av")]
        public String CommentName { get; set; }
        [Required(ErrorMessage = "Vennligst tast inn en kommentar!")]
        [Display(Name = "Kommentar")]
        [DataType(DataType.MultilineText)]
        public String CommentPost { get; set; }
        [Display(Name = "Lagt til")]
        public DateTime Datecreated { get; set; }
        [Display(Name = "Vis kommentar")]

        public virtual BlogPost BlogPost { get; set; }
        public virtual ApplicationUser OwnerOfComment { get; set; }//Setter eier på kommentar
    }
}
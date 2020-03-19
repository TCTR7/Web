using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PartyInvites.Models
{
    public class GuestRespone
    {
        [Required(ErrorMessage ="please enter your Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "please enter your Email adress")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage ="please enter a valid email adress")]
        public string Email { get; set; }
        [Required(ErrorMessage = "please enter your Phone")]
        public string Phone { get; set; }
        [Required(ErrorMessage ="please specify whether you'll attend")]
        public bool? WillAttend { get; set; }
    }
}
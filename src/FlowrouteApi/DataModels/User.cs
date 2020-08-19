using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlowrouteApi.DataModels
{
    [Table("User")]
    public class User
    {
        [Key,Required]
        public int Id { get; set; }

        [Required, MaxLength(40), Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required, MaxLength(20), Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required, MaxLength(20), Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required, MaxLength(250)]
        public string Password { get; set; }

        [Required, MaxLength(50)]
        public string Email { get; set; }

        [Required, MaxLength(15)]
        public string Phone { get; set; }
    }
}

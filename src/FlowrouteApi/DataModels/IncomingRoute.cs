using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FlowrouteApi.DataModels
{
    [Table("IncomingRoute")]
    public class IncomingRoute
    {
        public string Phone { get; set; }
        public string Email { get; set; }

        [Key]
        public int Id { get; set; }
    }
}

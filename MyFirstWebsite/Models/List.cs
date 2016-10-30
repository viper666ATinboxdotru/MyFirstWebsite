using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace MyFirstWebsite.Models
{
    public class List
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Details { get; set; }

        public string Date_Posted { get; set; }
        public string Time_Posted { get; set; }
        public string Date_Edited { get; set; }
        public string Time_Edited { get; set; }
        public string Public { get; set; }
        public int user_id { get; set; }
    }
}
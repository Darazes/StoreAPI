﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreAPI.Models
{
    public class Feedback
    {
        [Key]
        public int id_feedback { get; set; }

        [Required]
        public int id_customer { get; set; }

        [Required]
        public string content { get; set; }
    }

    public class FeedBackCustom
    {
        public int id_customer { get; set; }

        public string content { get; set; }
    }

    public class FeedBackID
    {
        public int id_customer { get; set; }
    }
}
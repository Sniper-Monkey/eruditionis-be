namespace eruditionis.Database.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Document
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string File { get; set; } // do not change it to byte[]
        public Chat Chat { get; set; }
        public User UploadedBy { get; set; }
    }
}

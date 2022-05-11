namespace BookStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ProductComment")]
    public partial class ProductComment
    {
        [Key]
        public int CommentID { get; set; }

        public int UserID { get; set; }

        [Required]
        [StringLength(20)]
        public string PrdID { get; set; }

        [Required]
        [StringLength(500)]
        public string Content { get; set; }

        public DateTime CommentDate { get; set; }
    }
}

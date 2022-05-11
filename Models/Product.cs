namespace BookStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        [Key]
        [StringLength(20)]
        public string PrdID { get; set; }

        [Required]
        [StringLength(3)]
        public string PrdStatus { get; set; }

        [Required]
        [StringLength(3)]
        public string PrdType { get; set; }

        public byte[] PrdImage { get; set; }

        [StringLength(10)]
        public string PrdImageEXT { get; set; }

        [Required]
        [StringLength(50)]
        public string PrdName { get; set; }

        [StringLength(500)]
        public string PrdContent { get; set; }

        public int PrdPrice { get; set; }

        public int PrdQuantity { get; set; }

        [StringLength(2)]
        public string MenuType { get; set; }
    }
}

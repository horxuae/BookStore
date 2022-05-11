namespace BookStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Image")]
    public partial class Image
    {
        [Key]
        public int ImageID { get; set; }

        [StringLength(1)]
        public string ImageType { get; set; }

        public byte[] ImageContent { get; set; }

        [StringLength(10)]
        public string ImageEXT { get; set; }
    }
}

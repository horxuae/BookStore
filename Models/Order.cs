namespace BookStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        public int OrderID { get; set; }

        public int UserID { get; set; }

        [Required]
        [StringLength(20)]
        public string RecieverName { get; set; }

        [Required]
        [StringLength(15)]
        public string RecieverPhone { get; set; }

        [Required]
        [StringLength(100)]
        public string RecieverAddress { get; set; }

        public int Total { get; set; }
    }
}

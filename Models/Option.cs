namespace BookStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Option")]
    public partial class Option
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(5)]
        public string OptID { get; set; }

        [StringLength(10)]
        public string OptName { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(5)]
        public string ListID { get; set; }

        [StringLength(20)]
        public string ListName { get; set; }
    }
    /*
    [Serializable] //可序列化
    public class MenuType //清單種類類別
    {
        public MenuType()
        {
            this.MenuTypeList = new List<Option>();
        }

        public List<Option> MenuTypeList;
    }

    [Serializable] //可序列化
    public class PrdType //產品總類
    {
        public PrdType()
        {
            this.PrdTypeList = new List<Option>();
        }

        public List<Option> PrdTypeList;
    }
    */
}

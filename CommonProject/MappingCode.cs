using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.CommonProject
{
    public class MappingCode
    {
        /// <summary>
        /// Option
        /// </summary>
        public struct OptionItem
        {
            /// <summary>
            /// 使用者類型
            /// </summary>
            public const string UserType = "1001";

            /// <summary>
            /// 性別
            /// </summary>
            public const string Gender = "1002";

            /// <summary>
            /// 產品總類
            /// </summary>
            public const string PrdType = "2001";

            /// <summary>
            /// 產品狀態
            /// </summary>
            public const string PrdStatus = "2002";

            /// <summary>
            /// 圖檔類型
            /// </summary>
            public const string ImageType = "2003";

            /// <summary>
            /// 清單總類
            /// </summary>
            public const string MenuType = "3001";
        }

        /// <summary>
        /// 清單總類
        /// </summary>
        public struct MenuTypeItem
        {
            /// <summary>
            /// 書籍
            /// </summary>
            public const string Book = "A";

            /// <summary>
            /// 文具
            /// </summary>
            public const string Stationery = "B";
        }

        /// <summary>
        /// 回傳訊息
        /// </summary>
        public struct ReturnMessage
        {
            public const string NotFound = "很抱歉，目前無相關產品!!";
        }
    }
}
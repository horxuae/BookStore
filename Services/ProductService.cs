using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Models;
using BookStore.CommonProject;
using System.IO;
using System.Web.Mvc;

namespace BookStore.Services
{
    public class ProductService 
    {
        //宣告資料庫的Models就算完成連線
        private Models.MVC_UserDBContext _db = new Models.MVC_UserDBContext();

        public List<Option> MenuOptionList = new List<Option>();

        #region 查詢

        /// <summary>
        /// 取得清單
        /// </summary>
        public bool GetMenu()
        {
            List<Option> MenuOption = null;

            //取得清單選項
            MenuOption = (from m in _db.Option
                          select m).ToList();

            if (MenuOption == null)
                return false;
            else
            { 
                MenuOptionList = MenuOption;
                return true;
            }
        }

        /// <summary>
        /// 取得活動圖檔
        /// </summary>
        public List<Image> GetImage()
        {
            //取得活動圖檔
            List<Image> ImageList = (from m in _db.Image
                                     select m).ToList();

            return ImageList;
        }

        /// <summary>
        /// 查詢產品資料
        /// </summary>
        public List<Product> SearchProduct(Product product)
        {
            var List = (from m_Product in _db.Product
                        join m_Option in _db.Option
                        on new
                        {
                            OptID = MappingCode.OptionItem.PrdStatus,
                            ListID = m_Product.PrdStatus
                        } equals
                        new
                        {
                            m_Option.OptID,
                            m_Option.ListID
                        }
                        where m_Product.PrdID == (string.IsNullOrEmpty(product.PrdID) ? m_Product.PrdID : product.PrdID)
                           && m_Product.PrdStatus == (string.IsNullOrEmpty(product.PrdStatus) ? m_Product.PrdStatus : product.PrdStatus)
                           && m_Product.PrdType == (string.IsNullOrEmpty(product.PrdType) ? m_Product.PrdType : product.PrdType)
                           && m_Product.PrdName.Contains(string.IsNullOrEmpty(product.PrdName) ? m_Product.PrdName : product.PrdName)
                           && m_Product.MenuType == (string.IsNullOrEmpty(product.MenuType) ? m_Product.MenuType : product.MenuType)
                        /* 建匿名型別 */
                        select new
                        {
                            PrdID = m_Product.PrdID,
                            PrdStatus = m_Option.ListName,
                            PrdType = m_Product.PrdType,
                            PrdImage = m_Product.PrdImage,
                            PrdImageEXT = m_Product.PrdImageEXT,
                            PrdName = m_Product.PrdName,
                            PrdContent = m_Product.PrdContent,
                            PrdPrice = m_Product.PrdPrice,
                            PrdQuantity = m_Product.PrdQuantity,
                            MenuType = m_Product.MenuType
                        }).ToList()
                        /* Select 指定型別 */
                        .Select(x => new Product
                        {
                            PrdID = x.PrdID,
                            PrdStatus = x.PrdStatus,
                            PrdType = x.PrdType,
                            PrdImage = x.PrdImage,
                            PrdImageEXT = x.PrdImageEXT,
                            PrdName = x.PrdName,
                            PrdContent = x.PrdContent,
                            PrdPrice = x.PrdPrice,
                            PrdQuantity = x.PrdQuantity,
                            MenuType = x.MenuType
                        });

            List<Product> ProductList = new List<Product>();
            ProductList.AddRange(List);

            return ProductList;
        }

        #endregion

        #region 修改

        /// <summary>
        /// 更新產品資料
        /// </summary>
        public string UpdateProduct(Product product)
        {
            //取得產品資料
            Product ProductDetails = _db.Product.Where(x => x.PrdID == product.PrdID).FirstOrDefault();

            ProductDetails.PrdStatus = product.PrdStatus;
            ProductDetails.PrdType = product.PrdType;
            ProductDetails.PrdName = product.PrdName;
            ProductDetails.PrdContent = product.PrdContent;
            ProductDetails.PrdPrice = product.PrdPrice;
            ProductDetails.PrdQuantity = product.PrdQuantity;
            ProductDetails.MenuType = product.PrdType.Substring(0, 1);
            _db.SaveChanges();

            var returnData = new
            {
                // 成功與否
                IsSuccess = true,
                Message = "修改成功"
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(returnData);
        }

        /// <summary>
        /// 更新產品圖檔
        /// </summary>
        public void UpdatePrdImage(HttpPostedFileBase _FileName, string FileEXT, string PrdID)
        {
            //轉成byte
            //方法一 直接轉
            byte[] FileBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                _FileName.InputStream.CopyTo(ms);
                FileBytes = ms.GetBuffer();
            }
            #region 方法二 讀實體檔案出來再轉

            //using (var Fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            //{
            //    using (var Reader = new BinaryReader(Fs))
            //    {
            //        FileBytes = Reader.ReadBytes((int)Fs.Length);
            //    }
            //}
            //存進資料庫再取出
            //InsertPhoto(FileBytes);
            //FileBytes = SelectPhoto();
            //TempData["Data"] = FileBytes;

            #endregion

            // 先查出資料
            Product ProductDetails = _db.Product.Where(x => x.PrdID == PrdID).FirstOrDefault();

            ProductDetails.PrdImage = FileBytes;
            ProductDetails.PrdImageEXT = FileEXT.TrimStart('.');
            _db.SaveChanges();        
        }

        #endregion

        #region 刪除

        /// <summary>
        /// 刪除產品資料
        /// </summary>
        public void RemoveProduct(string PrdID)
        {
            //先將欲刪除的資料查出來
            Product ProductDetails = _db.Product.Where(x => x.PrdID == PrdID).FirstOrDefault();

            _db.Entry(ProductDetails).State = System.Data.Entity.EntityState.Deleted;
            _db.SaveChanges();
        }

        #endregion

        #region 新增

        /// <summary>
        /// 新增產品資料
        /// </summary>
        public string AddProduct(Product product)
        {
            product.MenuType = product.PrdType.Substring(0, 1);
            _db.Entry(product).State = System.Data.Entity.EntityState.Added;
            _db.SaveChanges();

            var returnData = new
            {
                // 成功與否
                IsSuccess = true,
                Message = "新增成功"
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(returnData);
        }

        #endregion

        #region 檢核

        /// <summary>
        /// 檢核Pkey是否重複
        /// </summary>
        public bool ValidatePKey(string PrdID)
        {
            List<string> PKeyList = (from m in _db.Product
                                     where m.PrdID == PrdID
                                     select m.PrdID).ToList();

            if (PKeyList.Count == 0)
                return true;
            else
                return false;
        }

        #endregion
    }
}
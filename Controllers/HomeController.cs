using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using BookStore.CommonProject;
using BookStore.Models;
using BookStore.Services;
using PagedList;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        #region 關閉資料庫連結
        /*
        //覆蓋繼承的方法，virtual代表可以被覆蓋
        //資料庫一旦開啟連線，用完就得要關閉連線與釋放資源
        //disposing為是否開啟連線
        protected override void Dispose(bool disposing)
        {
            //如果開啟連線
            if (disposing)
            {
                _db.Dispose();
                //關閉連線與釋放資源
                //_db為連線資料庫
            }
            base.Dispose(disposing);
        }
        */
        #endregion 關閉資料庫連結

        private ProductService _ProductService = new ProductService();
        int IndexPageSize = 3;//首頁顯示筆數
        int ListPageSize = 6;//列表顯示筆數

        /// <summary>
        /// 首頁
        /// </summary>
        public ActionResult Index(int _Page = 1)
        {
            List<Product> ProductList = new List<Product>();

            //判斷是否已有產品資料
            if (Session["ProductList"] == null)
            {
                //取得清單
                if (!_ProductService.GetMenu())
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                else
                { 
                    Session["MenuTypeList"] = _ProductService.MenuOptionList.Where(x => x.OptID == MappingCode.OptionItem.MenuType).ToList();
                    Session["PrdTypeList"] = _ProductService.MenuOptionList.Where(x => x.OptID == MappingCode.OptionItem.PrdType).ToList();
                }

                //取得活動圖檔
                Session["ActivityImage"] = _ProductService.GetImage(); 
                //取得Product
                Product request = new Product();
                ProductList = _ProductService.SearchProduct(request);

                if (ProductList == null)
                    return HttpNotFound();
                //if (ProductList.Count() == 0)
                //    return Content("<h3>" + MappingCode.ReturnMessage.NotFound + "</h3>");

                Session["ProductList"] = ProductList;
            }
            else
            {
                ProductList = (List<Product>)Session["ProductList"];
            }

            //判斷顯示頁數
            int currentPage = _Page < 1 ? 1 : _Page; //判斷第幾頁
            ProductList.OrderBy(x => x.PrdID); //依編號排序
            IPagedList<Product> result = ProductList.ToPagedList(currentPage, IndexPageSize);

            return View(result);
        }

        /// <summary>
        /// 關於
        /// </summary>
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        /// <summary>
        /// 聯絡方式
        /// </summary>
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// 找不到動作就跳回首頁
        /// </summary>
        protected override void HandleUnknownAction(string actionName)
        {
            //override = 覆蓋繼承的事件
            //HandleUnknownAction = 以上皆非
            //如果找不到動作（Action）或是輸入錯誤的動作名稱，一律跳回首頁
            Response.Redirect("~/Home/ReturnIndex");
        }

        #region Selete

        /// <summary>
        /// 返回首頁
        /// </summary>
        public ActionResult ReturnIndex()
        {
            Session["ProductList"] = null;

            return RedirectToAction("Index", "Home", 1);
        }

        /// <summary>
        /// 搜尋動作
        /// </summary>
        [HttpGet]
        public ActionResult Search(string PrdName, string PrdType, int PrdPriceS, int PrdPriceE)
        {
            //取得Product
            Product request = new Product()
            {
                PrdName = PrdName,
                PrdType = PrdType
            };
            List<Product> ProductList = _ProductService.SearchProduct(request);
            ProductList = (from m in ProductList
                           where m.PrdPrice >= PrdPriceS
                              && m.PrdPrice <= PrdPriceE
                           select m).ToList();
            Session["ProductList"] = ProductList;

            //顯示第一頁
            IPagedList<Product> result = ProductList.ToPagedList(1, IndexPageSize);

            if (ProductList.Count == 0)
            {
                ViewBag.Message = "<h1>" + MappingCode.ReturnMessage.NotFound + "</h1>";
                return View("Index", result);
            }
            else
            {
                return View("Index", result);
            }
        }

        /// <summary>
        /// 點選商品種類
        /// </summary>
        [HttpGet]
        public ActionResult IndexConfirm(string _PrdType)
        {
            //取得Product
            Product request = new Product()
            {
                PrdType = _PrdType
            };
            List<Product> ProductList = _ProductService.SearchProduct(request);
            Session["ProductList"] = ProductList;

            //顯示第一頁
            IPagedList<Product> result = ProductList.ToPagedList(1, IndexPageSize);

            if (ProductList.Count == 0)
            {
                ViewBag.Message = "<h1>" + MappingCode.ReturnMessage.NotFound + "</h1>";
                return View("Index", result);
            }
            else
            { 
                return View("Index", result);            
            }
        }

        /// <summary>
        /// 商品明細
        /// </summary>
        [HttpGet]
        public ActionResult Details(string _PrdID)
        {
            if (_PrdID == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            //取得Product
            Product request = new Product()
            {
                PrdID = _PrdID
            };
            List<Product> ProductDetails = _ProductService.SearchProduct(request);

            if (ProductDetails.Count == 0)
                return HttpNotFound();
            else
                return View(ProductDetails.FirstOrDefault());
        }

        /// <summary>
        /// 商品列表
        /// </summary>
        [HttpGet]
        public ActionResult List(int _Page = 1)
        {
            //取得Product
            Product request = new Product();
            List<Product> ProductList = _ProductService.SearchProduct(request);

            if (ProductList.Count == 0)
                return HttpNotFound();

            //判斷顯示頁數
            int currentPage = _Page < 1 ? 1 : _Page; //判斷第幾頁
            ProductList.OrderBy(x => x.PrdID); //依編號排序
            IPagedList<Product> result = ProductList.ToPagedList(currentPage, ListPageSize);

            return View(result);
        }

        #endregion

        #region Edit

        /// <summary>
        /// 修改畫面
        /// </summary>
        [HttpGet]
        public ActionResult Edit(string _PrdID)
        {
            if (_PrdID == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            //取得Product
            Product request = new Product()
            {
                PrdID = _PrdID
            };
            Product ProductDetails = _ProductService.SearchProduct(request).FirstOrDefault();

            if (ProductDetails == null)
                return HttpNotFound();
            else
            {
                //取得產品狀態
                if (!_ProductService.GetMenu())
                {
                    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
                }
                else
                {
                    ViewBag.PrdStatus = _ProductService.MenuOptionList.Where(x => x.OptID == MappingCode.OptionItem.PrdStatus).ToList();
                    ViewBag.PrdType = _ProductService.MenuOptionList.Where(x => x.OptID == MappingCode.OptionItem.PrdType).ToList();
                }

                return View("Edit", ProductDetails);
            }             
        }

        /// <summary>
        /// 修改動作
        /// </summary>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditConfirm([Bind(Include = "PrdID, PrdStatus, PrdType, PrdImage, PrdImageEXT, PrdName, PrdContent, PrdPrice, PrdQuantity, MenuType")] Product product)
        {
            if (product == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                // 確認是否有資料
                Product request = new Product() 
                {
                    PrdID = product.PrdID
                };
                Product ProductDetails = _ProductService.SearchProduct(request).FirstOrDefault();

                if (ProductDetails == null)
                {
                    var returnData = new
                    {
                        // 成功與否
                        IsSuccess = false,
                        Message = MappingCode.ReturnMessage.NotFound
                    };

                    return Content(Newtonsoft.Json.JsonConvert.SerializeObject(returnData), "application/json");
                }
                else
                { 
                    return Content(_ProductService.UpdateProduct(product), "application/json");
                }
            }
            else
            {
                //驗證錯誤訊息
                string msg = string.Empty;
                foreach (var m in ModelState.Values)
                {
                    if (m.Errors.Count > 0)
                    {
                        foreach (var error in m.Errors)
                        {
                            msg = msg + error.ErrorMessage + "\n";
                        }
                    }
                }

                var returnData = new
                {
                    // 成功與否
                    IsSuccess = false,
                    Message = msg
                };

                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(returnData), "application/json");
            }
        }

        /// <summary>
        /// 修改圖檔
        /// </summary>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditUploadFile(HttpPostedFileBase _FileName, string PrdID)
        {
            if (_FileName != null && _FileName.ContentLength > 0)
            {
                if (ModelState.IsValid)
                {
                    //取得Product
                    Product request = new Product() 
                    {
                        PrdID = PrdID
                    };
                    Product ProductDetails = _ProductService.SearchProduct(request).FirstOrDefault();

                    if (ProductDetails == null)
                        return HttpNotFound();
                    else
                    {
                        //存到資料夾
                        string FileName = Path.GetFileNameWithoutExtension(_FileName.FileName);          //檔名
                        string FileEXT = Path.GetExtension(_FileName.FileName);                          //副檔名
                        string FilePath = Path.Combine(Server.MapPath("~/Images/"), FileName + FileEXT); //路徑
                        _FileName.SaveAs(FilePath);

                        _ProductService.UpdatePrdImage(_FileName, FileEXT, PrdID);
                    }
                }
                else
                {
                    //驗證錯誤訊息
                    string msg = string.Empty;
                    foreach (var m in ModelState.Values)
                    {
                        if (m.Errors.Count > 0)
                        {
                            foreach (var error in m.Errors)
                            {
                                msg = msg + error.ErrorMessage + "\n";
                            }
                        }
                    }

                    var returnData = new
                    {
                        // 成功與否
                        IsSuccess = false,
                        Message = msg
                    };

                    return RedirectToAction("Edit", "Home", new { _PrdID = PrdID });
                }
            }
            else
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            return RedirectToAction("Edit", "Home", new { _PrdID = PrdID });
        }

        #endregion

        #region Delete

        /// <summary>
        /// 刪除動作
        /// </summary>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Delete(string PrdID)
        {
            if (ModelState.IsValid)
            {
                // 確認是否有資料
                Product request = new Product()
                {
                    PrdID = PrdID
                };
                Product ProductDetails = _ProductService.SearchProduct(request).FirstOrDefault();

                if (ProductDetails == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    _ProductService.RemoveProduct(PrdID);
                }
            }
            else
            {
                //驗證錯誤訊息
                string msg = string.Empty;
                foreach (var m in ModelState.Values)
                {
                    if (m.Errors.Count > 0)
                    {
                        foreach (var error in m.Errors)
                        {
                            msg = msg + error.ErrorMessage + "\n";
                        }
                    }
                }

                var returnData = new
                {
                    // 成功與否
                    IsSuccess = false,
                    Message = msg
                };

                return RedirectToAction("Edit", "Home", new { _PrdID = PrdID });
            }

            return RedirectToAction("List", "Home");
        }

        #endregion

        #region Create

        /// <summary>
        /// 新增畫面
        /// </summary>
        public ActionResult Create()
        {
            //取得產品狀態
            if (!_ProductService.GetMenu())
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            else
            {
                ViewBag.PrdStatus = _ProductService.MenuOptionList.Where(x => x.OptID == MappingCode.OptionItem.PrdStatus).ToList();
                ViewBag.PrdType = _ProductService.MenuOptionList.Where(x => x.OptID == MappingCode.OptionItem.PrdType).ToList();
            }

            return View();
        }

        /// <summary>
        /// 新增動作
        /// </summary>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateConfirm(Product product)
        {
            if (product == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                //圖檔資料
                HttpPostedFileBase _FileName = Request.Files["_FileName"];

                if (_FileName != null && _FileName.ContentLength > 0)
                {
                    //存到資料夾
                    string FileName = Path.GetFileNameWithoutExtension(_FileName.FileName);          //檔名
                    string FileEXT = Path.GetExtension(_FileName.FileName);                          //副檔名
                    string FilePath = Path.Combine(Server.MapPath("~/Images/"), FileName + FileEXT); //路徑
                    _FileName.SaveAs(FilePath);

                    //轉成byte
                    byte[] FileBytes;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        _FileName.InputStream.CopyTo(ms);
                        FileBytes = ms.GetBuffer();
                    }

                    product.PrdImage = FileBytes;
                    product.PrdImageEXT = FileEXT.TrimStart('.');
                }

                return Content(_ProductService.AddProduct(product), "application/json");
                //return RedirectToAction("List", "Home");
            }
            else
            {
                //驗證錯誤訊息
                string msg = string.Empty;
                foreach (var m in ModelState.Values)
                {
                    if (m.Errors.Count > 0)
                    {
                        foreach (var error in m.Errors)
                        {
                            msg = msg + error.ErrorMessage + "\n";
                        }
                    }
                }

                var returnData = new
                {
                    // 成功與否
                    IsSuccess = false,
                    Message = msg
                };

                return Content(Newtonsoft.Json.JsonConvert.SerializeObject(returnData), "application/json");
            }
        }

        #endregion

        #region Validated

        /// <summary>
        /// 檢核動作
        /// </summary>
        public bool ValidatedQuery(string Type, string Value)
        {
            bool Result = true;

            switch (Type)
            {
                case "Add":
                    Result = _ProductService.ValidatePKey(Value);
                    break;

                default:
                    break;  
            }
            return Result;
        }

        #endregion

        #region Cart

        public ActionResult AddToCart(string _PrdID, int _Quantity = 1)
        {
            //取得Session["Cart"]
            Cart currentCart = Operation.GetCurrentCart();

            //如果Session["Cart"]無商品就新增一筆資料回傳
            //if (cart.CartList.Count == 0)
            //{
            //    cart.CartList.Add(
            //        new Models.CartItem() {
            //            PrdID = "01A0001",
            //            PrdName = "母親牌便當",
            //            PrdPrice = 238,
            //            BuyQuantity = 1
            //        });
            //}
            //else //如果有商品，就將第一筆資料購買數量+1回傳
            //{
            //    cart.CartList.First().BuyQuantity += 1;
            //}

            //回傳目前購物車加總計算金額
            return Content(string.Format("目前購物車總計 ${0}元", ""/*cart.TotalAmount*/));
        }

        #endregion

        public ActionResult Test()
        {
            return View();
        }

        public int[] TwoSum(int[] nums, int target)
        {
            int[] b = null;

            for (int i = 0; i <= nums.Length; i++)
            {
                if (nums[i] + nums[i + 1] == target)
                {
                    int[] a = { i, i + 1 };
                    b = a;
                }
            }

            return b;
        }



        //        //判斷信箱
        //        var regex = / ^([a - zA - Z0 - 9_\.\-\+]) +\@@(([a - zA - Z0 - 9\-])+\.)+([a - zA - Z0 - 9]{2,4})+$/;
        //        if (regex.test($("#Sname").val()) &&
        //            //判斷是否密碼相同
        //            ($("#Spassword").val() == $("#RSpassword").val())){
        //            $.ajax({
        //    url: '@Url.Action("SignUp", "Login")',
        //                data:
        //        {
        //        Sname: $("#Sname").val(),
        //                    Spassword: $("#Spassword").val()
        //                },
        //                type: 'post',
        //                success: function(data) {
        //        },
        //            });
        //        }
        //        else
        //{
        //            $("#errorMessage").text("請將輸入密碼一致，並且輸入正確的信箱格式");
        //}
    }
}
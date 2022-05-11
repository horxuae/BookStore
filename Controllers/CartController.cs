using BookStore.Models;
using BookStore.Services;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class CartController : Controller
    {
        private OrderService _OrderService = new OrderService();
        int OrderPageSize = 6;//列表顯示筆數

        #region Cart

        /// <summary>
        /// 購物車畫面
        /// </summary>
        public ActionResult GetCart()
        {
             return PartialView("_CartPartial");
        }

        /// <summary>
        /// 新增購物車動作
        /// </summary>
        public ActionResult AddToCart(string PrdID, int Quantity = 1)
        {
            //取得購物車Session
            Cart currentCart = Operation.GetCurrentCart();

            //新增購物車資料
            currentCart.AddCartData(PrdID, Quantity);

            //return Content(string.Format("目前購物車總計 ${0}元", currentCart.TotalAmount));
            return PartialView("_CartPartial");
        }

        /// <summary>
        /// 移除購物車動作
        /// </summary>
        public ActionResult RemoveToCart(string _PrdID)
        {
            //取得購物車Session
            Cart currentCart = Operation.GetCurrentCart();

            //移除購物車資料
            currentCart.RemoveCartData(_PrdID);

            return PartialView("_CartPartial");
        }

        /// <summary>
        /// 清空購物車動作
        /// </summary>
        public ActionResult ClearToCart()
        {
            //取得購物車Session
            Cart currentCart = Operation.GetCurrentCart();

            //移除購物車資料
            currentCart.ClearCartData();

            return PartialView("_CartPartial");
        }

        #endregion

        #region Order

        /// <summary>
        /// 結帳畫面
        /// </summary>
        [HttpGet]
        public ActionResult Order(int _Page = 1)
        {
            //取得購物車Session
            Cart currentCart = BookStore.Models.Operation.GetCurrentCart();

            //判斷顯示頁數
            int currentPage = _Page < 1 ? 1 : _Page; //判斷第幾頁
            currentCart.OrderBy(x => x.PrdID); //依編號排序
            IPagedList<CartItem> result = currentCart.ToPagedList(currentPage, OrderPageSize);

            return View(result);
        }

        #endregion
    }
}
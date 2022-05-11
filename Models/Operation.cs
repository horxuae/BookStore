using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace BookStore.Models
{
    //購物車Session
    public static class Operation
    {
        //取得Cart的Session物件
        [WebMethod(EnableSession = true)] //啟用Session
        public static Cart GetCurrentCart()
        {
            //確認參考System.Web.HttpContext.Current是否為空
            if (HttpContext.Current != null)
            {
                //如果Session["Cart"]不存在，則新增一個空的Session["Cart"]
                if (HttpContext.Current.Session["Cart"] == null)
                {
                    //取得購物車(List)類別
                    Cart cart = new Cart();
                    HttpContext.Current.Session["Cart"] = cart;
                }

                //回傳已存在或新增的Session["Cart"]
                return (Cart)HttpContext.Current.Session["Cart"];
            }
            else
            {
                throw new InvalidOperationException("System.Web.HttpContext.Current為空，請檢查");
            }
        }
    }
}
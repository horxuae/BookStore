using BookStore.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    //購物車類別(List)
    [Serializable] //可序列化
    public class Cart : IEnumerable<CartItem>
    {
        //建構子
        public Cart()
        {
            this.CartList = new List<CartItem>();
        }

        //用於儲存所有購物資料
        private List<CartItem> CartList;

        //取得加總計算金額
        public decimal TotalAmount
        {
            get 
            {
                decimal Amount = 0;

                foreach (CartItem m in this.CartList)
                {
                    Amount += m.BuyAmount;
                }
                return Amount;
            }
        }

        //新增購物車資料
        public bool AddCartData(string _PrdID, int _Quantity)
        {
            CartItem CartItem = (from m in this.CartList
                                 where m.PrdID == _PrdID
                                 select m).FirstOrDefault();

            //判斷是否有相同的PrdID存在在購物車內
            if (CartItem is null)
            {
                //不存在購物車內，則新增一筆
                ProductService _ProductService = new ProductService();

                //取得Product
                Product product = new Product()
                {
                    PrdID = _PrdID
                };
                Product ProductDetails = _ProductService.SearchProduct(product).FirstOrDefault();

                //將Product轉為CartItem
                CartItem cartItem = new CartItem()
                {
                    PrdID = _PrdID,
                    PrdName = ProductDetails.PrdName,
                    PrdPrice = ProductDetails.PrdPrice,
                    BuyQuantity = _Quantity
                };

                this.CartList.Add(cartItem);
            }
            else
            {   
                //存在在購物車內，則將商品數量累加
                CartItem.BuyQuantity += _Quantity;
            }
            return true;
        }

        //移除購物車資料
        public bool RemoveCartData(string _PrdID)
        {
            CartItem CartItem = (from m in this.CartList
                                 where m.PrdID == _PrdID
                                 select m).FirstOrDefault();

            //判斷是否有相同的PrdID存在在購物車內
            if (CartItem is null)
            {
                //不存在購物車內，則不做任何動作
            }
            else
            {
                //存在在購物車內，則將商品資料移除
                this.CartList.Remove(CartItem);
            }
            return true;
        }

        //清空購物車資料
        public bool ClearCartData()
        {
            this.CartList.Clear();
            
            return true;
        }

        IEnumerator<CartItem> IEnumerable<CartItem>.GetEnumerator()
        {
            return this.CartList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.CartList.GetEnumerator();
        }
    }

    //購物車單一類別
    [Serializable] //可序列化
    public class CartItem
    {
        //商品編號
        public string PrdID { get; set; }

        //商品名稱
        public string PrdName { get; set; }

        //商品售價
        public int PrdPrice { get; set; }

        //購買數量
        public int BuyQuantity { get; set; }

        //計算金額
        public decimal BuyAmount
        {
            get
            {
                return this.PrdPrice * this.BuyQuantity;
            }
        }
    }
}
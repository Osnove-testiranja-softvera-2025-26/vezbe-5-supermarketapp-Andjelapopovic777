using NUnit.Framework;
using OTS_Supermarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace OTS_Supermarket.Test
{
    [TestFixture]
    public class CartTest
    {
        [Test]
        public void AddOneToCart_ShouldAddItemToCart_Success()
        {
            //ARRANGE
            Cart cart = new Cart();
            Monitor monitor = new Monitor();

            //ACT
            cart.AddOneToCart(monitor);

            //ASSERT
            Assert.That(cart.Size, Is.EqualTo(1));
            Assert.That(cart.Amount, Is.EqualTo(100));

        }

        [Test]
        public void AddMultipleToCart_ShouldAddMultipleItems_Success()
        {
            // ARRANGE
            Cart cart = new Cart();
            Keyboard keyboard = new Keyboard();

            // ACT
            cart.AddMultipleToCart(keyboard, 3);

            // ASSERT
            Assert.That(cart.Size, Is.EqualTo(3));
            Assert.That(cart.Amount, Is.EqualTo(300)); // pretpostavka cena 100
        }

        [Test]
        public void DeleteAll_ShouldClearCart()
        {
            // ARRANGE
            Cart cart = new Cart();
            cart.AddOneToCart(new Monitor());

            // ACT
            cart.DeleteAll();

            // ASSERT
            Assert.That(cart.Size, Is.EqualTo(0));
            Assert.That(cart.Amount, Is.EqualTo(0));
        }
        //

        [Test]
        public void Calculate_ShouldThrowException_WhenDateIsInPast()
        {
            // ARRANGE
            Cart cart = new Cart();
            cart.AddOneToCart(new Monitor());
            DateTime invalidDate = DateTime.Now.AddDays(-1);

            // ACT & ASSERT
            Assert.Throws<Exception>(() => cart.Calculate(invalidDate, 2000));
        }
        


        [Test]
        public void Calculate_ShouldThrowException_WhenDateIsTooFar()
        {
            // ARRANGE
            Cart cart = new Cart();
            cart.AddOneToCart(new Monitor());
            DateTime invalidDate = DateTime.Now.AddDays(8);

            // ACT & ASSERT
            Assert.Throws<Exception>(() => cart.Calculate(invalidDate, 2000));
        }


        [Test]
        public void Calculate_ShouldReturnFullPrice_WhenNoDiscountConditionsMet()
        {
            // ARRANGE
            Cart cart = new Cart();
            cart.AddOneToCart(new Monitor());
            DateTime validDate = DateTime.Now.AddDays(2);

            // ACT
            double result = cart.Calculate(validDate, 2000);

            // ASSERT
            Assert.That(result, Is.EqualTo(cart.Amount));
        }


        [Test]
        public void Calculate_ShouldApplyDiscount_WhenThreeLaptops()
        {
            // ARRANGE
            Cart cart = new Cart();
            cart.AddMultipleToCart(new Laptop(), 3);
            DateTime validDate = DateTime.Now.AddDays(2);

            // ACT
            double result = cart.Calculate(validDate, 5000);

            // ASSERT
            Assert.That(result, Is.LessThan(cart.Amount));
        }



        [Test]
        public void Calculate_ShouldApplyHighestDiscount_WhenMultipleConditionsMet()
        {
            // ARRANGE
            Cart cart = new Cart();

            cart.AddMultipleToCart(new Laptop(), 3);
            cart.AddMultipleToCart(new Computer(), 4);

            DateTime validDate = DateTime.Now.AddDays(2);

            // ACT
            double result = cart.Calculate(validDate, 10000);

            // ASSERT
            Assert.That(result, Is.LessThan(cart.Amount));
        }



        [Test]
        public void Calculate_ShouldLimitDiscountToFivePercent_OnWeekend()
        {
            // ARRANGE
            Cart cart = new Cart();
            cart.AddMultipleToCart(new Laptop(), 3);

            // pronađi prvi subotu
            DateTime date = DateTime.Now;
            while (date.DayOfWeek != DayOfWeek.Saturday)
            {
                date = date.AddDays(1);
            }

            // ACT
            double result = cart.Calculate(date, 5000);

            double expectedMin = cart.Amount * 0.95;

            // ASSERT
            Assert.That(result, Is.GreaterThanOrEqualTo(expectedMin));
        }




        [Test]
        public void Calculate_ShouldThrowException_WhenBudgetIsNotEnough()
        {
            // ARRANGE
            Cart cart = new Cart();
            cart.AddMultipleToCart(new Laptop(), 3);
            DateTime validDate = DateTime.Now.AddDays(2);

            // ACT & ASSERT
            Assert.Throws<Exception>(() => cart.Calculate(validDate, 100));
        }



    } 

}

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.ComprarMerch_UIT
{
    public class SelectMerch_PO
    {
        private readonly IWebDriver _driver;
        private readonly ITestOutputHelper _output;

        public SelectMerch_PO(IWebDriver driver, ITestOutputHelper output)
        {
            _driver = driver;
            _output = output;
        }

        private void EsperarVisible(By by)
        {
            try
            {
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                wait.Until(d => d.FindElement(by).Displayed);
            }
            catch { }
        }

        public void FiltrarPorTipo(string tipo)
        {
            Thread.Sleep(1500);
            EsperarVisible(By.Id("selectTipo"));

            var select = new SelectElement(_driver.FindElement(By.Id("selectTipo")));
            select.SelectByText(tipo);

            var btn = _driver.FindElement(By.Id("searchMerch"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", btn);

            Thread.Sleep(1500);
        }

        public void AddFirstProductoToCart()
        {
            Thread.Sleep(1500);
            var botones = _driver.FindElements(By.CssSelector("#TableOfMerch tbody button"));

            if (botones.Count > 0)
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", botones.First());
                Thread.Sleep(500);
            }
            else
            {
                throw new Exception("No hay productos para añadir.");
            }
        }

        public void RemoveFirstItemFromCart()
        {
            Thread.Sleep(500);

            try
            {
                var btnCarrito = _driver.FindElement(By.Id("showCarrito"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", btnCarrito);
                Thread.Sleep(500);
            }
            catch { }

            var botonesBorrar = _driver.FindElements(By.XPath("//button[contains(text(), 'Quitar')]"));

            if (botonesBorrar.Count > 0)
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", botonesBorrar.First());
                Thread.Sleep(1000);
            }
        }

        public string GetTotalText()
        {
            try
            {
                Thread.Sleep(500);
                return _driver.FindElement(By.CssSelector(".h4.fw-bold")).Text;
            }
            catch
            {
                return "";
            }
        }

        public void ClickTramitar()
        {
            Thread.Sleep(500);
            var btn = _driver.FindElement(By.Id("ComprarMerch"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", btn);
            Thread.Sleep(1000);
        }

        public bool ComprarNotAvailable()
        {
            try
            {
                var btn = _driver.FindElement(By.Id("ComprarMerch"));
                return !btn.Enabled;
            }
            catch
            {
                return true;
            }
        }

        public bool CheckListOfProductos(string textoEsperado)
        {
            Thread.Sleep(500);
            return _driver.PageSource.Contains(textoEsperado);
        }
    }
}
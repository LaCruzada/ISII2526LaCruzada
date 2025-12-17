using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Threading;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.PedirBocadillo_UIT
{
    public class SelectPedirBocadillo_PO
    {
        private readonly IWebDriver _driver;
        private readonly ITestOutputHelper _output;

        public SelectPedirBocadillo_PO(IWebDriver driver, ITestOutputHelper output)
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

        public void SearchBocadillos(string nombre)
        {
            Thread.Sleep(2000); 
            EsperarVisible(By.Id("filtroNombre"));

            var input = _driver.FindElement(By.Id("filtroNombre"));
            input.Clear();

            input = _driver.FindElement(By.Id("filtroNombre"));
            input.SendKeys(nombre);
            input.SendKeys(Keys.Enter);

            var btn = _driver.FindElement(By.Id("btnBuscar"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", btn);

            Thread.Sleep(1500); 
        }

        public void AddFirstBocadilloToCart()
        {
            Thread.Sleep(1500);
            var botones = _driver.FindElements(By.CssSelector(".btn-outline-success"));

            if (botones.Count > 0)
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", botones.First());
                Thread.Sleep(500);
            }
            else
            {
                throw new Exception("No hay bocadillos para añadir.");
            }
        }

        public void RemoveFirstItemFromCart()
        {
            Thread.Sleep(500);
            var botonesBorrar = _driver.FindElements(By.CssSelector(".card-body .btn-danger"));

            if (botonesBorrar.Count > 0)
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", botonesBorrar.First());
                Thread.Sleep(1000); 
            }
        }

        public void RemoveItemFromCart(int indice)
        {
            Thread.Sleep(500);
            var botonesBorrar = _driver.FindElements(By.CssSelector(".card-body .btn-danger"));

            if (botonesBorrar.Count > indice)
            {
                var boton = botonesBorrar[indice];
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", botonesBorrar.First());
                Thread.Sleep(1000);
            }
        }

        public string GetTotalText()
        {
            try
            {
                return _driver.FindElement(By.CssSelector(".card-footer h5")).Text;
            }
            catch
            {
                return "";
            }
        }

        public void ClickTramitar()
        {
            Thread.Sleep(500);
            var btn = _driver.FindElement(By.Id("btnTramitar"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", btn);
        }

        public bool ComprarNotAvailable()
        {
            try
            {
                var btn = _driver.FindElement(By.Id("btnTramitar"));
                return !btn.Enabled;
            }
            catch
            {
                return true;
            }
        }

        public bool CheckListOfBocadillos(string nombreEsperado)
        {
            return _driver.PageSource.Contains(nombreEsperado);
        }

      
    }
}
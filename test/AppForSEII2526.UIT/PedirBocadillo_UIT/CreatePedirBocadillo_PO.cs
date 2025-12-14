using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.PedirBocadillo_UIT
{
    public class CreatePedirBocadillo_PO
    {
        private readonly IWebDriver _driver;
        private readonly ITestOutputHelper _output;

        public CreatePedirBocadillo_PO(IWebDriver driver, ITestOutputHelper output)
        {
            _driver = driver;
            _output = output;
        }

        public void RellenarFormulario(string nombre, string apellido, string email, string metodoPago)
        {
            Thread.Sleep(1000);
            if (!string.IsNullOrEmpty(nombre))
            {
                var inputNombre = _driver.FindElement(By.Id("inputNombre"));
                inputNombre.Clear();
                inputNombre.SendKeys(nombre);
                inputNombre.SendKeys(Keys.Tab); 
                Thread.Sleep(200); 
            }
            if (!string.IsNullOrEmpty(apellido))
            {
                var inputApellido = _driver.FindElement(By.Id("inputApellido1"));
                inputApellido.SendKeys(apellido);
                inputApellido.SendKeys(Keys.Tab); 
            }
            if (!string.IsNullOrEmpty(email))
            {
                var inputEmail = _driver.FindElement(By.Id("inputEmail"));
                inputEmail.SendKeys(email);
                inputEmail.SendKeys(Keys.Tab);
            }
            if (!string.IsNullOrEmpty(metodoPago))
            {
                var selectElement = _driver.FindElement(By.Id("selectPago"));
                var select = new SelectElement(selectElement);
                select.SelectByValue(metodoPago);
            }
        }

        public void ContinuarCompra()
        {
            var btn = _driver.FindElement(By.Id("btnPagar"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", btn);
        }

        public void ConfirmarCompraModal()
        {
            Thread.Sleep(1000);
            var botonesModal = _driver.FindElements(By.CssSelector(".modal-footer .btn-primary"));

            if (botonesModal.Count > 0)
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", botonesModal[0]);
            }
            else
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("document.querySelector('.modal-footer .btn-primary').click();");
            }
        }

        public void ClickSeguirComprando()
        {
            Thread.Sleep(500);
            var btn = _driver.FindElement(By.CssSelector(".btn-secondary"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", btn);
        }
        public string GetNombreValue()
        {
            Thread.Sleep(500);
            return _driver.FindElement(By.Id("inputNombre")).GetAttribute("value");
        }
    }
}
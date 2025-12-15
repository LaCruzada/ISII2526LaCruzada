using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.ComprarMerch_UIT
{
    public class CreateMerch_PO
    {
        private readonly IWebDriver _driver;
        private readonly ITestOutputHelper _output;

        public CreateMerch_PO(IWebDriver driver, ITestOutputHelper output)
        {
            _driver = driver;
            _output = output;
        }

        public void RellenarFormulario(string nombre, string apellido, string email, string direccion, string metodoPago)
        {
            Thread.Sleep(1000);

            if (!string.IsNullOrEmpty(nombre))
            {
                var inputNombre = _driver.FindElement(By.CssSelector("input[placeholder='Ej: Ana']"));
                inputNombre.Clear();
                inputNombre.SendKeys(nombre);
                inputNombre.SendKeys(Keys.Tab);
                Thread.Sleep(200);
            }

            if (!string.IsNullOrEmpty(apellido))
            {
                var inputApellido = _driver.FindElement(By.CssSelector("input[placeholder='Ej: García']"));
                inputApellido.Clear();
                inputApellido.SendKeys(apellido);
                inputApellido.SendKeys(Keys.Tab);
            }

            if (!string.IsNullOrEmpty(email))
            {
                var inputEmail = _driver.FindElement(By.CssSelector("input[placeholder='correo@ejemplo.com']"));
                inputEmail.Clear();
                inputEmail.SendKeys(email);
                inputEmail.SendKeys(Keys.Tab);
            }

            if (!string.IsNullOrEmpty(direccion))
            {
                var inputDireccion = _driver.FindElement(By.CssSelector("input[placeholder='C/ Ejemplo, 123, 1ºA']"));
                inputDireccion.Clear();
                inputDireccion.SendKeys(direccion);
                inputDireccion.SendKeys(Keys.Tab);
            }

            if (!string.IsNullOrEmpty(metodoPago))
            {
                var selectElement = _driver.FindElement(By.TagName("select"));
                var select = new SelectElement(selectElement);
                select.SelectByValue(metodoPago);
            }
        }

        public void ContinuarCompra()
        {
            var btn = _driver.FindElement(By.XPath("//button[contains(text(), 'CONFIRMAR Y PAGAR')]"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].scrollIntoView(true);", btn);
            Thread.Sleep(500);
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", btn);
        }

        public void ConfirmarCompraModal()
        {
            Thread.Sleep(1000);
            var botonesModal = _driver.FindElements(By.XPath("//button[contains(text(), 'Confirmar')]"));

            if (botonesModal.Count > 0)
            {
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", botonesModal[0]);
                Thread.Sleep(2000);
            }
        }

        public void ClickVolverAtras()
        {
            Thread.Sleep(500);
            var btn = _driver.FindElement(By.CssSelector(".btn-outline-secondary"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", btn);
        }

        public string GetNombreValue()
        {
            Thread.Sleep(500);
            return _driver.FindElement(By.CssSelector("input[placeholder='Ej: Ana']")).GetAttribute("value");
        }
    }
}

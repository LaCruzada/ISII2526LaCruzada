using OpenQA.Selenium;
using System.Threading;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.PedirBocadillo_UIT
{
    public class DetallePedido_PO
    {
        private readonly IWebDriver _driver;
        private readonly ITestOutputHelper _output;

        public DetallePedido_PO(IWebDriver driver, ITestOutputHelper output)
        {
            _driver = driver;
            _output = output;
        }

        public bool EsPaginaDetalle()
        {
            Thread.Sleep(2000); 
            return _driver.Url.Contains("DetallePedido");
        }

        public bool ContieneTexto(string texto)
        {
            return _driver.PageSource.Contains(texto);
        }
    }
}
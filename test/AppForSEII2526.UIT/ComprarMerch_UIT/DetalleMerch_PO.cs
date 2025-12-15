using OpenQA.Selenium;
using System.Threading;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.ComprarMerch_UIT
{
    public class DetalleMerch_PO
    {
        private readonly IWebDriver _driver;
        private readonly ITestOutputHelper _output;

        public DetalleMerch_PO(IWebDriver driver, ITestOutputHelper output)
        {
            _driver = driver;
            _output = output;
        }

        public bool EsPaginaDetalle()
        {
            Thread.Sleep(2000);
            return _driver.Url.Contains("detallecompra");
        }

        public bool ContieneTexto(string texto)
        {
            return _driver.PageSource.Contains(texto);
        }
    }
}

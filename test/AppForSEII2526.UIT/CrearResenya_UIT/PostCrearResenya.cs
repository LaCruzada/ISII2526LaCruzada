using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;
using OpenQA.Selenium.Support.UI;

namespace AppForSEII2526.UIT.Resenya_UIT
{
    public class PostCrearResenya_PO : PageObject
    {


        private By _nombreUsuarioBy = By.Id("NombreUsuario");
        private By _tituloResenyaBy = By.Id("TituloResenya");
        private By _descripcionResenyaBy = By.Id("DescripcionResenya");
        private By _valoracionGeneralBy = By.Id("ValoracionGeneral");

        private string _puntuacionBocadilloPrefix = "Puntuacion_";

        private By _botonPublicarBy = By.Id("PublicarResenya");
        private By _botonModificarBocadillosBy = By.Id("ModificarBocadillos");

        private By _errorsShownBy = By.Id("ErrorsShown");

        private By _resumenResenyaBy = By.Id("ResumenResenya");
        private By _tablaResumenBocadillosBy = By.Id("TableResumenBocadillos");


        private IWebElement _nombreUsuario() => _driver.FindElement(_nombreUsuarioBy);
        private IWebElement _tituloResenya() => _driver.FindElement(_tituloResenyaBy);
        private IWebElement _descripcionResenya() => _driver.FindElement(_descripcionResenyaBy);
        private IWebElement _valoracionGeneral() => _driver.FindElement(_valoracionGeneralBy);
        private IWebElement _botonPublicar() => _driver.FindElement(_botonPublicarBy);
        private IWebElement _botonModificarBocadillos() => _driver.FindElement(_botonModificarBocadillosBy);

        public PostCrearResenya_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        
        // Rellenar formulario
        

        public void SetNombreUsuario(string nombre)
        {
            _nombreUsuario().SendKeys(nombre);
        }

        public void SetTituloResenya(string titulo)
        {
            _tituloResenya().SendKeys(titulo);
        }

        public void SetDescripcionResenya(string descripcion)
        {
            _descripcionResenya().SendKeys(descripcion);
        }

        public void SetValoracionGeneral(string estrellas)
        {
            var select = new SelectElement(_valoracionGeneral());
            select.SelectByText(estrellas);
        }


        // Puntuación por bocadillo


        public void SetPuntuacionBocadillo(string bocadilloId, string puntuacion)
        {
            var selectBy = By.Id(_puntuacionBocadilloPrefix + bocadilloId);
            var select = new SelectElement(_driver.FindElement(selectBy));
            select.SelectByText(puntuacion);
        }

        // Publicar reseña


        public void PublicarResenya()
        {
            _botonPublicar().Click();
            System.Threading.Thread.Sleep(300);
        }

        //  Modificar bocadillos (ESC-06)

        public void ModificarBocadillos()
        {
            _botonModificarBocadillos().Click();
            System.Threading.Thread.Sleep(300);
        }

        //  Botón publicar habilitado

        public bool IsEnabledPublicar()
        {
            return _botonPublicar().Enabled;
        }

        // Mensajes de error (ESC-05)

        public bool CheckMessageError(string errorMessage)
        {
            IWebElement actualErrorShown = _driver.FindElement(_errorsShownBy);
            _output.WriteLine($"Mensaje mostrado: {actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(errorMessage);
        }

        // Resumen de reseña (ESC-01)


        public bool IsResumenResenyaVisible()
        {
            try
            {
                _driver.FindElement(_resumenResenyaBy);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CheckResumenBocadillos(List<string[]> expectedBocadillos)
        {
            return CheckBodyTable(expectedBocadillos, _tablaResumenBocadillosBy);
        }
    }
}

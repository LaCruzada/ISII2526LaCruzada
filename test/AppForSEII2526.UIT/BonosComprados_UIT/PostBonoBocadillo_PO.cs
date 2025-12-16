using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.BonosComprados_UIT
{
    public class PostBonoBocadillo_PO : PageObject
    {

        // Se definen los localizadores de cada uno de los elementos de la página con el que se a interactuar usando su id.
        private By _nombreBy = By.Id("Nombre");
        private By _apellido1By = By.Id("Apellido1");
        private By _apellido2By = By.Id("Apellido2");
        private By _metodoPagoBy = By.Id("MetodoPago");
        private By _botonSubmitBy = By.Id("Submit");
        private By _botonModificarBonosBy = By.Id("ModificarBonos");
        private By _botonConfirmarBy = By.Id("ConfirmButton");
        private By _tablaBonoItemsBy = By.Id("TableOfBonoItems");
        private By _errorsShownBy = By.Id("ErrorsShown");

        // Este código es equivalente a: // private IWebElement _nombre() { return _driver.FindElement(By.Id("_nombreBy")); }
        private IWebElement _nombre() => _driver.FindElement(_nombreBy);
        private IWebElement _apellido1() => _driver.FindElement(_apellido1By);
        private IWebElement _apellido2() => _driver.FindElement(_apellido2By);
        private IWebElement _metodoPago() => _driver.FindElement(_metodoPagoBy);
        private IWebElement _botonSubmit() => _driver.FindElement(_botonSubmitBy);
        private IWebElement _botonModificarBonos() => _driver.FindElement(_botonModificarBonosBy);

        public PostBonoBocadillo_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        // Pulsar el botón modificar bonos.
        public void ModificarBonos()
        {
            _botonModificarBonos().Click();
            System.Threading.Thread.Sleep(200);
        }

        // Pulsar el botón continuar con la compra.
        public void ContinuarCompra()
        {
            _botonSubmit().Click();
            System.Threading.Thread.Sleep(200);
        }

        // Pulsar el botón confirmar en el modal.
        public void ConfirmarCompra()
        {
            WaitForBeingClickable(_botonConfirmarBy);
            _driver.FindElement(_botonConfirmarBy).Click();
            System.Threading.Thread.Sleep(200);
        }

        public void setNombre(string nombre)
        {
            _nombre().SendKeys(nombre);
        }

        public void setApellido1(string apellido1)
        {
            _apellido1().SendKeys(apellido1);
        }

        public void setApellido2(string apellido2)
        {
            _apellido2().SendKeys(apellido2);
        }

        public void setMetodoPago(string metodoPago)
        {
                var selectElement = _driver.FindElement(_metodoPagoBy);
                var select = new SelectElement(selectElement);
                select.SelectByValue(metodoPago);
           
        }

        // Devuelve si el botón Submit está activo o no.
        public bool isEnabledSubmit()
        {
            IWebElement botonSubmit = _botonSubmit();

            return botonSubmit.Enabled;
        }

        // Este método permite comprobar si la lista de bonos mostrada en la tabla coincide con la esperada o no.
        public bool CompruebaListaBonos(List<string[]> expectedBonos)
        {
            return CheckBodyTable(expectedBonos, _tablaBonoItemsBy);
        }

        // Este método permite comprobar si el mensaje de error coincide con el esperado.
        public bool CheckMessageError(string errorMessage)
        {
            IWebElement actualErrorShown = _driver.FindElement(_errorsShownBy);
            _output.WriteLine($"actual Message shown:{actualErrorShown.Text}");
            return actualErrorShown.Text.Contains(errorMessage);
        }
    }
}

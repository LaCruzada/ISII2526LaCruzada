using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.BonosComprados_UIT
{
    public class SelectBonoBocadillo_PO : PageObject
    {
        By inputNombre = By.Id("inputNombre");
        By inputTipo = By.Id("inputTipo");
        By buttonSearchBonos = By.Id("searchBonos");
        By tableOfBonosBy = By.Id("TableOfBonos");
        By errorShownBy = By.Id("ErrorsShown");
        By buttonComprarBonos = By.Id("ComprarBonos");

        public SelectBonoBocadillo_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SearchBonos(string nombre, string tipo)
        {
            WaitForBeingClickable(inputNombre);
            
            var inputNombreElement = _driver.FindElement(inputNombre);
            inputNombreElement.Clear();
            if (!string.IsNullOrEmpty(nombre))
            {
                inputNombreElement.SendKeys(nombre);
            }

            if (!string.IsNullOrEmpty(tipo))
            {
                var inputTipoElement = _driver.FindElement(inputTipo);
                inputTipoElement.Clear();
                inputTipoElement.SendKeys(tipo);
            }

            _driver.FindElement(buttonSearchBonos).Click();
            Thread.Sleep(2000);
            
            try
            {
                WaitForBeingVisible(tableOfBonosBy);
            }
            catch
            {
                _output.WriteLine("No se encontró la tabla de bonos, posiblemente no hay resultados");
            }
        }

        public bool CheckListOfBonos(List<string[]> expectedBonos)
        {
            try
            {
                WaitForBeingVisible(tableOfBonosBy);
                return CheckBodyTable(expectedBonos, tableOfBonosBy);
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Error al verificar la lista de bonos: {ex.Message}");
                return false;
            }
        }

        public bool CheckMessageError(string errorMessage)
        {
            try
            {
                WaitForBeingVisible(errorShownBy);
                IWebElement actualErrorShown = _driver.FindElement(errorShownBy);
                _output.WriteLine($"Mensaje de error mostrado: {actualErrorShown.Text}");
                return actualErrorShown.Text.Contains(errorMessage, StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                _output.WriteLine($"No se encontró mensaje de error: {ex.Message}");
                return false;
            }
        }

        public void AddBonoToCartByName(string nombreBono)
        {
            _output.WriteLine($"Buscando bono: {nombreBono}");
            
            Thread.Sleep(1500);
            WaitForBeingVisible(tableOfBonosBy);

            var tabla = _driver.FindElement(tableOfBonosBy);
            var filas = tabla.FindElements(By.TagName("tr"));

            foreach (var fila in filas)
            {
                try
                {
                    var celdas = fila.FindElements(By.TagName("td"));
                    if (celdas.Count > 0)
                    {
                        string nombreEnFila = celdas[0].Text;
                        
                        if (nombreEnFila.Equals(nombreBono, StringComparison.OrdinalIgnoreCase))
                        {
                            _output.WriteLine($"Bono encontrado: {nombreEnFila}");
                            
                            var botonAnadir = celdas[celdas.Count - 1].FindElement(By.TagName("button"));
                            
                            if (!botonAnadir.Enabled)
                            {
                                _output.WriteLine($"El botón para añadir '{nombreBono}' está deshabilitado (sin stock)");
                                throw new InvalidOperationException($"No se puede añadir el bono '{nombreBono}' - sin stock disponible");
                            }
                            
                            botonAnadir.Click();
                            _output.WriteLine($"Bono '{nombreBono}' añadido al carrito");
                            
                            Thread.Sleep(800);
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _output.WriteLine($"Error al procesar fila: {ex.Message}");
                }
            }

            throw new NoSuchElementException($"No se encontró el bono con nombre: {nombreBono}");
        }

        public void AddBonoToCart(string bonoId)
        {
            Thread.Sleep(1500);
            
            var buttonId = By.Id("bonoToAdd_" + bonoId);
            
            try
            {
                WaitForBeingClickable(buttonId);
                var button = _driver.FindElement(buttonId);
                
                if (!button.Enabled)
                {
                    _output.WriteLine($"El botón para añadir el bono {bonoId} está deshabilitado (sin stock)");
                    throw new InvalidOperationException($"No se puede añadir el bono {bonoId} - sin stock disponible");
                }
                
                button.Click();
                _output.WriteLine($"Bono {bonoId} añadido al carrito");
                
                Thread.Sleep(800);
            }
            catch (NoSuchElementException)
            {
                _output.WriteLine($"No se encontró el botón para añadir el bono con ID: {bonoId}");
                throw;
            }
        }

        public void RemoveBonoFromCart(string bonoNombre)
        {
            var removeButtonId = By.Id("removeBono_" + bonoNombre);
            
            try
            {
                WaitForBeingClickable(removeButtonId);
                _driver.FindElement(removeButtonId).Click();
                _output.WriteLine($"Bono '{bonoNombre}' eliminado del carrito");
                
                Thread.Sleep(800);
            }
            catch (NoSuchElementException)
            {
                _output.WriteLine($"No se encontró el botón para eliminar el bono: {bonoNombre}");
                throw;
            }
        }

        public bool ComprarNotAvailable()
        {
            try
            {
                Thread.Sleep(500);
                
                var button = _driver.FindElement(buttonComprarBonos);
                
                bool isNotDisplayed = !button.Displayed;
                _output.WriteLine($"Botón 'Comprar Bonos' - Displayed: {button.Displayed}, NotAvailable: {isNotDisplayed}");
                
                return isNotDisplayed;
            }
            catch (NoSuchElementException)
            {
                _output.WriteLine("Botón 'Comprar Bonos' no encontrado en el DOM (carrito vacío)");
                return true;
            }
        }
    }
}
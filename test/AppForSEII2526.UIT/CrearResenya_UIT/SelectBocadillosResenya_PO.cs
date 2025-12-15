using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AppForSEII2526.UIT.Resenya_UIT
{
    public class SelectBocadillosResenya_PO : PageObject
    {
        // Inputs de búsqueda
        By inputNombreBy = By.XPath("//input[@placeholder='Ej: Vegetal']");
        By inputPVPMinBy = By.XPath("//input[@placeholder='Ej: 2,00']");
        By inputPVPMaxBy = By.XPath("//input[@placeholder='Ej: 10,00']");
        By buttonBuscarBy = By.XPath("//button[contains(text(),'Buscar')]");

        // Tabla de bocadillos
        By tableBocadillosBy = By.XPath("//table");
        By tableRowsBy = By.XPath("//table/tbody/tr");

        // Carrito
        By carritoItemsBy = By.XPath("//ul[contains(@class,'list-group')]/li[contains(@class,'list-group-item')]");
        By carritoVacioBy = By.XPath("//li[contains(text(),'Vacío')]");

        // Botón Crear reseña
        By buttonCrearResenyaBy = By.Id("CrearResenya");

        public SelectBocadillosResenya_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        // Buscar bocadillos (ESC-02)
        public void SearchBocadillos(string? nombre, double? pvpMin, double? pvpMax)
        {
            _output.WriteLine("Buscando bocadillos con filtros");

            WaitForBeingVisible(inputNombreBy);

            var inputNombre = _driver.FindElement(inputNombreBy);
            inputNombre.Clear();
            if (!string.IsNullOrEmpty(nombre))
                inputNombre.SendKeys(nombre);

            var inputMin = _driver.FindElement(inputPVPMinBy);
            inputMin.Clear();
            if (pvpMin.HasValue)
                inputMin.SendKeys(pvpMin.Value.ToString());

            var inputMax = _driver.FindElement(inputPVPMaxBy);
            inputMax.Clear();
            if (pvpMax.HasValue)
                inputMax.SendKeys(pvpMax.Value.ToString());

            _driver.FindElement(buttonBuscarBy).Click();
            Thread.Sleep(1500);

            try
            {
                WaitForBeingVisible(tableBocadillosBy);
            }
            catch
            {
                _output.WriteLine("No hay bocadillos que coincidan con el filtro");
            }
        }

        // Comprobar lista de bocadillos
        public bool CheckListOfBocadillos(List<string> expectedNames)
        {
            WaitForBeingVisible(tableBocadillosBy);

            var rows = _driver.FindElements(tableRowsBy);
            var nombresActuales = rows
                .Select(r => r.FindElements(By.TagName("td")).FirstOrDefault()?.Text)
                .Where(n => !string.IsNullOrEmpty(n))
                .ToList();

            foreach (var expected in expectedNames)
            {
                if (!nombresActuales.Any(n => n!.Equals(expected, StringComparison.OrdinalIgnoreCase)))
                {
                    _output.WriteLine($"Falta el bocadillo '{expected}' en la tabla");
                    return false;
                }
            }

            _output.WriteLine("La lista de bocadillos mostrada es correcta");
            return true;
        }

        // Añadir bocadillo por nombre (ESC-01)
        public void AddBocadilloByName(string nombreBocadillo)
        {
            _output.WriteLine($"Añadiendo bocadillo: {nombreBocadillo}");

            WaitForBeingVisible(tableBocadillosBy);

            var rows = _driver.FindElements(tableRowsBy);

            foreach (var row in rows)
            {
                var cells = row.FindElements(By.TagName("td"));
                if (cells.Count == 0) continue;

                if (cells[0].Text.Equals(nombreBocadillo, StringComparison.OrdinalIgnoreCase))
                {
                    var addButton = row.FindElement(By.XPath(".//button[contains(text(),'Añadir')]"));
                    addButton.Click();
                    Thread.Sleep(500);

                    _output.WriteLine($"Bocadillo '{nombreBocadillo}' añadido al carrito");
                    return;
                }
            }

            throw new NoSuchElementException($"No se encontró el bocadillo '{nombreBocadillo}'");
        }

        // Esperar a que un botón esté habilitado
        public void WaitForButtonEnabled(By by, int timeoutSeconds = 10)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Until(driver =>
            {
                try
                {
                    var button = driver.FindElement(by);
                    return button.Enabled;
                }
                catch
                {
                    return false;
                }
            });
        }

        // Click seguro en Crear Resenya
        public void ClickCrearResenya()
        {
            Thread.Sleep(500);
            var btn = _driver.FindElement(By.Id("btnCrearResenya"));
            ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].click();", btn);
        }

        // Eliminar bocadillo del carrito (ESC-04)
        public void RemoveBocadilloFromCart(string nombreBocadillo)
        {
            _output.WriteLine($"Eliminando bocadillo '{nombreBocadillo}' del carrito");

            var items = _driver.FindElements(carritoItemsBy);

            foreach (var item in items)
            {
                if (item.Text.Contains(nombreBocadillo, StringComparison.OrdinalIgnoreCase))
                {
                    item.FindElement(By.XPath(".//button[contains(text(),'X')]")).Click();
                    Thread.Sleep(500);
                    return;
                }
            }

            throw new NoSuchElementException($"El bocadillo '{nombreBocadillo}' no está en el carrito");
        }

        // Comprobar si carrito vacío (ESC-03)
        public bool IsCartEmpty()
        {
            try
            {
                _driver.FindElement(carritoVacioBy);
                _output.WriteLine("El carrito está vacío");
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Comprobar bocadillo en carrito (ESC-06)
        public bool IsBocadilloInCart(string nombreBocadillo)
        {
            var items = _driver.FindElements(carritoItemsBy);

            foreach (var item in items)
            {
                if (item.Text.Contains(nombreBocadillo, StringComparison.OrdinalIgnoreCase))
                {
                    _output.WriteLine($"Bocadillo '{nombreBocadillo}' sigue en el carrito");
                    return true;
                }
            }

            _output.WriteLine($"Bocadillo '{nombreBocadillo}' NO está en el carrito");
            return false;
        }

        // Comprobar lista completa de carrito (ESC-06)
        public bool CheckCartBocadillos(List<string> expectedBocadillos)
        {
            var items = _driver.FindElements(carritoItemsBy);
            var textos = items.Select(i => i.Text).ToList();

            foreach (var expected in expectedBocadillos)
            {
                if (!textos.Any(t => t.Contains(expected, StringComparison.OrdinalIgnoreCase)))
                {
                    _output.WriteLine($"Falta el bocadillo '{expected}' en el carrito");
                    return false;
                }
            }

            _output.WriteLine("El carrito mantiene todos los bocadillos esperados");
            return true;
        }

        // Crear reseña no disponible (ESC-03)
        public bool CrearResenyaNotAvailable()
        {
            var button = _driver.FindElement(buttonCrearResenyaBy);
            bool notAvailable = !button.Enabled;

            _output.WriteLine($"Crear reseña disponible: {!notAvailable}");
            return notAvailable;
        }
    }
}

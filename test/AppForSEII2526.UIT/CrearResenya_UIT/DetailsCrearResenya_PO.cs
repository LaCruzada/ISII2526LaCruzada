using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;
using System.Collections.Generic;
using System.Linq;

namespace AppForSEII2526.UIT.Resenya_UIT
{
    public class DetailsResenya_PO : PageObject
    {


        private By _tablaResumenResenyaBy = By.Id("ResumenResenya");

        private By _tablaBocadillosResenyaBy = By.Id("TablaBocadillosResenya");

        public DetailsResenya_PO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        // Comprobar datos generales de la reseña
        // (Usuario, título, descripción, fecha, valoración)
        public bool CompruebaDatosResenya(List<string[]> expectedResenyaData)
        {
            return CheckBodyTable(expectedResenyaData, _tablaResumenResenyaBy);
        }


        // Comprobar bocadillos reseñados
        // (Nombre, precio, tamaño, puntuación)

        public bool CompruebaBocadillosResenya(List<string[]> expectedBocadillos)
        {
            return CheckBodyTable(expectedBocadillos, _tablaBocadillosResenyaBy);
        }

        // Comprobar valoración media / total (si existe footer)
        public bool CompruebaValoracionTotal(List<string[]> expectedTotal)
        {
            return CheckFooterTable(expectedTotal, _tablaBocadillosResenyaBy);
        }

        // Copia exacta del patrón de CheckFooterTable
        // (idéntico al de DetallesBonoBocadillo_PO)
        public bool CheckFooterTable(List<string[]> expectedRows, By IdTable)
        {
            string expectedRow, actualRow;
            int i, j;
            bool result = true;

            WaitForBeingVisible(IdTable);

            IList<IWebElement> actualrows = _driver
                .FindElement(IdTable)
                .FindElement(By.TagName("tfoot"))
                .FindElements(By.TagName("tr"))
                .ToList();

            if (actualrows.Count != expectedRows.Count)
            {
                _output.WriteLine(
                    $"Error: \n Expected number of rows:{expectedRows.Count} \n Actual number of rows:{actualrows.Count}"
                );
                return false;
            }

            for (i = 0; i < expectedRows.Count; i++)
            {
                expectedRow = expectedRows[i][0];
                for (j = 1; j < expectedRows[i].Count(); j++)
                    expectedRow = expectedRow + " " + expectedRows[i][j];

                actualRow = actualrows
                    .Select(m => m.Text)
                    .ToList()[i];

                if (!actualRow.StartsWith(expectedRow))
                {
                    _output.WriteLine(
                        $"Error: \n \t expected row:{expectedRow} \n \t actual row:{actualRow}"
                    );
                    result = false;
                }
            }

            return result;
        }
    }
}

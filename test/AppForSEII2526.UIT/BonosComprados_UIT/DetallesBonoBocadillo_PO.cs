using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.BonosComprados_UIT
{
    public class DetallesBonoBocadillo_PO : PageObject
    {

        private By _tablaBonosBy = By.Id("BonosComprados");


        public DetallesBonoBocadillo_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        // Este método permite comprobar si la lista de bonos mostrada en la tabla coincide con la esperada o no.
        public bool CompruebaListaBonos(List<string[]> expectedBonos)
        {

            return CheckBodyTable(expectedBonos, _tablaBonosBy);
        }

        // Este método permite comprobar si el total mostrado en la tabla coincide con el esperado o no.
        public bool CompruebaTotal(List<string[]> expectedTotal)
        {

            return CheckFooterTable(expectedTotal, _tablaBonosBy);
        }

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
                _output.WriteLine($"Error: \n Expected number of rows:{expectedRows.Count} \n Actual number of rows:{actualrows.Count}");
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
                    _output.WriteLine($"Error: \n \t expected row:{expectedRow} \n \t actual row:{actualRow}");
                    result = false;

                }
            }
            return result;

        }
    }
}

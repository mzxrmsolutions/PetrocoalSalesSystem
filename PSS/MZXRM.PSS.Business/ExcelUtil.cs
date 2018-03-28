using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MZXRM.PSS.Data;
using System.IO;
using Aspose.Cells;
using Aspose.Cells.Tables;

namespace MZXRM.PSS.Business
{
    public class ExcelUtil
    {
        static Style curency;
        public static MemoryStream GenerateStoreReport(Store thisStore)
        {
            //string licenseFilePath = @"E:\GroupDocs\Licenses\Aspose.Total.lic";
            //License license = new License();
            //license.SetLicense(licenseFilePath);
            Workbook workbook = new Workbook();
            workbook.Worksheets.RemoveAt(workbook.Worksheets.ActiveSheetIndex);
            curency = workbook.CreateStyle();
            curency.Number = 8;

            GenerateStoreSheet(workbook, thisStore);


            MemoryStream stream = workbook.SaveToStream();

            return stream;

        }

        private static void GenerateStoreSheet(Workbook workbook, Store thisStore)
        {
            Worksheet newWorksheet = workbook.Worksheets.Add(thisStore.Name);

            Cells cells = newWorksheet.Cells;

            cells["A1"].PutValue("Store Id");
            cells["A2"].PutValue(thisStore.Id);

            cells["B1"].PutValue("Name");
            cells["B2"].PutValue(thisStore.Name);

            cells["C1"].PutValue("Created");
            cells["C2"].PutValue(thisStore.CreatedBy.Name + ":" + thisStore.CreatedOn.ToShortDateString());

            cells["D1"].PutValue("Last Modified");
            cells["D2"].PutValue(thisStore.ModifiedBy.Name + ":" + thisStore.ModifiedOn.ToShortDateString());

            cells["E1"].PutValue("Location");
            cells["E2"].PutValue(thisStore.Location);

            cells["F1"].PutValue("Capacity");
            cells["F2"].PutValue(thisStore.Capacity);

            cells["G1"].PutValue("Total Stock");
            cells["G2"].PutValue(thisStore.TotalStock);




            cells["A4"].PutValue("Customer");
            cells["B4"].PutValue("Transaction Ref");
            cells["C4"].PutValue("Date");
            cells["D4"].PutValue("Origin");
            cells["ED4"].PutValue("Vessel");
            cells["F4"].PutValue("Size");
            cells["G4"].PutValue("In");
            cells["H4"].PutValue("Out");

            int row = 5;
            foreach (StockMovement stMov in thisStore.ListStockMovement)
            {
                cells["A" + row].PutValue(stMov.Customer.Name);
                cells["B" + row].PutValue(stMov.HistoryRef);
                cells["C" + row].PutValue(stMov.Date);
                cells["D" + row].PutValue(stMov.Origin.Value);
                cells["E" + row].PutValue(stMov.Vessel.Value);
                cells["F" + row].PutValue(stMov.Size.Value);
                if (stMov.IsIn)
                    cells["G" + row].PutValue(stMov.Quantity.ToString(Constants.QuantityFormat));
                else cells["H" + row].PutValue(stMov.Quantity.ToString(Constants.QuantityFormat));

                row++;
            }






            ListObject listObject = newWorksheet.ListObjects[newWorksheet.ListObjects.Add("A4", "H" + (row-1), true)];
            listObject.TableStyleType = TableStyleType.TableStyleMedium7;
            listObject.ShowTotals = true;
            //listObject.ListColumns[6].TotalsCalculation = TotalsCalculation.Sum;
            //listObject.ListColumns[7].TotalsCalculation = TotalsCalculation.Sum;
            
            newWorksheet.AutoFitColumns();

        }
    }
}

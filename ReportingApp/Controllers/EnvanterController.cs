using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Reporting.Models;
using Reporting.Services;
using Reporting.Services.Excel;


namespace ReportingApp.Controllers
{
    public class EnvanterController : Controller
    {
        private readonly string _connStr;
        private readonly string _firmaNo;
        private readonly string _donemNo;

        public EnvanterController(IConfiguration configuration)
        {
            _connStr = configuration.GetConnectionString("LogoDb") ?? "";
            _firmaNo = configuration["LogoSettings:FirmaNo"] ?? "001";
            _donemNo = configuration["LogoSettings:DonemNo"] ?? "01";
        }


        // Envanter raporu 
        public IActionResult Envanter()
        {
            StockAndInventoryReportService service = new StockAndInventoryReportService(_connStr, _firmaNo, _donemNo);

            InventoryReportViewModel model = service.EnvanterRaporu();

            return View(model);
        }

        public IActionResult EnvanterExcel()
        {
            var excelService = new StockAndInventoryExcelExportService(_connStr, _firmaNo, _donemNo);

            byte[] fileBytes = excelService.EnvanterExcel();

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"StokVeEnvanterRaporu_{DateTime.Now:yyyyMMddHHmm}.xlsx"
            );
        }

      


    }
}

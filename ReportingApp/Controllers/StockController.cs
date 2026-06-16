using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Reporting.Models;
using Reporting.Services;
using Reporting.Services.Excel;


namespace ReportingApp.Controllers
{
    public class StockController : Controller
    {
        private readonly string _connStr;
        private readonly string _firmaNo;
        private readonly string _donemNo;

        public StockController(IConfiguration configuration)
        {
            _connStr = configuration.GetConnectionString("LogoDb") ?? "";
            _firmaNo = configuration["LogoSettings:FirmaNo"] ?? "001";
            _donemNo = configuration["LogoSettings:DonemNo"] ?? "01";
        }

        public IActionResult CokVeKritikStok()
        {
            StockAndInventoryReportService service = new StockAndInventoryReportService(_connStr, _firmaNo, _donemNo);

            InventoryReportViewModel model = service.EnvanterRaporu();

            return View(model);
        }

        public IActionResult StokHareketRaporu(DateTime? baslangicTarihi, DateTime? bitisTarihi)
        {
            var service = new StockAndInventoryReportService(_connStr, _firmaNo, _donemNo);

            var model = service.GetStockMovementReport(baslangicTarihi, bitisTarihi);

            return View(model);
        }

        public IActionResult SatilmayanUrunler()
        {
            var service = new StockAndInventoryReportService(_connStr, _firmaNo, _donemNo);

            var model = service.GetUnsoldProductsReport();

            return View(model);
        }

        public IActionResult CokVeKritikStokExcel()
        {
            var excelService = new StockAndInventoryExcelExportService(_connStr, _firmaNo, _donemNo);

            byte[] fileBytes = excelService.CokVeKritikStokExcel();

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"CokVeKritikStok_{DateTime.Now:yyyyMMddHHmm}.xlsx"
            );
        }

        public IActionResult StokHareketRaporuExcel(DateTime? baslangicTarihi, DateTime? bitisTarihi)
        {
            var excelService = new StockAndInventoryExcelExportService(_connStr, _firmaNo, _donemNo);

            byte[] fileBytes = excelService.StokHareketRaporuExcel(baslangicTarihi,   bitisTarihi);

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"StokHareketRaporu_{DateTime.Now:yyyyMMddHHmm}.xlsx"
            );
        }

        public IActionResult SatilmayanUrunlerExcel()
        {
            var excelService = new StockAndInventoryExcelExportService(_connStr, _firmaNo, _donemNo);

            byte[] fileBytes = excelService.SatilmayanUrunlerExcel();

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"SatilmayanUrunler_{DateTime.Now:yyyyMMddHHmm}.xlsx"
            );
        }

    }
}

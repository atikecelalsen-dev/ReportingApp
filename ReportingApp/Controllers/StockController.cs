using Microsoft.AspNetCore.Mvc;
using Reporting.Models;
using Reporting.Services;

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

        // Envanter raporu 
        //public IActionResult Envanter()
        //{
        //    InventoryReportService service = new InventoryReportService(_connStr, _firmaNo, _donemNo);

        //    InventoryReportViewModel model = service.EnvanterRaporu();

        //    return View(model);
        //}

        public IActionResult CokVeKritikStok()
        {
            InventoryReportService service = new InventoryReportService(_connStr, _firmaNo, _donemNo);

            InventoryReportViewModel model = service.EnvanterRaporu();

            return View(model);
        }

        public IActionResult StokHareketRaporu(DateTime? baslangicTarihi, DateTime? bitisTarihi)
        {
            var service = new InventoryReportService(_connStr, _firmaNo, _donemNo);

            var model = service.GetStockMovementReport(baslangicTarihi, bitisTarihi);

            return View(model);
        }

        public IActionResult SatilmayanUrunler()
        {
            var service = new InventoryReportService(_connStr, _firmaNo, _donemNo);

            var model = service.GetUnsoldProductsReport();

            return View(model);
        }

    }
}

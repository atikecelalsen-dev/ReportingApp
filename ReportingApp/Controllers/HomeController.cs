using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.AspNetCore.Mvc;
using Reporting.Models;
using Reporting.Services;
using Reporting.Services.Excel;
using ReportingApp.Models;
using System.Diagnostics;

namespace ReportingApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connStr;
        private readonly string _firmaNo;
        private readonly string _donemNo;

        public HomeController(IConfiguration configuration)
        {
            _connStr = configuration.GetConnectionString("LogoDb") ?? "";
            _firmaNo = configuration["LogoSettings:FirmaNo"] ?? "001";
            _donemNo = configuration["LogoSettings:DonemNo"] ?? "01";
        }

        //Kar/Zarar Raporu 
        public IActionResult SatisRaporu(ReportFilter filter)
        {
            if (string.IsNullOrWhiteSpace(filter.Periyot))
                filter.Periyot = "gunluk"; 

            SalesReportService service = new SalesReportService(_connStr, _firmaNo, _donemNo);
            //ReportFilter filter = new ReportFilter();
            SalesReportDashboardModel model = service.SatisKarZararDashboard(filter, filter.Periyot);

            return View(model);
        }

        // Ürün karlılık raporu  
        public IActionResult UrunKarlilik(ReportFilter filter)
        {
            if (string.IsNullOrWhiteSpace(filter.Periyot))
                filter.Periyot = "gunluk";

            SalesReportService service = new SalesReportService(_connStr, _firmaNo, _donemNo);
             ProductProfitReportViewModel model = service.UrunKarlilikRaporu(filter);

            return View(model);
        }

        // Müsteri karlılık raporu 
        public IActionResult MusteriKarlilik(ReportFilter filter)
        {
            if (string.IsNullOrWhiteSpace(filter.Periyot))
                filter.Periyot = "gunluk";

            SalesReportService service = new SalesReportService(_connStr, _firmaNo, _donemNo);
            CustomerProfitReportViewModel model = service.MusteriKarlilikRaporu(filter);

            return View(model);
        }


        public IActionResult Index(ReportFilter filter)
        {
            if (string.IsNullOrWhiteSpace(filter.Periyot))
                filter.Periyot = "gunluk";

            SalesReportService service = new SalesReportService(_connStr, _firmaNo, _donemNo);

            var model = new TumRaporlarViewModel
            {
                SatisRaporu = service.SatisKarZararDashboard(filter, filter.Periyot),
                UrunKarlilik = service.UrunKarlilikRaporu(filter),
                MusteriKarlilik = service.MusteriKarlilikRaporu(filter)
            };

            return View(model);
        }


        public IActionResult SatisRaporuExcel(ReportFilter filter)
        {
            var excelService = new SalesExcelExportService(_connStr, _firmaNo, _donemNo);

            byte[] fileBytes = excelService.SatisRaporuExcel(filter);

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"SatisRaporu_{DateTime.Now:yyyyMMddHHmm}.xlsx"
            );
        }

        public IActionResult UrunKarlilikExcel(ReportFilter filter)
        {
            var excelService = new SalesExcelExportService(_connStr, _firmaNo, _donemNo);

            byte[] fileBytes = excelService.UrunKarlilikExcel(filter);

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"UrunKarlilikRaporu_{DateTime.Now:yyyyMMddHHmm}.xlsx"
            );
        }

        public IActionResult MusteriKarlilikExcel(ReportFilter filter)
        {
            var excelService = new SalesExcelExportService(_connStr, _firmaNo, _donemNo);

            byte[] fileBytes = excelService.MusteriKarlilikExcel(filter);

            return File(
                fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"MusteriKarlilikRaporu_{DateTime.Now:yyyyMMddHHmm}.xlsx"
            );
        }
    }
}

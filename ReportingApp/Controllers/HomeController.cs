using Microsoft.AspNetCore.Mvc;
using ReportingApp.Models;
using System.Diagnostics;
using Reporting.Models;
using Reporting.Services;

namespace ReportingApp.Controllers
{
    public class HomeController : Controller
    {
        string connectionString =
                "Server=.;Database=GODENEME;Trusted_Connection=True;TrustServerCertificate=True;";
   
        // Kar/Zarar Raporu 
        public SalesReportDashboardModel CiroModel(string periyot)
        {
            SalesReportService service = new SalesReportService(connectionString);
            ReportFilter filter = new ReportFilter();
            SalesReportDashboardModel model = service.SatisKarZararDashboard(filter, periyot);

            return model;
        }

        // Ürün karlılık raporu 
        public ProductProfitReportViewModel UrunKarlilikModel(string periyot)
        {
            SalesReportService service = new SalesReportService(connectionString);
            ReportFilter filter = new ReportFilter { Periyot = periyot };
            ProductProfitReportViewModel model = service.UrunKarlilikRaporu(filter);

            return model;
        }

        // Müsteri karlılık raporu 
        public CustomerProfitReportViewModel MusteriKarlilikModel(string periyot)
        {
            SalesReportService service = new SalesReportService(connectionString);
            ReportFilter filter = new ReportFilter { Periyot = periyot };
            CustomerProfitReportViewModel model = service.MusteriKarlilikRaporu(filter);

            return model;
        }

      

        // Satış raporları
        public IActionResult Index(string periyot = "aylik")
        {
            SalesReportService service = new SalesReportService(connectionString);

            ReportFilter filter = new ReportFilter
            {
                Periyot = periyot
            };

            var model = new TumRaporlarViewModel
            {
                SatisRaporu = CiroModel(periyot),
                UrunKarlilik = UrunKarlilikModel(periyot),
                MusteriKarlilik = MusteriKarlilikModel(periyot)
            };

            return View(model);
        }

        // Envanter raporu 
        public IActionResult Envanter()
        {
            SalesReportService service = new SalesReportService(connectionString);

            InventoryReportViewModel model = service.EnvanterRaporu();

            return View(model);
        }

    }
}

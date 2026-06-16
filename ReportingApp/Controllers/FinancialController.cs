using ClosedXML.Excel;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.AspNetCore.Mvc;
using Reporting.Models;
using Reporting.Services;
using Reporting.Services.Excel;
using ReportingApp.Models;
using System.Diagnostics;

namespace ReportingApp.Controllers
{
    public class FinancialController : Controller
    {
        private readonly string _connStr;
        private readonly string _firmaNo;
        private readonly string _donemNo;

        public FinancialController(IConfiguration configuration)
        {
            _connStr = configuration.GetConnectionString("LogoDb") ?? "";
            _firmaNo = configuration["LogoSettings:FirmaNo"] ?? "001";
            _donemNo = configuration["LogoSettings:DonemNo"] ?? "01";
        }


        public IActionResult GelirGiderOzeti(DateTime? baslangicTarihi, DateTime? bitisTarihi)
        {
            DateTime baslangic = baslangicTarihi ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime bitis = bitisTarihi ?? DateTime.Today;

            var service = new IncomeExpenseReportService(_connStr, _firmaNo, _donemNo);

            var model = service.GetReport(baslangic, bitis);

            return View(model);
        }


        public IActionResult CariHesapEkstresi(int? cariRef, DateTime? baslangicTarihi, DateTime? bitisTarihi)
        {
            DateTime baslangic = baslangicTarihi ?? new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime bitis = bitisTarihi ?? DateTime.Today;

            var service = new CurrentAccountStatementService(_connStr, _firmaNo, _donemNo);

            var model = service.GetReport(cariRef, baslangic, bitis);

            return View(model);
        }


        public IActionResult CariHesapEkstresiExcel(int cariRef, DateTime? baslangicTarihi, DateTime? bitisTarihi)
        {
            var excelService = new FinancialExcelExportService(_connStr, _firmaNo, _donemNo);
            byte[] fileBytes = excelService.CariHesapEkstresiExcel(cariRef, baslangicTarihi, bitisTarihi);
            string fileName = $"CariHesapEkstresi_{cariRef}_{DateTime.Now:yyyyMMddHHmm}.xlsx";

            return File(
               fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }


        public IActionResult GelirGiderOzetiExcel(DateTime? baslangicTarihi, DateTime? bitisTarihi)
        {
            var excelService = new FinancialExcelExportService(_connStr, _firmaNo, _donemNo);
            byte[] fileBytes = excelService.GelirGiderOzetiExcel(baslangicTarihi, bitisTarihi);
            string fileName = $"GelirGiderOzeti_{DateTime.Now:yyyyMMddHHmm}.xlsx";

            return File(
               fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }

    }
}

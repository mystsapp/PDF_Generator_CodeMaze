using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PDF_Generator.Utilities;

namespace PDF_Generator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfCreatorController : ControllerBase
    {
        private readonly IConverter _converter;

        public PdfCreatorController(IConverter converter)
        {
            _converter = converter;
        }

        [HttpGet]
        public IActionResult CreatePDF()
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report",
                // Out = @"D:\PDFCreator\Employee_Report.pdf"
            };

            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                //HtmlContent = TemplateGenerator.GetHTMLString(),
                Page = "https://code-maze.com/",
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [Page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var file = _converter.Convert(pdf);

            //return Ok("Successfully created PDF document.");

            //return File(file, "application/pdf");

            return File(file, "application/pdf", "EmployeeReport.pdf");
        }
    }
}
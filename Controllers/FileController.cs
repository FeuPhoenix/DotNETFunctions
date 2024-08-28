using DotNETFunctions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace DotNETFunctions.Controllers
{
    public class FileController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<FileController> _logger;
        private readonly IConfiguration _configuration;

        public FileController(IWebHostEnvironment environment, ILogger<FileController> logger, IConfiguration configuration)
        {
            _environment = environment;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(FileUploadModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.FinancialFile != null)
                    await SaveFile(model.FinancialFile, "Financial");
                if (model.TechnicalFile != null)
                    await SaveFile(model.TechnicalFile, "Technical");
                if (model.BankGuaranteeFile != null)
                    await SaveFile(model.BankGuaranteeFile, "BankGuarantee");

                return RedirectToAction("Download");
            }

            // If we got this far, something failed; redisplay form
            return View(model);
        }

        [HttpGet]
        public IActionResult Download()
        {
            var uploadPath = Path.Combine(_environment.ContentRootPath, 
                _configuration["UploadPath"] ?? throw new InvalidOperationException("UploadPath is not configured"));
            var files = Directory.GetFiles(uploadPath)
                .Select(f => new FileViewModel
                {
                    FileName = Path.GetFileName(f),
                    UploadTime = System.IO.File.GetCreationTime(f)
                })
                .OrderByDescending(f => f.UploadTime)
                .ToList();
            return View(files);
        }

        [HttpGet]
        public IActionResult DownloadFile(string fileName)
        {
            var uploadPath = Path.Combine(_environment.ContentRootPath, 
                _configuration["UploadPath"] ?? throw new InvalidOperationException("UploadPath is not configured"));
            var filePath = Path.Combine(uploadPath, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory);
            }
            memory.Position = 0;

            var contentType = "application/pdf";

            if (fileName.StartsWith("Financial"))
            {
                var stampedPdf = AddApprovedStamp(memory);
                return File(stampedPdf.ToArray(), contentType, fileName);
            }

            return File(memory, contentType, fileName);
        }

        private bool ValidateFile(IFormFile file, int maxSizeMB, string contentType)
        {
            return file.Length <= maxSizeMB * 1024 * 1024 && file.ContentType == contentType;
        }

        private async Task<string> SaveFile(IFormFile file, string prefix)
        {
            if (file != null && file.Length > 0)
            {
                var uploadPath = _configuration["UploadPath"] ?? throw new InvalidOperationException("UploadPath is not configured");
                var fullPath = Path.Combine(_environment.ContentRootPath, uploadPath);
                
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }

                var fileName = $"{prefix}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(fullPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return fileName;
            }
            return null!;
        }

        private MemoryStream AddApprovedStamp(MemoryStream inputPdf)
        {
            var outputPdfStream = new MemoryStream();
            var reader = new PdfReader(inputPdf.ToArray());
            var stamper = new PdfStamper(reader, outputPdfStream);

            var content = "APPROVED";
            var font = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                var pdfContentByte = stamper.GetOverContent(i);
                pdfContentByte.BeginText();
                pdfContentByte.SetFontAndSize(font, 30);
                pdfContentByte.SetRGBColorFill(255, 0, 0);
                pdfContentByte.ShowTextAligned(Element.ALIGN_CENTER, content, 300, 30, 0);
                pdfContentByte.EndText();
            }

            stamper.Close();
            reader.Close();

            return outputPdfStream;
        }
    }
}
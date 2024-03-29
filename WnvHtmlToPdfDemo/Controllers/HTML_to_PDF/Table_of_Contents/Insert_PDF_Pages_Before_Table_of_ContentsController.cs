﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Hosting;

// Use Winnovative Namespace
using Winnovative;

namespace WnvHtmlToPdfDemo.Controllers.HTML_to_PDF.Table_of_Contents
{
    public class Insert_PDF_Pages_Before_Table_of_ContentsController : Controller
    {
        private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment m_hostingEnvironment;
        public Insert_PDF_Pages_Before_Table_of_ContentsController(IWebHostEnvironment hostingEnvironment)
        {
            m_hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public ActionResult ConvertHtmlToPdf(IFormCollection collection)
        {
            // Create a HTML to PDF converter object with default settings
            HtmlToPdfConverter htmlToPdfConverter = new HtmlToPdfConverter();

            // Set license key received after purchase to use the converter in licensed mode
            // Leave it not set to use the converter in demo mode
            htmlToPdfConverter.LicenseKey = "fvDh8eDx4fHg4P/h8eLg/+Dj/+jo6Og=";

            // Set an adddional delay in seconds to wait for JavaScript or AJAX calls after page load completed
            // Set this property to 0 if you don't need to wait for such asynchcronous operations to finish
            htmlToPdfConverter.ConversionDelay = 2;

            // Set the PDF file to be inserted before the table of contents
            string pdfFileBefore = m_hostingEnvironment.ContentRootPath + "/wwwroot" + "/DemoAppFiles/Input/PDF_Files/Merge_Before_Conversion.pdf";
            htmlToPdfConverter.PdfDocumentOptions.AddStartDocument(pdfFileBefore);

            // Enable the creation of a table of contents from H1 to H6 tags found in HTML
            htmlToPdfConverter.TableOfContentsOptions.AutoTocItemsEnabled = collection["autoTableOfContentsCheckBox"].Count > 0;

            // Optionally set the table of contents title
            htmlToPdfConverter.TableOfContentsOptions.Title = "Table of Contents";

            // Optionally set the title style using CSS sttributes
            htmlToPdfConverter.TableOfContentsOptions.TitleStyle = "color:navy; font-family:'Times New Roman'; font-size:28px; font-weight:normal";

            // Optionally set the style of level 1 items in table of contents
            string level1TextStyle = "color:black; font-family:'Times New Roman'; font-size:20px; font-weight:normal; font-style:normal; background-color:#F0F0F0";
            htmlToPdfConverter.TableOfContentsOptions.SetItemStyle(1, level1TextStyle);

            // Optionally set the page numbers style of level 1 items in table of contents
            string level1PageNumberStyle = "color:black; padding-right:3px; background-color:#F0F0F0; font-size:14px; font-weight:bold";
            htmlToPdfConverter.TableOfContentsOptions.SetPageNumberStyle(1, level1PageNumberStyle);

            // The URL to convert
            string url = collection["urlTextBox"];

            // Convert the HTML page to a PDF document in a memory buffer
            byte[] outPdfBuffer = htmlToPdfConverter.ConvertUrl(url);
            
            // Send the PDF file to browser
            FileResult fileResult = new FileContentResult(outPdfBuffer, "application/pdf");
            fileResult.FileDownloadName = "Insert_PDF_Before_Table_of_Contents.pdf";

            return fileResult;
        }
    }
}
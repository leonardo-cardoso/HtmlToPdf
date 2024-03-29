﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

// Use Winnovative Namespace
using Winnovative;

namespace WnvHtmlToPdfDemo.Controllers.PDF_Creator
{
    public class PDF_Creator_HTML_to_PDF_ElementsController : Controller
    {
        // GET: PDF_Creator_HTML_to_PDF_Elements
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePdf(IFormCollection collection)
        {
            // Create a PDF document
            Document pdfDocument = new Document();

            // Set license key received after purchase to use the converter in licensed mode
            // Leave it not set to use the converter in demo mode
            pdfDocument.LicenseKey = "fvDh8eDx4fHg4P/h8eLg/+Dj/+jo6Og=";

            // Create a PDF page where to add the first HTML
            PdfPage firstPdfPage = pdfDocument.AddPage();

            try
            {
                // The element location in PDF
                float xLocation = float.Parse(collection["xLocationTextBox"]);
                float yLocation = float.Parse(collection["yLocationTextBox"]);

                // The URL of the HTML page to convert to PDF
                string urlToConvert = collection["urlTextBox"];

                // Create the HTML to PDF element
                HtmlToPdfElement htmlToPdfElement = new HtmlToPdfElement(xLocation, yLocation, urlToConvert);

                // Optionally set the HTML viewer width
                htmlToPdfElement.HtmlViewerWidth = int.Parse(collection["htmlViewerWidthTextBox"]);

                // Optionally set the HTML viewer height
                if (collection["htmlViewerHeightTextBox"][0].Length > 0)
                    htmlToPdfElement.HtmlViewerHeight = int.Parse(collection["htmlViewerHeightTextBox"]);

                // Optionally set the HTML content clipping option to force the HTML content width to be exactly HtmlViewerWidth pixels
                htmlToPdfElement.ClipHtmlView = collection["clipContentCheckBox"].Count > 0;

                // Optionally set the destination width in PDF
                if (collection["contentWidthTextBox"][0].Length > 0)
                    htmlToPdfElement.Width = float.Parse(collection["contentWidthTextBox"]);

                // Optionally set the destination height in PDF
                if (collection["contentHeightTextBox"][0].Length > 0)
                    htmlToPdfElement.Height = float.Parse(collection["contentHeightTextBox"]);

                // Optionally set a delay before conversion to allow asynchonous scripts to finish
                htmlToPdfElement.ConversionDelay = 2;

                // Add the HTML to PDF element to PDF document
                // The AddElementResult contains the bounds of the HTML to PDF Element in last rendered PDF page
                // such that you can start a new PDF element right under it
                AddElementResult result = firstPdfPage.AddElement(htmlToPdfElement);

                // Save the PDF document in a memory buffer
                byte[] outPdfBuffer = pdfDocument.Save();
                
                // Send the PDF file to browser
                FileResult fileResult = new FileContentResult(outPdfBuffer, "application/pdf");
                fileResult.FileDownloadName = "HTML_to_PDF_Elements.pdf";

                return fileResult;
            }
            finally
            {
                // Close the PDF document
                pdfDocument.Close();
            }
        }
    }
}
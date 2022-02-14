using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Text;

namespace BuilderPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Builder Pattern!");
        }
    }

    public interface IPdfGeneratorService
    {
        PdfDocument CreatePdfReport(Guid tenantId);
    }

    public class Coordinate
    {
        public static readonly int x = 40;
        public static readonly int y = 120 - 50;

    }

    public class LogoBuilder
    {
        public void BuildLogo(XGraphics gfx)
        {
            //var data = System.Reflection.Assembly.GetEntryAssembly().GetManifestResourceNames();

            var file = System.Reflection.Assembly.GetEntryAssembly()
                .GetManifestResourceStream("CoreCitRateDetector.Resources.Logo.png");


            int x = Coordinate.x + 200;
            int y = Coordinate.y - 120 + 50;

            gfx.DrawImage(XImage.FromStream(file), new XPoint(x, y));
        }
    }

    public class ReportTitles
    {
        public ReportTitles()
        {
            Title = "Raport - możliwość zastosowania 9% stawki podatku CIT";
            TitleEn = "Report - possibility to apply 9% CIT rate";
            TenantNamePl = "Nazwa badanego podmiotu / Entity's name";
            LagalFormPl = "Forma prawna / Legal form";
            YearStartDatePl = "Pierwszy dzień badanego roku podatkowego / First day of a tested tax year";
            YearEndDatePl = "Ostatni dzień badanego roku podatkowego / End of a tested tax year";
            NetIncomePl = "Przychody (netto, bez podatku VAT) w badanym roku / Revenue (net of VAT) in the tested year";
            GrossIncomePl = "Przychody ze sprzedaży (brutto, wraz z podatkiem VAT) w roku podatkowym poprzedzającym badany rok / Sales revenue (including VAT) in the tax year preceding the tested year";
            CompanyEstablishmentDatePl = "Data rozpoczęcia działalności / Start date of business activity";
            AlertPl = "UWAGA!Stawki 9 % nie stosuje się do przychodów / dochodów z zysków kapitałowych";
            Footer = "© 2021 TPA Poland , ul. Młyńska 12, 61-730 Poznań. Wszelkie prawa zastrzeżone / All rights reserved.";
            FooterEnglish = "All rigths reserved.";
            //SubTitlePl = "Detekcja spółki nieruchomościowej";
        }

        public string Title { get; set; }
        public string TitleEn { get; set; }
        public string TenantNamePl { get; set; }
        public string LagalFormPl { get; set; }
        public string YearStartDatePl { get; set; }
        public string YearEndDatePl { get; set; }
        public string NetIncomePl { get; set; }
        public string GrossIncomePl { get; set; }

        public string CompanyEstablishmentDatePl { get; set; }
        public string AlertPl { get; set; }
        public string Asterisk { get; set; }
        public string Footer { get; set; }
        public string FooterEnglish { get; set; }
        public string SubTitlePl { get; set; }
    }
    public class ReportHeader
    {
        public string ReportHeaderPl => "RAPORT";

        public static void GetReportHeader(PdfPage page, XGraphics gfx)
        {

            int x = Coordinate.x;
            int y = Coordinate.y + 60;
            int width = 490;
            int highth = 320;

            XTextFormatter tf = new XTextFormatter(gfx);
            tf.Alignment = XParagraphAlignment.Center;
            ReportTitles recoHeader = new ReportTitles();
            XRect rect = new XRect(x, y, width, highth);
            XFont font = new XFont("Arial", 14, XFontStyle.Bold);
            tf.DrawString(recoHeader.Title, font, XBrushes.Black, rect, XStringFormats.TopLeft);
            rect = new XRect(x, y + 15, width, highth);
            font = new XFont("Arial", 14, XFontStyle.BoldItalic);
            tf.DrawString(recoHeader.TitleEn, font, XBrushes.Black, rect, XStringFormats.TopLeft);

        }
    }

    public class PdfGeneratorService : IPdfGeneratorService
    {
        private readonly ITenantRepository tenantRepository;

        public PdfDocument CreatePdfReport(Guid tenantId)
        {
            Tenant tenant = tenantRepository.GetById(tenantId);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            //Logo
            LogoBuilder logo = new LogoBuilder();
            logo.BuildLogo(gfx);

            //Nagłówek
            ReportHeader.GetReportHeader(page, gfx);

            //Pytania
            // Subheader.BuildSubheaderFinancialData(page, gfx, tenant);
            // PdfReportResult.BuildSubheaderFinancialDataResult(page, gfx, tenant);
            //Subheader.BuildSubheaderReducedTax(page, gfx);
            //Subheader.BuildSubheaderNewCompany(page, gfx);
            //Subheader.BuildSubheaderCompanySplit(page, gfx);

            ////RECO RESULT

            // Footer.BuildFooter(page, gfx);
            return document;

        }
    }


        public interface ITenantRepository
    {
        Tenant GetById(Guid id);
    }

    public class BaseModel
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class LegalForm : BaseModel
    {
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public IEnumerable<Tenant> Tenant { get; set; }
    }

    public class Tenant : BaseModel
    {
        public string Name { get; set; } //podaje użytkownik
        public Guid LegalFormId { get; set; }//podaje użytkownik
        public LegalForm LegalForm { get; set; }
        public DateTime StartOfTaxYear { get; set; }//podaje użytkownik
        public DateTime EndOfTaxYear { get; set; }//podaje użytkownik
        public decimal LastYearGrossRevenue { get; set; }//podaje użytkownik
        public decimal CurrentYearNetRevenue { get; set; }//podaje użytkownik
        public DateTime YearOfBusinessStart { get; set; }//podaje użytkownik
        public string EmailAdress { get; set; }

    }
}

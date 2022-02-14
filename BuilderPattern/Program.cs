using PdfSharp.Drawing;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BuilderPattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Builder Pattern!");

            ITenantRepository tenantRepository = new FakeTenantRepository();
            IPdfGeneratorService generatorService = new PdfGeneratorService();

            Tenant tenant = tenantRepository.GetById(Guid.NewGuid());

            Report report = generatorService.CreatePdfReport(tenant);


            IPdfBuilder pdfBuilder = new PdfSharpBuilder();

            IFinacialReportBuilder finacialReportBuilder = new PdfSharpFinacialReportBuilder(pdfBuilder);

            //finacialReportBuilder.AddTenantSection(tenant);
            //finacialReportBuilder.AddTenantSection(tenant);
            //finacialReportBuilder.AddTenantSection(tenant);
            //finacialReportBuilder.AddTenantSection(tenant);
            //finacialReportBuilder.AddTenantSection(tenant);
            //finacialReportBuilder.AddFinancialSection(1000);            
            //Report finacialReport = finacialReportBuilder.Build();

            FinacialReportDirector finacialReportDirector = new FinacialReportDirector(finacialReportBuilder);
            finacialReportDirector.CreateReport(tenant, 1000);
            Report financialReport = finacialReportDirector.GetReport();

        }
    }

    public class FinacialReportDirector
    {
        private readonly IFinacialReportBuilder builder;

        public FinacialReportDirector(IFinacialReportBuilder builder)
        {
            this.builder = builder;
        }

        public void CreateReport(Tenant tenant, decimal totalAmount)
        {
            builder.AddTenantSection(tenant);
            builder.AddFinancialSection(totalAmount);
        }

        public Report GetReport()
        {
            return builder.Build();
        }
    }

    public class Report
    {
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; } = "application/pdf";

        public Report()
        {
            CreateDate = DateTime.Now;
        }
    }

    public class FakeTenantRepository : ITenantRepository
    {
        public Tenant GetById(Guid id)
        {
            return new Tenant { Id = id, Name = "John Smith", EmailAdress = "john.smith@domain.com" };
        }
    }

    public interface IPdfGeneratorService
    {
        Report CreatePdfReport(Tenant tenant);
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


    public class Subheader
    {
        public static void BuildSubheaderFinancialData(PdfPage page, XGraphics gfx, Tenant tenant)
        {
            XBrush green = new XSolidBrush(XColor.FromArgb(0, 40, 0));

            ReportTitles recoHeader = new ReportTitles();
            int x = Coordinate.x;
            int y = Coordinate.y + 160;
            XFont font = new XFont("Arial", 9, XFontStyle.Regular);

            gfx.DrawString(recoHeader.TenantNamePl, font, green, new XPoint(x, y));
            gfx.DrawString(recoHeader.LagalFormPl, font, green, new XPoint(x, y + 15));
            gfx.DrawString(recoHeader.YearStartDatePl, font, green, new XPoint(x, y + 30));
            gfx.DrawString(recoHeader.YearEndDatePl, font, green, new XPoint(x, y + 45));
            gfx.DrawString(recoHeader.NetIncomePl, font, green, new XPoint(x, y + 60));
            TextContainer.BuildContainerAdminDescriptions(page, gfx, x, y + 67, recoHeader.GrossIncomePl, "");
            gfx.DrawString(recoHeader.CompanyEstablishmentDatePl, font, green, new XPoint(x, y + 110));
        }
    }

    public class GreenTpa
    {
        public static XBrush GetGreenColor()
        {
            XBrush green = new XSolidBrush(XColor.FromArgb(0, 40, 0));
            return green;
        }
    }

    public class TextContainer
    {
        public static void BuildContainerAdminDescriptions(PdfPage page, XGraphics gfx, int x, int y, string content, string answer)
        {
            int width = 360;
            int width2 = 80;

            XFont font = new XFont("Arial", 9);
            XTextFormatter tf = new XTextFormatter(gfx);

            XRect rect = new XRect(x, y, width, 100);
            XRect rect2 = new XRect(x + 400, y, width2, 200);

            tf.Alignment = XParagraphAlignment.Justify;
            tf.DrawString(content, font, GreenTpa.GetGreenColor(), rect, XStringFormats.TopLeft);

            tf = new XTextFormatter(gfx);
            tf.Alignment = XParagraphAlignment.Left;
            tf.DrawString(answer, font, GreenTpa.GetGreenColor(), rect2, XStringFormats.TopLeft);
        }

    }

    // Abstrakcyjny budowniczy
    public interface IFinacialReportBuilder
    {
        void AddTenantSection(Tenant tenant);

        void AddFinancialSection(decimal totalAmount);

        // Produkt
        Report Build();
    }


    public interface IPdfBuilder
    {
        void AddImage(Stream image, int x, int y);

        void AddTitle(string title, int x, int width);
        void AddField(string caption, object value, int x, int y, int width = 100, int hight = 100);
        void AddTable<T>(IEnumerable<T> items);

        byte[] Build();
    }

    public class PdfSharpBuilder : IPdfBuilder
    {
        private readonly PdfDocument document;
        private readonly XGraphics gfx;

        private int currentY;

        public PdfSharpBuilder()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            document = new PdfDocument();
            PdfPage page = document.AddPage();

            gfx = XGraphics.FromPdfPage(page);
        }

        public void AddField(string caption, object value, int x, int y, int width, int hight)
        {
            XFont font = new XFont("Arial", 14, XFontStyle.Bold);
            XTextFormatter tf = new XTextFormatter(gfx);
            tf.Alignment = XParagraphAlignment.Center;
            XRect rect = new XRect(x, currentY, width, hight);

            tf.DrawString(caption, font, XBrushes.Black, rect, XStringFormats.TopLeft);
            tf.DrawString(value.ToString(), font, XBrushes.Black, rect, XStringFormats.TopLeft);
        }

        public void AddImage(Stream image, int x, int y)
        {
            gfx.DrawImage(XImage.FromStream(image), new XPoint(x, y));
        }

        public void AddTable<T>(IEnumerable<T> items)
        {
            throw new NotImplementedException();
        }

        public void AddTitle(string title, int x, int width)
        {
            int hight = 320;

            XTextFormatter tf = new XTextFormatter(gfx);
            tf.Alignment = XParagraphAlignment.Center;

            XRect rect = new XRect(x, currentY, width, hight);
            XFont font = new XFont("Arial", 14, XFontStyle.Bold);

            tf.DrawString(title, font, XBrushes.Black, rect, XStringFormats.TopLeft);
        }

        public byte[] Build()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                document.Save(stream);

                return stream.ToArray();
            }
        }
    }


    public class PdfSharpFinacialReportBuilder : IFinacialReportBuilder
    {
        private readonly IPdfBuilder builder;

        public PdfSharpFinacialReportBuilder(IPdfBuilder builder)
        {
            this.builder = builder;
        }

        public void AddFinancialSection(decimal totalAmount)
        {
            builder.AddTitle($"Total Amount: {totalAmount}", 100, 50);
        }

        public void AddTenantSection(Tenant tenant)
        {
            builder.AddField("E-mail", tenant.EmailAdress, 100, 50);
            builder.AddField("Name", tenant.Name, 100, 50);
            builder.AddField("Start Of Tax Year", tenant.StartOfTaxYear, 100, 50);
            builder.AddField("End Of Tax Year", tenant.EndOfTaxYear, 100, 50);
        }

        public Report Build()
        {
            byte[] content = builder.Build();

            return new Report { Title = "Raport finansowy", Content = content };
        }
    }

    public class PdfGeneratorService : IPdfGeneratorService
    {
        public Report CreatePdfReport(Tenant tenant)
        {
            // Tenant tenant = tenantRepository.GetById(tenantId);
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
            Subheader.BuildSubheaderFinancialData(page, gfx, tenant);
            // PdfReportResult.BuildSubheaderFinancialDataResult(page, gfx, tenant);
            //Subheader.BuildSubheaderReducedTax(page, gfx);
            //Subheader.BuildSubheaderNewCompany(page, gfx);
            //Subheader.BuildSubheaderCompanySplit(page, gfx);

            ////RECO RESULT

            // Footer.BuildFooter(page, gfx);

            using (MemoryStream stream = new MemoryStream())
            {
                document.Save(stream);

                return new Report { Title = "Tenant Report", Content = stream.ToArray() };
            }

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

using System.Globalization;
using System.Text;
using CsvHelper;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using StudentManagement.Application.DTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
namespace StudentManagement.Reporting.Generators
{
    public static class ReceiptSpooler
    {
        public static string GeneratePdf(FeeCollectionResult receipt, string institutionName, string outputDirectory)
        {
            Directory.CreateDirectory(outputDirectory);
            var path = Path.Combine(outputDirectory, $"{receipt.ReceiptNo}.pdf");

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A5);
                    page.Margin(30);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Column(col =>
                    {
                        col.Item().AlignCenter().Text(institutionName).Bold().FontSize(16).FontColor(Colors.Indigo.Darken2);
                        col.Item().AlignCenter().Text("MONEY RECEIPT").Bold().FontSize(12);
                        col.Item().PaddingTop(8).BorderBottom(1).BorderColor(Colors.Grey.Medium);
                    });

                    page.Content().PaddingVertical(12).Column(col =>
                    {
                        col.Spacing(6);
                        col.Item().Text($"Receipt No : {receipt.ReceiptNo}");
                        col.Item().Text($"Date       : {receipt.PaymentDate:dd-MMM-yyyy hh:mm tt}");
                        col.Item().Text($"Student    : {receipt.StudentName}");
                        col.Item().Text($"Reg. ID    : {receipt.RegistrationNo}");
                        col.Item().Text($"Fee Period : {receipt.FeePeriods}");
                        col.Item().Text($"Method     : {receipt.PaymentMethod}");
                        col.Item().PaddingTop(10).Text($"Amount Paid: ৳ {receipt.AmountPaid:N2}").Bold().FontSize(14).FontColor(Colors.Green.Darken2);
                    });

                    page.Footer().AlignCenter().Text("This is a computer-generated receipt.").FontSize(8).FontColor(Colors.Grey.Darken1);
                });
            }).GeneratePdf(path);

            return path;
        }
    }

    public static class LedgerStatementPrinter
    {
        public static string GeneratePdf(
            LedgerSummaryDto summary,
            IReadOnlyList<LedgerEntryDto> entries,
            string institutionName,
            string outputDirectory)
        {
            Directory.CreateDirectory(outputDirectory);
            var path = Path.Combine(outputDirectory, $"Ledger_{summary.RegistrationNo}_{DateTime.Now:yyyyMMddHHmmss}.pdf");

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(28);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header().Column(col =>
                    {
                        col.Item().AlignCenter().Text(institutionName).Bold().FontSize(16);
                        col.Item().AlignCenter().Text("STUDENT FINANCIAL LEDGER STATEMENT").Bold();
                        col.Item().PaddingTop(6).Text($"{summary.FullName}  |  {summary.RegistrationNo}");
                        col.Item().Text($"Invoiced: ৳{summary.TotalInvoiced:N2}   Paid: ৳{summary.TotalPaid:N2}   Due: ৳{summary.NetDue:N2}   Advance: ৳{summary.AdvanceBalance:N2}");
                    });

                    page.Content().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(c =>
                        {
                            c.RelativeColumn(1.2f);
                            c.RelativeColumn(1.4f);
                            c.RelativeColumn(2.4f);
                            c.RelativeColumn(1.2f);
                            c.RelativeColumn(1f);
                            c.RelativeColumn(1f);
                            c.RelativeColumn(1.1f);
                        });

                        table.Header(h =>
                        {
                            h.Cell().Background(Colors.BlueGrey.Darken3).Padding(4).Text("Date").FontColor(Colors.White);
                            h.Cell().Background(Colors.BlueGrey.Darken3).Padding(4).Text("Voucher").FontColor(Colors.White);
                            h.Cell().Background(Colors.BlueGrey.Darken3).Padding(4).Text("Particulars").FontColor(Colors.White);
                            h.Cell().Background(Colors.BlueGrey.Darken3).Padding(4).Text("Period").FontColor(Colors.White);
                            h.Cell().Background(Colors.BlueGrey.Darken3).Padding(4).AlignRight().Text("Debit").FontColor(Colors.White);
                            h.Cell().Background(Colors.BlueGrey.Darken3).Padding(4).AlignRight().Text("Credit").FontColor(Colors.White);
                            h.Cell().Background(Colors.BlueGrey.Darken3).Padding(4).AlignRight().Text("Balance").FontColor(Colors.White);
                        });

                        foreach (var e in entries)
                        {
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(3).Text($"{e.TransactionDate:dd-MMM-yyyy}");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(3).Text(e.VoucherNo);
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(3).Text(e.Particulars);
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(3).Text(e.FeePeriod ?? "");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(3).AlignRight().Text(e.DebitAmount > 0 ? e.DebitAmount.ToString("N2") : "-");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(3).AlignRight().Text(e.CreditAmount > 0 ? e.CreditAmount.ToString("N2") : "-");
                            table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(3).AlignRight().Text(e.RunningBalance.ToString("N2"));
                        }
                    });
                });
            }).GeneratePdf(path);

            return path;
        }

        public static string ExportCsv(IReadOnlyList<LedgerEntryDto> entries, string registrationNo, string outputDirectory)
        {
            Directory.CreateDirectory(outputDirectory);
            var path = Path.Combine(outputDirectory, $"Ledger_{registrationNo}_{DateTime.Now:yyyyMMddHHmmss}.csv");

            using var writer = new StreamWriter(path, false, Encoding.UTF8);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(entries.Select(e => new
            {
                e.TransactionDate,
                e.VoucherNo,
                e.Particulars,
                e.FeePeriod,
                e.DebitAmount,
                e.CreditAmount,
                e.RunningBalance,
                Status = e.Status.ToString()
            }));

            return path;
        }
    }

    public static class AdmitCardPdfGenerator
    {
        public static string GenerateBatch(IReadOnlyList<AdmitCardDto> cards, string institutionName, string outputDirectory)
        {
            Directory.CreateDirectory(outputDirectory);
            var path = Path.Combine(outputDirectory, $"AdmitCards_{DateTime.Now:yyyyMMddHHmmss}.pdf");

            Document.Create(container =>
            {
                foreach (var card in cards)
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A5);
                        page.Margin(24);
                        page.DefaultTextStyle(x => x.FontSize(10));

                        page.Header().Column(col =>
                        {
                            col.Item().AlignCenter().Text(institutionName).Bold().FontSize(15).FontColor(Colors.Indigo.Darken2);
                            col.Item().AlignCenter().Text("ADMIT CARD").Bold().FontSize(13);
                            col.Item().AlignCenter().Text(card.TermName).FontSize(11);
                            col.Item().PaddingTop(6).BorderBottom(1).BorderColor(Colors.Grey.Medium);
                        });

                        page.Content().PaddingTop(10).Column(col =>
                        {
                            col.Spacing(5);
                            col.Item().Text($"Admit No     : {card.AdmitCardNo}");
                            col.Item().Text($"Registration : {card.RegistrationNo}");
                            col.Item().Text($"Student Name : {card.FullName}");
                            col.Item().Text($"Father       : {card.FatherName}");
                            col.Item().Text($"Class/Sec    : {card.ClassName} - {card.SectionName}");
                            col.Item().Text($"Roll No      : {card.RollNumber}");
                            col.Item().PaddingTop(8).Text("Exam Timetable").Bold();
                            col.Item().Text(FormatTimetable(card.TimetableJson)).FontSize(9);
                            col.Item().PaddingTop(12).AlignCenter().Text($"* {card.BarcodeValue} *").FontFamily("Courier New").FontSize(12);
                            col.Item().AlignCenter().Text("Barcode").FontSize(8).FontColor(Colors.Grey.Darken1);
                        });

                        page.Footer().AlignCenter().Text($"Issued: {card.IssuedAt:dd-MMM-yyyy}  |  Bring this card to every exam").FontSize(8);
                    });
                }
            }).GeneratePdf(path);

            return path;
        }

        private static string FormatTimetable(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return "Timetable will be announced by the examination cell.";

            try
            {
                using var doc = System.Text.Json.JsonDocument.Parse(json);
                var sb = new StringBuilder();
                foreach (var item in doc.RootElement.EnumerateArray())
                {
                    var subject = item.GetProperty("subject").GetString();
                    var date = item.GetProperty("date").GetString();
                    var time = item.GetProperty("time").GetString();
                    sb.AppendLine($"• {subject} — {date} @ {time}");
                }
                return sb.ToString().TrimEnd();
            }
            catch
            {
                return json;
            }
        }
    }
}

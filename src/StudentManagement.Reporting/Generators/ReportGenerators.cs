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
        public static string GenerateBatch(
            IReadOnlyList<AdmitCardDto> cards,
            string institutionName,
            string outputDirectory,
            string? campusAddress = null)
        {
            Directory.CreateDirectory(outputDirectory);
            var path = Path.Combine(outputDirectory, $"AdmitCards_{DateTime.Now:yyyyMMddHHmmss}.pdf");
            var campus = string.IsNullOrWhiteSpace(campusAddress)
                ? (cards.FirstOrDefault()?.CampusAddress ?? "Dhanmondi Campus, Dhaka-1205")
                : campusAddress;

            Document.Create(container =>
            {
                for (int i = 0; i < cards.Count; i += 2)
                {
                    var first = cards[i];
                    var second = i + 1 < cards.Count ? cards[i + 1] : null;

                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(28);
                        page.DefaultTextStyle(x => x.FontSize(10).FontColor(Colors.BlueGrey.Darken4));

                        page.Content().Column(col =>
                        {
                            ComposeCard(col, first, institutionName, campus);
                            if (second != null)
                            {
                                col.Item().PaddingVertical(12).AlignCenter()
                                    .Text("--- cut here ---")
                                    .FontSize(8)
                                    .FontColor(Colors.Grey.Darken1);
                                ComposeCard(col, second, institutionName, campus);
                            }
                        });
                    });
                }
            }).GeneratePdf(path);

            return path;
        }

        private static void ComposeCard(ColumnDescriptor col, AdmitCardDto card, string institutionName, string campus)
        {
            col.Item().Border(1).BorderColor(Colors.Grey.Lighten1).Background(Colors.White).Padding(18).Column(cardCol =>
            {
                cardCol.Item().AlignCenter().Text(institutionName.ToUpperInvariant())
                    .Bold().FontSize(16).FontColor(Colors.BlueGrey.Darken4);
                cardCol.Item().AlignCenter().Text(campus)
                    .FontSize(9).FontColor(Colors.Grey.Darken1);
                cardCol.Item().PaddingTop(10).AlignCenter().Element(e =>
                {
                    e.Background(Colors.BlueGrey.Darken4).PaddingVertical(8).PaddingHorizontal(16)
                        .AlignCenter()
                        .Text((card.TermName ?? string.Empty).ToUpperInvariant() + " ADMIT CARD")
                        .Bold().FontSize(11).FontColor(Colors.White);
                });
                cardCol.Item().PaddingTop(10).BorderBottom(1).BorderColor(Colors.BlueGrey.Darken4).PaddingBottom(4);

                cardCol.Item().PaddingTop(12).Background(Colors.Grey.Lighten4).Padding(12).Row(row =>
                {
                    row.ConstantItem(90).Height(100).Background(Colors.White).Border(1).BorderColor(Colors.Grey.Lighten1)
                        .AlignCenter().AlignMiddle().Element(photo =>
                        {
                            if (!string.IsNullOrWhiteSpace(card.PhotoPath) && File.Exists(card.PhotoPath))
                            {
                                photo.Image(card.PhotoPath);
                            }
                            else
                            {
                                photo.AlignCenter().AlignMiddle().Text("PHOTO")
                                    .FontSize(9).FontColor(Colors.Grey.Darken1);
                            }
                        });

                    row.RelativeItem().PaddingLeft(14).Column(info =>
                    {
                        info.Spacing(4);
                        InfoLine(info, "Student Name:", card.FullName, false);
                        InfoLine(info, "Registration No:", card.RegistrationNo, true);
                        InfoLine(info, "Class & Section:", FormatClassSection(card), false);
                        InfoLine(info, "Roll No:", card.RollNumber, false);
                        InfoLine(info, "Venue:", card.Venue, false);
                    });
                });

                cardCol.Item().PaddingTop(12).Background(Colors.Grey.Lighten4).Padding(12).Column(inst =>
                {
                    inst.Item().Text("Instructions to Candidates:")
                        .Bold().FontSize(10).FontColor(Colors.BlueGrey.Darken4);
                    inst.Item().PaddingTop(4).Text("1. Candidates must bring this Admit Card and Student ID to the examination hall.")
                        .FontSize(8).FontColor(Colors.Grey.Darken1);
                    inst.Item().Text("2. Programmable calculators, mobile phones, and electronic devices are strictly prohibited.")
                        .FontSize(8).FontColor(Colors.Grey.Darken1);
                    inst.Item().Text("3. " + card.FeeClearanceNote)
                        .FontSize(8).FontColor(Colors.Grey.Darken1);
                });

                cardCol.Item().PaddingTop(28).Row(sig =>
                {
                    sig.RelativeItem().AlignCenter().Column(c =>
                    {
                        c.Item().Width(140).BorderBottom(1).BorderColor(Colors.BlueGrey.Darken4);
                        c.Item().PaddingTop(4).Text("Class Teacher").FontSize(9).FontColor(Colors.Grey.Darken1);
                    });
                    sig.RelativeItem().AlignCenter().Column(c =>
                    {
                        c.Item().Width(160).BorderBottom(1).BorderColor(Colors.BlueGrey.Darken4);
                        c.Item().PaddingTop(4).Text("Principal & Controller").Bold().FontSize(9).FontColor(Colors.BlueGrey.Darken4);
                    });
                });

                cardCol.Item().PaddingTop(8).AlignCenter()
                    .Text($"Admit No: {card.AdmitCardNo}   ·   Issued: {card.IssuedAt:dd-MMM-yyyy}")
                    .FontSize(7).FontColor(Colors.Grey.Darken1);
            });
        }

        private static void InfoLine(ColumnDescriptor col, string label, string value, bool accent)
        {
            col.Item().Row(r =>
            {
                r.ConstantItem(110).Text(label).FontSize(9).FontColor(Colors.Grey.Darken1);
                var text = r.RelativeItem().Text(value).Bold().FontSize(10);
                if (accent)
                    text.FontColor(Colors.Blue.Medium);
                else
                    text.FontColor(Colors.BlueGrey.Darken4);
            });
        }

        private static string FormatClassSection(AdmitCardDto card)
        {
            var section = string.IsNullOrWhiteSpace(card.SectionName) ? "" : $" - Section {card.SectionName}";
            return $"Class {card.ClassName}{section}";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using InvoicrApp.Models;

namespace InvoicrApp
{
    public interface IInvoiceHandler
    {
        Task ProcessEventAsync(InvoiceEvent invoiceEvent);
    }

    public class InvoiceHandler : IInvoiceHandler
    {
        private static readonly string InvoiceFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Invoices");
        private string GetInvoiceFilePath(Guid invoiceId) => Path.Combine(InvoiceFolder, $"Invoice-{invoiceId}.txt");

        public async Task ProcessEventAsync(InvoiceEvent invoiceEvent)
        {
            var lines = new List<string>
            {
                $"Invoice Number: {invoiceEvent.Content.InvoiceNumber}",
                $"Status: {invoiceEvent.Content.Status}",
                $"Created Date: {invoiceEvent.Content.CreatedDateUtc:O}",
                $"Due Date: {invoiceEvent.Content.DueDateUtc:O}",
                Environment.NewLine
            };

            lines.AddRange(invoiceEvent.Content.LineItems.SelectMany(li =>
                new[]
                {
                    $"Item description: {li.Description}",
                    $"Item quantity: {li.Quantity}",
                    $"Item cost: {li.UnitCost}",
                    $"Item total cost: {li.LineItemTotalCost}",
                    Environment.NewLine
                }));

            Directory.CreateDirectory(InvoiceFolder);
            await File.WriteAllLinesAsync(GetInvoiceFilePath(invoiceEvent.Content.InvoiceId), lines);
        }
    }
}

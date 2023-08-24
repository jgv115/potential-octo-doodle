using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using InvoicrApp.Models;
using Xunit;

namespace InvoicrApp.Tests
{
    public class InvoiceHandlerTests
    {
        private readonly InvoiceHandler _handler;

        public InvoiceHandlerTests()
        {
            _handler = new InvoiceHandler();
        }

        [Fact]
        public async Task ProcessEventAsync_ShouldAddRecord()
        {
            // Arrange
            var @event = new InvoiceEvent
            {
                Content = new Invoice
                {
                    InvoiceId = Guid.NewGuid(),
                    InvoiceNumber = "INV-Test-001",
                    Status = "DRAFT",
                    CreatedDateUtc = new DateTime(2021, 3, 22, 19, 15, 0, DateTimeKind.Utc),
                    DueDateUtc = new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    LineItems = new[]
                    {
                        new InvoiceLineItem { Description = "Xero Supplier", Quantity = 2, UnitCost = 24.25m, LineItemTotalCost = 48.5m }
                    }
                }
            };

            var expectedFileOutput =
$@"Invoice Number: INV-Test-001
Status: DRAFT
Created Date: 2021-03-22T19:15:00.0000000Z
Due Date: 2022-01-01T00:00:00.0000000Z


Item description: Xero Supplier
Item quantity: 2
Item cost: 24.25
Item total cost: 48.5


".Replace("\r", "").Replace("\n", Environment.NewLine);

            // Act
            await _handler.ProcessEventAsync(@event);

            // Assert
            var invoiceFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Invoices", $"Invoice-{@event.Content.InvoiceId}.txt");
            File.Exists(invoiceFilePath).Should().BeTrue();
            var result = await File.ReadAllTextAsync(invoiceFilePath);
            result.Should().BeEquivalentTo(expectedFileOutput);
        }
    }
}

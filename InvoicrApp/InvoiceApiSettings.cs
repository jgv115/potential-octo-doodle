using System.IO;
using System.Threading.Tasks;

namespace InvoicrApp
{
    public class InvoiceApiSettings
    {
        private const string InvoiceApiUri = "http://localhost:8200/api/v1/invoices/events";
        private static readonly string LastProcessedEventFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data");
        private static readonly string LastProcessedEventFilePath = Path.Combine(LastProcessedEventFolder, "InvoicrAppLastProcessedEvent.txt");

        public static async Task<string> GenerateUriAsync()
        {
            if (File.Exists(LastProcessedEventFilePath) && long.TryParse(await File.ReadAllTextAsync(LastProcessedEventFilePath), out var eventId))
            {
                return $"{InvoiceApiUri}?afterEventId={eventId}";
            }

            return InvoiceApiUri;
        }

        public static async Task SaveLastProcessedEventIdAsync(long eventId)
        {
            Directory.CreateDirectory(LastProcessedEventFolder);
            await File.WriteAllTextAsync(LastProcessedEventFilePath, eventId.ToString());
        }
    }
}

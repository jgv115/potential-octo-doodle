using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using InvoicrApp.Models;

namespace InvoicrApp
{
    public class InvoiceWorker
    {
        private readonly HttpClient _httpClient;
        private readonly IInvoiceHandler _invoiceHandler;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public InvoiceWorker(HttpClient httpClient, IInvoiceHandler invoiceHandler)
        {
            _httpClient = httpClient;
            _invoiceHandler = invoiceHandler;

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var response = await _httpClient.GetFromJsonAsync<InvoiceEventResponse>(await InvoiceApiSettings.GenerateUriAsync(), _jsonSerializerOptions, stoppingToken);

                if (response.Items.Any())
                {
                    await InvoiceApiSettings.SaveLastProcessedEventIdAsync(response.Items.Last().Id);
                    foreach (var invoiceEvent in response.Items)
                    {
                        await _invoiceHandler.ProcessEventAsync(invoiceEvent);
                    }
                }
            }
        }
    }

    public class InvoiceEventResponse
    {
        public IEnumerable<InvoiceEvent> Items { get; set; }
    }
}

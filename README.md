# InvoicrApp

The application consumes Invoices Event feed which Xero provides and creates a custom text file for each newly created invoice.

## Local Setup

1. Download and install the `.NET 6` SDK. Link [here](https://dotnet.microsoft.com/download/dotnet/6.0)
2. Clone this repository.
3. Use the following commands to start the mock API on http://localhost:8200. This is required for the Invoicr app to run.

   ```bash
   cd mock

   # to run the mock
   dotnet Invoicr.EventFeed.dll
   ```

4. Use the following commands to build and run the app.

   ```bash
   cd InvoicrApp

   # to run the app
   dotnet run
   ```

5. Use the following commands to run the tests

   ```bash
   cd Tests/InvoicrApp.Tests

   # to run the tests
   dotnet test
   ```

## Xero Invoices Event feed contract
Requires the mock to be running in Step 3

http://localhost:8200/docs/v1

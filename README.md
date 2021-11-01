# InvoicrApp

The application consumes Invoices Event feed which Xero provides and creates a custom text file for each newly created invoice.

## Local Setup

1. Download and install the `.NET 5` SDK. Link [here](https://dotnet.microsoft.com/download)
2. Clone this repository.
3. Use the following commands to build and run the app.

   ```bash
   cd InvoicrApp

   # to build the app
   dotnet build

   # to run the app
   dotnet run
   ```

4. Use the following commands to run the tests

   ```bash
   cd Tests/InvoicrApp.Tests

   # to run the tests
   dotnet test
   ```

## Xero Invoices Event feed contract

https://invoicr-eventfeed.netlify.app/#/

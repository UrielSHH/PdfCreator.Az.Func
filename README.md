# PdfCreator.Az.Func

Azure Function to create a PDF file based on a HTML template, once created, the file is storaged in an Azure File Share and returned as Base64.

Trigger type: Http with anonymous authentication.

It uses Azure App Configuration with Keyvault to get connection strings and values needed to run the app.

# Flowroute API Email Texting Platform
API application to enable sending/receiving text messages using Flowroute's API using Amazon's SES and SNS platform

## Set Up
This application is a ASP.NET Core 2.1 project expecting to be using Amazon's Simple Email Service and Simple Notification Service.

The application has built-in spam complaint and bounce handling at the following URL:

- example.com/api/Bounce

Flowroute's webhook settings should pointed to these URLs:

- **SMS:** example.com/api/Message
- **MMS:** example.com/api/MMS

For email receipt, SES should be configured to use SNS to send a notification to:

- example.com/api/Email

You'll need to configure a connection string in `appsettings` and `secrets`. There is a `secrets.json` in `src` to use for development. The `appsettings.Production.json` file is empty here but demonstrates the structure (`secrets.json` combined with `appsettings.Development.json`. Create an empty database and the application will create the necessary tables. You'll have to manually add a new user before you can login, however.

## Usage
Once set up and configured, go to example.com/IncomingRoutes and example.com/OutgoingRoutes to manage routes and example.com/Users to manage users.

## Notes
- By design, this application does not have a home page and will simply return a 404.
- Flowroute requires numbers to be 1xxxxxxxxxx so this application will add the 1 if not present (if number is 10 digits)
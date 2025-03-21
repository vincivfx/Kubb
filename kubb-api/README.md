# Kubb API

## Configuration

Everything is configured using `appsettings.json` file. Here is the listing of all settings:

- `ConnectionStrings`: database connection string (supported are Sqlite and Postgres)
- `DefaultConnection`: `Sqlite` or `Postgres` to select what database should be used by Kubb API
- `SSL`: Configuration to use Kestrel (default .NET server) directly
- `SystemConfiguration`: variables sent to frontend to configure UI
  - `RootUrl` should be set correctly to make the system run properly
- `Turnstile`: configuration used by [Cloudflare Turnstile](https://www.cloudflare.com/application-services/products/turnstile/) used as alternative to CAPTCHA. (not necessary)
- `EmailSettings`: configuration used to send emails

If you are running Kubb locally, please enter `http://localhost:5216` on your browser.
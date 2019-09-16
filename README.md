# Hangfire.RecurringJobAdmin

![dashboard](Content/dashboard.png)

A simple dashboard to manage Hangfire's recurring jobs.

This repo is the extension for [Hangfire](https://github.com/HangfireIO/Hangfire)

This package is based on the ["Hangfire.Recurring Job Extensions"](https://github.com/icsharp/Hangfire.RecurringJobExtensions/) package made by vigoss, thanks for your contribution to the community. It contains the following functionalities: 

* we can use RecurringJobAttribute they are stored in database and presented in the administrator.
* we can can create, edit jobs

## Instructions
Install a package from Nuget. 
```
Install-Package Hangfire.RecurringJobAdmin
```

Then add this in your code:

## For DotNetCore  :
for service side:
```csharp
services.AddHangfire(config => config.UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"))
                                                 .UseRecurringJobAdmin(typeof(Startup).Assembly))
```

## For NetFramework  :
for startup side:
```csharp
GlobalConfiguration.Configuration.UseSqlServerStorage("HangfireConnection").UseRecurringJobAdmin(typeof(Startup).Assembly)
```

## Credits
 * Braulio Alvarez

## License
Authored by: Brayan Mota (bamotav)

This project is under MIT license. You can obtain the license copy [here](https://github.com/bamotav/Hangfire.RecurringJobAdmin/blob/master/LICENSE).


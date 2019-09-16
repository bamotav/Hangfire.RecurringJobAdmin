# Hangfire.RecurringJobAdmin

![dashboard](Content/dashboard.png)

A simple dashboard that shows the Recurring Job to manage it.

This repo is the extension for [Hangfire](https://github.com/HangfireIO/Hangfire)

This package is based on the ["Hangfire.Recurring Job Extensions"](https://github.com/icsharp/Hangfire.RecurringJobExtensions/) package made by vigoss, thanks for your contribution to the community. It contains the following functionalities: we can use RecurringJobAttribute they are stored in database and presented in the administrator in which you can edit, create jobs, etc ...

## Instructions
Install a package from Nuget.

Then add this in your code:

## For DotNetCore  :
for service side:
```csharp
services.AddHangfire(config => config.UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"))
                                                 .UseRecurringJobAdmin(typeof(Startup).Assembly))
```

## For NetFramework  :
for service side:
```csharp
GlobalConfiguration.Configuration.UseSqlServerStorage("HangfireConnection").UseRecurringJobAdmin(typeof(Startup).Assembly)
```

## Credits
 * Braulio Alvarez

## License
Authored by: Brayan Mota (bamotav)



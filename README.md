# Hangfire.RecurringJobAdmin
[![NuGet](https://buildstats.info/nuget/Hangfire.RecurringJobAdmin)](https://www.nuget.org/packages/Hangfire.RecurringJobAdmin/)
[![Build status](https://ci.appveyor.com/api/projects/status/u2xrias2vk727beg/branch/master?svg=true)](https://ci.appveyor.com/project/bamotav/hangfire-recurringjobadmin/branch/master)
[![License MIT](https://img.shields.io/badge/license-MIT-green.svg)](http://opensource.org/licenses/MIT)


![dashboard](Content/dashboard.png)

A simple dashboard to manage Hangfire's recurring jobs.

This repo is an extension for [Hangfire](https://github.com/HangfireIO/Hangfire) based on ["Hangfire.Recurring Job Extensions"](https://github.com/icsharp/Hangfire.RecurringJobExtensions/) package made by vigoss, thanks for your contribution to the community. It contains the following functionalities: 

* We can use RecurringJobAttribute stored in database and presented in the administrator.
* We can create, edit jobs.

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
 
## Donation
If this project help you reduce time to develop, you can give me a cup of coffee :) 

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=KTW8JH379NXQL&item_name=+&currency_code=USD&source=url)


## License
Authored by: Brayan Mota (bamotav)

This project is under MIT license. You can obtain the license copy [here](https://github.com/bamotav/Hangfire.RecurringJobAdmin/blob/master/LICENSE).


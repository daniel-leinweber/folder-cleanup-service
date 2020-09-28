# folder-cleanup-service
The folder cleanup service enables to automatically cleanup user defined folders of old files. 

It is possible to define multiple folders for cleanup. Each folder can define its own maximum days until a file should be deleted. Furthermore each folder can define, if the files should be deleted in two steps or directly.

## Motivation
I created this service to cleanup my ever growing downloads folder automatically. 

## Technologies used
- C#
- .NET Core 3.1
- Worker Service (Microsoft.Extensions.Hosting.BackgroundService)

## How to use
The service can be installed as a Windows Service. For further information please visit: [Microsoft Docs](https://docs.microsoft.com/en-us/dotnet/framework/windows-services/how-to-install-and-uninstall-services)

### Service configuration
You can change the **appsettings.json** file to configure multiple folders for cleanup and to configure the interval of the service execution.

Add your folder objects in the section **Folders**.

```
"Folders": 
[
    {
        "Path": "C:\\Users\\DL\\Downloads",
        "MaximumFileAgeInDays": 7,
        "UseRecycleBin": true,
        "Recursive": true
    }
]
```

- *Path* will be the full path of the folder to cleanup
- *MaximumFileAgeInDays* should be the number of days a file is allowed to live in the specified folder
- *UseRecycleBin* is used to specify if the files should be moved into the systems recycle bin, or if they should be deleted immediately
- *Recursive* can be used to determine, if sub folders will also be processed

Change the *IntervalInMilliseconds* in the section **Service** to your needs.
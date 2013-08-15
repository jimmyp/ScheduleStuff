Schedule Stuff
=============

Schedule stuff is a library for scheduling in process background tasks.

The current implementation uses the ASP .NET cache provider to trigger the tasks.

Example Usage
-------------

```
Scheduler.Current.RunVolatileTask<IncrementCount>().Every(new TimeSpan(1));
```

####Based off
http://blog.stackoverflow.com/2008/07/easy-background-tasks-in-aspnet/
http://www.codeproject.com/Articles/12117/Simulate-a-Windows-Service-using-ASP-NET-to-run-sc
http://msdn.microsoft.com/en-us/magazine/cc163854.aspx#S7


open System.Daemon

[BatteryChargeMonitor.initTask()]
|> Async.Parallel
|> Async.RunSynchronously
|> ignore

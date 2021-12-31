open LinuxUtil.Daemon

[BatteryChargeMonitor.initTask()]
|> Async.Parallel
|> Async.RunSynchronously
|> ignore

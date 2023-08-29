open SystemUtil.Daemon

[BatteryChargeMonitor.initTask()]
|> Async.Parallel
|> Async.RunSynchronously
|> ignore

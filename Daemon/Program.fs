open SystemUtil
open SystemUtil.Daemon

printfn "Daemon started"
[BatteryChargeMonitor.initTask()]
|> Async.Parallel
|> Async.RunSynchronously
|> ignore

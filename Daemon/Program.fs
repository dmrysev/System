open SystemUtil
open SystemUtil.Daemon

printfn "Daemon started"
let resources = [ Wallpaper.init() ]
[BatteryChargeMonitor.initTask()]
|> Async.Parallel
|> Async.RunSynchronously
|> ignore

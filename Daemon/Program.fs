open SystemUtil

printfn "Daemon started"
[Battery.ChargeMonitor.initTask()]
|> Async.Parallel
|> Async.RunSynchronously
|> ignore

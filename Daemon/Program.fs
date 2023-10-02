open SystemUtil

printfn "Daemon started"
[
    Battery.ChargeMonitor.initTask()
    Wallpaper.initTask()    
]
|> Async.Parallel
|> Async.RunSynchronously
|> ignore

open SystemUtil

printfn "Wallpaper daemon started"
Wallpaper.initTask()
|> Async.RunSynchronously

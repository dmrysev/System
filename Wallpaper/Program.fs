open SystemUtil

printfn "Wallpaper daemon started"
Wallpaper.startDaemon()
System.Console.ReadLine() |> ignore

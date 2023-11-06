module SystemUtil.Daemon

open SystemUtil
open Util.Path
open CommandLine

[<Verb("daemon", HelpText = "Start daemon.")>]
type Options = { 
    [<Option(Hidden = true)>] PlaceHolder: unit
    
    [<Value(0, MetaName="ServiceName")>] 
    ServiceName: string }

let run (opts: Options) =
    // API.Event.infoMsg "Daemon started"
    printfn "Daemon started"
    [
        Battery.initTask()
        Wallpaper.initTask()
        DesktopEnvironment.initTask()
        Update.initTask()
    ]
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore
module SystemUtil.UI.Console.Command.Daemon2

open SystemUtil
open Util.IO.Path
open CommandLine

[<Verb("daemon2", HelpText = "Start daemon.")>]
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
    ]
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore
open SystemUtil
open CommandLine

[<EntryPoint>]
let main argv =
    let result = CommandLine.Parser.Default.ParseArguments<
        Daemon.Options,
        WebBrowser.Options>(argv)
    match result with
    | :? CommandLine.Parsed<obj> as command ->
        match command.Value with
        | :? Daemon.Options as opts -> Daemon.run opts 
        | :? WebBrowser.Options as opts -> WebBrowser.run opts 
        | _ -> ()
    | _ -> ()
    0

open SystemUtil
open SystemUtil.UI.Console
open CommandLine

[<EntryPoint>]
let main argv =
    let result = CommandLine.Parser.Default.ParseArguments<
        Command.Daemon.Options,
        Command.Daemon2.Options>(argv)
    match result with
    | :? CommandLine.Parsed<obj> as command ->
        match command.Value with
        | :? Command.Daemon.Options as opts -> Command.Daemon.run opts 
        | _ -> ()
    | _ -> ()
    0

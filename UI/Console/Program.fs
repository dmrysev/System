open SystemUtil
open CommandLine

[<EntryPoint>]
let main argv =
    let result = CommandLine.Parser.Default.ParseArguments<
        Daemon.Options,
        Compression.BatchExtract.Options,
        SessionLock.Options,
        FileSystem.SplitFolder.Options,
        Synchronization.Options,
        WebBrowser.Options>(argv)
    match result with
    | :? CommandLine.Parsed<obj> as command ->
        match command.Value with
        | :? Daemon.Options as opts -> Daemon.run opts 
        | :? Compression.BatchExtract.Options as opts -> Compression.BatchExtract.run opts 
        | :? SessionLock.Options as opts -> SessionLock.run opts 
        | :? FileSystem.SplitFolder.Options as opts -> FileSystem.SplitFolder.run opts 
        | :? Synchronization.Options as opts -> Synchronization.run opts 
        | :? WebBrowser.Options as opts -> WebBrowser.run opts 
        | _ -> ()
    | _ -> ()
    0

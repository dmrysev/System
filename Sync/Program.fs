open System
open Util.IO.Path
open CommandLine

type Options = {
    [<Option('s', "source", Required = true, 
     HelpText = "Source directory path.")>] Source: string 

    [<Option('d', "destination", Required = true, 
     HelpText = "Destination directory path.")>] Destination: string  }

let run (opts: Options) = 
    let sourceDirPath = opts.Source |> DirectoryPath
    let destinationDirPath = opts.Destination |> DirectoryPath
    Sync.run sourceDirPath destinationDirPath

[<EntryPoint>]
let main argv =
    let result = CommandLine.Parser.Default.ParseArguments<Options>(argv)
    result.WithParsed run |> ignore
    0
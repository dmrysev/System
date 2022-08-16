open CommandLine
open Util.IO.Path

type Options = {
    [<Value(0, MetaName="source", HelpText = "Source folder")>] Source: string 
    [<Value(1, MetaName="chunk_size", HelpText = "Chunk count")>] ChunkCount: int  }

let copyFilesChunk (destDirPath: DirectoryPath) (iteration: int) (filePaths: FilePath seq) =
    let chunkDestDirName = sprintf "%03i" iteration
    let chunkDestDirPath = destDirPath/DirectoryName chunkDestDirName
    Util.IO.Directory.create chunkDestDirPath
    for filePath in filePaths do 
        let destFilePath = chunkDestDirPath/(filePath |> FilePath.fileName)
        Util.IO.File.copy filePath destFilePath.Value

let rec splitFiles (filePaths: FilePath seq) (destDirPath: DirectoryPath) chunkSize iteration =
    let copyFilesChunk = copyFilesChunk destDirPath iteration
    if (filePaths |> Seq.length) <= chunkSize then 
        copyFilesChunk filePaths
    else
        filePaths 
        |> Seq.take chunkSize
        |> copyFilesChunk
        let filePaths = filePaths |> Seq.skip chunkSize
        let iteration = iteration + 1
        splitFiles filePaths destDirPath chunkSize iteration

let run (opts: Options) = 
    let sourceDirPath = Util.IO.Path.realPath opts.Source |> DirectoryPath
    let dirName = sourceDirPath |> DirectoryPath.directoryName
    let destinationDirName = $"{dirName.Value}_split"
    let destinationDirPath = sourceDirPath |> DirectoryPath.setHeadDirectoryName destinationDirName
    Util.IO.Directory.create destinationDirPath    
    let filePaths = 
        Util.Process.execute $"ls -vd '{sourceDirPath.Value}'/*"
        |> Util.String.split "\n"
        |> Seq.map FilePath
    splitFiles filePaths destinationDirPath opts.ChunkCount 0

[<EntryPoint>]
let main argv =
    let result = CommandLine.Parser.Default.ParseArguments<Options>(argv)
    match result with
    | :? Parsed<Options> as parsed -> run parsed.Value
    | :? NotParsed<Options> as notParsed -> printfn "%A" notParsed.Errors
    0
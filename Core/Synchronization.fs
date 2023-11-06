module SystemUtil.Synchronization

open Util.Path
open Util.Path
open CommandLine

[<Verb("sync", HelpText = "Synchronize two directories.")>]
type Options = {
    [<Option('s', "source", Required = true, 
     HelpText = "Source directory path.")>] Source: string 

    [<Option('d', "destination", Required = true, 
     HelpText = "Destination directory path.")>] Destination: string  }

let removeRedundantDirectoriesInDestination (source: DirectoryPath) (destination: DirectoryPath) =
    let sourceDirectories = 
        Util.IO.Directory.listDirectoriesRecursive source
        |> Seq.map (DirectoryPath.relativeTo source)
    Util.IO.Directory.listDirectoriesRecursive destination
    |> Seq.map (DirectoryPath.relativeTo destination)
    |> Seq.except sourceDirectories
    |> Seq.map (fun relativeFilePath -> destination/relativeFilePath)
    |> Seq.iter (fun dirPath -> Util.Process.run $"rm -rf '{dirPath.Value}'" )

let removeRedundantFilesInDestination (source: DirectoryPath) (destination: DirectoryPath) =
    let sourceFiles = 
        Util.IO.Directory.listFilesRecursive source
        |> Seq.map (FilePath.relativeTo source)
    Util.IO.Directory.listFilesRecursive destination
    |> Seq.map (FilePath.relativeTo destination)
    |> Seq.except sourceFiles
    |> Seq.map (fun relativeFilePath -> destination/relativeFilePath)
    |> Seq.iter (fun filePath -> Util.Process.run $"rm -f '{filePath.Value}'" )

let copyMissingFilesToDestination (source: DirectoryPath) (destination: DirectoryPath) =
    let destinationFiles = 
        Util.IO.Directory.listFilesRecursive destination
        |> Seq.map (FilePath.relativeTo destination)
    Util.IO.Directory.listFilesRecursive source
    |> Seq.map (FilePath.relativeTo source)
    |> Seq.iter (fun relativeFilePath -> 
        let sourceFilePath = source/relativeFilePath
        let destFilePath = destination/relativeFilePath
        Util.IO.Directory.create (destFilePath |> FilePath.directoryPath)
        Util.Process.run $"cp -uf '{sourceFilePath.Value}' '{destFilePath.Value}'" )
     
let sync sourceDirPath destinationDirPath =
    Util.IO.Directory.create destinationDirPath
    removeRedundantDirectoriesInDestination sourceDirPath destinationDirPath
    removeRedundantFilesInDestination sourceDirPath destinationDirPath
    copyMissingFilesToDestination sourceDirPath destinationDirPath

let run (opts: Options) = 
    let sourceDirPath = opts.Source |> DirectoryPath
    let destinationDirPath = opts.Destination |> DirectoryPath
    sync sourceDirPath destinationDirPath
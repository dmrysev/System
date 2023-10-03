module SystemUtil.Compression

open Util.IO.Path
open CommandLine

module BatchExtract =
    [<Verb("batch-extract", HelpText = "Batch extract compression archive files.")>]
    type Options = {
        [<Value(0, MetaName="Path", HelpText = "Source directory path.")>] 
        Source: string

        [<Option('o', "output", Required = false, Default = "",
        HelpText = "Output directory path.")>] 
        Output: string }

    let run (opts: Options) = 
        let sourceDirPath = opts.Source |> Util.IO.Path.realPath |> DirectoryPath
        let outputDirPath = 
            if opts.Output <> "" then opts.Output |> Util.IO.Path.realPath |> DirectoryPath
            else sourceDirPath
        Util.IO.Directory.ensureExists outputDirPath
        Util.IO.Directory.listFiles sourceDirPath
        |> Seq.iter(fun archiveFilePath ->
            let fileName = archiveFilePath |> FilePath.fileName |> FileName.withoutExtension |> FileName.value
            let extractOutputDirPath = outputDirPath/DirectoryName fileName
            Util.IO.Directory.create extractOutputDirPath
            $"~/scripts/extract.py '{archiveFilePath.Value}' '{extractOutputDirPath.Value}'"
            |> Util.Process.executeNoOutput )

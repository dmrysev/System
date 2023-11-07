module SystemUtil.Wallpaper

open SystemUtil
open Util.Path
open System

let configDirPath = Common.FileSystem.configDirPath/DirectoryName "wallpaper"
let configFilePath = configDirPath/FileName "config.json"
let currentWallpaperFilepath = FilePath "/tmp/current_wallpaper"
let applicationDataDirPath = Common.FileSystem.applicationDataDirPath/DirectoryName "wallpaper"
let stateAppDataFilePath = applicationDataDirPath/FileName "state"

type Config = {
    Delay: TimeSpan
    Shuffle: bool
    ImagesSources: ImagesSource seq
    ResumeLastState: bool }
and ImagesSource =
    | Directory of DirectoryPath
    | Playlist of FilePath

type State = {
    ImageIndex: int }
with static member Default = {
        ImageIndex = -1 }

let initTask () =
    [ configDirPath; applicationDataDirPath ] |> Seq.iter Util.IO.Directory.ensureExists
    let config = 
        Util.IO.File.readAllText configFilePath
        |> Util.Json.fromJson<Config>
    let images = config.ImagesSources |> Seq.collect (fun imagesSource ->
        match imagesSource with
        | Directory dirPath -> 
            Util.IO.Directory.listFilesRecursive dirPath
            |> Seq.filter FilePath.hasImageExtension
        | Playlist filePath ->
            Util.IO.File.readAllLines filePath
            |> Seq.filter Util.StringMatch.notEmpty
            |> Seq.map FilePath
        |> fun images ->
            if config.Shuffle then Util.Seq.shuffle images
            else images )
    let lastState = 
        if Util.IO.File.exists stateAppDataFilePath |> not then State.Default
        else
            Util.IO.File.readAllText stateAppDataFilePath
            |> Util.Json.fromJson<State>
    let mutable currentIndex = 
        if config.ResumeLastState then lastState.ImageIndex
        else -1
    let setWallpaper index =
        let imageFilePath = images |> Seq.item currentIndex
        Util.Process.run $"feh --bg-max '{imageFilePath.Value}'"
        Util.IO.File.copy imageFilePath currentWallpaperFilepath
        let state: State = { ImageIndex = currentIndex }
        Util.Json.toJson state
        |> Util.IO.File.writeText stateAppDataFilePath
    let rec nextImage() = 
        currentIndex <-
            if (currentIndex + 1) >= (images |> Util.Seq.lastIndex) then 0
            else currentIndex + 1
        try setWallpaper currentIndex
        with error -> 
            printfn $"{error}"
            Threading.Thread.Sleep (TimeSpan.FromSeconds(3))
            nextImage()
    async {
        while true do
            nextImage()
            if Util.Environment.WindowManagement.windowWithTitleExists "gui_background" |> not then
                Util.Process.run "feh --title=gui_background -ZFYr /tmp/current_wallpaper &"
                while Util.Environment.WindowManagement.windowWithTitleExists "gui_background" |> not do
                    do! Util.Async.sleep (TimeSpan.FromMilliseconds 100)
                Util.Environment.WindowManagement.setNoTaskbar "gui_background"
            do! Util.Async.sleep config.Delay }

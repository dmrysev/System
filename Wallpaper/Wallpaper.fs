module SystemUtil.Wallpaper

open SystemUtil
open Util.IO.Path
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

let init () =
    [ configDirPath; applicationDataDirPath ] |> Seq.iter Util.IO.Directory.ensureExists
    let config = Util.Json.deserializeFile<Config> configFilePath
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
        else Util.Json.deserializeFile<State> stateAppDataFilePath
    let mutable currentIndex = 
        if config.ResumeLastState then lastState.ImageIndex
        else -1
    let setWallpaper index =
        let imageFilePath = images |> Seq.item currentIndex
        Util.Process.run $"feh --bg-max '{imageFilePath.Value}'"
        Util.IO.File.copy imageFilePath currentWallpaperFilepath.Value
        let state: State = { ImageIndex = currentIndex }
        Util.Json.serializeToFile stateAppDataFilePath state
    setWallpaper currentIndex
    let nextImage _ = 
        currentIndex <-
            if (currentIndex + 1) >= (images |> Util.Seq.lastIndex) then 0
            else currentIndex + 1
        setWallpaper currentIndex
    let timer = new System.Timers.Timer (config.Delay)
    timer.Elapsed.Add nextImage
    timer.AutoReset <- true
    timer.Enabled <- true
    timer

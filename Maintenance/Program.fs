open SystemUtil
open Util.Path
open System

let createWallpaperConfig() =
    let config: Wallpaper.Config = {
        // Delay = TimeSpan.FromMinutes (1)
        Delay = TimeSpan.FromSeconds (5)
        Shuffle = false
        ImagesSources = [ 
            Wallpaper.ImagesSource.Playlist (FilePath "/mnt/data/sync/playlists/wallpapers") 
            // Wallpaper.ImagesSource.Directory (DirectoryPath "/mnt/data/sync/pictures")
        ]
        ResumeLastState = true }
    Util.IO.Directory.ensureExists Wallpaper.configDirPath
    Util.Json.serializeToFile Wallpaper.configFilePath config

createWallpaperConfig()

module SystemUtil.Update

open SystemUtil
open Util.Path
open System
open System.IO

type AppData = {
    LastUpdate: System.DateTime }

let initTask () = async {
    let appDataDirPath = Common.FileSystem.applicationDataDirPath/DirectoryName "update"
    Util.IO.Directory.ensureExists appDataDirPath
    let appDataFilePath = appDataDirPath/FileName "data.json"
    let needsUpdate =
        if not <| Util.IO.File.exists appDataFilePath then true
        else 
            let currentDateTime = System.DateTime.Now
            let data = Util.Json.deserializeFile<AppData> appDataFilePath
            (currentDateTime - data.LastUpdate).TotalDays >= 7.0
    if needsUpdate then
        while Util.Environment.XServer.isRunning() |> not do
            do! Util.Async.sleep (TimeSpan.FromSeconds 1)
        Async.Start(async {
            while Util.Environment.WindowManagement.windowWithTitleExists "system_update" |> not do
                do! Util.Async.sleep (TimeSpan.FromSeconds 1)
            Util.Process.run "wmctrl -a system_update"
        })
        Util.Process.run "xterm -T system_update -e 'sudo pacman -S archlinux-keyring && sudo pacman -Syu; read -p \"Press enter to continue\"'"
        let appData: AppData = { LastUpdate = System.DateTime.Now }
        Util.Json.serializeToFile appDataFilePath appData
}
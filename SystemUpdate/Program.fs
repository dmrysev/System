open System

open Util.IO.Path
open LinuxUtil
open Newtonsoft.Json
open System.IO

type Data = {
    updateDateTime: System.DateTime
}

let updateSystem () =
    Util.Process.executeNoOutput "sudo pacman -Syu"
    
let getDataEntry dataFilePath =
    Util.IO.File.readAllLines dataFilePath
    |> Seq.head
    |> JsonConvert.DeserializeObject<Data>

let setDataEntry dataFilePath (data: Data) =
    let json = JsonConvert.SerializeObject data
    Util.IO.File.writeText dataFilePath json

let needUpdate dataFilePath =
    if not <| Util.IO.File.exists dataFilePath then true
    else 
        let currentDateTime = System.DateTime.Now
        let data = getDataEntry dataFilePath
        (currentDateTime - data.updateDateTime).TotalDays >= 7.0

[<EntryPoint>]
let main argv =
    let appDataDirPath = Util.Environment.SpecialFolder.applicationData/DirectoryName "SystemUpdate"
    let dataFilePath = appDataDirPath/FileName "data.json"
    if not <| Util.IO.Directory.exists appDataDirPath then Util.IO.Directory.create appDataDirPath
    if needUpdate dataFilePath then
        updateSystem()
        let data = { updateDateTime = System.DateTime.Now }
        setDataEntry dataFilePath data
    0
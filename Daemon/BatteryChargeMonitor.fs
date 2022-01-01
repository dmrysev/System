module LinuxUtil.Daemon.BatteryChargeMonitor

open LinuxUtil
open Util.IO.Path

let checkBatteryCharge() =   
    let batteryWithHighestCharge =        
        Util.Environment.BatteryInfo.getBatteryInfo()
        |> Seq.sortByDescending (fun x -> x.Charge)
        |> Seq.head
    let warningMessageFilePath = 
        let guid = Util.Guid.generate()
        Util.Environment.SpecialFolder.temporary/FileName guid
    let warningMessage = $"[WARNING] Remaining battery level {batteryWithHighestCharge.Charge}%%"
    Util.IO.File.writeText warningMessageFilePath warningMessage
    if batteryWithHighestCharge.Charge < 10 then 
        if Util.Environment.XServer.isRunning() then
            let editor = Common.Default.Application.guiEditor
            Util.Process.executeNoOutput $"{editor} {warningMessageFilePath.Value}"
        else printfn $"{warningMessage}"
    elif batteryWithHighestCharge.Charge < 3 then
        Util.Process.executeNoOutput "suspend"

let initTask() = async {
    let timeout = System.TimeSpan.FromSeconds(30)
    while true do
        checkBatteryCharge()
        do! Util.Async.sleep timeout
}


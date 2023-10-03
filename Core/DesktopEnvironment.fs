module SystemUtil.DesktopEnvironment

open SystemUtil
open Util.IO.Path
open System

let bigScreenOutput = "HDMI-1"
let smallScreenOutput = "eDP-1"

let initTask() = async {
    while Util.Environment.XServer.isRunning() |> not do
        do! Util.Async.sleep (TimeSpan.FromSeconds 1)
    let isBigScreenConnected = (Util.Process.execute "xrandr") |> Util.String.contains $"{bigScreenOutput} connected"
    if isBigScreenConnected then
        Util.Process.run $"""
            xrandr --output {bigScreenOutput} --auto --primary
            xrandr --output {smallScreenOutput} --off
            xinput --set-prop 'pointer:MX Ergo Mouse' 'libinput Accel Speed' -0.3 > /dev/null 2>&1
        """
    else
        Util.Process.run $"""
            xrandr --output {smallScreenOutput} --auto --primary
        """
    while Util.Environment.WindowManagement.windowWithTitleExists "main_terminal" |> not do
        do! Util.Async.sleep (TimeSpan.FromSeconds 1)
    if isBigScreenConnected then Util.Process.run "wmctrl -a main_terminal -e 0,300,0,1400,800"
    else Util.Process.run "wmctrl -a main_terminal -e 0,160,0,1100,700"
    do! Util.Async.sleep (TimeSpan.FromSeconds 5)
    Util.Process.run "wmctrl -a main_terminal"
}

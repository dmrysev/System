﻿module SystemUtil.DesktopEnvironment

open SystemUtil
open Util.IO.Path
open System

let bigScreenOutput = "HDMI-1"
let smallScreenOutput = "eDP-1"

let initTask() = async {
    while Util.Environment.XServer.isRunning() |> not do
        do! Util.Async.sleep (TimeSpan.FromSeconds 1)
    Util.Process.run $"""
        xrandr --output {bigScreenOutput} --auto --primary
        xrandr --output {smallScreenOutput} --off
        xinput --set-prop 'pointer:MX Ergo Mouse' 'libinput Accel Speed' -0.3 > /dev/null 2>&1
    """
    while Util.Environment.WindowManagement.windowWithTitleExists "main_terminal" |> not do
        do! Util.Async.sleep (TimeSpan.FromSeconds 1)
    Util.Process.run "wmctrl -a main_terminal -e 0,300,0,1400,800"
    do! Util.Async.sleep (TimeSpan.FromSeconds 5)
    Util.Process.run "wmctrl -a main_terminal"
}
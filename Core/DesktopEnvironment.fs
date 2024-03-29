﻿module SystemUtil.DesktopEnvironment

open SystemUtil
open Util.Path
open System

let bigScreenOutput = "HDMI-1"
let smallScreenOutput = "eDP-1"

let initTask() = async {
    while Util.IO.Environment.XServer.isRunning() |> not do
        do! Util.Async.sleep (TimeSpan.FromSeconds 1)
    Util.Process.run "xinput --set-prop 'pointer:TPPS/2 IBM TrackPoint' 'libinput Accel Speed' -0.5 &>/dev/null"
    let isBigScreenConnected = (Util.Process.execute "xrandr") |> Util.String.contains $"{bigScreenOutput} connected"
    if isBigScreenConnected then
        Util.Process.run $"""
            xrandr --output {bigScreenOutput} --auto --primary
            xrandr --output {smallScreenOutput} --off
            xinput --set-prop 'pointer:MX Ergo Mouse' 'libinput Accel Speed' -0.3 > /dev/null 2>&1"""
    else
        Util.Process.run $"""
            xrandr --output {smallScreenOutput} --auto --primary"""
    while Util.IO.Environment.WindowManagement.windowWithTitleExists "main_terminal" |> not do
        do! Util.Async.sleep (TimeSpan.FromSeconds 1)
    if isBigScreenConnected then 
        let isBigScreenActive() = (Util.Process.execute "xrandr --listactivemonitors") |> Util.String.contains bigScreenOutput
        while isBigScreenActive() |> not do do! Util.Async.sleep (TimeSpan.FromSeconds 1)
        for i in {0..5} do
            Util.Process.run """
                wmctrl -a main_terminal -e 0,300,0,1400,800
                wmctrl -a Media -e 0,600,200,600,600
                wmctrl -a main_terminal
                wmctrl -a system_update"""
            do! Util.Async.sleep (TimeSpan.FromSeconds 1)
    else 
        let isSmallScreenActive() = (Util.Process.execute "xrandr --listactivemonitors") |> Util.String.contains smallScreenOutput
        while isSmallScreenActive() |> not do do! Util.Async.sleep (TimeSpan.FromSeconds 1)
        for i in {0..5} do
            Util.Process.run """
                wmctrl -a main_terminal -e 0,160,0,1100,700
                wmctrl -a Media -e 0,400,100,600,600
                wmctrl -a main_terminal
                wmctrl -a system_update"""
            do! Util.Async.sleep (TimeSpan.FromSeconds 1)
}

module SystemUtil.WebBrowser

open SystemUtil
open Util.Path
open CommandLine
open System

[<Verb("web-browser", HelpText = "Start web browser.")>]
type Options = { 
    [<Value(0, MetaName = "profile", Required = false, Default = "n")>] Profile: string
    [<Option('t', "tracked", Required = false, Default = false)>] IsTracked: bool }

let run (opts: Options) =
    let startOptions = { 
        Common.GuiApplication.StartOptions.Default with
            Maximized = true }
    if opts.IsTracked then
        Common.GuiApplication.start startOptions $"firefox -p {opts.Profile} > /dev/null 2>&1"
    else
        Common.GuiApplication.start startOptions $"librewolf -p {opts.Profile} > /dev/null 2>&1"

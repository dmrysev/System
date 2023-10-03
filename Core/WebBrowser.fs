module SystemUtil.WebBrowser

open SystemUtil
open Util.IO.Path
open CommandLine
open System

[<Verb("web-browser", HelpText = "Start web browser.")>]
type Options = { 
    [<Value(0, MetaName = "Profile", Required = false, Default = "n")>] Profile: string }

let run (opts: Options) =
    let startOptions = { 
        Common.GuiApplication.StartOptions.Default with
            Maximized = true }
    let startLibreWolf profile =
        Common.GuiApplication.start startOptions $"librewolf -p {profile} > /dev/null 2>&1"
    match opts.Profile with
    | "n" -> startLibreWolf "n"
    | "p" -> startLibreWolf "p"
    | "g" -> Common.GuiApplication.start startOptions "firefox > /dev/null 2>&1"
    | _ -> ()

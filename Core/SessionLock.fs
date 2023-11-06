module SystemUtil.SessionLock

open Util.Path
open CommandLine

[<Verb("session-lock", HelpText = "Lock session for current user.")>]
type Options = {
    [<Option(Hidden = true)>] PlaceHolder: unit }

let run (opts: Options) = 
    if Util.Environment.XServer.isRunning() then Util.Process.executeNoOutput "xsecurelock"
    else Util.Process.executeNoOutput "vlock"
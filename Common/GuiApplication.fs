module SystemUtil.Common.GuiApplication

open System

type StartOptions = {
    Maximized: bool
}
with static member Default = {
        Maximized = false }

let start (opts: StartOptions) command =
    let processName = command |> Util.String.split " " |> Seq.item 0
    Util.Process.run $"{command} &"
    while Util.Process.isRunningWithName processName |> not do
        Threading.Thread.Sleep (TimeSpan.FromSeconds 1)
    if opts.Maximized then
        for i in {0..5} do
            Util.Process.run $"wmctrl -a {processName} -b add,maximized_vert,maximized_horz"
            Threading.Thread.Sleep (TimeSpan.FromSeconds 1)

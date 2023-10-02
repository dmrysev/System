module SystemUtil.Common.Environment

open SystemUtil
open Util.IO.Path

let isDesktopEnvironmentRunning() = Util.Process.isRunningWithName "openbox"

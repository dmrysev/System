module SystemUtil.Common.Environment

open SystemUtil
open Util.Path

let isDesktopEnvironmentRunning() = Util.Process.isRunningWithName "openbox"

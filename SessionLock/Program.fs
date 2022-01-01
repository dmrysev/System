
if Util.Environment.XServer.isRunning() then Util.Process.executeNoOutput "xsecurelock"
else Util.Process.executeNoOutput "vlock"

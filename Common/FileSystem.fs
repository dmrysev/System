module SystemUtil.Common.FileSystem

open SystemUtil
open Util.Path

let applicationDataDirPath = Util.IO.Environment.SpecialFolder.applicationData/DirectoryName Common.Constant.applicationName
let configDirPath = Util.IO.Environment.SpecialFolder.home/DirectoryPath $"repo/dmrysev/platform/config/{Common.Constant.applicationName}"
let temporaryDirPath = Util.IO.Environment.SpecialFolder.temporary/DirectoryName $"{Common.Constant.applicationName}_{Common.Constant.applicationGuid}"

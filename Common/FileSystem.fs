module SystemUtil.Common.FileSystem

open SystemUtil
open Util.Path

let applicationDataDirPath = Util.Environment.SpecialFolder.applicationData/DirectoryName Common.Constant.applicationName
let configDirPath = Util.Environment.SpecialFolder.home/DirectoryPath $"repo/dmrysev/platform/config/{Common.Constant.applicationName}"
let temporaryDirPath = Util.Environment.SpecialFolder.temporary/DirectoryName $"{Common.Constant.applicationName}_{Common.Constant.applicationGuid}"

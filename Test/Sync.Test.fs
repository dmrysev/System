module System.Sync.Test

open System
open Util.IO.Path
open NUnit.Framework
open FsUnit

[<SetUp>]
let setUp() =
    Util.IO.Directory.delete (DirectoryPath "/tmp/bf166c83-c6d4-4c31-9f1b-3fd9e24a7a25")

[<TearDown>]
let tearDown() =
    Util.IO.Directory.delete (DirectoryPath "/tmp/bf166c83-c6d4-4c31-9f1b-3fd9e24a7a25")

[<Test>]
let ``If source has files that are not in destination, running sync, must copy those files from source to destination``() =
    // ARRANGE
    let tempDirPath = DirectoryPath "/tmp/bf166c83-c6d4-4c31-9f1b-3fd9e24a7a25"
    let sourceDirPath = tempDirPath/DirectoryName "source"
    let destinationDirPath = tempDirPath/DirectoryName "destination"
    Util.IO.File.create (sourceDirPath/FileName "file_1")
    Util.IO.File.create (sourceDirPath/FilePath "dir_A/file_2")
    Util.IO.File.create (sourceDirPath/FilePath "dir_A/dir_B/file_3")

    // ACT
    Sync.run sourceDirPath destinationDirPath
    let destinationFiles = Util.IO.Directory.listFilesRecursive destinationDirPath

    // ASSERT
    destinationFiles |> should contain (destinationDirPath/FileName "file_1")
    destinationFiles |> should contain (destinationDirPath/FilePath "dir_A/file_2")
    destinationFiles |> should contain (destinationDirPath/FilePath "dir_A/dir_B/file_3")

[<Test>]
let ``If destination has directories that are not in source, running sync, must delete those directories from destination``() =
    // ARRANGE
    let tempDirPath = DirectoryPath "/tmp/bf166c83-c6d4-4c31-9f1b-3fd9e24a7a25"
    let sourceDirPath = tempDirPath/DirectoryName "source"
    let destinationDirPath = tempDirPath/DirectoryName "destination"

    Util.IO.Directory.create (destinationDirPath/DirectoryName "dir_1")
    Util.IO.Directory.create (destinationDirPath/DirectoryName "dir_2")
    Util.IO.Directory.create (destinationDirPath/DirectoryPath "dir_3/dir_4")
    Util.IO.Directory.create (destinationDirPath/DirectoryPath "dir_3/dir_5")

    Util.IO.Directory.create (sourceDirPath/DirectoryPath "dir_1")
    Util.IO.Directory.create (sourceDirPath/DirectoryPath "dir_3/dir_5")

    // ACT
    Sync.run sourceDirPath destinationDirPath
    let destinationDirectories = Util.IO.Directory.listDirectoriesRecursive destinationDirPath

    // ASSERT
    destinationDirectories |> should contain (destinationDirPath/DirectoryName "dir_1")
    destinationDirectories |> should contain (destinationDirPath/DirectoryPath "dir_3/dir_5")

    destinationDirectories |> should not' (contain (destinationDirPath/DirectoryName "dir_2"))
    destinationDirectories |> should not' (contain (destinationDirPath/DirectoryPath "dir_3/dir_4"))

[<Test>]
let ``If destination has files that are not in source, running sync, must delete those files from destination``() =
    // ARRANGE
    let tempDirPath = DirectoryPath "/tmp/bf166c83-c6d4-4c31-9f1b-3fd9e24a7a25"
    let sourceDirPath = tempDirPath/DirectoryName "source"
    let destinationDirPath = tempDirPath/DirectoryName "destination"

    Util.IO.File.create (destinationDirPath/FileName "file_1")
    Util.IO.File.create (destinationDirPath/FileName "file_2")
    Util.IO.File.create (destinationDirPath/FilePath "dir_A/file_3")
    Util.IO.File.create (destinationDirPath/FilePath "dir_A/file_4")
    Util.IO.File.create (destinationDirPath/FilePath "dir_A/dir_B/file_5")
    Util.IO.File.create (destinationDirPath/FilePath "dir_A/dir_B/file_6")
    
    Util.IO.File.create (sourceDirPath/FileName "file_1")
    Util.IO.File.create (sourceDirPath/FilePath "dir_A/file_3")
    Util.IO.File.create (sourceDirPath/FilePath "dir_A/dir_B/file_5")

    // ACT
    Sync.run sourceDirPath destinationDirPath
    let destinationFiles = Util.IO.Directory.listFilesRecursive destinationDirPath

    // ASSERT
    destinationFiles |> should contain (destinationDirPath/FileName "file_1")
    destinationFiles |> should contain (destinationDirPath/FilePath "dir_A/file_3")
    destinationFiles |> should contain (destinationDirPath/FilePath "dir_A/dir_B/file_5")

    destinationFiles |> should not' (contain (destinationDirPath/FileName "file_2"))
    destinationFiles |> should not' (contain (destinationDirPath/FilePath "dir_A/file_4"))
    destinationFiles |> should not' (contain (destinationDirPath/FilePath "dir_A/dir_B/file_6"))

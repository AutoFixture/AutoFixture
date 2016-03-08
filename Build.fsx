#r @"packages/FAKE.4.19.0/tools/FakeLib.dll"

open Fake
open Fake.Testing
open System

let releaseFolder = "Release"
let nunitToolsFolder = "Packages/NUnit.Runners.2.6.2/tools"
let nuGetOutputFolder = "NuGetPackages"
let solutionsToBuild = !! "Src/*.sln"
let processorArchitecture = environVar "PROCESSOR_ARCHITECTURE"

let build target configuration = 
    let keyFile = 
        match getBuildParam "signkey" with
        | "" -> []
        | x  -> [ "AssemblyOriginatorKeyFile", FullName x ]
        
    let properties = [ "Configuration", configuration ] @ keyFile
    
    solutionsToBuild
    |> MSBuild "" target properties
    |> ignore
    
let clean   = build "Clean"
let rebuild = build "Rebuild" 

Target "CleanAll"           (fun _ -> ())    
Target "CleanVerify"        (fun _ -> clean "Verify")
Target "CleanRelease"       (fun _ -> clean "Release")
Target "CleanReleaseFolder" (fun _ -> CleanDir releaseFolder)

Target "Verify" (fun _ -> rebuild "Verify")

Target "BuildOnly" (fun _ -> rebuild "Release")
Target "TestOnly" (fun _ ->
    let configuration = getBuildParamOrDefault "Configuration" "Release"
    let parallelizeTests = getBuildParamOrDefault "ParallelizeTests" "False" |> Convert.ToBoolean
    let maxParallelThreads = getBuildParamOrDefault "MaxParallelThreads" "0" |> Convert.ToInt32  
    let parallelMode = if parallelizeTests then ParallelMode.All else ParallelMode.NoParallelization
    let maxThreads = if maxParallelThreads = 0 then CollectionConcurrencyMode.Default else CollectionConcurrencyMode.MaxThreads(maxParallelThreads)
      
    let testAssemblies = !! (sprintf "Src/*Test/bin/%s/*Test.dll" configuration) 
                         -- (sprintf "Src/AutoFixture.NUnit*.*Test/bin/%s/*Test.dll" configuration)

    testAssemblies
    |> xUnit2 (fun p -> { p with Parallel = parallelMode
                                 MaxThreads = maxThreads })

    let nunit2TestAssemblies = !! (sprintf "Src/AutoFixture.NUnit2.*Test/bin/%s/*Test.dll" configuration)

    nunit2TestAssemblies
    |> NUnit (fun p -> { p with StopOnError = false })

    let nunit3TestAssemblies = !! (sprintf "Src/AutoFixture.NUnit3.UnitTest/bin/%s/Ploeh.AutoFixture.NUnit3.UnitTest.dll" configuration)

    nunit3TestAssemblies
    |> NUnit3 (fun p -> { p with StopOnError = false })
)

Target "BuildAndTestOnly" (fun _ -> ())
Target "Build" (fun _ -> ())
Target "Test"  (fun _ -> ())

Target "DeleteTestResultFiles" (fun _ ->
    DeleteFile "TestResult.xml"
)

Target "CopyToReleaseFolder" (fun _ ->
    let buildOutput = [
      "Src/AutoFixture/bin/Release/Ploeh.AutoFixture.dll";
      "Src/AutoFixture/bin/Release/Ploeh.AutoFixture.pdb";
      "Src/AutoFixture/bin/Release/Ploeh.AutoFixture.XML";
      "Src/SemanticComparison/bin/Release/Ploeh.SemanticComparison.dll";
      "Src/SemanticComparison/bin/Release/Ploeh.SemanticComparison.pdb";
      "Src/SemanticComparison/bin/Release/Ploeh.SemanticComparison.XML";
      "Src/AutoMoq/bin/Release/Ploeh.AutoFixture.AutoMoq.dll";
      "Src/AutoMoq/bin/Release/Ploeh.AutoFixture.AutoMoq.pdb";
      "Src/AutoMoq/bin/Release/Ploeh.AutoFixture.AutoMoq.XML";
      "Src/AutoRhinoMock/bin/Release/Ploeh.AutoFixture.AutoRhinoMock.dll";
      "Src/AutoRhinoMock/bin/Release/Ploeh.AutoFixture.AutoRhinoMock.pdb";
      "Src/AutoRhinoMock/bin/Release/Ploeh.AutoFixture.AutoRhinoMock.XML";
      "Src/AutoFakeItEasy/bin/Release/Ploeh.AutoFixture.AutoFakeItEasy.dll";
      "Src/AutoFakeItEasy/bin/Release/Ploeh.AutoFixture.AutoFakeItEasy.pdb";
      "Src/AutoFakeItEasy/bin/Release/Ploeh.AutoFixture.AutoFakeItEasy.XML";
      "Src/AutoNSubstitute/bin/Release/Ploeh.AutoFixture.AutoNSubstitute.dll";
      "Src/AutoNSubstitute/bin/Release/Ploeh.AutoFixture.AutoNSubstitute.pdb";
      "Src/AutoNSubstitute/bin/Release/Ploeh.AutoFixture.AutoNSubstitute.XML";
      "Src/AutoFoq/bin/Release/Ploeh.AutoFixture.AutoFoq.dll";
      "Src/AutoFoq/bin/Release/Ploeh.AutoFixture.AutoFoq.pdb";
      "Src/AutoFoq/bin/Release/Ploeh.AutoFixture.AutoFoq.XML";
      "Src/AutoFixture.xUnit.net/bin/Release/Ploeh.AutoFixture.Xunit.dll";
      "Src/AutoFixture.xUnit.net/bin/Release/Ploeh.AutoFixture.Xunit.pdb";
      "Src/AutoFixture.xUnit.net/bin/Release/Ploeh.AutoFixture.Xunit.XML";
      "Src/AutoFixture.xUnit.net2/bin/Release/Ploeh.AutoFixture.Xunit2.dll";
      "Src/AutoFixture.xUnit.net2/bin/Release/Ploeh.AutoFixture.Xunit2.pdb";
      "Src/AutoFixture.xUnit.net2/bin/Release/Ploeh.AutoFixture.Xunit2.XML";
      "Src/AutoFixture.NUnit2/bin/Release/Ploeh.AutoFixture.NUnit2.dll";
      "Src/AutoFixture.NUnit2/bin/Release/Ploeh.AutoFixture.NUnit2.pdb";
      "Src/AutoFixture.NUnit2/bin/Release/Ploeh.AutoFixture.NUnit2.XML";
      "Src/AutoFixture.NUnit2/bin/Release/Ploeh.AutoFixture.NUnit2.Addins.dll";
      "Src/AutoFixture.NUnit2/bin/Release/Ploeh.AutoFixture.NUnit2.Addins.pdb";
      "Src/AutoFixture.NUnit2/bin/Release/Ploeh.AutoFixture.NUnit2.Addins.XML";      
      "Src/AutoFixture.NUnit3/bin/Release/Ploeh.AutoFixture.NUnit3.dll";
      "Src/AutoFixture.NUnit3/bin/Release/Ploeh.AutoFixture.NUnit3.pdb";
      "Src/AutoFixture.NUnit3/bin/Release/Ploeh.AutoFixture.NUnit3.XML";
      "Src/Idioms/bin/Release/Ploeh.AutoFixture.Idioms.dll";
      "Src/Idioms/bin/Release/Ploeh.AutoFixture.Idioms.pdb";
      "Src/Idioms/bin/Release/Ploeh.AutoFixture.Idioms.XML";
      "Src/Idioms.FsCheck/bin/Release/Ploeh.AutoFixture.Idioms.FsCheck.dll";
      "Src/Idioms.FsCheck/bin/Release/Ploeh.AutoFixture.Idioms.FsCheck.pdb";
      "Src/Idioms.FsCheck/bin/Release/Ploeh.AutoFixture.Idioms.FsCheck.XML";
      nunitToolsFolder @@ "lib/nunit.core.interfaces.dll"    
    ]        
    let nuGetPackageScripts = !! "NuGet/*.ps1" ++ "NuGet/*.txt" ++ "NuGet/*.pp" |> List.ofSeq
    let releaseFiles = buildOutput @ nuGetPackageScripts
        
    releaseFiles
    |> CopyFiles releaseFolder
)

Target "CleanNuGetPackages" (fun _ -> 
    CleanDir nuGetOutputFolder
)

Target "NuGetPack" (fun _ ->    
    let version = "Src/AutoFixture/bin/Release/Ploeh.AutoFixture.dll" 
                  |> GetAssemblyVersion
                  |> (fun v -> sprintf "%i.%i.%i" v.Major v.Minor v.Build)  
    
    let nuSpecFiles = !! "NuGet/*.nuspec"

    nuSpecFiles
    |> Seq.iter (fun f -> NuGet (fun p -> { p with Version = version
                                                   WorkingDir = releaseFolder
                                                   OutputPath = nuGetOutputFolder }) f)
)

Target "CompleteBuild" (fun _ -> ())
 
"CleanVerify"  ==> "CleanAll"  
"CleanRelease" ==> "CleanAll"

"CleanReleaseFolder" ==> "Verify"  
"CleanAll"           ==> "Verify"

"Verify"    ==> "Build"
"BuildOnly" ==> "Build"

"Build"    ==> "Test"
"TestOnly" ==> "Test"
 
"BuildOnly" ==> "TestOnly"

"BuildOnly" ==> "BuildAndTestOnly"
"TestOnly"  ==> "BuildAndTestOnly"
      
"Test" ==> "DeleteTestResultFiles"
"Test" ==> "CopyToReleaseFolder"

"CleanNuGetPackages"  ==> "NuGetPack"
"CopyToReleaseFolder" ==> "NuGetPack"

"NuGetPack"              ==> "CompleteBuild"
"DeleteTestResultFiles"  ==> "CompleteBuild"

RunTargetOrDefault "CompleteBuild"
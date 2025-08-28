require "vstudio"
function platformsElement(cfg)
   _p(2,'<Platforms>x64</Platforms>')
end

premake.override(premake.vstudio.cs2005.elements, "projectProperties", function (oldfn, cfg)
   return table.join(oldfn(cfg), {
   platformsElement,
   })
end)


workspace "foxhole-void"
architecture "x64"
   configurations { "Debug", "Release" }
   startproject "foxhole-void-bot"

   project "foxhole-void-bot"
      kind "ConsoleApp" -- CLI application
      dotnetframework "net9.0" -- Targeting .NET 9.0
      location "foxhole-void-bot"
dotnetsdk "Web"
      language "C#"
      targetdir "bin/%{cfg.buildcfg}"
files { "%{prj.name}/src/**.cs", "%{prj.name}/src/**.env" }       -- Include all C# source files
nuget { "NetCord:1.0.0-alpha.416", "DotNetEnv:3.1.1", "NetCord.Hosting:1.0.0-alpha.416", "NetCord.Hosting.Services:1.0.0-alpha.416", "Firebase.SDK:3.1.0" }
      vsprops {
         PublishSingleFile = "true",
         SelfContained = "true",
         IncludeNativeLibrariesForSelfExtract = "true"
      }

      filter "configurations:Debug"
         defines { "DEBUG" }
         optimize "Off"
      
      filter "configurations:Release"
         symbols "Off"
         defines { "NDEBUG" }
         optimize "On"
      

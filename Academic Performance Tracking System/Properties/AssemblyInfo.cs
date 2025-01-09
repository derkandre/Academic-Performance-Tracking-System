using System.Windows;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Channels;

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None,            //where theme specific resource dictionaries are located
                                                //(used if a resource is not found in the page,
                                                // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly   //where the generic resource dictionary is located
                                                //(used if a resource is not found in the page,
                                                // app, or any theme specific resource dictionaries)
)]

[assembly: System.Reflection.AssemblyCompanyAttribute("Academic Performance Tracking System")]
[assembly: System.Reflection.AssemblyConfigurationAttribute("Debug")]
[assembly: System.Reflection.AssemblyFileVersionAttribute("1.0.0.0")]

// MY VERSIONING SCHEME: MAJOR.MINOR.MICRO-MODIFIER.YY00D
// MAJOR: Significant changes or breaking changes
// MINOR: Backward-compatible feature additions
// MICRO: Bug fixes or minor changes
// MODIFIER: Development stage (e.g., alpha, beta)
// YY00D: Year and two 0-padded day of the year (e.g., 25009 = 9th day of 2025)

// Example: "0.1.0-prealpha.25009"
// - MAJOR: 0 initial development
// - MINOR: 1 first set of feature
// - MICRO: 0 no bug fixes yet
// - MODIFIER: prealpha pre-release stage
// - YY00D: 25009 build identifier
[assembly: System.Reflection.AssemblyInformationalVersion("0.1.0-prealpha.25009")]
[assembly: System.Reflection.AssemblyProductAttribute("Academic Performance Tracking System")]
[assembly: System.Reflection.AssemblyTitleAttribute("Academic Performance Tracking System")]
[assembly: System.Reflection.AssemblyCopyright("Copyright (c) 2025 Derk Andre")]
[assembly: System.Reflection.AssemblyVersionAttribute("1.0.0.0")]
[assembly: System.Runtime.Versioning.TargetPlatformAttribute("Windows7.0")]
[assembly: System.Runtime.Versioning.SupportedOSPlatformAttribute("Windows7.0")]

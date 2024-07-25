using System.Diagnostics.CodeAnalysis;
using CommandLine;
using CommandLine.Text;

namespace HydraScript;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
[ExcludeFromCodeCoverage]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class CommandLineSettings
{
    [Value(0, MetaName = "InputFilePath", Required = true, HelpText = "Path to input file")]
    public required string InputFilePath { get; set; }

    [Option('d', "dump", Default = false, HelpText = "Show dump data of interpreter")]
    public bool Dump { get; set; }

    [Usage(ApplicationAlias = "HydraScript")]
    public static IEnumerable<Example> Examples
    {
        get
        {
            yield return new Example("Simple interpretation call", 
                new CommandLineSettings { InputFilePath = "file.js" });
            yield return new Example("Request dump",
                new CommandLineSettings { InputFilePath = "file.js", Dump = true });
        }
    }
}
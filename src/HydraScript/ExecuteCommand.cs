using System.CommandLine;
using System.Diagnostics.CodeAnalysis;

namespace HydraScript;

[ExcludeFromCodeCoverage]
internal class ExecuteCommand : RootCommand
{
    internal ExecuteCommand() : base("HydraScript interpreter")
    {
        PathArgument = new Argument<FileInfo>(name: "path")
        {
            Description = "Path to input file"
        };
        Add(PathArgument);

        DumpOption = new Option<bool>(name: "--dump", aliases: ["-d", "/d"])
        {
            Description = "Show dump data of interpreter",
            DefaultValueFactory = _ => false
        };
        Add(DumpOption);
    }

    internal Argument<FileInfo> PathArgument { get; }
    internal Option<bool> DumpOption { get; }
}
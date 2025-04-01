using System.CommandLine;

namespace HydraScript;

internal class ExecuteCommand : CliRootCommand
{
    internal ExecuteCommand() : base("HydraScript interpreter")
    {
        PathArgument = new CliArgument<FileInfo>(name: "path")
        {
            Description = "Path to input file"
        };
        Add(PathArgument);

        DumpOption = new CliOption<bool>(name: "--dump", aliases: ["-d", "/d"])
        {
            Description = "Show dump data of interpreter",
            DefaultValueFactory = _ => false
        };
        Add(DumpOption);
    }

    internal CliArgument<FileInfo> PathArgument { get; }
    internal CliOption<bool> DumpOption { get; }
}
using System.CommandLine;

namespace HydraScript;

internal class ExecuteCommand : RootCommand
{
    public ExecuteCommand() : base("HydraScript interpreter")
    {
        PathArgument = new Argument<FileInfo>(
            name: "path",
            description: "Path to input file");
        AddArgument(PathArgument);

        DumpOption = new Option<bool>(
            ["-d", "--dump"],
            getDefaultValue: () => false,
            description: "Show dump data of interpreter");
        AddOption(DumpOption);
    }

    public Argument<FileInfo> PathArgument { get; }
    public Option<bool> DumpOption { get; }
}
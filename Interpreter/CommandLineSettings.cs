using System.Diagnostics.CodeAnalysis;
using CommandLine;
using CommandLine.Text;

namespace Interpreter
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
    [ExcludeFromCodeCoverage]
    // ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
    public class CommandLineSettings
    {
        [Value(0, MetaName = "InputFilePath", Required = true, HelpText = "Path to input file")]
        public virtual string InputFilePath { get; set; }

        [Option('d', "dump", Default = false, HelpText = "Show dump data of interpreter")]
        public virtual bool Dump { get; set; }

        [Usage(ApplicationAlias = "Interpreter")]
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

        public string GetInputFileName() =>
            InputFilePath.Split(".js")[0];

        public virtual string GetText() =>
            File.ReadAllText(InputFilePath);
    }
}
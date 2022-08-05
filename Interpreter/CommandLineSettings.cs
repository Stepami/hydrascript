using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using CommandLine;
using CommandLine.Text;
using Interpreter.Models;

namespace Interpreter
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
    public class CommandLineSettings
    {
        [Value(0, MetaName = "InputFilePath", Required = true, HelpText = "Path to input file")]
        public string InputFilePath { get; set; }

        [Option('d', "dump", Default = false, HelpText = "Show dump data of interpreter")]
        public bool Dump { get; set; }

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

        public string GetInputFileName() => InputFilePath.Split(".js")[0];

        public LexerQueryModel CreateLexerQuery() =>
            new()
            {
                Text = File.ReadAllText(InputFilePath)
            };
    }
}
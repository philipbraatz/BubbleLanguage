using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doorfail.Bebox
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Loading Test file");
            StreamReader reader = new StreamReader(
                @"C:\Users\Doorfail.WINDOWSPC\source\repos\BubbleLanguage\Bebox\Zetho\scope.zo");
            string file = reader.ReadToEnd().ToLower();
            AntlrInputStream antlrStream = new AntlrInputStream(file);
            BubblesLexer lexer = new BubblesLexer(antlrStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            BubblesParser parser = new BubblesParser(tokens);

            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(DescriptiveErrorListener.Instance);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(DescriptiveErrorListener.Instance);

            IParseTree tree = parser.file();

            BoxParser boxParser = new BoxParser();
            var rootBubble = boxParser.Visit(tree);

            if (rootBubble != null)
            {
                Console.WriteLine("\n--Root Dictionary--\n"+string.Join("\n",rootBubble.AsDictionary().Keys));

                Console.WriteLine("--File Contents--\n\n" + rootBubble.ToString());
                
                Function main = rootBubble.EntryPoint;
                if (main!=null)
                {

                    Console.WriteLine( main.Run());
                }


            }
            else
                Console.WriteLine("Error: Bad BoxParser\n\n");

            Console.WriteLine("--Tree--\n\n" + tree.ToStringTree());
            Console.Read();
        }
    }

    public class DescriptiveErrorListener : BaseErrorListener, IAntlrErrorListener<int>
    {
        public static DescriptiveErrorListener Instance { get; } = new DescriptiveErrorListener();
        public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            if (!REPORT_SYNTAX_ERRORS) return;
            string sourceName = recognizer.InputStream.SourceName;
            // never ""; might be "<unknown>" == IntStreamConstants.UnknownSourceName
            sourceName = $"{sourceName}:{line}:{charPositionInLine}";
            Console.Error.WriteLine($"{sourceName}: line {line}:{charPositionInLine} {msg}");
        }
        //public override void SyntaxError(TextWriter output, IRecognizer recognizer, Token offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        //{
        //    this.SyntaxError(output, recognizer, 0, line, charPositionInLine, msg, e);
        //}

        public void SyntaxError([NotNull] IRecognizer recognizer, [Nullable] int offendingSymbol, int line, int charPositionInLine, [NotNull] string msg, [Nullable] RecognitionException e)
        {
            if (!REPORT_SYNTAX_ERRORS) return;
            string sourceName = recognizer.InputStream.SourceName;
            // never ""; might be "<unknown>" == IntStreamConstants.UnknownSourceName
            sourceName = $"{sourceName}:{line}:{charPositionInLine}";
            Console.Error.WriteLine($"{sourceName}: line {line}:{charPositionInLine} {msg}");
        }

        static readonly bool REPORT_SYNTAX_ERRORS = true;
    }
}

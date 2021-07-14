using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bebox
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Loading Test file");
            StreamReader reader = new StreamReader(
                @"C:\Users\Doorfail.WINDOWSPC\source\repos\BubbleLanguage\Bebox\nestingTesting.zo");
            string file = reader.ReadToEnd().ToLower();
            AntlrInputStream antlrStream = new AntlrInputStream(file);
            BubblesLexer lexer = new BubblesLexer(antlrStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            BubblesParser parser = new BubblesParser(tokens);

            IParseTree tree = parser.file();

            FileParser fileParser = new FileParser();
            BoxParser boxParser = new BoxParser();
            var output = fileParser.Visit(tree);
            var box = boxParser.Visit(tree);

            if (box != null)
                Console.WriteLine("--Cooler Results--\n\n" + box.ToString());
            else
                Console.WriteLine("Error: Bad BoxParser\n\n");

            Console.WriteLine("--Just Everything as a mess--\n\n" + tree.ToStringTree());
            Console.Read();
        }
    }
}

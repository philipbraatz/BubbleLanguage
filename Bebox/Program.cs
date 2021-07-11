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
                @"C:\Users\Doorfail.WINDOWSPC\source\repos\BubbleLanguage\Bebox\helloWord.zo");
            string file = reader.ReadToEnd().ToLower();
            AntlrInputStream antlrStream = new AntlrInputStream(file);
            BubblesLexer lexer = new BubblesLexer(antlrStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            BubblesParser parser = new BubblesParser(tokens);

            IParseTree tree = parser.file();

            Bubble calculator = new Bubble();
            var output = calculator.Visit(tree);

            Console.WriteLine("--RESULT--\n\n" + output);
            Console.WriteLine(tree.ToStringTree());
            Console.Read();
        }
    }
}

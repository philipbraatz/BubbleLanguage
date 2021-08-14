using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Doorfail.Bebox.Test
{
    public class BoxParserTest
    {
        private static ITestOutputHelper _testOutput;
        public BoxParserTest(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }


        [Fact]
        public void EmptyFile()
        {
            string file = "";
            AntlrInputStream antlrStream = new AntlrInputStream(file);
            BubblesLexer lexer = new BubblesLexer(antlrStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            BubblesParser parser = new BubblesParser(tokens);

            IParseTree tree = parser.file();

            BoxParser boxParser = new BoxParser();
            var box = boxParser.Visit(tree);

            var expected = new Box("Root", null, BubbleType.FILE, ScopeType.PUBLIC);

            string expectedStr = expected.ToString();
            string resultStr = box.ToString();

            Assert.Equal(expected.ToString(), box.ToString());
        }

        [Fact]
        public void SimpleNesting()
        {

            string file = @"space topLevel{
    class topClass{
        }
    space bottomLevel{
        class bottomClass{
             public methods{
                public func eggsAndWaffles(){
                    itsjustmassive
                }
            }
        }
    }
}".ToLower();

            var expected = new Box("Root", new List<Object>(), BubbleType.FILE, ScopeType.PUBLIC);
            Space topSpace = new Space(expected, "toplevel", ScopeType.PUBLIC);
            Class topClass = new Class(topSpace, "topclass", ScopeType.PUBLIC);
            Space bottomSpace = new Space(topSpace as Space, "bottomlevel", ScopeType.PUBLIC);
            Class bottomClass = new Class(bottomSpace, "bottomclass", ScopeType.PUBLIC);
            MethodArea bottomMethods = new MethodArea(bottomClass, null, ScopeType.PUBLIC);
            Function functionEgg = new Function(bottomMethods, "eggsandwaffles", null,
                new List<string> { "itsjustmassive" });
            bottomMethods.Inside.Add(functionEgg);
            bottomClass.Inside.Add(bottomMethods);
            bottomSpace.Inside.Add(bottomClass);
            topSpace.Inside.Add(topClass);
            topSpace.Inside.Add(bottomSpace);

            expected.Inside.Add(topSpace);


            AntlrInputStream antlrStream = new AntlrInputStream(file);
            BubblesLexer lexer = new BubblesLexer(antlrStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            BubblesParser parser = new BubblesParser(tokens);

            IParseTree tree = parser.file();

            BoxParser boxParser = new BoxParser();
            var box = boxParser.Visit(tree);

            string expectedStr = expected.ToString();
            string resultStr = box.ToString();

            Assert.Equal(box.AsDictionary(), expected.AsDictionary());
        }

        [Fact]
        public void MultiNesting()
        {

            string file = @"space topLevel{
    class topClass{
        }
    space bottomLevel{
        class bottomClass{
             public methods{
                public func eggsAndWaffles(){
                    itsjustmassive
                }
            }
        }
        class bottomAreBetterClass{
             public methods{
                public func eggsAndWaffles(){
                    itsjustmassive
                }
                public func eggsAndWafflesWhenGood(){
                    Itwasdonewell
                }
            }
    }
}

space topLevel2{
    class topClass{
        }
    space bottomLevel{
        class bottomClass{
             public methods{
                public func eggsAndWaffles(){
                    itsjustmassive
                }
            }
        }
    }
}".ToLower();

            var expected = new Box("Root", new List<Object>(), BubbleType.FILE, ScopeType.PUBLIC);
            Space topSpace = new Space(expected, "toplevel", ScopeType.PUBLIC);
            Space bottomSpace = new Space(topSpace as Space, "bottomlevel", ScopeType.PUBLIC);
            Class bottomClass = new Class(bottomSpace, "bottomclass", ScopeType.PUBLIC);
            Class betterClass = new Class(bottomSpace, "bottomarebetterclass", ScopeType.PUBLIC);
            MethodArea bottomMethods = new MethodArea(bottomClass, null, ScopeType.PUBLIC);
            MethodArea betterMethods = new MethodArea(betterClass, null, ScopeType.PUBLIC);
            Function functionEgg = new Function(bottomMethods, "eggsandwaffles", null,
                new List<string> { "itsjustmassive" });
            Function functionBetter = new Function(betterMethods, "eggsandwaffles", null,
               new List<string> { "itsjustmassive" });
            Function functionBetterGood = new Function(betterMethods, "eggsandwaffleswhengood", null,
               new List<string> { "itwasdonewell" });

            bottomMethods.Inside.Add(functionEgg);
            betterMethods.Inside.Add(functionBetter);
            betterMethods.Inside.Add(functionBetterGood);
            bottomClass.Inside.Add(bottomMethods);
            betterClass.Inside.Add(betterMethods);
            bottomSpace.Inside.Add(bottomClass);
            bottomSpace.Inside.Add(betterClass);
            topSpace.Inside.Add(bottomSpace);

            Space topSpace2 = new Space(expected, "toplevel2", ScopeType.PUBLIC);
            Space bottomSpace2 = new Space(topSpace2 as Space, "bottomlevel", ScopeType.PUBLIC);
            Class bottomClass2 = new Class(bottomSpace2, "bottomclass", ScopeType.PUBLIC);
            MethodArea bottomMethods2 = new MethodArea(bottomClass2, null, ScopeType.PUBLIC);
            Function functionEgg2 = new Function(bottomMethods2, "eggsandwaffles", null,
                new List<string> { "itsjustmassive" });
            bottomMethods2.Inside.Add(functionEgg2);
            bottomClass2.Inside.Add(bottomMethods2);
            bottomSpace2.Inside.Add(bottomClass2);
            topSpace2.Inside.Add(bottomSpace2);

            expected.Inside.Add(topSpace);
            expected.Inside.Add(topSpace2);


            AntlrInputStream antlrStream = new AntlrInputStream(file);
            BubblesLexer lexer = new BubblesLexer(antlrStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            BubblesParser parser = new BubblesParser(tokens);

            IParseTree tree = parser.file();

            BoxParser boxParser = new BoxParser();
            var box = boxParser.Visit(tree);

            string expectedStr = expected.ToString();
            string resultStr = box.ToString();

            Assert.Equal(expected.ToString(), box.ToString());
        }

        [Fact]
        public void FunctionTest()
        {
            string file = @"space topLevel{
    class topClass{
        }
    space bottomLevel{
        class bottomClass{
            public methods{
                func eggsAndWaffles(){
                    itsjustmassive
                }
            }
        }
        function mmtoastyum(){
            iliedtoyou
        }
    }
    function main(){
        iamthebeginningandend
    }
}".ToLower();

            //var expected = new Box("Root", new List<Object>(), BubbleType.FILE, ScopeType.PUBLIC);
            Space topSpace = new Space("toplevel", ScopeType.PUBLIC);
            Class topClass = new Class(topSpace, "topclass", ScopeType.PUBLIC);
            Space bottomSpace = new Space(topSpace as Space, "bottomlevel", ScopeType.PUBLIC);
            Class bottomClass = new Class(bottomSpace, "bottomclass", ScopeType.PUBLIC);
            MethodArea bottomMethods = new MethodArea(bottomClass, null, ScopeType.PUBLIC);

            Function functionEgg = new Function(bottomMethods, "eggsandwaffles", null,
                new List<string> { "itsjustmassive" });
            Function functionToast = new Function(bottomSpace, "mmtoastyum", null,
                new List<string> { "iliedtoyou" });
            Function functionMain = new Function(topSpace, "main", null,
                new List<string> { "iamthebeginningandend" });

            bottomMethods.Inside.Add(functionEgg);
            bottomClass.Inside.Add(bottomMethods);
            bottomSpace.Inside.Add(bottomClass);
            topSpace.Inside.Add(topClass);
            topSpace.Inside.Add(bottomSpace);

            bottomSpace.Inside.Add(functionToast);
            topSpace.Inside.Add(functionMain);

            //expected.Inside.Add(topSpace);


            AntlrInputStream antlrStream = new AntlrInputStream(file);
            BubblesLexer lexer = new BubblesLexer(antlrStream);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            BubblesParser parser = new BubblesParser(tokens);

            IParseTree tree = parser.file();

            BoxParser boxParser = new BoxParser();
            var box = boxParser.Visit(tree);

            var expectedStr = topSpace.AsDictionary().Keys;
            var resultStr = box.AsDictionary().Keys;

            var test = box.AsDictionary().Keys;

            Assert.Equal(expectedStr,resultStr );
        }
    }
} 

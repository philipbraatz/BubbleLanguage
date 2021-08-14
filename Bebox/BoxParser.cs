using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using static Doorfail.Bebox.Box;
using static Doorfail.Bebox.BubblesParser;

namespace Doorfail.Bebox
{
    public class BoxParser : BubblesBaseVisitor<Box>
    {
        private static readonly string[] Space = new string[] { "space", "namespace", "sp" };
        private static readonly string[] Class = new string[] { "class", "cl" };
        private static readonly string[] Function = new string[] { "function", "func", "fn" };
        private static readonly string[] Interface = new string[] { "interface", "it" };
        private static readonly string[] Public = new string[] { "public", "pub" };
        private static readonly string[] Private = new string[] { "private", "pvt" };
        private static readonly string[] Internal = new string[] { "Internal", "inl" };
        private static readonly string[] Static = new string[] { "static", "stc" };
        private static readonly string[] Async = new string[] { "asyncronous", "async", "ac" };
        private static readonly string[] Using = new string[] { "using", "use" };
        private static readonly string[] From = new string[] { "From" };

        private static VaribleParser _VaribleParser = new VaribleParser();

        private List<Box> ParentChain = new List<Box>();

        private List<String> Errors = new List<String>();//Semantic

        public static Type GetEnum<Type>(ParserRuleContext context)
        {
            if (context == null)
                return default;
            string text = context.GetText();

            try
            {
                return (Type)Enum.Parse(typeof(Type), text.ToUpper());
            }
            catch
            {
                switch (typeof(Type).Name)
                {
                    case "ScopeType":
                        if (Public.Contains(text))
                            return (Type)(object)ScopeType.PUBLIC;
                        if (Private.Contains(text))
                            return (Type)(object)ScopeType.PRIVATE;
                        if (Internal.Contains(text))
                            return (Type)(object)ScopeType.PROTECTED;
                        break;
                    case "BubbleType":
                        if ("class" == text)
                            return (Type)(object)BubbleType.CLASS;
                        if ("code" == text)
                            return (Type)(object)BubbleType.FILE;
                        if (Function.Contains(text))
                            return (Type)(object)BubbleType.FUNCTION;
                        if (Internal.Contains(text))
                            return (Type)(object)BubbleType.INTERFACE;
                        break;
                    default:
                        break;
                }

                Console.WriteLine($"Could not parse \"{text}\" to {typeof(Type).Name}");
                return default;
            }
        }

        private ErrorBox ValidateContexts(ParserRuleContext context,List<Type> validTypes)
        {
            ErrorBox errors = new ErrorBox(ParentChain.Any()? ParentChain.Last() : null,null);
            errors.Inside.RemoveAt(0);

            if (context.exception != null)
                errors.Inside.Add( new Exception("'"+context.exception.OffendingToken.Text+"'"+ " Line "+
                    context.exception.OffendingToken.Line+" Col " + context.exception.OffendingToken.Column, context.exception));

            var checkForTypes = new List<Type>(validTypes)
            {
                typeof(TerminalNodeImpl),
                typeof(ErrorNodeImpl)
            };

            List<IParseTree> syntaxErrorItem = context.children.Where(c =>
                !checkForTypes.Contains(c.GetType())
            ).SkipLast(1).ToList();

            List<ParserRuleContext> errorNodes = syntaxErrorItem.Where(er => er.GetType() != typeof(ErrorNodeImpl)).Select(s => (ParserRuleContext)s).ToList();
            foreach (ParserRuleContext item in errorNodes)
                foreach (TerminalNodeImpl child in item.children)
                    errors.Inside.Add(new Exception(item.GetType().Name+" invalid token '" +item.GetText()+"'" +
                        " Line " + child.Symbol.Line +
                        " Col " + child.Symbol.Column+"\n\tExpected "+string.Join(" or ",validTypes.Select(s=>s.Name))
                    ));

            return errors;
        }

        public override Box VisitFile([NotNull] BubblesParser.FileContext context)
        {
            ErrorBox errors = ValidateContexts(context,new List<Type> { typeof(Space_declerationContext) });
            if (errors.Inside.Any())
                return errors;

            //Create Top level Root Box
            Box b = new Box(Box.Root);
            ParentChain.Add(b);
            //Put everything inside it
            //b.Inside = new List<object>(context.space_decleration().Select(s => VisitSpace_decleration(s)));
            foreach (var space in context.space_decleration())
            {
                b.Inside.Add(VisitSpace_decleration(space));
            }
            ParentChain.Remove(b);

            if (b.Inside.Count == 0 &&
                (context.ChildCount > 1 || context.ChildCount == 1 &&
                context.GetChild(0).GetText() != "<EOF>"))
            {
                //int line = context.GetChild(0).
                throw new Exception("File can only hold spaces: At '"+ context.GetChild(0).GetText() + "' Line "+ context.start.Line+1 +", Col "+ context.start.Column);
            }
            

            return b;
        }

        public override Box VisitSpace_decleration([NotNull] BubblesParser.Space_declerationContext context)
        {
            ErrorBox errors = ValidateContexts(context, new List<Type> {
                typeof(Space_declerationContext),
                typeof(Function_declerationContext),
                typeof(Interface_declerationContext),
                typeof(Class_declerationContext) ,
                typeof(ImportsContext)
            });
            if (errors.Inside.Any())
                return errors;


            bool isSpace = context.space_decleration().Length > 0;
            bool isClass = context.class_decleration().Length > 0;
            bool isInterface = context.interface_decleration().Length > 0;
            bool isFunction = context.function_decleration().Length > 0;

            Box result = new Box(
                ParentChain.Last(),
                context.NAME().GetText(),
                null,
                BubbleType.SPACE,
                BoxParser.GetEnum<ScopeType>(context.scope_type()));
            ParentChain.Add( result);

            if (isInterface)
                result.Inside.AddRange(new List<object>(context.interface_decleration().Select(c =>
                       VisitInterface_decleration(c)
                    ).ToList()));

            if (isSpace)
                result.Inside.AddRange( new List<object>(context.space_decleration().Select(c =>
                        VisitSpace_decleration(c)
                    ).ToList()));

            if (isClass)
                result.Inside.AddRange( new List<object>(context.class_decleration().Select(c =>
                       VisitClass_decleration(c)
                    ).ToList()));

            if (isFunction)
                result.Inside.AddRange(new List<object>(context.function_decleration().Select(c=>
                        VisitFunction_decleration(c)
                    ).ToList()));

            ParentChain.Remove(result);
            return result;
        }

        public override Box VisitClass_decleration([NotNull] BubblesParser.Class_declerationContext context)
        {
            ErrorBox errors = ValidateContexts(context, new List<Type> {
                typeof(Property_bubbleContext),
                typeof(Methods_bubbleContext),
                typeof(Constructor_bubbleContext),
                typeof(Inherit_classContext)});
            if (errors.Inside.Any())
                return errors;

            bool isProperty = context.property_bubble().Length > 0;
            bool isMethod = context.methods_bubble().Length > 0;
            bool isConstructor = context.constructor_bubble().Length > 0;
            bool isInherited = context.inherit_class() != null;

            Class result = new Class(new Space(ParentChain.Last()),
                context.NAME().GetText(), Box.GetScope(context.scope_type()));
            ParentChain.Add(result);

            if (isInherited)
            {
                //result.
            }
            if (isProperty)
            {
                result.Inside.AddRange( new List<object>(context.property_bubble().Select(c =>
                       VisitProperty_bubble(c)
                    ).ToList()));
            }
            if (isMethod)
            {
                result.Inside.AddRange(new List<object>(context.methods_bubble().Select(c =>
                        VisitMethods_bubble(c)
                    ).ToList()));
            }
            if (isConstructor)
            {
                result.Inside.AddRange(new List<object>(context.constructor_bubble().Select(c =>
                       VisitConstructor_bubble(c)
                    ).ToList()));
            }

            ParentChain.Remove(result);
            return result;
        }

        public override Box VisitInterface_decleration([NotNull] BubblesParser.Interface_declerationContext context)
        {
            ErrorBox errors = ValidateContexts(context, new List<Type> {
                typeof(Property_bubbleContext),
                typeof(Methods_bubbleContext),
                typeof(Constructor_bubbleContext),
                typeof(ImportsContext)});
            if (errors.Inside.Any())
                return errors;


            bool isProperty = context.property_bubble().Length > 0;
            bool isMethod = context.methods_bubble().Length > 0;
            bool isConstructor = context.constructor_bubble().Length > 0;

            Interface result = new Interface(new Space(ParentChain.Last()), context.NAME().GetText(), Box.GetScope(context.scope_type()));
            ParentChain.Add(result);

            if (isProperty)
            {
                result.Inside.AddRange(new List<object>(context.property_bubble().Select(c =>
                       VisitProperty_bubble(c)
                    ).ToList()));
            }
            if (isMethod)
            {
                result.Inside.AddRange(new List<object>(context.methods_bubble().Select(c =>
                        VisitMethods_bubble(c)
                    ).ToList()));
            }
            if (isConstructor)
            {
                result.Inside.AddRange(new List<object>(context.constructor_bubble().Select(c =>
                       VisitConstructor_bubble(c)
                    ).ToList()));
            }

            ParentChain.Remove(result);
            return result;
        }

        public override Box VisitFunction_decleration([NotNull] BubblesParser.Function_declerationContext context)
        {
            ErrorBox errors = ValidateContexts(context, new List<Type> {
                typeof(Code_lineContext),typeof(Return_valuesContext),typeof(StructureContext),typeof(ParametersContext)});
            if (errors.Inside.Any())
                return errors;

            List<string> returns = null;
            List<string> insides = null;
            List<Varible> parameters = null;

            //if (context.return_values()?.ChildCount > 0)
            //    returns = context.return_values().children.Select(p => p?.GetText()).ToList();
            if (context.return_values()?.NAME()?.Length > 0)
                returns = context.return_values().NAME()?.Select(p => p?.GetText()).ToList() ?? new List<string>();
            if (context.code_line()?.Length > 0)
                insides = context.code_line()?.Select(n => n?.GetText()).ToList() ?? new List<string>();

            Function result =new Function(ParentChain.Last(), context.NAME()?.GetText() ?? "Annonamas",null,returns,insides, ParentChain.Last().Scope);
            ParentChain.Add(result);
            _VaribleParser.Parent = result;

            if (context.parameters()?.param()?.Length > 0)
                result.Parameters = context.parameters().param().Select(s => _VaribleParser.VisitParam(s)).ToList() ?? new List<Varible>();

            ParentChain.Remove(result);
            return result;
        }

        public override Box VisitProperty_bubble([NotNull] BubblesParser.Property_bubbleContext context)
        {
            //ValidateContexts(context, new List<Type> {
            //    typeof(idkyet)});

            return new PropertyArea(ParentChain.Last(), context.NAME()?.GetText() ??"", Box.GetScope(context.scope_type()));
        }

        public override Box VisitMethods_bubble([NotNull] BubblesParser.Methods_bubbleContext context)
        {
            //ValidateContexts(context, new List<Type> {
            //    typeof(idkyet)});

            return new MethodArea(ParentChain.Last(), context.NAME()?.GetText() ?? "",
                context.function_decleration().Select(f => VisitFunction_decleration(f)).ToList(), Box.GetScope(context.scope_type()));
        }

        public override Box VisitConstructor_bubble([NotNull] BubblesParser.Constructor_bubbleContext context)
        {
            //ValidateContexts(context, new List<Type> {
            //    typeof(idkyet)});

            return new ConstructorArea((Class)ParentChain.Last(), context.NAME()?.GetText() ?? "constructor_"+ParentChain.Count,
                context.constructor_decleration().Select(f => VisitConstructor_decleration(f)).ToList());
        }

        public override Box VisitConstructor_decleration([NotNull] BubblesParser.Constructor_declerationContext context)
        {
            //ValidateContexts(context, new List<Type> {
            //    typeof(idkyet)});

            Function result = new Function(ParentChain.Last(), "constructor_"+ParentChain.Last().Scope.ToString(),
                context.param().Select(s => _VaribleParser.VisitParam(s)).ToList(),new List<string>(), ParentChain.Last().Scope);
            ParentChain.Add(result);
            if (context.param()?.Length > 0)
                result.Return.AddRange(context.param()?.Select(p => p.GetText()) ?? new List<string>());
            if (context.code_line()?.Length > 0)
                result.Inside.AddRange(context.code_line()?.Select(n => n.GetText()) ?? new List<string>());
            ParentChain.Remove(result);
            return result;
        }
    }
}

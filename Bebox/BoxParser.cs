using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bebox
{



    class BoxParser : BubblesBaseVisitor<Box>
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

        private Box parent = null;

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
                            return (Type)(object)ScopeType.INTERNAL;
                        break;
                    case "BubbleType":
                        if ("class" == text)
                            return (Type)(object)BubbleType.CLASS;
                        if ("code" == text)
                            return (Type)(object)BubbleType.CODE;
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

        public override Box VisitFile([NotNull] BubblesParser.FileContext context)
        {
            List<object> inside = new List<object>(context.space_decleration().Select(s => VisitSpace_decleration(s)));
            Box b = new Box("Root", inside, BubbleType.CODE);
            return b;
        }

        public override Box VisitSpace_decleration([NotNull] BubblesParser.Space_declerationContext context)
        {
            bool isSpace = context.space_decleration().Length > 0;
            bool isClass = context.class_decleration().Length > 0;
            bool isInterface = context.interface_decleration().Length > 0;

            Box result = result = new Box(
                    context.NAME().GetText(),
                    null,
                    isSpace?BubbleType.SPACE:isClass?BubbleType.CLASS:isInterface?BubbleType.INTERFACE:BubbleType.CODE,
                    BoxParser.GetEnum<ScopeType>(context.scope_type())); ;
            parent = result;

            if (isInterface)
            {
                result.Inside = new List<object>( context.interface_decleration().Select(c =>
                        VisitInterface_decleration(c)
                    ).ToList());
            }
            else if (isSpace)
            {
                result.Inside = new List<object>(context.space_decleration().Select(c =>
                        VisitSpace_decleration(c)
                    ).ToList());
            }
            else if (isClass)
            {
                result.Inside = new List<object>(context.class_decleration().Select(c =>
                       VisitClass_decleration(c)
                    ).ToList());
            }


            return result;
        }

        public override Box VisitClass_decleration([NotNull] BubblesParser.Class_declerationContext context)
        {
            return base.VisitClass_decleration(context);
        }

        public override Box VisitInterface_decleration([NotNull] BubblesParser.Interface_declerationContext context)
        {
            return base.VisitInterface_decleration(context);
        }

        public override Box VisitFunction([NotNull] BubblesParser.FunctionContext context)
        {
            return new Function(parent, context.NAME().GetText(), null, parent.Scope);
        }


    }
}

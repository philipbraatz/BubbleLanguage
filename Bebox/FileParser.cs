using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;

namespace Doorfail.Bebox
{
    class FileParser :BubblesBaseVisitor<string>
    {
        int Depth = 0;

        public override string VisitFile([NotNull] BubblesParser.FileContext context)
        {
            return string.Join("\n\n",context.space_decleration().Select(s=> VisitSpace_decleration(s)));
        }

        public override string VisitSpace_decleration(BubblesParser.Space_declerationContext context)
        {
            int startingDepth = Depth;

            string import = String.Empty;
            string code = " EMPTY ";
            bool isUsing = false;
            bool isFrom = false;

            bool isSpace = context.space_decleration().Any();
            bool isClass = context.class_decleration().Any();
            bool isInterface = context.interface_decleration().Any();

            if (context.imports() != null)
            {
                isUsing = context.imports().USING() != null;
                //isFrom = context.imports(). != null;

                string importNames = string.Join(", ",context.imports().import_name().Select(v => VisitImport_name(v)));
                import = isUsing ? "Libraries: [" + importNames + "]" :
                                "";//(isFrom ? "Inherits: [" + importNames + "]" : "");
            }

            //Depth++;
            //string padding = new string('\t', Depth-1);
            //if (isSpace)
            //{
            //    
            //    code = string.Join("\n", context.space_decleration().Select(f=> VisitCode_lines(f)));
            //}
            //else if (isBubble)
            //{
            //    code = padding+ string.Join("\n\t"+ padding, context.bubble_decleration().Select(x => 
            //        VisitBubble_decleration(x)));
            //}
            //
            //if (isFunction)
            //{
            //    if (code == " EMPTY ")
            //        code = "";
            //    else
            //        code += "\n";
            //
            //    code += padding + string.Join("\n\t"+padding , context.function().Select(x=> VisitFunction(x)));
            //}
            //Depth--;

            //string result =$"[{startingDepth}]\tScope: " +VisitScope_type(context.scope_type())+", "+
            //    VisitBubble_type(context.bubble_type()) + ", "+
            //    "\""+context.NAME().GetText() + "\":\n\t" +
            //    import+"\n\t"+padding+ code+"\n";
            //
            return "";
        }

        public string VisitCode_lines([NotNull] BubblesParser.Code_lineContext[] context)
        {
            string padding = new string('\t', Depth - 1);
            return padding + string.Join("\n" + padding, context.Select(x => VisitCode_line(x)));
        }

        public override string VisitImport_name([NotNull] BubblesParser.Import_nameContext context)
        {
            return context?.GetText() ?? "NONE";
        }

        public override string VisitImports(BubblesParser.ImportsContext context)
        {
            return context.GetText().ToUpper();
        }

        public override string VisitBubble_type(BubblesParser.Bubble_typeContext context)
        {
            return context?.GetText().ToUpper() ?? "Code";
        }

        public override string VisitCode_line([NotNull] BubblesParser.Code_lineContext context)
        {
            if (context == null)
                return string.Empty;

            int StartingDepth = Depth;
            return $"[{StartingDepth}]\t"+ context.GetText();
        }

        public override string VisitScope_type(BubblesParser.Scope_typeContext context)
        {
            return context?.GetText() ?? "Public";
        }

        public override string VisitFunction_decleration(BubblesParser.Function_declerationContext context)
        {
            string padding = new string('\t', Depth);
            Depth++;
            string result =$"[{Depth-1}]"+padding+"FUNC: '"+context.NAME().GetText()+"' "+context.parameters().param().Select(p=>VisitParam(p))+"\n"+
                VisitCode_lines(context.code_line())+context.code_line().Select(c=> VisitCode_line(c));
            Depth--;
            return result;
        }
        public override string VisitParam(BubblesParser.ParamContext context)
        {
            return context.GetText();
        }
    }
}

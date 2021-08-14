using Antlr4.Runtime.Misc;
using static Doorfail.Bebox.BubblesParser;

namespace Doorfail.Bebox
{
    public class VaribleParser : BubblesBaseVisitor<Varible>
    {
        public Box Parent;

        public override Varible VisitParam([NotNull] BubblesParser.ParamContext context)
        {
            string type = context.ChildCount >= 1 ? context.children[0].GetText() : "";
            string name = context.ChildCount >= 2 ? context.children[1].GetText() : "";
            string value = context.ChildCount >= 3 ? context.children[2].GetText() : "";

            return new Varible
            (
                Parent,
                type,//TODO turn into Box typing
                name,
                value
            );
        }
    }
}

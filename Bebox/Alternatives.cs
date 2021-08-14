using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime;
using static Doorfail.Bebox.BubblesParser;

namespace Doorfail.Bebox
{
    public static class Alternative
    {

        public static ScopeType GetScope(Scope_typeContext context)
        {
            if (context.PUBLIC() != null)
                return ScopeType.PUBLIC;
            if (context.PRIVATE() != null)
                return ScopeType.PRIVATE;
            else if (context.PROTECTED() != null)
                return ScopeType.PROTECTED;
            else
                return ScopeType.PUBLIC;
        }

        public static BubbleType GetType(Bubble_typeContext context)
        {
            if (context.SPACE() != null)
                return BubbleType.SPACE;
            if (context.CLASS() != null)
                return BubbleType.CLASS;
            else if (context.INTERFACE() != null)
                return BubbleType.INTERFACE;
            else if (context.INTERFACE() != null)
                return BubbleType.INTERFACE;
            else if (context.FUNCTION() != null)
                return BubbleType.FUNCTION;
            else
                return BubbleType.FILE;
        }
    }
}

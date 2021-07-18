using System;
using System.Collections.Generic;
using System.Text;

namespace Bebox
{
    public class PropertyArea : Box
    {
        public PropertyArea(Box b) : base(b, new List<BubbleType> { BubbleType.CLASS, BubbleType.INTERFACE }) { }

        public PropertyArea(Class @class, string name, ScopeType scope = 0)
            : base(@class, name, new List<object>(), BubbleType.PROPERTYAREA, scope) { }

        public PropertyArea(Class @class, string name, List<object> inside, ScopeType scope = 0)
            : base(@class, name, inside, BubbleType.PROPERTYAREA, scope) { }

        public PropertyArea(Box @class, string name, ScopeType scope = 0)
            : base(@class, name, new List<object>(), BubbleType.PROPERTYAREA, scope) { }

        public PropertyArea(Box @class, string name, List<object> inside, ScopeType scope = 0)
            : base(@class, name, inside, BubbleType.PROPERTYAREA, scope) { }
    }

    public class MethodArea : Box
    {
        public MethodArea(Box b) : base(b, new List<BubbleType> { BubbleType.CLASS, BubbleType.INTERFACE }) { }

        public MethodArea(Class @class, string name, ScopeType scope = 0)
            : base(@class, name, new List<object>(), BubbleType.METHODAREA, scope) { }

        public MethodArea(Class @class, string name, List<Function> inside, ScopeType scope = 0)
            : base(@class, name, new List<object>(inside), BubbleType.METHODAREA, scope) { }

        public MethodArea(Box @class, string name, ScopeType scope = 0)
            : base(@class, name, new List<object>(), BubbleType.METHODAREA, scope) { }

        public MethodArea(Box @class, string name, List<Function> inside, ScopeType scope = 0)
            : base(@class, name, new List<object>(inside), BubbleType.METHODAREA, scope) { }
    }

    public class ConstructorArea : Box
    {
        public ConstructorArea(Box b) : base(b, new List<BubbleType> { BubbleType.CLASS, BubbleType.INTERFACE }) { }

        public ConstructorArea(Class @class, string name, ScopeType scope = 0)
            : base(@class, name, new List<object>(), BubbleType.CONSTRUCTORAREA, scope) { }

        public ConstructorArea(Class @class, string name, List<Function> inside, ScopeType scope = 0)
            : base(@class, name, new List<object>(inside), BubbleType.CONSTRUCTORAREA, scope) { }

        public ConstructorArea(Box @class, string name, ScopeType scope = 0)
            : base(@class, name, new List<object>(), BubbleType.CONSTRUCTORAREA, scope) { }

        public ConstructorArea(Box @class, string name, List<Function> inside, ScopeType scope = 0)
            : base(@class, name, new List<object>(inside), BubbleType.CONSTRUCTORAREA, scope) { }
    }

    public class Function : Box
    {
        List<Box> Return;

        public Function(Box b) : base(b, new List<BubbleType> { BubbleType.CLASS, BubbleType.INTERFACE }) { }

        public Function(
        MethodArea area, string name,                                              ScopeType scope = 0)
            : base(area,        name, new List<object>(),       BubbleType.METHODAREA,       scope) { }

        public Function(
        MethodArea area, string name,  List<string> inside,                       ScopeType scope = 0)
            : base(area,        name, new List<object>(inside), BubbleType.METHODAREA,        scope) { }

        public Function(
               Box area, string name,                                               ScopeType scope = 0)
            : base(area,        name, new List<object>(),       BubbleType.METHODAREA,        scope) { }

        public Function(
               Box area, string name,   List<string> inside,                      ScopeType scope = 0)
            : base(area,        name, new List<object>(inside), BubbleType.METHODAREA,        scope) { }

        public Function(
        MethodArea area, string name,  List<Box> returns,                           ScopeType scope = 0)
            : base(area,        name, new List<object>(),       BubbleType.METHODAREA,        scope)
                                     => Return = returns;

        public Function(
        MethodArea area, string name, List<Box> returns, List<string> inside,                ScopeType scope = 0)
            : base(area,        name,                   new List<object>(inside), BubbleType.METHODAREA, scope)
                                    => Return = returns;

        public Function(
               Box area, string name, List<Box> returns,                                       ScopeType scope = 0)
            : base(area,        name, new List<object>(),                         BubbleType.METHODAREA, scope)
                                    => Return = returns;                         

        public Function(
               Box area, string name, List<Box> returns, List<string> inside,                ScopeType scope = 0)
            : base(area,        name,                  new List<object>(inside),  BubbleType.METHODAREA, scope)
                                    => Return = returns;
    }
}

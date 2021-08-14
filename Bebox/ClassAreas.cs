using System;
using System.Collections.Generic;
using System.Linq;

namespace Doorfail.Bebox
{
    public class PropertyArea : Box
    {
        public PropertyArea(Box b) : base(b, new List<BubbleType> { BubbleType.CLASS, BubbleType.INTERFACE }) { }

        public PropertyArea(Class @class, string name, ScopeType scope = 0)
            : base(@class, string.IsNullOrWhiteSpace(name) ? scope.ToString() + "_property" : name, new List<object>(), BubbleType.PROPERTYAREA, scope) { }

        public PropertyArea(Class @class, string name, List<object> inside, ScopeType scope = 0)
            : base(@class, string.IsNullOrWhiteSpace(name) ? scope.ToString() + "_property" : name, inside, BubbleType.PROPERTYAREA, scope) { }

        public PropertyArea(Box @class, string name, ScopeType scope = 0)
            : base(@class, string.IsNullOrWhiteSpace(name) ? scope.ToString() + "_property" : name, new List<object>(), BubbleType.PROPERTYAREA, scope) { }

        public PropertyArea(Box @class, string name, List<object> inside, ScopeType scope = 0)
            : base(@class, string.IsNullOrWhiteSpace(name) ? scope.ToString() + "_property" : name, inside, BubbleType.PROPERTYAREA, scope) { }

        public override string ToString()
        {
            String namesection = Name == "" ? "" : (" " + Name);

            string prop = $"( {Scope.ToString()} ):";
            string name = $"{Bubble.ToString()}{namesection}\n";

            List<string> insides = new List<string>();
            if ((Inside?.Count ?? -1) > 0)
                foreach (var item in Inside)
                {
                    if (item != null)
                        insides.Add(item.ToString().Replace("\n", "\n\t"));
                }
            else
                insides.Add("EMPTY");

            return prop + name + "\t" + string.Join("\n\t", insides);
        }
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

        public MethodArea(Class @class, string name, List<Box> inside, ScopeType scope = 0)
    : base(@class, name, new List<object>(inside), BubbleType.METHODAREA, scope) { }

        public MethodArea(Box @class, string name, List<Box> inside, ScopeType scope = 0)
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

        public ConstructorArea(Class @class, string name, List<Box> inside, ScopeType scope = 0)
: base(@class, name, new List<object>(inside), BubbleType.CONSTRUCTORAREA, scope) { }

        public ConstructorArea(Box @class, string name, List<Box> inside, ScopeType scope = 0)
    : base(@class, name, new List<object>(inside), BubbleType.CONSTRUCTORAREA, scope) { }
    }

    public class Function : Box
    {
        public override string ToString()
        {
            string[] result = base.ToString().Split('\n');
            return result[0] + "\t\nParameters:" +
                String.Join(", ", Parameters.Select(s=> s.ToString()))+
                "\n"+string.Join("\t\n",result.Skip(1)) + "\t\nReturns " + 
                string.Join(", ", Return.Select(s=>s.ToString()));
        }

        public List<Object> Return = new List<object>();
        public List<Varible> Parameters = new List<Varible>();


        //TODO Redo constructor params to make sense
        public Function(Box b) : base(b, new List<BubbleType> { BubbleType.CLASS, BubbleType.INTERFACE })
        { }

        public Function(
        MethodArea area, string name, List<Varible> parmeters, ScopeType scope = 0)
            : base(area, name, new List<object>(), BubbleType.FUNCTION, scope)
        { Parameters = parmeters?? new List<Varible>(); }

        public Function(
        MethodArea area, string name, List<Varible> parmeters, List<string> inside, ScopeType scope = 0)
            : base(area, name, new List<object>(inside), BubbleType.FUNCTION, scope)
        { Parameters = parmeters ?? new List<Varible>(); }

        public Function(
               Box area, string name, List<Varible> parmeters, ScopeType scope = 0)
            : base(area, name, new List<object>(), BubbleType.FUNCTION, scope)
        { Parameters = parmeters ?? new List<Varible>(); }

        public Function(
               Box area, string name, List<Varible> parmeters, List<string> inside, ScopeType scope = 0)
            : base(area, name, new List<object>(inside), BubbleType.FUNCTION, scope) {
            Parameters = parmeters ?? new List<Varible>(); 
        }

        public Function(
        MethodArea area, string name, List<Varible> parmeters, List<Box> returns, ScopeType scope = 0)
            : base(area, name, new List<object>(), BubbleType.FUNCTION, scope)
        {
            Parameters = parmeters ?? new List<Varible>();
            Return = new List<object>(returns);
        }

        public Function(
        MethodArea area, string name, List<Varible> parmeters, List<string> returns, List<string> inside, ScopeType scope = 0)
            : base(area, name, new List<object>(inside), BubbleType.FUNCTION, scope)
        {
            Parameters = parmeters ?? new List<Varible>();
            Return = new List<object>(returns);
        }

        public Function(
               Box area, string name, List<Varible> parmeters, List<Box> returns, ScopeType scope = 0)
            : base(area, name, new List<object>(), BubbleType.FUNCTION, scope)
        {
            Parameters = parmeters ?? new List<Varible>();
            Return = new List<object>(returns);
        }

        public Function(
               Box area, string name, List<Varible> parmeters, List<string> returns, List<string> inside, ScopeType scope = 0)
            : base(area, name, new List<object>(inside ?? new List<string>()), BubbleType.FUNCTION, scope)
        {
            Parameters = parmeters ?? new List<Varible>();
            Return = new List<object>(returns ?? new List<string>());
        }

        internal string Run()
        {
            return Inside.ToString();
        }
    }
}

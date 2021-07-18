using System;
using System.Collections.Generic;
using System.Text;

namespace Bebox
{
    public enum ScopeType
    {
        PUBLIC = 0,
        PRIVATE,
        INTERNAL
    }

    public enum BubbleType
    {
        FILE,
        SPACE,
        CLASS,
        INTERFACE,
        FUNCTION,
        PROPERTYAREA,
        METHODAREA,
        CONSTRUCTORAREA
    }

    public struct Attributes
    {
        bool Static;
        bool Event;
        bool Async;

        public override string ToString()
        => string.Join(" ", (Static ? "Static" : "Instanced"),
               (Event ? "Self-Triggered" : "Manual"),
               (Async ? "Multi-Threaded" : "Single-Threaded"));
    }

    public class Box
    {
        public Box Parent;

        public readonly ScopeType Scope;
        public readonly BubbleType Bubble;
        public readonly string Name;
        public Dictionary<string, Box> Aliases;
        public List<object> Inside;

        public Attributes Attributes;

        private int Depth
        {
            get
            {
                if (Parent == null)
                    return 0;
                return 1 + Parent.Depth;
            }
        }

        public string FullName { get => Parent?.FullName ?? String.Empty + Name; }
        public override string ToString()
        {
            string prop = $"( {Scope.ToString()} {Bubble.ToString()} ): ";
            string name = $"{FullName} Attributes: [{Attributes.ToString()}]\n";

            List<string> insides = new List<string>();
            if (Inside != null)
                foreach (var item in Inside)
                {
                    if (item != null)
                        insides.Add(item.ToString().Replace("\n", "\n\t"));
                }
            else
                insides.Add("EMPTY");

            return prop + name + "\t" + string.Join("\n\t", insides);
        }


        public Box(Box b, List<BubbleType> validTypes) : this(b.Parent, b.Name, b.Inside, b.Bubble, b.Scope)
        {
            if (!validTypes.Contains(Bubble))
                throw new InvalidCastException("Cannot Convert from " + Bubble.ToString() + " to " + this.GetType().Name);
        }

        public Box(string name, List<object> inside, BubbleType type, ScopeType scope = 0)
        {
            Name = name;
            Inside = inside;
            Bubble = type;
            Scope = scope;
        }

        public Box(Box parent, string name, List<object> inside, BubbleType type, ScopeType scope = 0)
        {
            Parent = parent;
            Name = name;
            Inside = inside;
            Bubble = type;
            Scope = scope;
        }


        public static ScopeType GetScope(Scope_typeContext context)
        {
            return context.PUBLIC() != null ? ScopeType.PUBLIC :
                context.PRIVATE() != null ? ScopeType.PRIVATE :
                context.INTERNAL() != null ? ScopeType.INTERNAL : ScopeType.INTERNAL;
        }
    }
}

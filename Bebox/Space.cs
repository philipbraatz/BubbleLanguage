using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bebox
{
    public enum ScopeType
    {
        PUBLIC =0,
        PRIVATE,
        INTERNAL
    }

    public enum BubbleType
    {
        CODE,
        SPACE,
        CLASS,
        INTERFACE,
        FUNCTION
    }

    public struct Attributes
    {
        bool Static;
        bool Event;
        bool Async;

        public override string ToString()
        => string.Join(" ", (Static?"Static":"Instanced"),
               (Event?"Self-Triggered":"Manual"),
               (Async?"Multi-Threaded":"Single-Threaded"));
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

        private int Depth { get {
                if (Parent == null)
                    return 0;
                return 1 + Parent.Depth;
            }}

        public string FullName { get => Parent?.FullName?? String.Empty + Name; }
        public override string ToString()
        {
            string prop = $"( {Scope.ToString()} {Bubble.ToString()} ): ";
            string name = $"{FullName} Attributes: [{Attributes.ToString()}]\n";

            List<string> insides = new List<string>();
            if (Inside != null)
                foreach (var item in Inside)
                {
                    if(item !=null)
                        insides.Add(item.ToString().Replace("\n", "\n\t"));
                }
            else
                insides.Add("EMPTY");

            return prop+name+ "\t" + string.Join("\n\t", insides);
        }

        public Box(string name, List<object> inside, BubbleType type, ScopeType scope = 0)
        {
            Name = name;
            Inside = inside;
            Bubble = type; 
            Scope = scope;
        }

        public Box(Box parent,string name, List<object> inside, BubbleType type, ScopeType scope = 0)
        {
            Parent = parent;
            Name = name;
            Inside = inside;
            Bubble = type;
            Scope = scope;
        }

    }
    public class Space : Box
    {

        private Space(string name) 
               :base(name, null, BubbleType.SPACE, ScopeType.PUBLIC){
        }

        public Space(Space space,string name, ScopeType scope = 0)
               : base(space, name,new List<object>(new List<Box>()), BubbleType.SPACE, scope)
        {}

        public Space CreateRootSpace(string name) => new Space(name);

    }

    public class Class : Box
    {
        public Class(Space space,string name, ScopeType scope = 0)
               : base(space, name, (List<object>)(object)new List<Box>(), BubbleType.CLASS, scope)
        {
        }
    }

    public class Interface : Box
    {
        public Interface(Space space, string name, ScopeType scope = 0)
               : base(space, name, new List<object>(new List<Box>()), BubbleType.INTERFACE, scope)
        {
        }
    }

    public class Function : Box
    {
        List<Box> Return;

        public Function(Box parent, string name, List<Box> returnType, ScopeType scope = 0)
               : base(parent ,name, new List<object>(new List<string>()), BubbleType.FUNCTION, scope)
        {
            Return = returnType;
        }
    }

}

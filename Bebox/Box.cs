using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Doorfail.Bebox
{
    public enum ScopeType
    {
        PUBLIC = 0,
        PRIVATE,
        PROTECTED,
        PARAMETER,
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
        CONSTRUCTORAREA,
        ERROR
    }

    public struct Attributes
    {
        bool Static;
        bool Event;
        bool Async;

        public override string ToString()
        => string.Join(" ", (Static ? "Static" : "Instanced"),
               (Event ? "Event-Driven" : "M"),
               (Async ? "Multi-Threaded" : "S"));
    }

    public class Box
    {
        public Box Parent;
        public static Box Root = new Box("root", new List<object>(), BubbleType.FILE, ScopeType.PUBLIC);

        public readonly ScopeType Scope;
        public readonly BubbleType Bubble;
        public readonly string Name;
        public Dictionary<string, Box> Aliases;
        public List<object> Inside = new List<object>();

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

        public List<string> ScopeChain
        {
            get
            {
                if (Parent != null && !string.IsNullOrEmpty(Parent.FullName))
                    return new List<string>(Parent.ScopeChain) { Name };
                else
                    return new List<string> { Name};
            }
        }

        public string FullName
        {
            get
            {
                if (Parent != null && !string.IsNullOrEmpty(Parent.FullName))
                    return Parent.FullName + "." + Name;
                else
                    return Name;
            }
        }
        public override string ToString()
        {
            string prop = $"( {Scope.ToString()} {Bubble.ToString()} ) [{Attributes.ToString()}]: {Name}";

            List<string> insides = new List<string>();
            if ((Inside?.Count ?? -1) > 0)
                foreach (var item in Inside)
                {
                    if (item != null)
                        insides.Add(item.ToString().Replace("\n", "\n\t"));
                }
            else
                insides.Add("EMPTY");

            return prop + "\n\t" + string.Join("\n\t", insides);
        }

        public Box(Box self) : this(self, new List<BubbleType> { BubbleType.FILE }) { }

        public Box(Box b, List<BubbleType> validTypes) : this(b.Parent, b.Name, b.Inside, b.Bubble, b.Scope)
        {
            if (!validTypes.Contains(Bubble))
                throw new InvalidCastException("Cannot Convert from " + Bubble.ToString() + " to " + GetType().Name);
        }

        public Box(string name, List<object> inside, BubbleType type, ScopeType scope = 0)
        {
            Name = name;
            Inside = inside ?? new List<object>();
            Bubble = type;
            Scope = scope;

            Parent = Root;
        }

        public Box(Box parent, string name, List<object> inside, BubbleType type, ScopeType scope = 0)
        {
            Parent = parent;
            Name = name;
            Inside = inside ?? new List<object>();
            Bubble = type;
            Scope = scope;

            //Parent = Root;
        }

        //MOVE to parsing
        public static ScopeType GetScope(BubblesParser.Scope_typeContext context)
        {
            if (context == null) return ScopeType.PUBLIC;
            return context.PUBLIC() != null ? ScopeType.PUBLIC :
                context.PRIVATE() != null ? ScopeType.PRIVATE :
                context.PROTECTED() != null ? ScopeType.PROTECTED : ScopeType.PROTECTED;
        }

        //public List<string> GetAccessableScopes()
        //{
        //    switch (Scope)
        //    {
        //        case ScopeType.PUBLIC:
        //            List<string> scope= this.ScopeChain;
        //            scope.RemoveAt(scope.Count - 1);
        //        case ScopeType.PRIVATE:
        //            return new List<string>(this.ScopeChain) { "*" };
        //            break;
        //        case ScopeType.INTERNAL:
        //            break;
        //        case ScopeType.PARAMETER:
        //            break;
        //    }
        //}
        //
        //public bool HasAccessTo(Box box)
        //{
        //    //access to self
        //    if (box.FullName == this.FullName)
        //        return true;
        //
        //    if(box.Inside.Contains(box))
        //        foreach (Box item in box.Inside)
        //        {
        //            return HasAccessTo(item);
        //        }
        //}

        //public Dictionary<string,Box> AsPublicDictionary()
        //{
        //    Dictionary<string, Box> result = new Dictionary<string, Box>();
        //
        //    if (Inside != null)
        //    {
        //        foreach (object inner in Inside)
        //        {
        //            if (inner is Box b)
        //            {
        //                try
        //                {
        //                    if (!(inner is ErrorBox))
        //                        result.Add(b.FullName, b);
        //                    else
        //                    {
        //                        if ((inner as Box).Scope == ScopeType.PUBLIC)
        //                        {
        //                            result.Add(b.FullName, b);
        //                        }
        //                    }
        //                }
        //                catch
        //                {
        //                    result.Add("DUP_" + b.FullName, b);
        //                }
        //                
        //                try
        //                {
        //                    foreach (var item in b.AsDictionary())
        //                        result.Add(item.Key, item.Value);
        //                }
        //                catch
        //                {
        //                    result.Add("DUP_" + b.FullName, b);
        //                }
        //            }
        //        }
        //    }
        //    Debug.WriteLine(string.Join(", ", result.Keys.ToList()), "Debugging");
        //    return result;
        //}

        public Dictionary<string, Box> AsDictionary()
        {
            Dictionary<string, Box> result = new Dictionary<string, Box>();

            if (Inside != null)
            {
                foreach (object inner in Inside)
                {
                    if (inner is Box b)
                    {
                        try
                        {
                            if (!(inner is ErrorBox))
                                result.Add(b.FullName, b);
                            else
                                result.Add(b.FullName, b);
                        }
                        catch
                        {
                            result.Add("DUP_" + b.FullName+"["+inner.GetType()+"_"+result.Count+"]", b);
                        }

                        //result.Last().Value.Inside = new List<object>();//Don't display insides normally

                        try
                        {
                            foreach (var item in b.AsDictionary())
                                result.Add(item.Key, item.Value);
                        }
                        catch
                        {
                            result.Add("DUP_" + b.FullName, b);
                        }
                    }
                }
            }
            Debug.WriteLine(string.Join(", ", result.Keys.ToList()), "Debugging");
            return result;
        }

        public static Box GetType(string fullName)
            => !string.IsNullOrEmpty(fullName) ?
            (Root.AsDictionary().Where(c => c.Key == fullName).FirstOrDefault().Value ?? 
                throw new Exception("Type of \"" + fullName + "\"" + "does not exist")) :
                throw new ArgumentNullException("fullName");

        //TODO This will have to check using attributes in the future
        //public static Box GetType(string type, Box parent, string startingLocation ="")
        //{
        //    if (startingLocation == string.Empty)
        //        startingLocation = parent.FullName;
        //
        //    if (parent == null)
        //        throw new Exception($"Type \"{type}\" does not exist or is not accesible to {startingLocation}");
        //    if (string.IsNullOrEmpty(type))
        //        throw new ArgumentNullException("type");
        //
        //    //Quickly check before doing the more intense check
        //    if (type == parent.FullName)
        //        return parent;
        //
        //    string[] sections = type.Split('.');
        //    string[] current = parent.FullName.Split('.');
        //
        //    int matchs = 0;
        //    foreach (var piece in current.Zip(sections,(x,y)=>new Tuple<string,string>(x,y)))
        //    {
        //        if (piece.Item1 != piece.Item2)
        //            break;
        //
        //        matchs++;
        //    }
        //
        //    //need to go deeper to find class
        //    if (matchs == current.Count())
        //    {
        //        foreach (Box child in parent.Inside)
        //        {
        //            Box b = GetType(type, child, startingLocation);
        //            if (b != null)
        //                return b;
        //        }
        //    }
        //    //allowed to go higher
        //    else if (parent.FullName.Contains( startingLocation))
        //    {
        //        
        //    }
        //
        //}

        public Function EntryPoint
        {
            get
            {
                if (ScopeType.PUBLIC == Scope && (
                    BubbleType.FILE == Bubble ||
                    BubbleType.SPACE == Bubble &&
                    Inside != null
                    ))
                {
                    foreach (var box in Inside)
                    {
                        if (((Box)box).Bubble == BubbleType.SPACE)
                        {
                            Function entry = ((Box)box).EntryPoint;
                            if (entry != null)
                                return entry;
                        }

                        else if (((Box)box).Name == "main" &&
                                 ((Box)box).Bubble == BubbleType.FUNCTION)
                            return (Function)box;
                    }
                }
                return null;
            }
        }

        public class ErrorBox : Box
        {
            public ErrorBox(Exception e) :
                base(new Box(e.GetType().Name, new List<object> { (object)e }, BubbleType.ERROR))
            { }

            public ErrorBox(Exception e, BubbleType type, ScopeType scope = ScopeType.PUBLIC) :
                base(e.GetType().Name, new List<object> { (object)e }, type, scope)
            { }

            public ErrorBox(Box parent, Exception e, ScopeType scope = ScopeType.PUBLIC) :
                base(parent, e?.GetType().Name ?? null, new List<object> { (object)e }, BubbleType.ERROR, scope)
            { }

            public IEnumerable<Exception> GetExceptions { get => Inside.Select(e => (Exception)e); }

            public override string ToString()
            {
                string name = $"{Name}\n";

                List<string> insides = new List<string>();
                if ((Inside?.Count ?? -1) > 0)
                    foreach (Exception item in Inside)
                    {
                        if (item != null)
                            insides.Add("\t" + item.Message + ": " + item.InnerException);
                    }
                else
                    insides.Add("EMPTY");

                return name + "\t" + string.Join("\n\t", insides);
            }
        }
    }
}

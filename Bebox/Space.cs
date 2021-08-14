using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Doorfail.Bebox
{
    
    public class Space : Box
    {

        private Space(string name) 
               :base(name, null, BubbleType.SPACE, ScopeType.PUBLIC){}

        public Space(Box b) : base(b, new List<BubbleType> { BubbleType.SPACE, BubbleType.FILE }){}

        public Space(Box space, string name, ScopeType scope = 0)
            :
            base(space, name, new List<object>(), BubbleType.SPACE, scope)
        { }

        public Space(Box space, string name, List<object> inside, ScopeType scope = 0)
            : base(space, name, inside, BubbleType.SPACE, scope)
        { }

        public Space(Space space, string name, ScopeType scope = 0)
            : base(space, name, new List<object>(), BubbleType.SPACE, scope)
        { }

        public Space(Space space, string name, List<object> inside, ScopeType scope = 0)
            : base(space, name, inside, BubbleType.SPACE, scope)
        { }

        public Space( string name, ScopeType scope = 0)
            : base( name, new List<object>(), BubbleType.SPACE, scope)
        {}

        public Space(string name, List<object> inside, ScopeType scope = 0)
            : base(name, inside, BubbleType.SPACE, scope)
        { }

        public Space CreateRootSpace(string name) => new Space(name);
    }

    public class Class : Box
    {
        public string BaseClass = "Root";//by default make this 'object'/'box' type/box

        public override string ToString()
        {
            string[] basestring = base.ToString().Split(':',2);
            return basestring[0] + " Inherits '" + BaseClass + "' :" + basestring[1];
        }

            public Class(Box b) : base(b, new List<BubbleType> { BubbleType.SPACE, BubbleType.FILE }) {}

        public Class(Space space,string name, ScopeType scope = 0)
            : base(space, name, new List<Object>(), BubbleType.CLASS, scope)
        => space.GetType();
        

        public Class(Space space, string name, List<object> inside, ScopeType scope = 0)
            : base(space, name, inside, BubbleType.CLASS, scope)
        => space.GetType();
    }

    public class Interface : Box
    {
        public Interface(Box b) : base(b, new List<BubbleType> { BubbleType.SPACE, BubbleType.FILE }) { }

        public Interface(Space space, string name, ScopeType scope = 0)
            : base(space, name, new List<object>(), BubbleType.INTERFACE, scope){}

        public Interface(Space space, string name, List<object> inside, ScopeType scope = 0)
            : base(space, name, inside, BubbleType.INTERFACE, scope){}
    }
}

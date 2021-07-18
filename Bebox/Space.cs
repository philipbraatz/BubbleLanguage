using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Bebox.BubblesParser;

namespace Bebox
{
    
    public class Space : Box
    {

        private Space(string name) 
               :base(name, null, BubbleType.SPACE, ScopeType.PUBLIC){}

        public Space(Box b) : base(b, new List<BubbleType> { BubbleType.SPACE, BubbleType.FILE }){}

        public Space(Space space,string name, ScopeType scope = 0)
            : base(space, name,new List<object>(), BubbleType.SPACE, scope)
        =>space.GetType();

        public Space(Space space, string name,List<object> inside, ScopeType scope = 0)
            : base(space, name, inside, BubbleType.SPACE, scope)
        => space.GetType();

        public Space CreateRootSpace(string name) => new Space(name);
    }

    public class Class : Box
    {

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

using System;
using System.Collections.Generic;
using System.Linq;

namespace Doorfail.Bebox
{
    public enum Operator
    {
        /*equality*/
        EQUAL,/*Normally equality*/
        NOT_EQUAL,
        SAME,/*are these the same instance of object*/
        NOT_SAME,
        LEFT_GREATER,
        RIGHT_GREATER,
        LEFT_EQUAL_GREATER,
        RIGHT_EQUAL_GREATER,
        IS_TYPE,

        /*Assignment*/
        ASSIGN,
        INC_ASSIGN,
        DEC_ASSIGN,
        MULT_ASSIGN,
        DIVIDE_ASSIGN,
        INVERT_ASSIGN,

        /*Logic*/
        NOT,
        AND,
        OR,
        NAND,
        XOR,
        NOR,

        /*Math*/
        INC,
        DEC,

        ADD,
        SUB,
        MULT,
        DIVIDE,
        MOD,
        POWER,

        /*Nullablity*/
        SKIP_IF_NULL,
        IF_NULL,

        INLINE_IF,
        INLINE_ELSE,
    }

    public class Varible
    {
        public Box Parent;
        public string Name;
        public string Type;
        public ScopeType Scope;
        public object Value;

        public string FullName { get => Parent.FullName + "." + Name; }

        public override string ToString()
        {
            return Scope+" "+Type+" "+Name+" = '"+Value.ToString()+"'";
        }

        public Varible(Box parent,string type, string name, object value)
        {
            Parent = parent;
            Type = type;
            Name = name;
            Value = value;
        }

        public Varible(Box parent, string type, string name) : this(parent, type, name,null) {}
    }

    public class Expression
    {
        public List<Expression> ExpressionTree;

        public List<object> Values;
        public List<Operation> Operations;
    }

    public class Operation
    {
        public Operator Op;

        public Operation(Operator op)
        {
            Op = op;
        }

        public bool Single(object var)
        {
            switch (Op)
            {
                case Operator.NOT:
                    return !(bool)var;
                default:
                    throw new NotImplementedException(Op.ToString()+" does not take a single value");
            }
        }

        public void Assignment(out object var, object value)
        {
            switch (Op)
            {
                case Operator.ASSIGN:
                    var = value;
                    break;
                //case Operator.INC_ASSIGN:
                //    break;
                //case Operator.DEC_ASSIGN:
                //    break;
                //case Operator.MULT_ASSIGN:
                //    break;
                //case Operator.DIVIDE_ASSIGN:
                //    break;
                //case Operator.INVERT_ASSIGN:
                //    break;
                default:
                    throw new NotImplementedException(Op.ToString() + " does not take a single value");
            }
        }

        public bool Double(object v1, object v2)
        {
            switch (Op)
            {
                case Operator.EQUAL:
                    return Object.Equals(v1, v2);
                case Operator.NOT_EQUAL:
                    return !Object.Equals(v1, v2);
                case Operator.SAME:
                    return Object.ReferenceEquals(v1, v2);
                case Operator.NOT_SAME:
                    return !Object.ReferenceEquals(v1, v2);
                case Operator.IS_TYPE:
                    return v1.GetType().IsInstanceOfType(v2);
                default:
                    throw new NotImplementedException(Op.ToString()+" does not take 2 values");
            }
        }

        public bool Double(bool v1, bool v2)
        {
            switch (Op)
            {
                case Operator.AND:
                    return v1 && v2;
                case Operator.OR:
                    return v1 || v2;
                case Operator.NAND:
                    return false;//check all
                case Operator.XOR:
                    return false;
                case Operator.NOR:
                    return false;
                case Operator.INC:
                    break;
                case Operator.DEC:
                    break;
                case Operator.ADD:
                    break;
                case Operator.SUB:
                    break;
                case Operator.MULT:
                    break;
                case Operator.DIVIDE:
                    break;
                case Operator.MOD:
                    break;
                default:
                    return false;
            }
            return false;
        }

        public bool ManyToOne(object v1, params object[] varibles)
        { foreach (var v2 in varibles) if (!Double(v1, v2)) return false; return true; }

        public bool ManyToMany(params object[] varibles)
        {
            if (varibles.Length % 2 != 0)
                throw new InvalidOperationException("Cannot do a many to many with an uneven number of values");

            object[] set1 = new object[varibles.Length / 2];
            object[] set2 = new object[varibles.Length / 2];
            Array.Copy(varibles,0 ,set1,0, set1.Length);
            Array.Copy(varibles,set1.Length, set1,0, set2.Length);

            foreach (var set in set1.Zip( set2,(a1,a2)=> new { a1,a2}))
            {if (!Double(set.a1, set.a2)) return false; return true; }

            object v1 = varibles[0];
            object v2 = varibles[1];

            switch (Op)
            {
                

                case Operator.POWER:
                    break;
                case Operator.SKIP_IF_NULL:
                    break;
                case Operator.IF_NULL:
                    break;
                case Operator.INLINE_IF:
                    break;
                case Operator.INLINE_ELSE:
                    break;
                case Operator.LEFT_GREATER:
                    return (float)v1 > (float)v2;
                case Operator.RIGHT_GREATER:
                    return (float)v1 < (float)v2;
                case Operator.LEFT_EQUAL_GREATER:
                    return (float)v1 >= (float)v2;
                case Operator.RIGHT_EQUAL_GREATER:
                    return (float)v1 <= (float)v2;
                default:
                    break;
            }

            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interpreter.Values.Interfaces;
using Lexer.Tokens;

namespace Interpreter.Values;

public abstract class BaseValue : IValue
{
    public virtual object Value {get; set;}
    public abstract BaseValue OperatedBy(Token _operator, BaseValue other);

    public int Matches(BaseValue other)
    {
        if (Value.GetType() != other.Value.GetType())
        {
            Console.WriteLine("Types dont match");
            return 0;
        }
        if (!Value.Equals(other.Value))
        {
            Console.WriteLine("Values don't match");
            return 0;
        }
        return 1;
    }
}
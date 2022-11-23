using PirateInterpreter.Values.Interfaces;

namespace PirateInterpreter.Values;

public class CharValue : BaseValue, IValue
{
    public CharValue(object value, ILogger logger) :base(value, logger) {}

    public override BaseValue OperatedBy(Token _operator, BaseValue other)
    {
        if (Value is not char && Value is not string)
        {
            throw new TypeConversionException(typeof(char));
        }
        var value = (char)Value;

        switch (_operator.TokenType)
        {
            case TokenOperators.PLUS:
                Logger.Log("<char> + <char> is not supported", Common.Enum.LogType.ERROR);
                throw new NotImplementedException();

            case TokenOperators.MINUS:
                Logger.Log("<char> - <char> is not supported", Common.Enum.LogType.ERROR);
                throw new NotImplementedException();

            case TokenOperators.MULTIPLY:
                Logger.Log("<char> * <char> is not supported", Common.Enum.LogType.ERROR);
                throw new NotImplementedException();

            case TokenOperators.DIVIDE:
                Logger.Log("<char> / <char> is not supported", Common.Enum.LogType.ERROR);
                throw new NotImplementedException();

            case TokenOperators.POWER:
                Logger.Log("<char> ^ <char> is not supported", Common.Enum.LogType.ERROR);
                throw new NotImplementedException();

        }
        throw new NotImplementedException($"{_operator.TokenType.ToString()} has not been implemented");
    }
}
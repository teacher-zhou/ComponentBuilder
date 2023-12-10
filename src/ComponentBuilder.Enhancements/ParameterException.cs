namespace ComponentBuilder;


public class ParameterException : Exception
{
    public ParameterException():base("The parameter is not valid for component")
    {
    }

    public ParameterException(string parameterName):base($"Parameter '{parameterName}' is not valid for component") { }

    public ParameterException(string message,string parameterName) : base($"{message}{Environment.NewLine}Parameter: {parameterName}") { }
}

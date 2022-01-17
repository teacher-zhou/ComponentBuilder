namespace ComponentBuilder.Abstrations;

public interface IStyleBuilder:IDisposable
{
    IStyleBuilder Append(string value);
    string ToString();
}

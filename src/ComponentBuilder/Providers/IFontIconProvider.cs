namespace ComponentBuilder.Providers;
public interface IFontIconProvider
{
    bool NameSpace { get; }

    bool Prefix { get; }

    string GetIconName(string name);
}

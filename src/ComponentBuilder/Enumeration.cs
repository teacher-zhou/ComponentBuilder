namespace ComponentBuilder;
public abstract class Enumeration<TEnum> where TEnum : Enumeration<TEnum>
{
    protected Enumeration(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public IEnumerable<string> GetMembers()
    {
        return typeof(TEnum).GetFields(System.Reflection.BindingFlags.Static).Select(m => ((TEnum)m.GetValue(this)).Value);
    }
}

using System.Collections.ObjectModel;

namespace ComponentBuilder;
public class StyleSelector
{
    readonly Dictionary<string, string> selectors = new();

    public void AddStyle(string key, object properties)
    {
        AddStyle(key, new(properties));
    }

    public void AddStyle(string key, StyleProperty values)
    {
        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException($"'{nameof(key)}' cannot be null or empty.", nameof(key));
        }

        if (values is null)
        {
            throw new ArgumentNullException(nameof(values));
        }

        selectors[key] = values.ToString();
    }


    public void AddKeyFrames(string name, Action<StyleKeyFrame> configure)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
        }

        if (configure is null)
        {
            throw new ArgumentNullException(nameof(configure));
        }
        var keyFrames = new StyleKeyFrame();
        configure.Invoke(keyFrames);
        selectors.Add($"@keyframes {name}", keyFrames.ToString());
    }

    public override string ToString()
    => selectors.Select(m => $"{m.Key}{{ {m.Value} }}").Aggregate((prev, next) => $"{prev}\n{next}");
}

public class StyleProperty : Collection<KeyValuePair<string, object>>
{
    public StyleProperty(object values) : this(values.GetType().GetProperties().Select(m => new KeyValuePair<string, object>(m.Name, m.GetValue(values))).ToList())
    {

    }
    public StyleProperty(IList<KeyValuePair<string, object>> values) : base(values)
    {
    }

    public override string ToString()
    => this.Items.Select(m => $"{m.Key}:{m.Value};").Aggregate((prev, next) => $"{prev} {next}");
}
public class StyleKeyFrame : Collection<KeyValuePair<string, StyleProperty>>
{
    public StyleKeyFrame Add(string name, object values)
    {
        base.Add(new KeyValuePair<string, StyleProperty>(name, new(values)));
        return this;
    }

    public override string ToString()
    => this.Items.Select(m => $"{m.Key} {{ {m.Value} }}").Aggregate((prev, next) => $"{prev} {next}");
}
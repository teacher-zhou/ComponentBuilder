//namespace ComponentBuilder;
//public sealed class FragmentContent
//{
//    internal FragmentContent(RenderFragment content)
//    {
//        Content = content;
//    }

//    public RenderFragment Content { get; }

//    public static implicit operator FragmentContent(string? content)
//    {
//        return new FragmentContent(builder => builder.AddContent(0, content));
//    }

//    public static implicit operator FragmentContent(RenderFragment? content)
//    {
//        return new FragmentContent(content);
//    }

//    public static explicit operator RenderFragment(FragmentContent content)
//        => content.Content;
//}

//public sealed class FragmentContent<TValue>
//{
//    internal FragmentContent(RenderFragment<TValue?> content)
//    {
//        Content = content;
//    }

//    public RenderFragment<TValue> Content { get; }

//    public static implicit operator FragmentContent<TValue?>(RenderFragment<TValue?>? content)
//    {
//        return new FragmentContent<TValue?>(content);
//    }
//}
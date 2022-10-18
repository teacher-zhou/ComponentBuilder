namespace ComponentBuilder
{
    public class HtmlReference
    {
        public OneOf<ElementReference,object>? Reference { get; set; }

        public HtmlReference(OneOf<ElementReference, object>? reference)
        {
            Reference = reference;
        }
    }
}

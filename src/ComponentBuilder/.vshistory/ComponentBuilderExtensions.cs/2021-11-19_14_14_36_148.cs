using ComponentBuilder.Abstrations;
using System;
using System.Reflection;

namespace ComponentBuilder
{
    /// <summary>
    /// The extensions of component builder.
    /// </summary>
    public static class ComponentBuilderExtensions
    {
        /// <summary>
        /// Try to get specified attribute from <see cref=" Type"/> instance.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute.</typeparam>
        /// <param name="type">The instance of type.</param>
        /// <param name="attribute">If found, return a specified attribute instance, otherwise return <c>null</c>.</param>
        /// <returns><c>true</c> if found the specified attribute, otherwise <c>false</c>.</returns>
        public static bool TryGetAttribute<TAttribute>(this Type type, out TAttribute attribute) where TAttribute : Attribute
        {
            attribute = type.GetCustomAttribute<TAttribute>();
            return attribute != null;
        }
        /// <summary>
        /// Try to get specified attribute from <see cref="FieldInfo"/> instance.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute.</typeparam>
        /// <param name="type">The instance of type.</param>
        /// <param name="attribute">If found, return a specified attribute instance, otherwise return <c>null</c>.</param>
        /// <returns><c>true</c> if found the specified attribute, otherwise <c>false</c>.</returns>
        public static bool TryGetAttribute<TAttribute>(this FieldInfo field, out TAttribute attribute) where TAttribute : Attribute
        {
            attribute = field.GetCustomAttribute<TAttribute>();
            return attribute != null;
        }

        /// <summary>
        /// Return <see cref="CssClassAttribute.Css"/> for enum member if specified, otherwise return enum member name.
        /// </summary>
        /// <param name="enum">The instance of enum.</param>
        /// <param name="original">If not specified <see cref="CssClassAttribute"/> for enum member, <c>true</c> to return the member name, otherwise return enum member name with lower case.</param>
        /// <returns>A value represent for css class string.</returns>
        public static string GetCssClass(this Enum @enum, bool original = default)
        {
            var enumType = @enum.GetType();

            var enumMember = enumType.GetField(@enum.ToString());

            if (enumMember.TryGetAttribute<CssClassAttribute>(out var cssClassAttribute))
            {
                return cssClassAttribute.Css;
            }
            return original ? enumMember.Name : enumMember.Name.ToLower();
        }

        public static string Build(this ICssClassBuilder builder, bool disposing = false)
        {
            var result = builder.Build();
            if (disposing)
            {
                builder.Dispose();
            }
            return result;
        }
    }
}
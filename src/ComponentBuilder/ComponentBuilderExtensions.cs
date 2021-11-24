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
        public static bool TryGetCustomAttribute<TAttribute>(this Type type, out TAttribute attribute) where TAttribute : Attribute
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            attribute = type.GetCustomAttribute<TAttribute>();
            return attribute != null;
        }
        /// <summary>
        /// Try to get specified attribute from <see cref="FieldInfo"/> instance.
        /// </summary>
        /// <typeparam name="TAttribute">The type of attribute.</typeparam>
        /// <param name="field">The instance of field.</param>
        /// <param name="attribute">If found, return a specified attribute instance, otherwise return <c>null</c>.</param>
        /// <returns><c>true</c> if found the specified attribute, otherwise <c>false</c>.</returns>
        public static bool TryGetCustomAttribute<TAttribute>(this FieldInfo field, out TAttribute attribute) where TAttribute : Attribute
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

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
            if (enumMember is null)
            {
                return string.Empty;
            }
            if (enumMember.TryGetCustomAttribute<CssClassAttribute>(out var cssClassAttribute))
            {
                return cssClassAttribute.Css;
            }
            return original ? enumMember.Name : enumMember.Name.ToLower();
        }

        /// <summary>
        /// Build css class string and dispose builder collection.
        /// </summary>
        /// <param name="builder">The instance of <see cref="ICssClassBuilder"/>.</param>
        /// <param name="disposing"><c>true</c> to dispose collection of builder, otherwise <c>false</c>.</param>
        /// <returns>A css class string separated by space for each item.</returns>
        public static string Build(this ICssClassBuilder builder, bool disposing)
        {
            var result = builder.ToString();
            if (disposing)
            {
                builder.Dispose();
            }
            return result;
        }
    }
}
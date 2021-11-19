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

        public static string GetCssClass(this Enum @enum, bool original = default)
        {
            if (@enum is not Enum)
            {
                throw new InvalidOperationException($"This type is not a Enum type");
            }

            var enumType = @enum.GetType();

            var enumMember = enumType.GetField(@enum.ToString());

            if (enumMember.TryGetAttribute<CssClassAttribute>(out var cssClassAttribute))
            {
                return cssClassAttribute.Css;
            }
            return original ? enumMember.Name : enumMember.Name.ToLower();
        }
    }
}
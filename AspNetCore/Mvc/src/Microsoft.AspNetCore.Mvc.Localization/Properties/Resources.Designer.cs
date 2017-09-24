// <auto-generated />
namespace Microsoft.AspNetCore.Mvc.Localization
{
    using System.Globalization;
    using System.Reflection;
    using System.Resources;

    internal static class Resources
    {
        private static readonly ResourceManager _resourceManager
            = new ResourceManager("Microsoft.AspNetCore.Mvc.Localization.Resources", typeof(Resources).GetTypeInfo().Assembly);

        /// <summary>
        /// Must call CreateStringLocalizer method before using this property.
        /// </summary>
        internal static string NullStringLocalizer
        {
            get { return GetString("NullStringLocalizer"); }
        }

        /// <summary>
        /// Must call CreateStringLocalizer method before using this property.
        /// </summary>
        internal static string FormatNullStringLocalizer()
        {
            return GetString("NullStringLocalizer");
        }

        /// <summary>
        /// IStringLocalizerFactory is null. Must call other constructor overload to use this property.
        /// </summary>
        internal static string NullStringLocalizerFactory
        {
            get { return GetString("NullStringLocalizerFactory"); }
        }

        /// <summary>
        /// IStringLocalizerFactory is null. Must call other constructor overload to use this property.
        /// </summary>
        internal static string FormatNullStringLocalizerFactory()
        {
            return GetString("NullStringLocalizerFactory");
        }

        private static string GetString(string name, params string[] formatterNames)
        {
            var value = _resourceManager.GetString(name);

            System.Diagnostics.Debug.Assert(value != null);

            if (formatterNames != null)
            {
                for (var i = 0; i < formatterNames.Length; i++)
                {
                    value = value.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
                }
            }

            return value;
        }
    }
}

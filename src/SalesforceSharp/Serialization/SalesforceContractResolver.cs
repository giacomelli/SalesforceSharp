using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SalesforceSharp.Serialization
{
    /// <summary>
    /// Only pulling properties that doesn't ignore with salesforce attribute.
    /// </summary>
    public class SalesforceContractResolver : DefaultContractResolver
    {
        private bool updateResolver;

        /// <summary>
        /// This Resolver will extract the correct salesforce FieldName/ApiName
        /// </summary>
        /// <param name="updateResolver"></param>
        public SalesforceContractResolver(bool updateResolver)
        {
            this.updateResolver = updateResolver;
        }

        /// <summary>
        /// Create a list of JsonProperties of the members in the type.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberSerialization"></param>
        /// <returns></returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);

            return properties.Where(x=> x != null).ToList();
        }

        /// <summary>
        /// Creates a <see cref="T:Newtonsoft.Json.Serialization.JsonProperty"/> for the given <see cref="T:System.Reflection.MemberInfo"/>.
        /// </summary>
        /// <param name="memberSerialization">The member's parent <see cref="T:Newtonsoft.Json.MemberSerialization"/>.</param><param name="member">The member to create a <see cref="T:Newtonsoft.Json.Serialization.JsonProperty"/> for.</param>
        /// <returns>
        /// A created <see cref="T:Newtonsoft.Json.Serialization.JsonProperty"/> for the given <see cref="T:System.Reflection.MemberInfo"/>.
        /// </returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var jsonProp = base.CreateProperty(member, memberSerialization);

            var sfAttrs = member.GetCustomAttributes(typeof(SalesforceAttribute), true);

            // if there are no attr then no need to process any further
            if (!sfAttrs.Any()) return jsonProp;

            var sfAttr = sfAttrs.FirstOrDefault() as SalesforceAttribute;
            // if there are no attr then no need to process any further
            if (sfAttr == null)
            {
                return jsonProp;
            }

            // if ignore then we should skip it and return null
            if (sfAttr.Ignore || (updateResolver && sfAttr.IgnoreUpdate))
            {
                return null;
            }

            // if no fieldname then we use the default
            if (string.IsNullOrEmpty(sfAttr.FieldName))
            {
                return jsonProp;
            }

            jsonProp.PropertyName = sfAttr.FieldName;

            return jsonProp;
        }
    }
}
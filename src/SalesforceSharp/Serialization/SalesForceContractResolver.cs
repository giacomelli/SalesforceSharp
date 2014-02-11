using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SalesforceSharp.Serialization.SalesForceAttributes;

namespace SalesforceSharp.Serialization
{
    /// <summary>
    /// Only pulling properties that doesn't ignore with salesforce attribute.
    /// </summary>
    public class SalesForceContractResolver : DefaultContractResolver
    {
        private readonly bool update;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="update"></param>
        public SalesForceContractResolver(bool update)
        {
            this.update = update;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="memberSerialization"></param>
        /// <returns></returns>
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);
            
            var ignoreProps = type.GetProperties().Where(x =>
            {
                var sfAttrs = x.GetCustomAttributes(typeof(SalesForceAttribute), true);
                // if there are no attr then definitely include by default
                if (!sfAttrs.Any())
                {
                    return false;
                }

                var sfAttr = sfAttrs.FirstOrDefault() as SalesForceAttribute;
                // it should be of type SalesForceAttribute but just to cover our track.
                if (sfAttr == null)
                {
                    return false;
                }

                // if ignore then doesn't matter if it is pull or update we will ignore it.
                if (sfAttr.Ignore)
                {
                    return true;
                }

                // if it is update and proptery is also ignore update then we ignore it.
                return update && sfAttr.IgnoreUpdate;
            }).Select(y=> y.Name);

            // only get propteries that we do not ignore.
            properties =
                properties.Where(p => ignoreProps.All(x => x != p.PropertyName)).ToList();

            return properties;
        }

    }
}
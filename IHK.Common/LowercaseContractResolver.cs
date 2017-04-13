using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace IHK.Common
{
    public class LowercaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return Char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
        }
    }
}

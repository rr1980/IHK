using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IHK.MultiUserBlock
{
    //[JsonConverter(typeof(StringEnumConverter))]
    public enum MultiUserBlockCommand
    {
        Ping,
        Update,
        Active
    }
}

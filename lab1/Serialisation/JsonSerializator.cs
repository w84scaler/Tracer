using Newtonsoft.Json;

namespace lab1.Serialisation
{
    class JsonSerializator : ISerializator
    {
        private JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Include,
            Formatting = Formatting.Indented,
        };
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, JsonSettings);
        }
    }
}

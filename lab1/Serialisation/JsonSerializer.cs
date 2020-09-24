using Newtonsoft.Json;

namespace lab1.Serialisation
{
    class JsonSerializer : ISerializer
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
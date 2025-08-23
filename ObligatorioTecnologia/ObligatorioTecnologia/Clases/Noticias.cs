using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;


namespace ObligatorioTecnologia.Clases
{
    public class Noticias
    {
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty("totalResults", NullValueHandling = NullValueHandling.Ignore)]
        public long? TotalResults { get; set; }

        [JsonProperty("results", NullValueHandling = NullValueHandling.Ignore)]
        public List<NoticiaResult> Results { get; set; }

        [JsonProperty("nextPage", NullValueHandling = NullValueHandling.Ignore)]
        public string NextPage { get; set; }
    }

    public partial class NoticiaResult
    {
        [JsonProperty("article_id", NullValueHandling = NullValueHandling.Ignore)]
        public string ArticleId { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title { get; set; }

        [JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Link { get; set; }

        [JsonProperty("keywords")]
        public List<string> Keywords { get; set; }

        [JsonProperty("creator", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Creator { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
        public string Content { get; set; }

        [JsonProperty("pubDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset? PubDate { get; set; }

        [JsonProperty("pubDateTZ", NullValueHandling = NullValueHandling.Ignore)]
        public PubDateTz? PubDateTz { get; set; }

        [JsonProperty("image_url")]
        public Uri ImageUrl { get; set; }

        [JsonProperty("video_url")]
        public object VideoUrl { get; set; }

        [JsonProperty("source_id", NullValueHandling = NullValueHandling.Ignore)]
        public string SourceId { get; set; }

        [JsonProperty("source_name", NullValueHandling = NullValueHandling.Ignore)]
        public string SourceName { get; set; }

        [JsonProperty("source_priority", NullValueHandling = NullValueHandling.Ignore)]
        public long? SourcePriority { get; set; }

        [JsonProperty("source_url", NullValueHandling = NullValueHandling.Ignore)]
        public Uri SourceUrl { get; set; }

        [JsonProperty("source_icon", NullValueHandling = NullValueHandling.Ignore)]
        public Uri SourceIcon { get; set; }

        [JsonProperty("language", NullValueHandling = NullValueHandling.Ignore)]
        public Language? Language { get; set; }

        [JsonProperty("country", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Country { get; set; }

        [JsonProperty("category", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Category { get; set; }

        [JsonProperty("sentiment", NullValueHandling = NullValueHandling.Ignore)]
        public string? Sentiment { get; set; }

        [JsonProperty("sentiment_stats", NullValueHandling = NullValueHandling.Ignore)]
        public string? SentimentStats { get; set; }

        [JsonProperty("ai_tag", NullValueHandling = NullValueHandling.Ignore)]
        public string? AiTag { get; set; }

        [JsonProperty("ai_region", NullValueHandling = NullValueHandling.Ignore)]
        public string? AiRegion { get; set; }

        [JsonProperty("ai_org", NullValueHandling = NullValueHandling.Ignore)]
        public string? AiOrg { get; set; }

        [JsonProperty("ai_summary", NullValueHandling = NullValueHandling.Ignore)]
        public string? AiSummary { get; set; }

        [JsonProperty("ai_content", NullValueHandling = NullValueHandling.Ignore)]
        public string? AiContent { get; set; }

        [JsonProperty("duplicate", NullValueHandling = NullValueHandling.Ignore)]
        public bool? Duplicate { get; set; }
    }

    public enum AiContent { OnlyAvailableInProfessionalAndCorporatePlans };

    public enum Ai { OnlyAvailableInCorporatePlans };

    public enum AiSummary { OnlyAvailableInPaidPlans };

    public enum Category { Top };

    public enum Language { Spanish };

    public enum PubDateTz { Utc };


    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                AiContentConverter.Singleton,
                AiConverter.Singleton,
                AiSummaryConverter.Singleton,
                CategoryConverter.Singleton,
                LanguageConverter.Singleton,
                PubDateTzConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class AiContentConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(AiContent) || t == typeof(AiContent?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "ONLY AVAILABLE IN PROFESSIONAL AND CORPORATE PLANS")
            {
                return AiContent.OnlyAvailableInProfessionalAndCorporatePlans;
            }
            throw new Exception("Cannot unmarshal type AiContent");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (AiContent)untypedValue;
            if (value == AiContent.OnlyAvailableInProfessionalAndCorporatePlans)
            {
                serializer.Serialize(writer, "ONLY AVAILABLE IN PROFESSIONAL AND CORPORATE PLANS");
                return;
            }
            throw new Exception("Cannot marshal type AiContent");
        }

        public static readonly AiContentConverter Singleton = new AiContentConverter();
    }

    internal class AiConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Ai) || t == typeof(Ai?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "ONLY AVAILABLE IN CORPORATE PLANS")
            {
                return Ai.OnlyAvailableInCorporatePlans;
            }
            throw new Exception("Cannot unmarshal type Ai");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Ai)untypedValue;
            if (value == Ai.OnlyAvailableInCorporatePlans)
            {
                serializer.Serialize(writer, "ONLY AVAILABLE IN CORPORATE PLANS");
                return;
            }
            throw new Exception("Cannot marshal type Ai");
        }

        public static readonly AiConverter Singleton = new AiConverter();
    }

    internal class AiSummaryConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(AiSummary) || t == typeof(AiSummary?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "ONLY AVAILABLE IN PAID PLANS")
            {
                return AiSummary.OnlyAvailableInPaidPlans;
            }
            throw new Exception("Cannot unmarshal type AiSummary");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (AiSummary)untypedValue;
            if (value == AiSummary.OnlyAvailableInPaidPlans)
            {
                serializer.Serialize(writer, "ONLY AVAILABLE IN PAID PLANS");
                return;
            }
            throw new Exception("Cannot marshal type AiSummary");
        }

        public static readonly AiSummaryConverter Singleton = new AiSummaryConverter();
    }

    internal class CategoryConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Category) || t == typeof(Category?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "top")
            {
                return Category.Top;
            }
            throw new Exception("Cannot unmarshal type Category");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Category)untypedValue;
            if (value == Category.Top)
            {
                serializer.Serialize(writer, "top");
                return;
            }
            throw new Exception("Cannot marshal type Category");
        }

        public static readonly CategoryConverter Singleton = new CategoryConverter();
    }

    internal class LanguageConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Language) || t == typeof(Language?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "spanish")
            {
                return Language.Spanish;
            }
            throw new Exception("Cannot unmarshal type Language");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Language)untypedValue;
            if (value == Language.Spanish)
            {
                serializer.Serialize(writer, "spanish");
                return;
            }
            throw new Exception("Cannot marshal type Language");
        }

        public static readonly LanguageConverter Singleton = new LanguageConverter();
    }

    internal class PubDateTzConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(PubDateTz) || t == typeof(PubDateTz?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "UTC")
            {
                return PubDateTz.Utc;
            }
            throw new Exception("Cannot unmarshal type PubDateTz");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (PubDateTz)untypedValue;
            if (value == PubDateTz.Utc)
            {
                serializer.Serialize(writer, "UTC");
                return;
            }
            throw new Exception("Cannot marshal type PubDateTz");
        }

        public static readonly PubDateTzConverter Singleton = new PubDateTzConverter();
    }
}


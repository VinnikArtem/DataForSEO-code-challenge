using System.Text.Json.Serialization;

namespace TaskProcessor.Worker.Infrastructure.Models
{
    public class Keyword
    {
        [JsonPropertyName("keyword")]
        public string? KeywordValue { get; set; }

        [JsonPropertyName("location")]
        public int? Location { get; set; }

        [JsonPropertyName("language")]
        public string? Language { get; set; }

        [JsonPropertyName("spell")]
        public string? Spell { get; set; }

        [JsonPropertyName("spell_type")]
        public string? SpellType { get; set; }

        [JsonPropertyName("keyword_info")]
        public KeywordInfo? KeywordInfo { get; set; }

        [JsonPropertyName("extra")]
        public Extra? Extra { get; set; }

        [JsonPropertyName("search_intent_info")]
        public SearchIntentInfo? SearchIntentInfo { get; set; }
    }

    public class KeywordInfo
    {
        [JsonPropertyName("search_volume")]
        public int? SearchVolume { get; set; }

        [JsonPropertyName("cpc")]
        public double? Cpc { get; set; }

        [JsonPropertyName("competition")]
        public double? Competition { get; set; }

        [JsonPropertyName("competition_level")]
        public string? CompetitionLevel { get; set; }

        [JsonPropertyName("low_top_of_page_bid")]
        public double? LowTopOfPageBid { get; set; }

        [JsonPropertyName("high_top_of_page_bid")]
        public double? HighTopOfPageBid { get; set; }

        [JsonPropertyName("time_update")]
        public DateTime? TimeUpdate { get; set; }

        [JsonPropertyName("categories")]
        public IEnumerable<int?>? Categories { get; set; }

        [JsonPropertyName("history")]
        public IDictionary<string, int?>? History { get; set; }
    }

    public class Extra
    {
        [JsonPropertyName("core_keyword")]
        public string? CoreKeyword { get; set; }

        [JsonPropertyName("synonym_clustering_algorithm")]
        public string? SynonymClusteringAlgorithm { get; set; }

        [JsonPropertyName("detected_language")]
        public string? DetectedLanguage { get; set; }

        [JsonPropertyName("keyword_difficulty")]
        public int? KeywordDifficulty { get; set; }
    }

    public class SearchIntentInfo
    {
        [JsonPropertyName("main_intent")]
        public string? MainIntent { get; set; }

        [JsonPropertyName("foreign_intent")]
        public IEnumerable<string>? ForeignIntent { get; set; }

        [JsonPropertyName("last_updated_time")]
        public DateTime? LastUpdatedTime { get; set; }
    }
}


/*
 using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuickType {
    public partial class Temperatures {
        [JsonPropertyName("viewer_cid")]
        public long ViewerCid { get; set; }

        [JsonPropertyName("cid")]
        public long Cid { get; set; }

        [JsonPropertyName("is_buddy")]
        public bool IsBuddy { get; set; }

        [JsonPropertyName("is_friend")]
        public long IsFriend { get; set; }

        [JsonPropertyName("is_qa")]
        public bool IsQa { get; set; }

        [JsonPropertyName("avname")]
        public string Avname { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("avpic_url")]
        public Uri AvpicUrl { get; set; }

        [JsonPropertyName("registered")]
        public DateTimeOffset Registered { get; set; }

        [JsonPropertyName("last_login")]
        public string LastLogin { get; set; }

        [JsonPropertyName("interests")]
        public Interests Interests { get; set; }

        [JsonPropertyName("dating")]
        public Dating Dating { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }

        [JsonPropertyName("age")]
        public string Age { get; set; }

        [JsonPropertyName("tagline")]
        public string Tagline { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("country_code")]
        public long CountryCode { get; set; }

        [JsonPropertyName("location_state")]
        public string LocationState { get; set; }

        [JsonPropertyName("online")]
        public bool Online { get; set; }

        [JsonPropertyName("availability")]
        public string Availability { get; set; }

        [JsonPropertyName("badge_count")]
        public long BadgeCount { get; set; }

        [JsonPropertyName("badge_level")]
        public long BadgeLevel { get; set; }

        [JsonPropertyName("badge_layout")]
        public BadgeLayout BadgeLayout { get; set; }

        [JsonPropertyName("badge_area_html")]
        public string BadgeAreaHtml { get; set; }

        [JsonPropertyName("show_ageverify")]
        public long ShowAgeverify { get; set; }

        [JsonPropertyName("show_badgecount")]
        public bool ShowBadgecount { get; set; }

        [JsonPropertyName("show_flag_icon")]
        public long ShowFlagIcon { get; set; }

        [JsonPropertyName("show_flag_av")]
        public long ShowFlagAv { get; set; }

        [JsonPropertyName("show_message")]
        public long ShowMessage { get; set; }

        [JsonPropertyName("avpic_default")]
        public long AvpicDefault { get; set; }

        [JsonPropertyName("show_block")]
        public bool ShowBlock { get; set; }

        [JsonPropertyName("welcome_moderator_score")]
        public long WelcomeModeratorScore { get; set; }

        [JsonPropertyName("is_welcome_moderator")]
        public long IsWelcomeModerator { get; set; }

        [JsonPropertyName("is_creator")]
        public long IsCreator { get; set; }

        [JsonPropertyName("public_rooms")]
        public List<PublicRoom> PublicRooms { get; set; }

        [JsonPropertyName("visible_albums")]
        public long VisibleAlbums { get; set; }

        [JsonPropertyName("imvu_level")]
        public long ImvuLevel { get; set; }

        [JsonPropertyName("wallpaper_id")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long WallpaperId { get; set; }
    }

    public partial class BadgeLayout {
        [JsonPropertyName("badge-39-4")]
        public Badge394 Badge394 { get; set; }
    }

    public partial class Badge {
        [JsonPropertyName("creator_id")]
        public long CreatorId { get; set; }

        [JsonPropertyName("creator_badge_index")]
        public long CreatorBadgeIndex { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("image_mogilekey")]
        public string ImageMogilekey { get; set; }

        [JsonPropertyName("image_width")]
        public long ImageWidth { get; set; }

        [JsonPropertyName("image_height")]
        public long ImageHeight { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("allow_autogrant")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long AllowAutogrant { get; set; }

        [JsonPropertyName("badge_type")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long BadgeType { get; set; }

        [JsonPropertyName("review_status")]
        public ReviewStatus ReviewStatus { get; set; }

        [JsonPropertyName("flagger_id")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long FlaggerId { get; set; }

        [JsonPropertyName("flag_time")]
        public FlagTime FlagTime { get; set; }

        [JsonPropertyName("badgeid")]
        public string Badgeid { get; set; }

        [JsonPropertyName("image_url")]
        public Uri ImageUrl { get; set; }

        [JsonPropertyName("xloc")]
        public long Xloc { get; set; }

        [JsonPropertyName("yloc")]
        public long Yloc { get; set; }
    }

    public partial class Badge394 {
        [JsonPropertyName("creator_id")]
        public long CreatorId { get; set; }

        [JsonPropertyName("creator_badge_index")]
        public long CreatorBadgeIndex { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("image_url")]
        public Uri ImageUrl { get; set; }

        [JsonPropertyName("image_width")]
        public long ImageWidth { get; set; }

        [JsonPropertyName("image_height")]
        public long ImageHeight { get; set; }

        [JsonPropertyName("predicate")]
        public string Predicate { get; set; }

        [JsonPropertyName("badgeid")]
        public string Badgeid { get; set; }

        [JsonPropertyName("review_status")]
        public ReviewStatus ReviewStatus { get; set; }

        [JsonPropertyName("xloc")]
        public long Xloc { get; set; }

        [JsonPropertyName("yloc")]
        public long Yloc { get; set; }
    }

    public partial class Dating {
        [JsonPropertyName("relationship_status")]
        public string RelationshipStatus { get; set; }

        [JsonPropertyName("orientation")]
        public string Orientation { get; set; }

        [JsonPropertyName("looking_for")]
        public string LookingFor { get; set; }
    }

    public partial class Interests {
        [JsonPropertyName("full_text_string")]
        public FullTextString FullTextString { get; set; }
    }

    public partial class FullTextString {
        [JsonPropertyName("tag")]
        public string Tag { get; set; }

        [JsonPropertyName("raw_tag")]
        public string RawTag { get; set; }
    }

    public partial class PublicRoom {
        [JsonPropertyName("room_instance_id")]
        public string RoomInstanceId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("is_ap")]
        public long IsAp { get; set; }

        [JsonPropertyName("is_vip")]
        public long IsVip { get; set; }
    }

    public enum FlagTime {
        The00000000000000
    };

    public enum ReviewStatus {
        NotReviewed,
        PassedReview
    };

    public partial class Temperatures {
        public static Temperatures FromJson(string json)
            => JsonConvert.DeserializeObject<Temperatures>(json, QuickType.Converter.Settings);
    }

    public static class Serialize {
        public static string ToJson(this Temperatures self)
            => JsonConvert.SerializeObject(self, QuickType.Converter.Settings);
    }

    internal static class Converter {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                FlagTimeConverter.Singleton,
                ReviewStatusConverter.Singleton,
                new IsoDateTimeConverter {DateTimeStyles = DateTimeStyles.AssumeUniversal}
            },
        };
    }

    internal class ParseStringConverter : JsonConverter {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer) {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l)) {
                return l;
            }

            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer) {
            if (untypedValue == null) {
                serializer.Serialize(writer, null);
                return;
            }

            var value = (long) untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }

    internal class FlagTimeConverter : JsonConverter {
        public override bool CanConvert(Type t) => t == typeof(FlagTime) || t == typeof(FlagTime?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer) {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "0000-00-00 00:00:00") {
                return FlagTime.The00000000000000;
            }

            throw new Exception("Cannot unmarshal type FlagTime");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer) {
            if (untypedValue == null) {
                serializer.Serialize(writer, null);
                return;
            }

            var value = (FlagTime) untypedValue;
            if (value == FlagTime.The00000000000000) {
                serializer.Serialize(writer, "0000-00-00 00:00:00");
                return;
            }

            throw new Exception("Cannot marshal type FlagTime");
        }

        public static readonly FlagTimeConverter Singleton = new FlagTimeConverter();
    }

    internal class ReviewStatusConverter : JsonConverter {
        public override bool CanConvert(Type t) => t == typeof(ReviewStatus) || t == typeof(ReviewStatus?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer) {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value) {
                case "not_reviewed":
                    return ReviewStatus.NotReviewed;
                case "passed_review":
                    return ReviewStatus.PassedReview;
            }

            throw new Exception("Cannot unmarshal type ReviewStatus");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer) {
            if (untypedValue == null) {
                serializer.Serialize(writer, null);
                return;
            }

            var value = (ReviewStatus) untypedValue;
            switch (value) {
                case ReviewStatus.NotReviewed:
                    serializer.Serialize(writer, "not_reviewed");
                    return;
                case ReviewStatus.PassedReview:
                    serializer.Serialize(writer, "passed_review");
                    return;
            }

            throw new Exception("Cannot marshal type ReviewStatus");
        }

        public static readonly ReviewStatusConverter Singleton = new ReviewStatusConverter();
    }
}
*/
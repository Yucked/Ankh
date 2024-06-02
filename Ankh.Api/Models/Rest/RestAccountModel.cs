using System.Text.Json.Serialization;
using Ankh.Api.Converters;
using Ankh.Api.Models.Enums;

namespace Ankh.Api.Models.Rest;

public record RestAccountModel(
    [property: JsonPropertyName("show_age")]
    bool ShowAge,
    [property: JsonPropertyName("age")]
    long Age,
    [property: JsonPropertyName("show_gender")]
    bool ShowGender,
    [property: JsonPropertyName("gender")]
    string Gender,
    [property: JsonPropertyName("show_ap")]
    bool ShowAccessPass,
    [property: JsonPropertyName("is_ap")]
    bool IsAp,
    [property: JsonPropertyName("show_vip")]
    bool ShowVip,
    [property: JsonPropertyName("is_vip")]
    bool IsVip,
    [property: JsonPropertyName("vip_expires")]
    string VipExpiration,
    [property: JsonPropertyName("show_ageverified")]
    bool ShowAgeVerification,
    [property: JsonPropertyName("is_ageverified")]
    bool IsAgeverified,
    [property: JsonPropertyName("show_location")]
    bool ShowLocation,
    [property: JsonPropertyName("location")]
    string Location,
    [property: JsonPropertyName("show_current_chat_room")]
    bool ShowCurrentChatRoom,
    [property: JsonPropertyName("availability"),
               JsonConverter(typeof(AvailabilityConverter))]
    Availability Availability,
    [property: JsonPropertyName("email")]
    string Email,
    [property: JsonPropertyName("last_password_change")]
    string LastPasswordChange
) : IRestModel;
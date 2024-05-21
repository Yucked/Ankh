namespace Ankh.Api.Attributes;

[AttributeUsage(validOn: AttributeTargets.Property | AttributeTargets.Field)]
public sealed class JsonPropertyNames(params string[] names) : Attribute {
    public string[] Names { get; } = names;
}
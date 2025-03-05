namespace HydraScript.Domain.Constants;

internal class StringSyntaxAttribute(string syntax) : Attribute
{
    public const string Json = nameof(Json);

    public override string ToString() =>
        nameof(StringSyntaxAttribute) + "." + syntax;
}
namespace HydraScript.Domain.BackEnd;

public interface IFrame
{
    object? this[string id] { get; set; }
}
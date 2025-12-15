namespace HydraScript.Domain.BackEnd;

public interface IFrameContext
{
    IFrame Current { get; }

    void StepIn();

    IFrame StepOut();
}
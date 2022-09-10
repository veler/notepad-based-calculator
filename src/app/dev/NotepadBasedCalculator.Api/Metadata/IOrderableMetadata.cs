namespace NotepadBasedCalculator.Api
{
    public interface IOrderableMetadata
    {
        IReadOnlyList<string> Before { get; }

        IReadOnlyList<string> After { get; }

        string Name { get; }
    }
}

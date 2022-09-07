namespace NotepadBasedCalculator.Shared
{
    public interface IConfigurationReader
    {
        string WebServiceAppId { get; }

        string WebServiceUrl { get; }
    }
}

namespace NotepadBasedCalculator.Core
{
    internal sealed class TextDocument
    {
        private string _text = string.Empty;

        internal string Text
        {
            get => _text;
            set
            {
                _text = value ?? string.Empty;
                TextChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        internal event EventHandler? TextChanged;
    }
}

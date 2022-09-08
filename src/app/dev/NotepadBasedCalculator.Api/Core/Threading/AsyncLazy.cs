namespace NotepadBasedCalculator.Api
{
    public sealed class AsyncLazy<T>
    {
        private readonly Lazy<Task<T>> _innerLazy;

        public bool IsValueCreated => _innerLazy.IsValueCreated && _innerLazy.Value.IsCompleted && !_innerLazy.Value.IsFaulted;

        public AsyncLazy(Func<Task<T>> valueFactory)
        {
            _innerLazy = new Lazy<Task<T>>(valueFactory);
        }

        public AsyncLazy(Func<Task<T>> valueFactory, bool isThreadSafe)
        {
            _innerLazy = new Lazy<Task<T>>(valueFactory, isThreadSafe);
        }

        public AsyncLazy(Func<Task<T>> valueFactory, LazyThreadSafetyMode mode)
        {
            _innerLazy = new Lazy<Task<T>>(valueFactory, mode);
        }

        public Task<T> GetValueAsync()
        {
            return IsValueCreated ? Task.FromResult(_innerLazy.Value.Result) : _innerLazy.Value;
        }
    }
}

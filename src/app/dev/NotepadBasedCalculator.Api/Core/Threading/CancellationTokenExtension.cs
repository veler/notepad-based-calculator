namespace NotepadBasedCalculator.Api
{
    public static class CancellationTokenExtension
    {
        public static Task AsTask(this CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<object>();
            cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken), useSynchronizationContext: false);
            return tcs.Task;
        }
    }
}

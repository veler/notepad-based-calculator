namespace NotepadBasedCalculator.Api
{
    public static class CancellationTokenExtension
    {
        /// <summary>
        /// Converts the <see cref="CancellationToken"/> to a <see cref="Task"/>
        /// that canceled when the <see cref="CancellationToken"/> is being canceled.
        /// </summary>
        public static Task AsTask(this CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<object>();
            cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken), useSynchronizationContext: false);
            return tcs.Task;
        }
    }
}

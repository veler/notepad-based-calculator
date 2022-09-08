namespace NotepadBasedCalculator.Api
{
    /// <summary>
    /// Provides a set of helper method to play around with threads.
    /// </summary>
    public static class TaskExtension
    {
        /// <summary>
        /// Runs a task without waiting for its result.
        /// </summary>
        public static void Forget(this Task _)
        {
        }

        /// <summary>
        /// Runs a task without waiting for its result.
        /// </summary>
        public static void Forget<T>(this Task<T> _)
        {
        }

        /// <summary>
        /// Runs a task without waiting for its result. Swallows or handle any exception caused by the task.
        /// </summary>
        /// <param name="errorHandler">The action to run when an exception is caught.</param>
        public static async void ForgetSafely(this Task task, Action<Exception>? errorHandler = null)
        {
            try
            {
                await task.ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                errorHandler?.Invoke(ex);
            }
        }

        public static T CompleteOnCurrentThread<T>(this Task<T> task)
        {
            return task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Gets an awaiter that schedules continuations on the specified scheduler.
        /// </summary>
        /// <param name="scheduler">The task scheduler used to execute continuations.</param>
        /// <returns>An awaitable.</returns>
        public static TaskSchedulerAwaiter GetAwaiter(this TaskScheduler scheduler)
        {
            Guard.IsNotNull(scheduler);
            return new TaskSchedulerAwaiter(scheduler);
        }

        public static Task AsTask(this CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<object>();
            cancellationToken.Register(() => tcs.TrySetCanceled(cancellationToken), useSynchronizationContext: false);
            return tcs.Task;
        }
    }
}

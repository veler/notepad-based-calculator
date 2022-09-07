using BenchmarkDotNet.Running;

namespace NotepadBasedCalculator.Benchmark
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            new BenchmarkSwitcher(typeof(Program).Assembly).Run(args);
        }
    }
}

using System.Diagnostics;
using System.Globalization;

namespace parallel_prime_counter;

class Program
{
    static int Main(string[] args)
    {
        string userInput;
        if (args.Length == 1)
        {
            userInput = args[0];
        }
        else if (args.Length < 1)
        {
            Console.Write("Enter an upperbound as positive integer between 2 and 10,000,000: ");
            userInput = Console.ReadLine() ?? "";
        }
        else
        {
            Console.WriteLine("Invalid number of program arguments. Please enter a single positive integer between 2 and 10,000,000.");
            return -1;
        }

        bool result = int.TryParse(userInput, out int upperBound);
        if (!result)
        {
            Console.WriteLine("Input for upperbound must be a positive integer between 2 and 10,000,000.");
            return -1;
        }

        if (upperBound < 2 || upperBound > 10_000_000)
        {
            Console.WriteLine("Upper bound must be between 2 and 10,000,000.");
            return -1;
        }

        Stopwatch stopwatch = new();

        stopwatch.Start();
        var sequentialCount = CountPrimesSequential(upperBound);
        stopwatch.Stop();
        var sequentialDuration = stopwatch.Elapsed;
        stopwatch.Reset();


        stopwatch.Start();
        var parallelCount = CoutnPrimesParallel(upperBound);
        stopwatch.Stop();
        var parallelDuration = stopwatch.Elapsed;

        Console.WriteLine($"Upper bound: {upperBound}");
        Console.WriteLine($"Sequential count: {sequentialCount} in {sequentialDuration.TotalMilliseconds} ms");
        Console.WriteLine($"Parallel count: {parallelCount} in {parallelDuration.TotalMilliseconds} ms");

        return 0;
    }

    private static bool IsPrime(int number)
    {
        if (number < 2) return false;

        var limit = (int)Math.Sqrt(number);

        for (var divisor = 2; divisor <= limit; divisor++)
        {
            if (number % divisor == 0) return false;
        }

        return true;
    }

    private static int CountPrimesSequential(int upperBound)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(upperBound, 2);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(upperBound, 10_000_000);

        var count = 0;
        for (var i = 2; i <= upperBound; i++)
        {
            if (IsPrime(i)) count++;
        }

        return count;
    }

    private static int CoutnPrimesParallel(int upperBound)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(upperBound, 2);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(upperBound, 10_000_000);

        var count = 0;

        Parallel.For(
            2,
            upperBound + 1,
            () => 0,
            (i, state, localCount) => IsPrime(i) ? localCount + 1 : localCount,
            localCount => Interlocked.Add(ref count, localCount));

        return count;
    }
}

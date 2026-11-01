# Week 2 Multithreading: Build a parallel prime counter

**Time**: 30-45 minutes
**Dependencies**: .NET SDK; no external packages.

Last week's exercise explored overlapping file reads. This standalone project explores dividing CPU work among threads; it doesn't require completing Week 1.

## Coding task

Build a console app that accepts an inclusive upper bound and counts the prime numbers from 2 through that bound. Reject invalid input and values outside `2-10,000,000`.

Implement two versions using teh same `IsPrime(int number)` helper:

- A sequential version that checks each number in a loop.
- A parallet version using `Parallel.For` with local subtotals, combining each subtotal into the final count with `Interlocked.Add`.

Print both counts and their elapsed times, measured separately with `Stopwatch`. For example:

```text
Upper bound: 100000
Sequential count: 9592
Parallel count: 9592
```

Check divisors only through the square root of each candidate. Avoid shared mutabled collections, creating a task for every candidate, or updating a shared coutner for every prime. Keep console output outside the measured loops.

## Acceptance criteria

- Both implementations return 4 for an upper bound of 10, 25 for 100, and 9,592 for 100,000.
- Repeating the parallel version always produces the same count as the sequential version.
- Invalid input produces a clear message without starting the calculation.

## Underlying concept

CPU-bound work can run simultaneously on multiple cores. However, `count++` involves reading, modifying, and writing a value; concurrent updates can overwrite each other. Local subtotals reduce shared access, and an atomic addition safely combines them.

Parallel execution also introduces scheduling and coordination costs. Small workloads may take longer in the parallel version, so treat timming as an observation rather than a pass/fail requirement.

**Hint**: Look for the `Parallel.For<TLocal>` overload with `localInit`, `body`, and `localFinally`. Its local state lets iterations accumulate results before touching the shared total.

**Stretch question**: Why might one atomic addition per local subtotal scale better than an atomic increment for every prime, even though both produce correct results?

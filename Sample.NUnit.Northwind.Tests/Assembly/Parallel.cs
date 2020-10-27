using NUnit.Framework;

[assembly: LevelOfParallelism(3)]
[assembly: Parallelizable(ParallelScope.Children)]
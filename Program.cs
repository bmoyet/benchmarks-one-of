// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using benchmarks.oneof;

BenchmarkRunner.Run<ControlFlowBenchmarks>();
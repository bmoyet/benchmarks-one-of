using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OneOf;

namespace benchmarks.oneof;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class ControlFlowBenchmarks
{
    static readonly Random _random = new Random();

    [Benchmark]
    public bool ExceptionBased()
    {
        try
        {
            DoSomethingExceptional();
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    [Benchmark]
    public bool OneOfBased()
    {
        var result = DoSomethingOneOf();

        return result.Match(
            success => true,
            failure => false
        );
    }

    [Benchmark]
    public bool OneOfBasedStatic()
    {
        var result = DoSomethingOneOfStatic();

        return result.Match(
            success => true,
            failure => false
        );
    }

    [Benchmark]
    public bool ReturnTypeBased()
    {
        var result = DoSomethingReturnType();

        return result.Match(
            success => true,
            failure => false
        );
    }

    [Benchmark]
    public bool ReturnTypeBasedStatic()
    {
        var result = DoSomethingReturnTypeStatic();

        return result.Match(
            success => true,
            failure => false
        );
    }

    int DoSomethingExceptional()
    {
        var v = _random.Next(1);

        if (v == 0)
        {
            throw new Exception();
        }
        return v;
    }

    OneOf<int, Failure> DoSomethingOneOf()
    {
        var v = _random.Next(1);

        if (v == 0)
        {
            return new Failure();
        }
        return v;
    }

    OneOf<int, Failure> DoSomethingOneOfStatic()
    {
        var v = _random.Next(1);

        if (v == 0)
        {
            return Failure.Default;
        }
        return v;
    }

    ReturnType DoSomethingReturnType()
    {
        var v = _random.Next(1);

        if (v == 0)
        {
            return new Failure();
        }
        return v;
    }

    ReturnType DoSomethingReturnTypeStatic()
    {
        var v = _random.Next(1);

        if (v == 0)
        {
            return Failure.Default;
        }
        return v;
    }
}

public record Failure()
{
    public static readonly Failure Default = new Failure();
}

[GenerateOneOf]
public partial class ReturnType : OneOfBase<int, Failure> {}
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OneOf;

namespace benchmarks.oneof;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class ControlFlowBenchmarks
{
    static readonly Random _random = new();

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
    public bool NullableBased()
    {
        var result = DoSomethingNullable();

        return result.HasValue;
    }

    [Benchmark]
    public bool TupleBased()
    {
        var (success, _) = DoSomethingTuple();

        return success;
    }

    [Benchmark]
    public bool ObjectBased()
    {
        var result = DoSomethingObject();

        return result.IsSuccess;
    }

    [Benchmark]
    public bool RecordBased()
    {
        var result = DoSomethingRecord();

        return result.IsSuccess;
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
    
    [Benchmark]
    public bool ReturnTypePoly()
    {
        var result = DoSomethingPoly();

        return result switch {
            AReturn.AReturnValue _ => true,
            _ => false
        };
    }
    
    [Benchmark]
    public bool ReturnTypePolyStatic()
    {
        var result = DoSomethingPolyStatic();

        return result switch {
            AReturn.AReturnValue _ => true,
            _ => false
        };
    }

    AReturn DoSomethingPolyStatic()
    {
        var v = _random.Next(1);

        if (v == 0)
        {
            return AReturn.AnError;
        }
        return new AReturn.AReturnValue(v);
    }
    
    AReturn DoSomethingPoly()
    {
        var v = _random.Next(1);

        if (v == 0)
        {
            return new AReturn.AError();
        }
        return new AReturn.AReturnValue(v);
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

    int? DoSomethingNullable()
    {
        var v = _random.Next(1);

        if (v == 0)
        {
            return null;
        }
        return v;
    }

    (bool, int) DoSomethingTuple()
    {
        var v = _random.Next(1);

        if (v == 0)
        {
            return (false, 0);
        }
        return (true, v);
    }

    AnObject DoSomethingObject()
    {
        var v = _random.Next(1);

        if (v == 0)
        {
            return new AnObject { IsSuccess = false };
        }
        return new AnObject { IsSuccess = true, Value = v };
    }

    ARecord DoSomethingRecord()
    {
        var v = _random.Next(1);

        if (v == 0)
        {
            return new ARecord(0, false);
        }
        return new ARecord(v, true);
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
            return Failure.SingleInstance;
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
            return Failure.SingleInstance;
        }
        return v;
    }
}

public record Failure()
{
    public static readonly Failure SingleInstance = new();
}

[GenerateOneOf]
public partial class ReturnType : OneOfBase<int, Failure> {}

public record AReturn
{
    public static AError AnError = new AError();
    
    public record AReturnValue(int Value) : AReturn;
    public record AError : AReturn;
}

public class AnObject
{
    public int Value { get; set; }
    public bool IsSuccess { get; set; }
}

public record ARecord(int Value, bool IsSuccess);
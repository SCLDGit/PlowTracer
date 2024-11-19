using System;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace PlowTracer.Core.DataStructures.Math.Primitives;

internal readonly struct Interval<T>(T c_minimum, T c_maximum) where T : struct, INumber<T>
{
    public T Minimum { get; } = c_minimum < c_maximum ? c_minimum : throw new ArgumentException("Minimum cannot be greater than maximum.", nameof(c_minimum));
    public T Maximum { get; } = c_maximum > c_minimum ? c_maximum : throw new ArgumentException("Maximum cannot be less than minimum.", nameof(c_maximum));
    
    public T Size => Maximum - Minimum;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool ContainsInclusive(T p_value)
    {
        return Minimum <= p_value &&
               Maximum >= p_value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool ContainsExclusive(T p_value)
    {
        return Minimum < p_value &&
               Maximum > p_value;
    }
}
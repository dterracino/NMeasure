﻿using System;
using System.Diagnostics;

namespace NMeasure
{
    [DebuggerDisplay("{Value} {Unit.stringRepresentation()}")]
    public struct Measure
    {
        public readonly double Value;
        public readonly Unit Unit;

        public Measure(double value) : this(value, new Unit())
        {
        }

        public Measure(double value, Unit unit)
        {
            Value = Math.Round(value, UnitConfiguration.UnitSystem.Precision, MidpointRounding.AwayFromZero);
            Unit = unit;
        }

        public Measure(double value, U unit) : this(value, Unit.From(unit))
        {
        }

        public bool IsDimensionless
        {
            get { return Unit.IsDimensionless; }
        }

        public static explicit operator Measure(double value)
        {
            return new Measure(value);
        }

        public static Measure operator *(Measure x, U singleUnit)
        {
            return new Measure(x.Value, x.Unit * singleUnit);
        }

        public static Measure operator *(Measure x, Unit unit)
        {
            return new Measure(x.Value, x.Unit * unit);
        }

        public static Measure operator *(Measure x, Measure y)
        {
            return new Measure(x.Value * y.Value, x.Unit * y.Unit);
        }

        public static Measure operator +(Measure x, Measure y)
        {
            if (!x.Unit.Equals(y.Unit))
                throw new InvalidOperationException("These measures cannot be sensibly added to a single new measure");
            return new Measure(x.Value + y.Value, x.Unit);
        }

        public static Measure operator -(Measure x, Measure y)
        {
            if (!x.Unit.Equals(y.Unit))
                throw new InvalidOperationException("These measures cannot be sensibly added to a single new measure");
            return new Measure(x.Value - y.Value, x.Unit);
        }

        public static Measure operator /(Measure x, Measure y)
        {
            return new Measure(x.Value / y.Value, x.Unit / y.Unit);
        }

        
    }
}
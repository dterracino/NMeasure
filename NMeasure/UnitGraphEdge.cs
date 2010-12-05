﻿using System;

namespace NMeasure
{
    /// <summary>
    /// A unit graph edge links two unnits and is hence a principal information about a conversion
    /// </summary>
    internal class UnitGraphEdge
    {
        private readonly Func<double, double> fromToTo;
        private readonly Func<Measure, Measure> fromToToMeasures;
        private readonly Unit to;
        private readonly Unit from;
        private readonly Unit unitOperator;
        private readonly bool measureBasedConversion;

        public UnitGraphEdge(Unit from, Func<double, double> fromToTo, Unit to)
        {
            this.fromToTo = fromToTo;
            this.to = to;
            this.from = from;
            unitOperator = to/from;
            measureBasedConversion = false;
        }

        public UnitGraphEdge(Unit from, Func<Measure, Measure> fromToTo, Unit to)
        {
            fromToToMeasures = fromToTo;
            this.to = to;
            this.from = from;
            measureBasedConversion = true;
        }

        public Unit From
        {
            get { return from; }
        }

        public Unit To
        {
            get { return to; }
        }

        public Measure ApplyConversion(Measure m)
        {
            if (measureBasedConversion)
            {
                var result = fromToToMeasures(m);
                return new Measure(result.Value, result.Unit.TryCompaction());
            }
            return new Measure(fromToTo(m.Value), m.Unit * unitOperator);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (UnitGraphEdge)) return false;
            return Equals((UnitGraphEdge) obj);
        }

        public bool Equals(UnitGraphEdge other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.to, to) && Equals(other.from, from);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (to.GetHashCode()*397) ^ from.GetHashCode();
            }
        }

        public override string ToString()
        {
            return string.Format("From {0} to {1}", from, to);
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace NMeasure
{
    // ReSharper disable InconsistentNaming
    /// <summary>
    /// All units known to the system and used by the <see cref="StandardUnitConfiguration"/>
    /// </summary>
    public static class U
    {
        #region Unit Implementations

        private class FundamentalUnit : Unit, IComparable
        {
            private readonly string _id;

            public FundamentalUnit(string id) { _id = id; }
            public override bool IsFundamental { get { return true; } }
            public override bool IsDimensionless { get { return false; } }
            public override bool Equals(Unit other) { return ReferenceEquals(this, other); }
            public override int  GetHashCode() { return _id.GetHashCode(); }
            public override string ToString() { return _id; }

            int IComparable.CompareTo(object obj)
            {
                if (!(obj is FundamentalUnit)) return 0;
                return _id.CompareTo(((FundamentalUnit)obj)._id);
            }
        }

        private class RootUnit : Unit, IComparable
        {
            private readonly string _id;
            public RootUnit(string id) { _id = id; }
            public override bool Equals(Unit obj) { return obj.ToString().Equals(ToString()); }
            public override int GetHashCode() { return _id.GetHashCode(); }
            public override bool IsDimensionless { get { return false; } }
            public override bool IsFundamental { get { return false; } }
            public override string ToString() { return _id; }

            int IComparable.CompareTo(object obj)
            {
                if (!(obj is RootUnit)) return 0;
                return _id.CompareTo(((RootUnit)obj)._id);
            }
        }

        private class AnyUnit : Unit
        {
            public AnyUnit() { }
            public AnyUnit(IEnumerable<Unit> numerators, IEnumerable<Unit> denominators) : base(numerators, denominators) {}
        }

        #endregion

        public static Unit GetRootUnit(string unit)
        {
            return new AnyUnit(new [] { new RootUnit(unit) }, new Unit[0]);
        }

        public static readonly Unit Dimensionless = new AnyUnit();

        // ------- LENGTH --------
        public static readonly Unit _LENGTH = new FundamentalUnit("[LENGTH]");
        public static readonly Unit Kilometer = GetRootUnit("km");
        public static readonly Unit Meter = GetRootUnit("m");
        public static readonly Unit Centimeter = GetRootUnit("cm");
        public static readonly Unit Millimeter = GetRootUnit("mm");
        public static readonly Unit Nanometer = GetRootUnit("nm");
        public static readonly Unit Micrometer = GetRootUnit("µ");

        public static readonly Unit Mile = GetRootUnit("mi");
        public static readonly Unit Yard = GetRootUnit("yd");
        public static readonly Unit Foot = GetRootUnit("ft");
        public static readonly Unit Inch = GetRootUnit("in");
        public static readonly Unit Microinch = GetRootUnit("µin");
        
        public static readonly Unit SquareMeter = GetRootUnit("m²");
        public static readonly Unit Hectare = GetRootUnit("ha");
        // ------- MASS --------
        public static readonly Unit _MASS = new FundamentalUnit("[MASS]");
        public static readonly Unit Ton = GetRootUnit("t");
        public static readonly Unit Kilogram = GetRootUnit("kg");
        public static readonly Unit Gram = GetRootUnit("g");
        public static readonly Unit Milligram = GetRootUnit("mg");
        public static readonly Unit Carat = GetRootUnit("kt");
        public static readonly Unit Ounce = GetRootUnit("oz");
        public static readonly Unit Pound = GetRootUnit("lb");
        // ------- TIME --------
        public static readonly Unit _TIME = new FundamentalUnit("[TIME]");
        public static readonly Unit Day = GetRootUnit("d");
        public static readonly Unit Hour = GetRootUnit("h");
        public static readonly Unit Minute = GetRootUnit("min");
        public static readonly Unit Second = GetRootUnit("sec");
        public static readonly Unit Millisecond = GetRootUnit("ms");
        public static readonly Unit Microsecond = GetRootUnit("µs");
        public static readonly Unit Nanosecond = GetRootUnit("ns");
        // ------- PRESSURE --------
        public static readonly Unit Pascal = GetRootUnit("Pa");
        public static readonly Unit Psi = GetRootUnit("psi");
        public static readonly Unit Bar = GetRootUnit("bar");

        // ------- TEMPERATURE --------
        public static readonly Unit _TEMPERATURE = new FundamentalUnit("[TEMPERATURE]");
        public static readonly Unit Kelvin = GetRootUnit("K");
        public static readonly Unit Celsius = GetRootUnit("°C");
        public static readonly Unit Fahrenheit = GetRootUnit("°F");

        public static readonly Unit Joule = GetRootUnit("J");
        public static readonly Unit Newton = GetRootUnit("N");


        static U()
        {
            // This is a self-check, since the functionality heavily depends on all units being different and there is an off-chance that
            // a Hashcode is not unique (especially when somebody copy-pastes a Unit)
            var allUnits = typeof(U)
                .GetFields(BindingFlags.Static | BindingFlags.Public)
                .Where(fi => typeof(Unit).IsAssignableFrom(fi.FieldType))
                .Select(fi => fi.GetValue(null))
                .OfType<Unit>().ToList();

            var distinctUnitsCount = allUnits.Distinct(new UnitEqualityComparer()).Count();

            if (allUnits.Count != distinctUnitsCount)
                throw new InvalidOperationException("The known units are not unique in themselves!");

        }
    }
    // ReSharper restore InconsistentNaming

    
}
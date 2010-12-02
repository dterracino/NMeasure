﻿using System;
using System.Linq;

namespace NMeasure
{
    public static class UnitInfo
    {

        private static UnitConfiguration getConfig()
        {
            return UnitConfiguration.UnitSystem;
        }

        public static UnitMeta GetUnitData(this U unit)
        {
            return getConfig()[Unit.From(unit)];
        }

        public static UnitMeta GetUnitData(this Unit unit)
        {
            return getConfig()[unit];
        }

        public static Unit ToPhysicalUnit(this Unit unit)
        {
            var expandedUnit = unit.Expand();

            try
            {
                var numerators = from u in expandedUnit.Numerators
                                 select GetUnitData(u).PhysicalUnit;
                var denominators = from u in expandedUnit.Denominators
                                   select GetUnitData(u).PhysicalUnit;
                return Unit.From(numerators, denominators);
            }
            catch (NullReferenceException x)
            {
                throw new InvalidOperationException("No metadata could be derived for unit " + unit + ". Have you forgotten to run a configuration?", x);
            }
        }

        public static Measure ConvertTo(this Measure measure, Unit unit)
        {
            return UnitConfiguration.UnitSystem.UnitGraph.Convert(measure, unit);
        }

        public static Measure ConvertTo(this Measure measure, U unit)
        {
            return measure.ConvertTo(Unit.From(unit));
        }
    }
}
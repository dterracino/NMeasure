﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace NMeasure
{
    internal class ConversionTracker
    {
        private readonly UnitGraph unitGraph;

        public ConversionTracker(UnitGraph unitGraph)
        {
            this.unitGraph = unitGraph;
        }

        public IConversion FindConversionSequence(Unit unit, Unit target)
        {
            if (unitGraph[unit] == null)
                return decomposeAndSearch(unit, target);
            return unitGraph[unit].Conversions.SelectMany(e=> findConversionSequence(new UnitGraphEdgeSequence { e }, target)).FirstOrDefault();
        }

        private IConversion decomposeAndSearch(Unit unit, Unit target)
        {
            var exStart = unit.Expand();
            var exEnd = target.Expand();

            var matches = matchViaPhysicalUnits(exStart, exEnd);

            var numeratorconverters = matches.NumeratorPairs.Select(pair => FindConversionSequence(pair.Item1.Unit(), pair.Item2.Unit()));
            // For denominators we search in the opposite way to give the inverse conversion, as they are divisors
            var denominatorConverters = matches.DenominatorPairs.Select(pair => FindConversionSequence(pair.Item2.Unit(), pair.Item1.Unit()));
            return new ComplexConversion(numeratorconverters, denominatorConverters);
        }

        private static MatchStructure matchViaPhysicalUnits(ExpandedUnit exStart, ExpandedUnit exEnd)
        {
            var ms = new MatchStructure();
            var mutatingList = new List<U>(exEnd.Numerators);
            foreach (var u in exStart.Numerators)
            {
                var u2 = mutatingList.First(unit => unit.ToPhysicalUnit() == u.ToPhysicalUnit());
                ms.AddNumeratorPair(u, u2);
                mutatingList.Remove(u2);
            }

            mutatingList = new List<U>(exEnd.Denominators);

            foreach (var u in exStart.Denominators)
            {
                var u2 = mutatingList.First(unit => unit.ToPhysicalUnit() == u.ToPhysicalUnit());
                ms.AddDenominatorPair(u, u2);
                mutatingList.Remove(u2);
            }
            return ms;
        }

        private IEnumerable<UnitGraphEdgeSequence> findConversionSequence(UnitGraphEdgeSequence currentRoute, Unit target)
        {
            var sequences = new List<UnitGraphEdgeSequence>();
            var to = currentRoute.Last.To;
            if (to.Equals(target))
            {
                sequences.Add(currentRoute);
                return sequences;
            }

            foreach (var c in unitGraph[to].Conversions)
            {
                if (currentRoute.References(c))
                    continue;
                var newRoute = currentRoute.Clone();
                newRoute.Add(c);
                var innerSequences = findConversionSequence(newRoute, target);
                sequences.AddRange(innerSequences.Where(seq=>seq.Last.To.Equals(target)));
            }
            return sequences;
        }

        private class MatchStructure
        {
            private readonly List<Tuple<U,U>> numerators = new List<Tuple<U, U>>();
            private readonly List<Tuple<U, U>> denominators = new List<Tuple<U, U>>();


            public void AddNumeratorPair(U startUnit, U endUnit)
            {
                numerators.Add(Tuple.Create(startUnit,endUnit));
            }

            public void AddDenominatorPair(U startUnit, U endUnit)
            {
                denominators.Add(Tuple.Create(startUnit, endUnit));
            }

            public IEnumerable<Tuple<U,U>> NumeratorPairs
            {
                get { return numerators; }
            }

            public IEnumerable<Tuple<U, U>> DenominatorPairs
            {
                get { return denominators; }
            }
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace NMeasure.Converting
{
    internal class UnitGraphNode
    {
        private readonly Unit unit;
        private readonly Dictionary<UnitGraphNode, UnitGraphEdge> targets = new Dictionary<UnitGraphNode, UnitGraphEdge>();

        public UnitGraphNode(Unit unit)
        {
            this.unit = unit;
        }

        public Unit Unit
        {
            get { return unit; }
        }

        public IList<UnitGraphEdge> Conversions
        {
            get { return targets.Values.ToList(); }
        }

        public UnitGraphEdge AddConversion(UnitGraphNode to, Func<decimal,decimal> edgeToTarget)
        {
            UnitGraphEdge unitGraphEdge;
            var success = targets.TryGetValue(to, out unitGraphEdge);
            if (!success)
                targets.Add(to, unitGraphEdge = new UnitGraphEdge(unit, edgeToTarget, to.Unit));
            return unitGraphEdge;
        }

        public UnitGraphEdge AddConversion(UnitGraphNode to, Func<Measure, Measure> edgeToTarget)
        {
            UnitGraphEdge unitGraphEdge;
            var success = targets.TryGetValue(to, out unitGraphEdge);
            if (!success)
                targets.Add(to, unitGraphEdge = new UnitGraphEdge(unit, edgeToTarget, to.Unit));
            return unitGraphEdge;
        }
    }
}
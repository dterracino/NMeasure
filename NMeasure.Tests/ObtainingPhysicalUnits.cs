using Xunit;

namespace NMeasure.Tests
{

    public class ObtainingPhysicalUnits
    {

        public ObtainingPhysicalUnits()
        {
            AdHocConfig.Use(c =>
            {
                c.Unit(U.Joule).IsPhysicalUnit((U._MASS * U._LENGTH.Squared()) / U._TIME.Squared());
                c.Unit(U.Meter).IsPhysicalUnit(U._LENGTH);
                c.Unit(U.Second).IsPhysicalUnit(U._TIME);
            });
        }

        [Fact]
        public void DirectDerivation()
        {
            U.Joule.ToPhysicalUnit().IsEqualTo((U._MASS * U._LENGTH.Squared()) / U._TIME.Squared());
        }

        [Fact]
        public void DerivationOfCombinedUnitsDivision()
        {
            var u = U.Meter / U.Second;
            u.ToPhysicalUnit().IsEqualTo(U._LENGTH / U._TIME);
        }

        [Fact]
        public void DerivationOfCombinedUnitsMultiplication()
        {
            var u = U.Meter * U.Meter * U.Second;
            u.ToPhysicalUnit().IsEqualTo(U._LENGTH * U._LENGTH * U._TIME);
        }

    }
}
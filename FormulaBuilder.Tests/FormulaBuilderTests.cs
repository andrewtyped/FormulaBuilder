using FormulaBuilder.Core.Domain;
using FormulaBuilder.Core.Domain.Model;
using FormulaBuilder.Core.Domain.Model.Nodes;
using FormulaBuilder.Core.Models;
using FormulaBuilder.Tests.SqlLite;
using MiscUtil;
using NHibernate;
using NHibernate.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FormulaBuilder.Core.Domain.ExecutableFormulaBuilder;

namespace FormulaBuilder.Tests
{
    [TestFixture]
    public class FormulaBuilderTests : BaseUnitTest
    {

        [Test]
        public void Can_Build_Triple_Sum_Formula()
        {
            var entity = GetTripleSumFormulaEntity();
            var formula = BuildTripleSumFormula(entity);

            Assert.AreEqual(entity.Id, formula.Id, "Id");
            Assert.AreEqual(entity.Name, formula.Name, "Name");
            Assert.AreEqual(entity.RootNode.Id, formula.RootNode.Id, "Root Node Id");
            Assert.AreEqual(entity.RootNode.Children.Count, formula.RootNode.Children.Count(), "Child Node Count");
            Assert.AreEqual(3, formula.RequiredParameters.Count);
            Assert.That(formula.RequiredParameters.Contains("Param1"), "Param1");
            Assert.That(formula.RequiredParameters.Contains("Param2"), "Param2");
            Assert.That(formula.RequiredParameters.Contains("Param3"), "Param3");
        }

        [Test]
        public void Can_Execute_Triple_Sum_Formula()
        {
            var entity = GetTripleSumFormulaEntity();
            var formula = BuildTripleSumFormula(entity);

            var executable = Executable()
                .Formula(formula)
                .ReturnsDecimal()
                .WithParameter("Param1", 10.00m)
                .WithParameter("Param2", 20.00m)
                .WithParameter("Param3", 30.00m)
               .Build();

            decimal result = executable.Execute();
            Assert.AreEqual(60.00m, result);
        }

        private Formula BuildTripleSumFormula(FormulaEntity entity)
        {
            var formula = Core.Domain.FormulaBuilder.Initialize()
               .WithId(entity.Id)
               .WithName(entity.Name)
               .WithRootNode()
                    .Operation("+")
                    .WithId(entity.RootNode.Id)
                    .WithChild()
                        .Operation("+")
                        .WithChild()
                            .Parameter("Param1")
                        .EndNode()
                        .WithChild()
                            .Parameter("Param2")
                        .EndNode()
                    .EndNode()
                    .WithChild()
                        .Parameter("Param3")
                    .EndNode()
                .EndNode()
               .EndNodes()
               .Build();


            return formula;
        }

        [Test]
        public void Can_Build_General_Gravity_Formula()
        {
            var entity = GetGeneralGravityFormulaEntity();
            var formula = BuildGeneralGravityFormula(entity);

            Assert.AreEqual(entity.RootNode.Children.Count, formula.RootNode.Children.Count());
            Assert.AreEqual(4, formula.RequiredParameters.Count);
            Assert.That(formula.RequiredParameters.Contains("G"));
            Assert.That(formula.RequiredParameters.Contains("m1"));
            Assert.That(formula.RequiredParameters.Contains("m2"));
            Assert.That(formula.RequiredParameters.Contains("d"));
        }

        [Test]
        public void Can_Execute_General_Gravity_Formula()
        {
            var entity = GetGeneralGravityFormulaEntity();
            var formula = BuildGeneralGravityFormula(entity);

            var executable = Executable()
                .Formula(formula)
                .ReturnsDecimal()
                .WithParameter("G", 6.67408m * .0000000001m)
                .WithParameter("m1", 150000)
                .WithParameter("m2", 650000.5m)
                .WithParameter("d", 450.345m)
                .Build();

            var expectedResult = (6.67408m * .0000000001m * 150000 * 650000.5m) / (450.345m * 450.345m); 
            var result = executable.Execute();
            Assert.That(expectedResult == result);
        }

        private Formula BuildGeneralGravityFormula(FormulaEntity entity)
        {
            var formula = Core.Domain.FormulaBuilder.Initialize()
                .WithId(0)
                .WithName("General Gravitational Force")
                .WithRootNode()
                    .Operation("/")
                    .WithChild()
                        .Operation("*")
                        .WithChild()
                            .Parameter("G")
                        .EndNode()
                        .WithChild()
                            .Parameter("m1")
                        .EndNode()
                        .WithChild()
                            .Parameter("m2")
                        .EndNode()
                    .EndNode()
                    .WithChild()
                        .Operation("*")
                        .WithChild()
                            .Parameter("d")
                        .EndNode()
                        .WithChild()
                            .Parameter("d")
                        .EndNode()
                    .EndNode()
                .EndNodes()
                .Build();

            return formula;
        }

        [Test]
        public void Build_Crazy_Nested_Formula()
        {
            var formula = Core.Domain.FormulaBuilder.Initialize()
                .WithId(0)
                .WithName("Crazy")
                .WithRootNode()
                    .Operation("*")
                    .WithChild()
                        .NestedFormula("F")
                    .EndNode()
                    .WithChild()
                        .Parameter("Param1")
                    .EndNode()
                .EndNode()
                .EndNodes()
                .WithNestedFormula()
                    .WithId(0)
                    .WithName("F")
                    .WithRootNode()
                        .Operation("*")
                        .WithChild()
                            .Parameter("m")
                        .EndNode()
                        .WithChild()
                            .Parameter("a")
                        .EndNode()
                    .EndNode()
                    .EndNodes()
                .EndNestedFormula()
                .EndNestedFormulas()
                .Build();

            var executable = Executable()
                .Formula(formula)
                .ReturnsDecimal()
                .WithParameter("Param1", 10.00m)
                .WithParameter("m", 20.00m)
                .WithParameter("a", 30.00m)
                .Build();

            var result = executable.Execute();
            Assert.That(result == 6000.00m);
        }
    }
}

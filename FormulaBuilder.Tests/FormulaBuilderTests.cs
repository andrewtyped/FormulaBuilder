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

namespace FormulaBuilder.Tests
{
    [TestFixture]
    public class FormulaBuilderTests : BaseUnitTest
    {
        [Test]
        public void Can_Build_Parameter()
        {
            var parameter = ParameterBuilder.Initialize()
                .WithName("foo")
                .WithValue<decimal>(10.00m)
                .Build();

            Assert.AreEqual(typeof(Parameter<decimal>), parameter.GetType());
            Assert.AreEqual("foo", parameter.Name);
            Assert.AreEqual(10.00m, parameter.GetValue());
        }

        [Test]
        public void Can_Build_Triple_Sum_Formula()
        {
            var entity = GetTripleSumFormulaEntity();
            var formula = BuildTripleSumFormula(entity);

            Assert.AreEqual(entity.Id, formula.Id);
            Assert.AreEqual(entity.Name, formula.Name);
            Assert.AreEqual(entity.RootNode.Id, formula.RootNode.Id);
            Assert.AreEqual(entity.RootNode.Children.Count, formula.RootNode.Children.Count());
            Assert.AreEqual(3, formula.Parameters.Count);
            Assert.That(formula.Parameters.ContainsKey("Param1"));
            Assert.That(formula.Parameters.ContainsKey("Param2"));
            Assert.That(formula.Parameters.ContainsKey("Param3"));
        }

        [Test]
        public void Can_Execute_Triple_Sum_Formula()
        {
            var entity = GetTripleSumFormulaEntity();
            var formula = BuildTripleSumFormula(entity);

            var result = formula.Execute();
            Assert.AreEqual(60.00m, result);
        }

        private ExecutableFormula<decimal> BuildTripleSumFormula(FormulaEntity entity)
        {
            var formula = ExecutableFormulaBuilder.Initialize()
               .WithId(entity.Id)
               .WithName(entity.Name)
               .WithRootNode(Node.Create(entity.RootNode))
               .Build<decimal>()
               .WithParameter(ParameterBuilder.Initialize()
                   .WithName("Param1")
                   .WithValue(10.00m)
                   .Build())
               .WithParameter(ParameterBuilder.Initialize()
                   .WithName("Param2")
                   .WithValue(20.00f)
                   .Build())
               .WithParameter(ParameterBuilder.Initialize()
                   .WithName("Param3")
                   .WithValue(30.00m)
                   .Build())
               .AndNoMoreParameters()
               as ExecutableFormula<decimal>;

            return formula;
        }

        [Test]
        public void Can_Build_General_Gravity_Formula()
        {
            var entity = GetGeneralGravityFormulaEntity();
            var formula = BuildGeneralGravityFormula(entity);

            Assert.AreEqual(entity.RootNode.Children.Count, formula.RootNode.Children.Count());
            Assert.AreEqual(4, formula.Parameters.Count);
            Assert.That(formula.Parameters.ContainsKey("G"));
            Assert.That(formula.Parameters.ContainsKey("m1"));
            Assert.That(formula.Parameters.ContainsKey("m2"));
            Assert.That(formula.Parameters.ContainsKey("d"));
        }

        [Test]
        public void Can_Execute_General_Gravity_Formula()
        {
            var entity = GetGeneralGravityFormulaEntity();
            var formula = BuildGeneralGravityFormula(entity);

            var expectedResult = (6.67408m * .0000000001m * 150000 * 650000.5m) / (450.345m * 450.345m); 
            var result = formula.Execute();
            Assert.That(expectedResult == result);
        }

        private ExecutableFormula<decimal> BuildGeneralGravityFormula(FormulaEntity entity)
        {
            var formula = ExecutableFormulaBuilder.Initialize()
                .WithId(entity.Id)
                .WithName(entity.Name)
                .WithRootNode(Node.Create(entity.RootNode))
                .Build<decimal>()
                .WithParameter(new Parameter<decimal>("G", 6.67408m * .0000000001m))
                .WithParameter(new Parameter<int>("m1",150000))
                .WithParameter(new Parameter<decimal>("m2",650000.5m))
                .WithParameter(new Parameter<decimal>("d",450.345m))
                .AndNoMoreParameters()
                as ExecutableFormula<decimal>;

            return formula;
        }
    }
}

using FormulaBuilder.Core.Domain;
using FormulaBuilder.Core.Domain.Model;
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
        public void Can_Execute_Triple_Sum_Formula()
        {
            var tripleSumFormula = GetTripleSumFormula();

            var parser = new FormulaParser(tripleSumFormula);
            var executable = parser.Parse(tripleSumFormula);

            var tokens = new Dictionary<string, decimal>()
            {
                {"Param1", 1.00m },
                {"Param2", 2.00m },
                {"Param3", 3.00m }
            };

            var result = executable.Execute(tokens);
            Assert.That(result == 6.00m);
            
        }

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
        public void Can_Build_Formula()
        {
            var entity = GetTripleSumFormulaEntity();

            var formula = FormulaBuilderImpl.Initialize()
                .WithId(entity.Id)
                .WithName(entity.Name)
                .WithRootNode(entity.RootNode)
                .WithParameter(ParameterBuilder.Initialize()
                    .WithName("Param1")
                    .WithValue(10.00m)
                    .Build())
                .WithParameter(ParameterBuilder.Initialize()
                    .WithName("Param2")
                    .WithValue(20.00m)
                    .Build())
                .WithParameter(ParameterBuilder.Initialize()
                    .WithName("Param3")
                    .WithValue(30.00m)
                    .Build())
                .AndNoMoreParameters()
                .WithReturnType<decimal>()
                .Build();

            Assert.AreEqual(entity.Id, formula.Id);
            Assert.AreEqual(entity.Name, formula.Name);
            Assert.AreEqual(entity.RootNode.Id, formula.RootNode.Id);
            Assert.AreEqual(entity.RootNode.Children.Count, formula.RootNode.Children.Count());
            Assert.AreEqual(3, formula.Parameters.Count);
            Assert.That(formula.Parameters.ContainsKey("Param1"));
            Assert.That(formula.Parameters.ContainsKey("Param2"));
            Assert.That(formula.Parameters.ContainsKey("Param3"));
            Assert.AreEqual(typeof(decimal), formula.ReturnType);

            var output = formula.Execute();
            Assert.AreEqual(60.00m, (decimal)output);
        }
    }
}

using FormulaBuilder.Core.Domain.Model;
using FormulaBuilder.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Tests
{
    [TestFixture]
    [Category("unit")]
    [Category("domain model")]
    class NodeTests
    {
        [Test]
        public void Can_Create_Node()
        {
            var childEntity = new NodeEntity(
                new NodeTypeEntity(1, "token"), 
                "test", 
                0,
                new List<NodeEntity>()
            );
            var parentEntity = new NodeEntity(
                new NodeTypeEntity(1, "operator"),
                "+",
                0,
                new List<NodeEntity>() { childEntity });
            var childNode = new Node(childEntity);
            var parentNode = new Node(parentEntity);
            var expectedNodeType = NodeType.Create(parentEntity.Type);

            Assert.AreEqual(expectedNodeType, parentNode.Type);
            Assert.AreEqual(parentEntity.Value, parentNode.Value);
            Assert.AreEqual(parentEntity.Position, parentNode.Position);
            Assert.AreEqual(parentEntity.Children.Count(), parentNode.Children.Count());
        }
    }
}

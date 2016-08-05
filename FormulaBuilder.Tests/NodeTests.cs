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
            /*
            var nodeEntity = new NodeEntity(
                new Formula("test"),
                null, 
                new NodeTypeEntity(1, "token"), 
                "test", 
                0
            );
            var childNodeEntity = new NodeEntity(
                new Formula("test"),
                nodeEntity,
                new NodeTypeEntity(1, "operator"),
                "+",
                0);
            var childNode = new Node(childNodeEntity, new List<Node>());
            var node = new Node(nodeEntity, new List<Node>() { childNode });
            var expectedNodeType = NodeType.Create(nodeEntity.Type);

            Assert.AreEqual(nodeEntity.Id, node.Id);
            Assert.AreEqual(expectedNodeType, node.Type);
            Assert.AreEqual(nodeEntity.Value, node.Value);
            Assert.AreEqual(nodeEntity.Position, node.Position);
            Assert.AreEqual(1, node.Children.Count());
            */
        }
    }
}

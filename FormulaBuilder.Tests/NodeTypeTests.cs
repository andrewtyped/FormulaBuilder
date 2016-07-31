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
    public class NodeTypeTests
    {
        [Test]
        public void Equal_Node_Types_Are_Equal()
        {
            Assert.That(NodeType.Operator.Equals(NodeType.Operator));
            Assert.That(NodeType.Token.Equals(NodeType.Token));
        }

        [Test]
        public void Unequal_Node_Types_Are_Not_Equal()
        {
            Assert.That(!NodeType.Operator.Equals(NodeType.Token));
        }

        [Test]
        public void Can_Create_NodeType_From_Entity()
        {
            var entity = new NodeTypeEntity(1, "operator");
            var nodeType = NodeType.Create(entity);
            Assert.That(nodeType.Equals(NodeType.Operator));

            entity = new NodeTypeEntity(2, "token");
            nodeType = NodeType.Create(entity);
            Assert.That(nodeType.Equals(NodeType.Token));
        }

        [Test]
        public void Cannot_Create_NodeType_From_Invalid_Entity()
        {
            try
            {
                var entity = new NodeTypeEntity(1, "invalid gibberish");
                var nodeType = NodeType.Create(entity);
                Assert.Fail("Created an invalid nodetype");
            }
            catch(InvalidOperationException)
            {
                Assert.Pass();
            }
        }
    }
}

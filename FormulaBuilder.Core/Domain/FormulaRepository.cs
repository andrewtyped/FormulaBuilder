using FormulaBuilder.Core.Domain.Model;
using FormulaBuilder.Core.Models;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain
{
    public class FormulaRepository
    {
        private readonly ISession _session;
        public FormulaRepository(ISession session)
        {
            _session = session;
        }

        public Formula GetById(int id)
        {
            var formulaEntity = _session.Get<FormulaEntity>(id);

            if (formulaEntity == null)
                throw new InvalidOperationException($"No formula with id {id} exists");

            return new Formula(formulaEntity);
        }

        public Formula GetByName(string name)
        {
            var formulaEntity = _session.QueryOver<FormulaEntity>()
                .Where(f => f.Name == name)
                .SingleOrDefault();

            if (formulaEntity == null)
                throw new InvalidOperationException($"No formula named [{name}] exists.");

            return new Formula(formulaEntity);
        }

        public IEnumerable<Formula> GetFormulas()
        {
            var formulas = _session.QueryOver<FormulaEntity>()
                .List()
                .Select(fe => new Formula(fe));

            return formulas;
        }

        public void Save(Formula formula)
        {
            if (formula == null)
                throw new ArgumentNullException(nameof(formula));

            var formulaEntity = new FormulaEntity(formula);

            using (var tx = _session.BeginTransaction())
            {
                SaveNode(formulaEntity.RootNode);
                _session.SaveOrUpdate(formulaEntity);
                tx.Commit();
            }
        }

        private void SaveNode(NodeEntity nodeEntity)
        {
            foreach (var child in nodeEntity.Children)
            {
                SaveNode(child);
            }

            _session.SaveOrUpdate(nodeEntity);
        }
    }
}

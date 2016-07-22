using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Models
{
    public class Formula
    {
        public virtual int Id { get; protected internal set; }
        public virtual string Name { get; protected internal set; }

        public virtual ICollection<FormulaLink> Links { get; protected internal set; }

        protected Formula()
        {
            Links = new List<FormulaLink>();
        }

        public Formula(string name, ICollection<FormulaLink> links)
        {
            Links = new List<FormulaLink>();

            if (name == null)
                throw new ArgumentNullException(nameof(name));
            if (links == null)
                throw new ArgumentNullException(nameof(links));

            Name = name;

            foreach (var link in links)
            {
                if (link == null)
                    throw new InvalidOperationException($"no member of {nameof(links)} may be null");

                link.Formula = this;
                Links.Add(link);
            }


        }

    }
}

﻿using FormulaBuilder.Core.Domain.Model.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormulaBuilder.Core.Domain.Model
{
    public interface IParameterSetter<T>
    {
        IParameterSetter<T> WithParameter(Parameter parameter);
        IExecutable<T> AndNoMoreParameters();
    }

    public interface IExecutable<T>
    {
        T Execute();
        IParameterSetter<T> ClearParameters();
    }

    public class ExecutableFormula<T> : Formula, IParameterSetter<T>, IExecutable<T>
    {
        public HashSet<string> RequiredParameters { get; }
        private readonly Dictionary<string, Parameter> _parameters = new Dictionary<string, Parameter>();
        public IReadOnlyDictionary<string, Parameter> Parameters { get { return _parameters; } }

        internal ExecutableFormula(
           int id,
           string name,
           Node rootNode)
            : base(id, name, rootNode)
        {
            RequiredParameters = RootNode.GatherParameters();
        }

        public IParameterSetter<T> WithParameter(Parameter parameter)
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));
            if (RequiredParameters.Contains(parameter.Name) == false)
                throw new InvalidOperationException($"Formula doesn't require a parameter named {parameter.Name}. Expected parameter names are {string.Join(", " + Environment.NewLine, RequiredParameters)}");

            _parameters.Add(parameter.Name, parameter);

            return this;
        }

        public IExecutable<T> AndNoMoreParameters()
        {
            if (HasMissingParameters())
                throw new InvalidOperationException($"The following required parameters are missing: {string.Join(", " + Environment.NewLine, GetMissingParameters())}");

            return this;
        }

        public IParameterSetter<T> ClearParameters()
        {
            _parameters.Clear();
            return this;
        }

        public T Execute()
        {
            if (HasMissingParameters())
                throw new InvalidOperationException($"The following required parameters are missing: {string.Join(", " + Environment.NewLine, GetMissingParameters())}");

            return RootNode.Resolve<T>(this);
        }

        private bool HasMissingParameters()
        {
            return GetMissingParameters().Count() > 0;
        }

        private IEnumerable<string> GetMissingParameters()
        {
            return RequiredParameters.Except(Parameters.Keys);
        }
    }
}
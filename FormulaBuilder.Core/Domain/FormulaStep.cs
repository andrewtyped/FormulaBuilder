namespace FormulaBuilder.Core.Domain
{
    public abstract class FormulaStep
    {
        public int ContextId { get; }

        protected FormulaStep(int contextId)
        {
            ContextId = contextId;
        }

        public abstract T Execute<T>(ExecutableFormula<T> formula);
    }
}

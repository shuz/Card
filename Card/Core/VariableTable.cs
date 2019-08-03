using System;
using System.Collections.Generic;
using System.Text;


namespace Card.Core
{
    public class VariableExpr : IExpression
    {
        private string name;
        private Rational value;

        public VariableExpr(string name)
        {
            this.name = name;
            this.value = null;
        }

        public Rational Eval()
        {
            return (Rational)value;
        }

        public Rational Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public string Expression
        {
            get
            {
                if (value != null)
                {
                    return value.ToString();
                }
                else
                {
                    return name;
                }
            }
        }

        public string Name
        {
            get { return name; }
        }
    }

    public class VariableTable : IEnumerable<VariableExpr>
    {
        private VariableExpr[] variables;
        private IDictionary<string, VariableExpr> variableNameTable;

        public VariableTable(string[] names)
        {
            this.variables = new VariableExpr[names.Length];
            this.variableNameTable = new Dictionary<string, VariableExpr>();

            for (int i = 0; i < names.Length; ++i)
            {
                variables[i] = new VariableExpr(names[i]);
                variableNameTable[names[i]] = variables[i];
            }
        }

        public VariableExpr[] Variables
        {
            get { return variables; }
        }

        public VariableExpr this[string varName]
        {
            get { return variableNameTable[varName]; }
        }

        public int Count
        {
            get { return variables.Length; }
        }


        #region IEnumerable<VariableExpr> Members

        public IEnumerator<VariableExpr> GetEnumerator()
        {
            foreach (VariableExpr expr in this.variables)
            {
                yield return expr;
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return variables.GetEnumerator();
        }

        #endregion
    }

}

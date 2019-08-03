using System;
using System.Collections.Generic;
using System.Text;

namespace Card.Core
{
    public interface IExpression
    {
        Rational Eval();
        string Expression { get; }
    }

    public class ConstantExpr : IExpression
    {
        private Rational value;

        public ConstantExpr(Rational value)
        {
            this.value = value;
        }

        public Rational Eval()
        {
            return value;
        }

        public string Expression
        {
            get { return value.ToString(); }
        }
    }
}

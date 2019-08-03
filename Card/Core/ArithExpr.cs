using System;
using System.Collections.Generic;
using System.Text;

namespace Card.Core
{
    public class ArithOperator
    {
        public delegate Rational Op(Rational v1, Rational v2);

        private Op op;
        private string opStr;
        private bool exchangable;
        private int precedence;
        private int subPrecedence;

        public ArithOperator(Op op, string opStr, bool exchangable, int precedence, int subPrecedence)
        {
            this.op = op;
            this.opStr = opStr;
            this.exchangable = exchangable;
            this.precedence = precedence;
            this.subPrecedence = subPrecedence;
        }

        public Rational Compute(Rational v1, Rational v2)
        {
            return op(v1, v2);
        }

        public string GetExprString(string expr1, string expr2)
        {
            return string.Format("({0} {1} {2})", expr1, opStr, expr2);
        }

        public string OpStr
        {
            get { return opStr; }
        }

        public bool Exchangable
        {
            get { return exchangable; }
        }

        public int Precedence
        {
            get { return precedence; }
        }

        public int SubPrecedence
        {
            get { return subPrecedence; }
        }

        public static readonly ArithOperator[] DefaultOperators = new ArithOperator[]
        {
            new ArithOperator(
                delegate(Rational v1, Rational v2) { return v1 + v2; },
                "+", true,  1, 1),
            new ArithOperator(
                delegate(Rational v1, Rational v2) { return v1 - v2; },
                "-", false, 1, 2),
            new ArithOperator(
                delegate(Rational v1, Rational v2) { return v1 * v2; },
                "*", true,  2, 1),
            new ArithOperator(
                delegate(Rational v1, Rational v2) { return v1 / v2; },
                "/", false, 2, 2),
        };

    }

    public class ArithExpr : IExpression
    {
        private IExpression left;
        private IExpression right;
        private ArithOperator op;

        public ArithExpr(ArithOperator op, IExpression left, IExpression right)
        {
            this.left = left;
            this.right = right;
            this.op = op;
        }

        public Rational Eval()
        {
            return op.Compute(left.Eval(), right.Eval());
        }

        public string Expression
        {
            get { return op.GetExprString(left.Expression, right.Expression); }
        }

        public IExpression Left
        {
            get { return left; }
        }

        public IExpression Right
        {
            get { return right; }
        }

        public ArithOperator Op
        {
            get { return op; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;


namespace Card.Core
{
    public class NormalizedExprFilter
    {
        public bool IsNormalized(IExpression expr)
        {
            if (expr is ArithExpr)
            {
                return IsNormalized(expr as ArithExpr);
            }

            return true;
        }

        public bool IsNormalized(ArithExpr expr)
        {
            if (expr.Right is ArithExpr)
            {
                ArithExpr right = expr.Right as ArithExpr;
                if (expr.Op.Precedence == right.Op.Precedence)
                {
                    return false;
                }
                if (IsNormalized(right) == false)
                {
                    return false;
                }
            }

            if (expr.Left is ArithExpr)
            {
                ArithExpr left = expr.Left as ArithExpr;
                if (expr.Op.Precedence == left.Op.Precedence)
                {
                    if (expr.Op.SubPrecedence < left.Op.SubPrecedence)
                    {
                        return false;
                    }

                    if (expr.Op.SubPrecedence == left.Op.SubPrecedence &&
                        Compare(left.Right, expr.Right) > 0)
                    {
                        return false;
                    }
                }

                if (IsNormalized(left) == false)
                {
                    return false;
                }
            }

            if (expr.Op.Exchangable &&
                Compare(expr.Left, expr.Right) > 0)
            {
                return false;
            }

            return true;
        }

        public int Compare(IExpression left, IExpression right)
        {
            if (left.GetType() != right.GetType())
            {
                return left.GetType().FullName.CompareTo(right.GetType().FullName);
            }

            if (left is ArithExpr)
            {
                return Compare(left as ArithExpr, right as ArithExpr);
            }

            if (left is ConstantExpr)
            {
                return Compare(left as ConstantExpr, right as ConstantExpr);
            }

            if (left is VariableExpr)
            {
                return Compare(left as VariableExpr, right as VariableExpr);
            }

            throw new InvalidOperationException();
        }

        public int Compare(ArithExpr left, ArithExpr right)
        {
            int value = left.Op.Precedence.CompareTo(right.Op.Precedence);
            if (value != 0)
            {
                return value;
            }

            value = left.Op.SubPrecedence.CompareTo(right.Op.SubPrecedence);
            if (value != 0)
            {
                return value;
            }

            value = Compare(left.Left, right.Left);
            if (value != 0)
            {
                return value;
            }

            value = Compare(left.Right, right.Right);
            return value;
        }

        public int Compare(ConstantExpr left, ConstantExpr right)
        {
            return left.Eval().CompareTo(right.Eval());
        }

        public int Compare(VariableExpr left, VariableExpr right)
        {
            return left.Name.CompareTo(right.Name);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Card.Core
{
    public class ExprPopulator
    {
        private ArithOperator[] operators;
        public delegate void ExprProcessor(ArithExpr expr);

        public ExprPopulator(ArithOperator[] operators)
        {
            this.operators = operators;
        }

        public int Populate(IExpression[] exprs, ExprProcessor processExpr)
        {
            int combinationCount = 0;
            this.PopulateAux(exprs, processExpr, ref combinationCount);
            return combinationCount;
        }

        private void PopulateAux(IExpression[] exprs, ExprProcessor processExpr, ref int combinationCount)
        {
            if (exprs.Length == 1)
            {
                ++combinationCount;
                processExpr(exprs[0] as ArithExpr);
            }
            else
            {
                for (int i = 0; i < exprs.Length; ++i)
                {
                    for (int j = i + 1; j < exprs.Length; ++j)
                    {
                        foreach (ArithOperator op in operators)
                        {
                            IExpression[] newexprs = new IExpression[exprs.Length - 1];
                            Array.Copy(exprs, newexprs, i);
                            Array.Copy(exprs, i + 1, newexprs, i, j - i - 1);
                            Array.Copy(exprs, j + 1, newexprs, j - 1, exprs.Length - j - 1);

                            newexprs[newexprs.Length - 1] = new ArithExpr(op, exprs[i], exprs[j]);
                            PopulateAux(newexprs, processExpr, ref combinationCount);

                            newexprs[newexprs.Length - 1] = new ArithExpr(op, exprs[j], exprs[i]);
                            PopulateAux(newexprs, processExpr, ref combinationCount);
                        }
                    }
                }
            }
        }
    }

}

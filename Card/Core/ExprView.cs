using System;
using System.Collections.Generic;
using System.Text;

namespace Card.Core
{
    public class ExprView
    {
        private ArithOperator[] operators;
        private ExprFilter filter;
        private IExpression[] initialExpressions;

        private List<ArithExpr> filteredExprs;

        public delegate bool ExprFilter(ArithExpr expr);

        public ExprView()
        { }

        public ExprView(
            ArithOperator[] operators,
            ExprFilter filter,
            IExpression[] initialExpressions)
        {
            this.operators = operators;
            this.filter = filter;
            this.initialExpressions = initialExpressions;
        }

        public ArithOperator[] Operators
        {
            get { return operators; }
            set { operators = value; }
        }

        public ExprFilter Filter
        {
            get { return filter; }
            set { filter = value; }
        }

        public IExpression[] InitialExpressions
        {
            get { return initialExpressions; }
            set { initialExpressions = value; }
        }

        public IList<ArithExpr> FilteredExprs
        {
            get { return filteredExprs.AsReadOnly(); }
        }

        public void FilterExpressions()
        {
            filteredExprs = new List<ArithExpr>();
            new ExprPopulator(this.operators).Populate(
                this.initialExpressions,
                delegate(ArithExpr expr)
                {
                    if (filter(expr))
                    {
                        this.filteredExprs.Add(expr);
                    }
                }
            );
        }
    }
}

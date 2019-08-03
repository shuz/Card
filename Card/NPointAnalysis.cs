using System;
using System.Collections.Generic;
using System.Text;

using Card.Core;
using System.Data;

namespace Card
{

    public class NPointsAnalysis
    {
        public delegate void CardCallback(int[] cards, int index, FinalExprDataSet data);

        private FinalExprDataSet data;

        private readonly int point;
        private readonly int cardCount;
        private readonly int cardMin;
        private readonly int cardMax;
        private readonly ExprView.ExprFilter filter;
        private CardCallback cardCallback;
        
        public NPointsAnalysis(
            int point, int cardCount, int cardMin, int cardMax, 
            ExprView.ExprFilter filter,
            CardCallback cardCallback)
        {
            this.point = point;
            this.cardCount = cardCount;
            this.cardMin = cardMin;
            this.cardMax = cardMax;
            this.filter = filter;
            this.cardCallback = cardCallback;

            this.data = new FinalExprDataSet();
        }

        private string FinalExpr(ArithExpr expr)
        {
            //if (expr.Left is ArithExpr)
            //{
            //    ArithExpr left = expr.Left as ArithExpr;
            //    if (left.Op.Precedence == expr.Op.Precedence)
            //    {
            //        return expr.Op.GetExprString(FinalExpr(left), expr.Right.Eval().ToString());
            //    }
            //}
            Rational left = expr.Left.Eval();
            Rational right = expr.Right.Eval();

            if (expr.Op.Exchangable && left.CompareTo(right) > 0)
            {
                Utility.Swap<Rational>(ref left, ref right);
            }

            return expr.Op.GetExprString(left.ToString(), right.ToString());
        }

        public void ComputeResults()
        {
            string[] str = new string[cardCount];
            for (int i = 0; i < cardCount; ++i)
            {
                str[i] = i.ToString();
            }

            VariableTable table = new VariableTable(str);
            ExprView view = new ExprView(
                ArithOperator.DefaultOperators,
                filter,
                table.Variables);

            view.FilterExpressions();

            IList<ArithExpr> filteredExprs = view.FilteredExprs;
            NormalizedExprFilter normalized = new NormalizedExprFilter();

            CardPopulator cardPop = new CardPopulator(cardCount, cardMin, cardMax);
            cardPop.Populate(
                delegate(int[] cards)
                {
                    this.cardCallback(cards, cardPop.CombinationCount, this.data);

                    string strCards = string.Empty;
                    for (int i = 0; i < cards.Length; ++i)
                    {
                        table.Variables[i].Value = cards[i];
                        strCards += cards[i].ToString() + " ";
                    }

                    FinalExprDataSet.CardSolutionsRow solCntRow = data.CardSolutions.AddCardSolutionsRow(
                        strCards, 0, 0
                    );

                    foreach (ArithExpr expr in filteredExprs)
                    {
                        Rational value = expr.Eval();
                        if (value == point)
                        {
                            string strExpression = expr.Expression;
                            string finalExpr = FinalExpr(expr);

                            //FinalExprDataSet.FinalExprCountRow countRow = data.FinalExprCount.FindByFinalExpr(finalExpr);
                            //if (countRow == null)
                            //{
                            //    countRow = data.FinalExprCount.AddFinalExprCountRow(finalExpr, 0);
                            //}

                            //FinalExprDataSet.DetailExprCountRow exprRow = data.DetailExprCount.FindByFinalExprCardCombination(
                            //    finalExpr, strCards
                            //);
                            
                            //if (exprRow == null)
                            //{
                            //    data.DetailExprCount.AddDetailExprCountRow(
                            //        finalExpr,
                            //        strCards,
                            //        strExpression
                            //    );
                            //    countRow.Count = countRow.Count + 1;
                            //}

                            if (data.SolutionDetail.FindByCardSolution(strCards, strExpression) == null)
                            {
                                solCntRow.SolutionCount = solCntRow.SolutionCount + 1;
                                data.SolutionDetail.AddSolutionDetailRow(
                                    strCards,
                                    strExpression,
                                    finalExpr
                                );
                            }
                        }
                    }
                }
            );
        }

        public FinalExprDataSet Result
        {
            get { return data; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Card.Core;
using System.Data.SqlClient;
using System.Configuration;
using Card.FinalExprDataSetTableAdapters;


namespace Card
{
    class Program
    {
        static void Main(string[] args)
        {
            NPointsAnalysis analysiser = new NPointsAnalysis(
                30,
                5,
                1,
                13,
                new NormalizedExprFilter().IsNormalized,
                delegate(int[] cards, int index, FinalExprDataSet data)
                {
                    if (index % 200 == 0)
                    {
                        Console.Write("#");

                        FinalExprCountTableAdapter finalExprAdapter = new FinalExprCountTableAdapter();
                        finalExprAdapter.Update(data.FinalExprCount);

                        DetailExprCountTableAdapter detailExprAdapter = new DetailExprCountTableAdapter();
                        detailExprAdapter.Update(data.DetailExprCount);

                        CardSolutionsTableAdapter cardExprAdapter = new CardSolutionsTableAdapter();
                        cardExprAdapter.Update(data.CardSolutions);

                        SolutionDetailTableAdapter solutionDetailAdapter = new SolutionDetailTableAdapter();
                        solutionDetailAdapter.Update(data.SolutionDetail);
                    }
                }
            );
            analysiser.ComputeResults();

            {
                FinalExprCountTableAdapter finalExprAdapter = new FinalExprCountTableAdapter();
                finalExprAdapter.Update(analysiser.Result.FinalExprCount);

                DetailExprCountTableAdapter detailExprAdapter = new DetailExprCountTableAdapter();
                detailExprAdapter.Update(analysiser.Result.DetailExprCount);

                CardSolutionsTableAdapter cardExprAdapter = new CardSolutionsTableAdapter();
                cardExprAdapter.Update(analysiser.Result.CardSolutions);

                SolutionDetailTableAdapter solutionDetailAdapter = new SolutionDetailTableAdapter();
                solutionDetailAdapter.Update(analysiser.Result.SolutionDetail);
            }
        }
    }
}

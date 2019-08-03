using System;
using System.Collections.Generic;
using System.Text;

namespace Card.Core
{
    public class CardPopulator
    {
        public delegate void CardProcessor(int[] cards);

        private readonly int cardCount;
        private readonly int cardMax;
        private readonly int cardMin;
        private int combinationCount;

        public CardPopulator(int cardCount, int cardMin, int cardMax)
        {
            this.cardCount = cardCount;
            this.cardMin = cardMin;
            this.cardMax = cardMax;
        }

        public void Populate(CardProcessor processor)
        {
            combinationCount = 0;
            int[] indices = new int[cardCount];
            for (int i = 0; i < indices.Length; ++i)
            {
                indices[i] = cardMin;
            }

            while (indices[0] <= cardMax)
            {
                ++combinationCount;

                processor(indices);

                int n = cardCount - 1;
                indices[n]++;
                while (indices[n] > cardMax && n > 0)
                {
                    --n;
                    indices[n]++;
                }

                for (int j = n; j < cardCount; ++j)
                {
                    indices[j] = indices[n];
                }
            }
        }

        public int CombinationCount
        {
            get { return combinationCount; }
        }
    }
}

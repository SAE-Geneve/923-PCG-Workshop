using UnityEngine;

namespace MarkovWithBudget
{

    public class MarkovLink
    {
        public MarkovLink(MarkovRoom room, int weight = 1)
        {
            _room = room;
            _weight = weight;
        }

        private MarkovRoom _room;
        private int _weight;

        public MarkovRoom Room => _room;
        public int Weight => _weight;
        
    }

}
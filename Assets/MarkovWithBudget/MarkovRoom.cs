using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MarkovWithBudget
{
    
    public class MarkovRoom
    {
        public enum RoomDifficulty
        {
            Easy = 1,
            Soft = 3,
            Hard = 5
        }

        private RoomDifficulty _difficulty;
        private string _name;
        private int _goldCount;

        public RoomDifficulty Difficulty => _difficulty;

        public MarkovRoom(RoomDifficulty diff, string name, int gold)
        {
            _difficulty = diff;
            _name = name;
            _goldCount = gold;
        }

        public override string ToString()
        {
            return "Room " + _name + ":" + _difficulty + " Gold=" + _goldCount;
        }

    }

}
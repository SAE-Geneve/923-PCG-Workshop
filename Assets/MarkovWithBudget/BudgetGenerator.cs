using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MarkovWithBudget
{
    public class BudgetGenerator : MonoBehaviour
    {

        [SerializeField] private int _difficultyBudget = 15;
        [SerializeField] private MarkovRoom.RoomDifficulty _startRoom;

        private List<KeyValuePair<MarkovRoom.RoomDifficulty, MarkovLink>> _markovRooms = new List<KeyValuePair<MarkovRoom.RoomDifficulty, MarkovLink>>();
        private List<MarkovRoom> _rooms = new List<MarkovRoom>();

        
        // Start is called before the first frame update
        void Start()
        {
            // Links
            MarkovRoom easyRoom = new MarkovRoom(MarkovRoom.RoomDifficulty.Easy, "Easy Peasy", 50);
            MarkovRoom softRoom = new MarkovRoom(MarkovRoom.RoomDifficulty.Soft, "Soft is'nt it ?", 50);
            MarkovRoom hardRoom = new MarkovRoom(MarkovRoom.RoomDifficulty.Hard, "Run !!!", 50);

            AddMarkovRoom(MarkovRoom.RoomDifficulty.Easy, softRoom, 3);
            AddMarkovRoom(MarkovRoom.RoomDifficulty.Easy, hardRoom);

            AddMarkovRoom(MarkovRoom.RoomDifficulty.Soft, easyRoom);

            AddMarkovRoom(MarkovRoom.RoomDifficulty.Hard, easyRoom, 5);
            AddMarkovRoom(MarkovRoom.RoomDifficulty.Hard, hardRoom);

        }

        private void AddMarkovRoom(MarkovRoom.RoomDifficulty difficulty, MarkovRoom room, int weight = 1)
        {
            // Key : Difficulty
            // Value : room + chance of appearance
            _markovRooms.Add(new KeyValuePair<MarkovRoom.RoomDifficulty, MarkovLink>(
                difficulty,
                new MarkovLink(room, weight))
            );
        }

        public void Generate()
        {

            MarkovRoom.RoomDifficulty _currentDiff = _startRoom;
            List<KeyValuePair<MarkovRoom.RoomDifficulty, MarkovLink>> _availableRooms = new List<KeyValuePair<MarkovRoom.RoomDifficulty, MarkovLink>>();

            _rooms.Clear();
            
            int generatedBudget = 0;

            do
            {
                _availableRooms = _markovRooms
                    .Where(n => n.Key == _currentDiff)
                    .OrderByDescending(n => n.Value.Weight)
                    .ToList();

                int sumWeights = _availableRooms.Sum(n => n.Value.Weight);

                if (_availableRooms.Count > 0)
                {
                    int idxElement = Random.Range(0, sumWeights);
                    int partialSum = 0;

                    foreach (var availableRoom in _availableRooms)
                    {
                        partialSum += availableRoom.Value.Weight;
                        if (idxElement < partialSum)
                        {
                            // J'ai mon vainqueur !!!!!!!!!!
                            _currentDiff = availableRoom.Value.Room.Difficulty;
                            _rooms.Add(availableRoom.Value.Room);
                            generatedBudget += (int)availableRoom.Value.Room.Difficulty;
                            break;
                        }
                    }
                }

                Debug.Log("Difficulty budget " + generatedBudget + "/" + _difficultyBudget);
                
                
            } while (generatedBudget < _difficultyBudget);

            // Print --------------------------------------------
            foreach (MarkovRoom room in _rooms)
            {
                Debug.Log(room);
            }
            
            
        }
    }

}

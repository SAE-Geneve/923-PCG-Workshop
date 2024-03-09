using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace NameGenerator
{

    public class MarkovNameGenerator : MonoBehaviour
    {

        [SerializeField] private Text _nameText;

        private List<KeyValuePair<string, MarkovLink>> _markovNames = new List<KeyValuePair<string, MarkovLink>>();
        private string _startName = "The";

        // Start is called before the first frame update
        void Start()
        {
            _markovNames.Add(new KeyValuePair<string, MarkovLink>("The", new MarkovLink("Lord")));
            _markovNames.Add(new KeyValuePair<string, MarkovLink>("The", new MarkovLink("Prince")));

            _markovNames.Add(new KeyValuePair<string, MarkovLink>("Prince", new MarkovLink("of")));

            _markovNames.Add(new KeyValuePair<string, MarkovLink>("Lord", new MarkovLink("of")));

            _markovNames.Add(new KeyValuePair<string, MarkovLink>("of", new MarkovLink("Silver", 2)));
            _markovNames.Add(new KeyValuePair<string, MarkovLink>("of", new MarkovLink("Golden", 2)));
            _markovNames.Add(new KeyValuePair<string, MarkovLink>("of", new MarkovLink("Blue")));
            _markovNames.Add(new KeyValuePair<string, MarkovLink>("of", new MarkovLink("Vanished", 5)));

            _markovNames.Add(new KeyValuePair<string, MarkovLink>("Silver", new MarkovLink("Forest")));
            _markovNames.Add(new KeyValuePair<string, MarkovLink>("Silver", new MarkovLink("Lagoon")));

            _markovNames.Add(new KeyValuePair<string, MarkovLink>("Golden", new MarkovLink("Forest")));
            _markovNames.Add(new KeyValuePair<string, MarkovLink>("Golden", new MarkovLink("Lagoon")));

            _markovNames.Add(new KeyValuePair<string, MarkovLink>("Blue", new MarkovLink("Lagoon")));
            _markovNames.Add(new KeyValuePair<string, MarkovLink>("Blue", new MarkovLink("Castle")));

            _markovNames.Add(new KeyValuePair<string, MarkovLink>("Vanished", new MarkovLink("Lagoon")));
            _markovNames.Add(new KeyValuePair<string, MarkovLink>("Vanished", new MarkovLink("Castle")));

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Generate()
        {

            string _currentName = _startName;
            List<KeyValuePair<string, MarkovLink>> _availableNames = new List<KeyValuePair<string, MarkovLink>>();

            _nameText.text = _currentName;

            do
            {
                _availableNames = _markovNames
                    .Where(n => n.Key == _currentName)
                    .OrderByDescending(n => n.Value.Weight)
                    .ToList();

                int sumWeights = _availableNames.Sum(n => n.Value.Weight);

                if (_availableNames.Count > 0)
                {
                    int idxElement = Random.Range(0, sumWeights);
                    int partialSum = 0;

                    foreach (var availableName in _availableNames)
                    {
                        partialSum += availableName.Value.Weight;
                        if (idxElement < partialSum)
                        {
                            // J'ai mon vainqueur !!!!!!!!!!
                            _currentName = availableName.Value.Text;
                            break;
                        }
                    }

                    _nameText.text += " " + _currentName;
                }

            } while (_availableNames.Count > 0);

        }
    }

}

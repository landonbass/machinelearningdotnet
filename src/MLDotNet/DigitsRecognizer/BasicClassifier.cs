using System;
using System.Collections.Generic;

namespace DigitsRecognizer
{
    public class BasicClassifier : IClassifier
    {
        private IEnumerable<Observation> _data;
        private readonly IDistance _distance;

        public BasicClassifier(IDistance distance)
        {
            _distance = distance;
        }
        public string Predict(int[] pixels)
        {
            Observation currentBest = null;
            var shortest = Double.MaxValue;

            foreach(var observation in _data)
            {
                var distance = _distance.Between(observation.Pixels, pixels);
                if (distance < shortest)
                {
                    shortest = distance;
                    currentBest = observation;
                }
            }

            return currentBest.Label;
        }

        public void Train(IEnumerable<Observation> trainingSet)
        {
            _data = trainingSet;
        }
    }
}

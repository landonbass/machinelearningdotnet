﻿using System.Collections.Generic;
using System.Linq;

namespace DigitsRecognizer
{
    public class Evaluator
    {
        public static double Correct(IEnumerable<Observation> validationSet, IClassifier classifier)
        {
            return validationSet.Select(observation => Score(observation, classifier))
                    .Average();
        }

        internal static double Score(Observation observation, IClassifier classifier)
        {
            return classifier.Predict(observation.Pixels) == observation.Label ? 1.0 : 0.0;
        }
    }
}

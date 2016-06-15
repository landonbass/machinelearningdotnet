
namespace DigitsRecognizer
{
    class Program
    {
        static void Main(string[] args)
        {
            const string trainingPath = @"..\..\..\Data\Ch1\trainingsample.csv";
            const string validationPath = @"..\..\..\Data\Ch1\validationsample.csv";

            var training = DataReader.ReadObservations(trainingPath);
            var validation = DataReader.ReadObservations(validationPath);

            var distance = new ManhattenDistance();
            var classifier = new BasicClassifier(distance);

            classifier.Train(training);
            var correct = Evaluator.Correct(validation, classifier);

            System.Console.WriteLine($"correctly classified: {correct * 100}%");

        }
    }
}

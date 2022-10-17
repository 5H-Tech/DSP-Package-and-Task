using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        public override void Run()
        {
            OutputNormalizedSignal = new Signal(new List<float>(), false);
            float oldMin = InputSignal.Samples.Min();
            float oldMax = InputSignal.Samples.Max();
            float oldRange = oldMax - oldMin;
            float newRange = InputMaxRange - InputMinRange;
            foreach (float sampl in InputSignal.Samples)
            {
                float res = (((sampl-oldMin) / oldRange) * newRange)+InputMinRange;
                OutputNormalizedSignal.Samples.Add(res);
            }
        }
    }
}

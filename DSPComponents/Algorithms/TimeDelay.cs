using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class TimeDelay:Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public float InputSamplingPeriod { get; set; }
        public float OutputTimeDelay { get; set; }

        public override void Run()
        {
            DirectConvolution d = new DirectConvolution();
            d.InputSignal1 = InputSignal1;
            d.InputSignal2 = InputSignal2;

            d.Run();

            float max_val = float.MinValue;
            int max_idx = 0;

            for (int i = 0; i < d.OutputConvolvedSignal.Samples.Count; i++)
            {
                if (d.OutputConvolvedSignal.Samples[i] >= max_val)
                {
                    max_val = d.OutputConvolvedSignal.Samples[i];
                    max_idx = i;
                }
            }

            OutputTimeDelay = max_idx * InputSamplingPeriod;
        }
    }
}

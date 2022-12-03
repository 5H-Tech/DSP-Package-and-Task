using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            int num = InputSignal.Samples.Count;
            OutputSignal = new Signal(new List<float>(), false);

            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                float sum = 0;
                for (int j = 0; j < InputSignal.Samples.Count; j++)
                {
                    sum += InputSignal.Samples[j] * (float)Math.Cos((((2 * j) + 1) * i * (float)Math.PI) / (2 * num));
                }
                if (i == 0)
                {
                    sum *= (float)Math.Sqrt(1.0 / num);
                }
                else
                {
                    sum *= (float)Math.Sqrt(2.0 / num);
                }
                OutputSignal.Samples.Add(sum);
            }
        }
    }
}

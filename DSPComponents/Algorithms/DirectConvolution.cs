using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            int start=0;
            if (InputSignal1.SamplesIndices.Count !=0 && InputSignal2.SamplesIndices.Count!=0)
            {
                start = InputSignal1.SamplesIndices[0] + InputSignal2.SamplesIndices[0];
            }
            OutputConvolvedSignal = new Signal(new List<float>(),new List<int>(),false);
            for (int i = 0; i < (InputSignal1.Samples.Count+InputSignal2.Samples.Count)-1; i++)
            {
                float sum = 0f;
                for (int j = 0; j < InputSignal2.Samples.Count; j++)
                {
                    if (i - j >= 0 && i - j < InputSignal1.Samples.Count)
                    {
                        sum += InputSignal1.Samples[i - j] * InputSignal2.Samples[j];
                    }

                }
                OutputConvolvedSignal.Samples.Add(sum);
                OutputConvolvedSignal.SamplesIndices.Add(start);
                start++;

            }
            for (int i = OutputConvolvedSignal.Samples.Count - 1; i >= 0; i--)
            {
                if (OutputConvolvedSignal.Samples[i] == 0)
                {
                    OutputConvolvedSignal.Samples.RemoveAt(i);
                    OutputConvolvedSignal.SamplesIndices.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            OutputSignal = new Signal(new List<float>(), false);

           
            int maxLen = int.MinValue;
            int maxIdx = 0;
            for (int i = 0; i < InputSignals.Count; i++)
            {
                if(InputSignals[i].Samples.Count>=maxLen)
                {
                    maxLen = InputSignals[i].Samples.Count;
                    OutputSignal = InputSignals[i];
                    maxIdx = i;
                }
            }

            for (int i = 0; i < InputSignals.Count; i++)
            {
                if (i == maxIdx)
                    continue;
                for (int j = 0; j < InputSignals[i].Samples.Count; j++)
                {
                    OutputSignal.Samples[j] += InputSignals[i].Samples[j];
                }
            }


            //for (int i = 0; i < InputSignals.Count; i++)
            //{
            //    if (OutputSignal.Samples.Count == 0)
            //    {
            //        foreach (float sample in InputSignals[i].Samples)
            //        {
            //            OutputSignal.Samples.Add(sample);
            //        }
            //    }
            //    else
            //    {
            //        for (int k = 0; k < Math.Min(OutputSignal.Samples.Count, InputSignals[i].Samples.Count); k++)
            //        {
            //            OutputSignal.Samples[k] += InputSignals[i].Samples[k];
            //        }
            //    }
            //}
        }
    }
}
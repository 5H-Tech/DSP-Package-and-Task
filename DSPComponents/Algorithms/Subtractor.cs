using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Subtractor : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputSignal { get; set; }

        /// <summary>
        /// To do: Subtract Signal2 from Signal1 
        /// i.e OutSig = Sig1 - Sig2 
        /// </summary>
        public override void Run()
        {
            /*OutputSignal = new Signal(new List<float>(), false);
            MultiplySignalByConstant multiplySignalByConstant = new MultiplySignalByConstant();
            Adder adder = new Adder();
            multiplySignalByConstant.InputSignal = InputSignal2;
            multiplySignalByConstant.InputConstant = -1;
            multiplySignalByConstant.Run();
            Adder a = new Adder();
            a.InputSignals = new List<Signal>();
            a.InputSignals.Add(InputSignal1);
            a.InputSignals.Add(multiplySignalByConstant.OutputMultipliedSignal);
            a.Run();
            foreach (float sample in a.OutputSignal.Samples)
            {
                OutputSignal.Samples.Add(sample);
            }*/
            OutputSignal = new Signal(new List<float>(), false);
            int size = Math.Max(InputSignal1.Samples.Count, InputSignal2.Samples.Count);
            for (int i = 0; i < size; i++)
            {
                if (InputSignal1.Samples.Count <= i)
                {
                    OutputSignal.Samples.Add(0 - InputSignal2.Samples[i]);
                }
                else if (InputSignal2.Samples.Count <= i)
                {
                    OutputSignal.Samples.Add(InputSignal1.Samples[i]);
                }
                else
                {
                    OutputSignal.Samples.Add(InputSignal1.Samples[i] - InputSignal2.Samples[i]);
                }

            }
        }
    }
}
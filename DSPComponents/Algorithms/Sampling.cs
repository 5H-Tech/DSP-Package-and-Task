using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }
        public Signal TestSignal { get; set; }
        public Signal FirSignal { get; set; }


        public override void Run()
        {
            FIR fir = new FIR();
            fir.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
            fir.InputFS = 8000;
            fir.InputStopBandAttenuation = 50;
            fir.InputCutOffFrequency = 1500;
            fir.InputTransitionBand = 500;

            OutputSignal = new Signal(new List<float>(), false);
            TestSignal = new Signal(new List<float>(), false);



           // up sample by L factor and then apply low pass filter
            if (M == 0 && L != 0)
            {
                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    OutputSignal.Samples.Add(InputSignal.Samples[i]);
                    for (int j = 0; j < L - 1; j++)
                    {
                        OutputSignal.Samples.Add(0);
                    }
                }
        
                fir.InputTimeDomainSignal = OutputSignal;
                fir.Run();
                OutputSignal = fir.OutputYn;

            }
            //apply filter first and thereafter down sample by M factor.
           else if (M != 0 && L == 0)
            {

                fir.InputTimeDomainSignal = InputSignal;
                fir.InputTimeDomainSignal.SamplesIndices = InputSignal.SamplesIndices;
                fir.Run();

                for (int i = 0; i < fir.OutputYn.Samples.Count; i += M)
                {
                    OutputSignal.Samples.Add(fir.OutputYn.Samples[i]);
                    OutputSignal.SamplesIndices.Add(i / M);
                }
            }
            //change sample rate by fraction. Thus, first up sample by L factor, apply low pass filter and then down sample by M factor.
            else if (M != 0 && L != 0)
            {

                for (int i = 0; i < InputSignal.Samples.Count; i++)
                {
                    TestSignal.Samples.Add(InputSignal.Samples[i]);
                    for (int j = 0; j < L - 1; j++)
                    {
                        TestSignal.Samples.Add(0);
                    }
                }

                fir.InputTimeDomainSignal = TestSignal;
                fir.Run();
                TestSignal = fir.OutputYn;

                for (int i = 0; i < TestSignal.Samples.Count; i += M)
                {
                    OutputSignal.Samples.Add(fir.OutputYn.Samples[i]);
                    OutputSignal.SamplesIndices.Add(i / M);
                }
            }

            //return error message
            else
            {
                Console.WriteLine("Error");
            }
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FastFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public int InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            List<float> phase = new List<float>();
            List<float> amplitudes = new List<float>();

            OutputFreqDomainSignal = new Signal(new List<float>(), false);
            for (int j = 0; j < InputTimeDomainSignal.Samples.Count; j++)
            {
                float sum = 0;
                float real = 0;
                float imagen = 0;
                for (int i = 0; i < InputTimeDomainSignal.Samples.Count; i++)
                {
                    sum = (j * 2 * (float)(Math.PI) * i) / InputTimeDomainSignal.Samples.Count;
                    real += InputTimeDomainSignal.Samples[i] * (float)Math.Cos(sum);
                    imagen += -InputTimeDomainSignal.Samples[i] * (float)Math.Sin(sum);
                }

                amplitudes.Add((float)Math.Sqrt(real * real + imagen * imagen));
                phase.Add((float)Math.Atan2(imagen, real));
            }
            OutputFreqDomainSignal.FrequenciesAmplitudes = amplitudes;
            OutputFreqDomainSignal.FrequenciesPhaseShifts = phase;


        }
    }
}

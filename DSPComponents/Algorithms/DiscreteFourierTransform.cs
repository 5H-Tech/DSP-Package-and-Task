using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            List<float> phase = new List<float>();
            List<float> amplitudes = new List<float>();
            List<float> freqs = new List<float>();
            Complex c = new Complex();
            OutputFreqDomainSignal = new Signal(new List<float>(), false);
            for (int j = 0; j < InputTimeDomainSignal.Samples.Count; j++)
            {
                float sum = 0;
                float real = 0;
                float imagen = 0;
                c = 0;
                for (int i = 0; i < InputTimeDomainSignal.Samples.Count; i++)
                {
                  //  sum = (j * 2 * (float)(Math.PI) * i) / InputTimeDomainSignal.Samples.Count;
                    //real += InputTimeDomainSignal.Samples[i] * (float)Math.Cos(sum);
                    //imagen += -InputTimeDomainSignal.Samples[i] * (float)Math.Sin(sum);
                    c += InputTimeDomainSignal.Samples[i]*Complex.Pow(Math.E,-2*i*j*Math.PI*Complex.ImaginaryOne/InputTimeDomainSignal.Samples.Count);
                }

                //amplitudes.Add((float)Math.Sqrt(real * real + imagen * imagen));
                amplitudes.Add((float)(c.Magnitude));
                freqs.Add((float)Math.Round(((2 * Math.PI * InputSamplingFrequency) /
                    InputTimeDomainSignal.Samples.Count) * j, 1));
                //phase.Add((float)Math.Atan2(imagen, real));
                phase.Add((float) (c.Phase));
            }
            OutputFreqDomainSignal.FrequenciesAmplitudes = amplitudes;
            OutputFreqDomainSignal.Frequencies = freqs;
            OutputFreqDomainSignal.FrequenciesPhaseShifts = phase;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {
            List<Complex> comp = new List<Complex>();
            List<Complex> ans = new List<Complex>();

            for (int i = 0; i < InputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
            {
                comp.Add(Complex.FromPolarCoordinates(InputFreqDomainSignal.FrequenciesAmplitudes[i], InputFreqDomainSignal.FrequenciesPhaseShifts[i]));
            }
            for (int i = 0; i < InputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
            {
                ans.Add(0);
                for (int k = 0; k < InputFreqDomainSignal.FrequenciesAmplitudes.Count; k++)
                {
                    double amount = (2 * k * Math.PI * i) / InputFreqDomainSignal.FrequenciesAmplitudes.Count;
                    ans[i] += comp[k] * (Math.Cos(amount) + Complex.ImaginaryOne * Math.Sin(amount));
                }
                ans[i] /= InputFreqDomainSignal.FrequenciesAmplitudes.Count;
            }
            List<float> answer = new List<float>();
            for (int i = 0; i < InputFreqDomainSignal.FrequenciesAmplitudes.Count; i++)
            {
                answer.Add((float)ans[i].Real);
            }
            OutputTimeDomainSignal = new Signal(answer, false);
        }
    }
}

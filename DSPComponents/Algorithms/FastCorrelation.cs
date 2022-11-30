using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            OutputNormalizedCorrelation = new List<float>();
            OutputNonNormalizedCorrelation = new List<float>();

            if (InputSignal2 == null)    // Auto corolation 
            {
                DiscreteFourierTransform dft = new DiscreteFourierTransform();
                dft.InputTimeDomainSignal = InputSignal1;
                dft.Run();
                Signal sig1 = dft.OutputFreqDomainSignal;

                List<Complex> res = new List<Complex>();

                List<float> amp = new List<float>();
                List<float> phas = new List<float>();
                for (int i = 0; i < sig1.FrequenciesAmplitudes.Count; i++)
                {
                    Complex tmp = (Complex.FromPolarCoordinates(sig1.FrequenciesAmplitudes[i], sig1.FrequenciesPhaseShifts[i]));
                    Complex a = (tmp * Complex.Conjugate(tmp));
                    amp.Add((float)a.Magnitude);
                    phas.Add((float)a.Phase);
                }
                Signal coco = new Signal(new List<float>(), false);
                coco.FrequenciesPhaseShifts = phas;
                coco.FrequenciesAmplitudes = amp;
                InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
                idft.InputFreqDomainSignal = coco;
                idft.Run();
                double normal= getNormalTearm(InputSignal1.Samples,InputSignal1.Samples);
                
                for (int i = 0; i < idft.OutputTimeDomainSignal.Samples.Count; i++)
                {
                    float tmp = idft.OutputTimeDomainSignal.Samples[i] / idft.OutputTimeDomainSignal.Samples.Count; 
                    OutputNonNormalizedCorrelation.Add(tmp);
                    OutputNormalizedCorrelation.Add(tmp / (float)normal);

                }
            }
            else
            {
                DiscreteFourierTransform dft1 = new DiscreteFourierTransform();
                DiscreteFourierTransform dft2 = new DiscreteFourierTransform();

                dft1.InputTimeDomainSignal = InputSignal1;
                dft1.Run();
                dft2.InputTimeDomainSignal = InputSignal2;
                dft2.Run();
                Signal sig1 = dft1.OutputFreqDomainSignal;
                Signal sig2 = dft2.OutputFreqDomainSignal;
                List<Complex> res = new List<Complex>();

                List<float> amp = new List<float>();
                List<float> phas = new List<float>();
                for (int i = 0; i < sig1.FrequenciesAmplitudes.Count; i++)
                {
                    Complex tmp1 = (Complex.FromPolarCoordinates(sig1.FrequenciesAmplitudes[i], sig1.FrequenciesPhaseShifts[i]));
                    Complex tmp2 = (Complex.FromPolarCoordinates(sig2.FrequenciesAmplitudes[i], sig2.FrequenciesPhaseShifts[i]));
                    
                    Complex a = (tmp2 * Complex.Conjugate(tmp1));
                    amp.Add((float)a.Magnitude);
                    phas.Add((float)a.Phase);
                }
                Signal coco = new Signal(new List<float>(), false);
                coco.FrequenciesPhaseShifts = phas;
                coco.FrequenciesAmplitudes = amp;
                InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
                idft.InputFreqDomainSignal = coco;
                idft.Run();
                double normal = getNormalTearm(InputSignal1.Samples, InputSignal2.Samples);

                for (int i = 0; i < idft.OutputTimeDomainSignal.Samples.Count; i++)
                {
                    float tmp = idft.OutputTimeDomainSignal.Samples[i] / idft.OutputTimeDomainSignal.Samples.Count;
                    OutputNonNormalizedCorrelation.Add(tmp);
                    OutputNormalizedCorrelation.Add(tmp / (float)normal);

                }
            }
        }

        public double getNormalTearm(List<float> l1, List<float> l2){
            double normalization_summation = 0, signal_samples_summation = 0, signal_samples_copy_summation = 0;
            for (int i = 0; i < l1.Count; i++)
            {
                signal_samples_summation += l1[i] * l1[i];
                signal_samples_copy_summation += l2[i] * l2[i];
            }
            normalization_summation = signal_samples_summation * signal_samples_copy_summation;
            normalization_summation = Math.Sqrt(normalization_summation);
            normalization_summation /= l1.Count;

            return normalization_summation;

        }
    }
}
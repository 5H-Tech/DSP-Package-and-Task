using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            int in1_cout = InputSignal1.Samples.Count;
            int in2_cout = InputSignal2.Samples.Count;

            int res_count = in1_cout + in2_cout - 1;
            for (int i = 0; i < res_count-in1_cout; i++)
            {
                InputSignal1.Samples.Add(0);
            }
            for (int i = 0; i < res_count-in2_cout; i++)
            {
                InputSignal2.Samples.Add(0);
            }

            DiscreteFourierTransform in1_dft = new DiscreteFourierTransform();
            in1_dft.InputTimeDomainSignal = InputSignal1;
            in1_dft.Run();
            Signal transformedInput1 = in1_dft.OutputFreqDomainSignal;

            DiscreteFourierTransform in2_dft = new DiscreteFourierTransform();
            in2_dft.InputTimeDomainSignal = InputSignal2;
            in2_dft.Run();
            Signal transformedInput2 = in2_dft.OutputFreqDomainSignal;


            List<float> amp = new List<float>();
            List<float> phas = new List<float>();

            for (int i = 0; i < res_count; i++)
            {
                Complex tmp_in1 = Complex.FromPolarCoordinates(transformedInput1.FrequenciesAmplitudes[i], transformedInput1.FrequenciesPhaseShifts[i]);
                Complex tmp_in2 = Complex.FromPolarCoordinates(transformedInput2.FrequenciesAmplitudes[i], transformedInput2.FrequenciesPhaseShifts[i]);
                Complex tmp_res = tmp_in1 * tmp_in2;
                amp.Add((float)tmp_res.Magnitude);
                phas.Add((float)tmp_res.Phase);
            }

            Signal res_friq_domain = new Signal(new List<float>(), false);

            res_friq_domain.FrequenciesPhaseShifts = phas;
            res_friq_domain.FrequenciesAmplitudes = amp;

            InverseDiscreteFourierTransform idft = new InverseDiscreteFourierTransform();
            idft.InputFreqDomainSignal = res_friq_domain;
            idft.Run();
            OutputConvolvedSignal = new Signal(new List<float>(), false);
            for (int i = 0; i < idft.OutputTimeDomainSignal.Samples.Count; i++)
            {
                OutputConvolvedSignal.Samples.Add((float)Math.Round(idft.OutputTimeDomainSignal.Samples[i], 1));
            }
            //OutputConvolvedSignal = idft.OutputTimeDomainSignal;



        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public FILTER_TYPES InputFilterType { get; set; }
        public float InputFS { get; set; } // for normalization
        public float InputCutOffFrequency { get; set; } // in low and high only
        public float InputF1 { get; set; } // in pand pass and regect only
        public float InputF2 { get; set; } // in pand pass and regect only
        public float InputStopBandAttenuation { get; set; } //
        public float InputTransitionBand { get; set; }
        public Signal OutputHn { get; set; }
        public Signal OutputYn { get; set; }

        public override void Run()
        {
          
            OutputHn = new Signal(new List<float>(),false);
            OutputYn = new Signal(new List<float>(),false);
            if (InputFilterType == FILTER_TYPES.LOW)
            {
                List<float> hn = LowpassAndHighPss(true);
                float dlat_f = (InputTransitionBand / InputFS);
                float tmp = getWidth() / dlat_f;
                int NumberOfSamples = (int)Math.Round(tmp);
                if (NumberOfSamples % 2 == 0)
                    NumberOfSamples++;
                List<float> W = new List<float>();
                if (InputStopBandAttenuation <= 21)
                {
                    W = RedtangularWindow(NumberOfSamples);
                }
                else if (InputStopBandAttenuation <= 44)
                {
                    W = HanningWindow(NumberOfSamples);
                }
                else if (InputStopBandAttenuation <= 53)
                {
                    W = HammingWindow(NumberOfSamples);
                }
                else
                {
                    W = BlackmanWindow(NumberOfSamples);
                }
           
                for (int i = 0; i < NumberOfSamples; i++)
                {
                    OutputHn.Samples.Add(hn[i] * W[i]);
                }


            }
        }

        private float getWidth()
        {
            float widnow = 0;
            if (InputStopBandAttenuation <= 21)
            {
                widnow = 0.9f;
            }
            else if (InputStopBandAttenuation <= 44)
            {
                widnow =3.1f;
            }
            else if (InputStopBandAttenuation <= 53)
            {
                widnow = 3.3f;
            }
            else
            {
                widnow = 5.5f;
            }
            return widnow;
        }
        public List<float> LowpassAndHighPss(bool isLow)
        {
            int High = isLow ? 1 : -1;
            float dlat_f = (InputTransitionBand / InputFS)* High;
            float f_dash = (InputCutOffFrequency + (dlat_f / 2)) / InputFS;
            List<float> Resutl =new List<float>();
            float tmpHofZero = 2 * f_dash;
            if (isLow)
               Resutl.Add(tmpHofZero);
            else
                Resutl.Add(1 - tmpHofZero);

            for (int i = 1; i < InputTimeDomainSignal.Samples.Count; i++)
            {
                    double Wi =2 * Math.PI * (double)f_dash * i;
                    Resutl.Add((float)(2 * f_dash * (Math.Sin(Wi) / Wi))* High);
            }

            return Resutl;
        }
        public List<float> BandPassAndBandStop(bool isPass)
        {

            float Pass = isPass ? 1 : -1;

            List<float> Result = new List<float>();
            float dlat_f = (InputTransitionBand / InputFS);
            float f_dash1 = InputF1 - (dlat_f / 2);
            float f_dash2 = InputF2 - (dlat_f / 2);
            float tmp_zero = 2 * (InputF2 - InputF1);
            if (isPass)
            {
                Result.Add(tmp_zero);
            }
            else
            {
                Result.Add(1 - tmp_zero);
            }
            for (int i = 1; i < InputTimeDomainSignal.Samples.Count; i++)
            {
                double Wi1 = 2 * Math.PI * (double)f_dash1 * i;
                double Wi2 = 2 * Math.PI * (double)f_dash2 * i;
                float res = (float)((2 * InputF2 * (Math.Sin(Wi2) / Wi2)) - (2 * InputF1 * (Math.Sin(Wi1) / Wi1)));
                Result.Add(res* Pass);
            }




            return Result;
        }

        public List<float> RedtangularWindow(int NumberOfSamples)
        {
            List<float> Reslult=new List<float>();
            for (int i = 0; i < NumberOfSamples; i++)
            {
                Reslult.Add(1);
            }
            return Reslult;

        }
        public List<float> HanningWindow(int NumberOfSamples)
        {
            List<float> Reslult = new List<float>();
            for (int i = 0; i < NumberOfSamples; i++)
            {
                Reslult.Add((float)(0.5 + (0.5 * Math.Cos((2 * Math.PI * i) / (NumberOfSamples)))));
            }
            return Reslult;

        }

        public List<float> HammingWindow(int NumberOfSamples)
        {
            List<float> Reslult = new List<float>();
            for (int i = 0; i < NumberOfSamples; i++)
            {
                Reslult.Add((float)(0.54 + (0.46 * Math.Cos((2 * Math.PI * i) / (NumberOfSamples)))));
            }
            return Reslult;

        }
        public List<float> BlackmanWindow(int NumberOfSamples)
        {
            List<float> Reslult = new List<float>();
            for (int i = 0; i < NumberOfSamples; i++)
            {
                Reslult.Add((float)((0.42 + (0.5 * Math.Cos((2 * Math.PI * i) / (NumberOfSamples-1)))) + (0.38 + (0.5 * Math.Cos((4 * Math.PI * i) / (NumberOfSamples-1))))));
            }
            return Reslult;

        }
    }
}

﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.Common;

namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
                                   // public Signal TestSignal { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }
        public String myPath = "D:\\My OutPut";
        public static string dirParameter = @"C:\Users\hadi\Documents\GitHub\DSP-Package-and-Task\Saved Signals\";

        public override void Run()
        {
            #region init
            Signal InputSignal = LoadSignal(SignalPath);
            OutputFreqDomainSignal = new Signal(new List<float>(), false);
            #endregion

            #region display current signal
            #endregion

            #region band filter
            Signal filterdSignal = new Signal(new List<float>(), false);
            FIR fir = new FIR();
            fir.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.BAND_PASS;
            fir.InputStopBandAttenuation = 50;
            fir.InputTransitionBand = 500;
            fir.InputF1 = miniF;
            fir.InputF2 = maxF;
            fir.InputFS = Fs;
            fir.InputTimeDomainSignal = InputSignal;
            fir.Run();
            filterdSignal = fir.OutputYn;
            saveSignal("Filtered Signal.txt", filterdSignal);
            #endregion

            #region resampiling
            Signal resampiledSignal = null;
            Sampling sampling = new Sampling();
            if (newFs >=2*maxF)
            {
                resampiledSignal = new Signal(new List<float>(), false);
                sampling.InputSignal = filterdSignal;
                sampling.L = L;
                sampling.M = M;
                sampling.Run();
                resampiledSignal = sampling.OutputSignal;
                saveSignal("resampiled Signal.txt", resampiledSignal);
            }


            #endregion
            if (resampiledSignal == null)
            {
                resampiledSignal = filterdSignal;
            }

            #region Remove the DC Component
            Signal dcSignal = new Signal(new List<float>(), false);
            DC_Component dc = new DC_Component();
            dc.InputSignal = resampiledSignal;
            dc.Run();
            dcSignal = dc.OutputSignal;
            saveSignal("DC Signal.txt", dcSignal);
            #endregion

            #region Normalization
            Signal normalizedSignal = new Signal(new List<float>(), false);
            Normalizer n = new Normalizer();
            n.InputSignal = dcSignal;
            n.InputMaxRange = 1;
            n.InputMinRange = -1;
            n.Run();
            normalizedSignal = n.OutputNormalizedSignal;
            saveSignal("normalized Signal.txt", normalizedSignal);
            #endregion

            #region Dft
            DiscreteFourierTransform dft = new DiscreteFourierTransform();
            dft.InputTimeDomainSignal = normalizedSignal;
            dft.InputSamplingFrequency = Fs;
            dft.Run();
            OutputFreqDomainSignal = dft.OutputFreqDomainSignal;
            saveSignal("OutputFreqDomainSignal.txt", OutputFreqDomainSignal);
            #endregion
        }



        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }

        public void saveSignal(string fileName, Signal signal)
        {
            // Save File to .txt  
            FileStream fParameter = new FileStream(dirParameter+fileName, FileMode.Create, FileAccess.Write);
            StreamWriter m_WriterParameter = new StreamWriter(fParameter);
            m_WriterParameter.BaseStream.Seek(0, SeekOrigin.End);
            for (int i = 0; i < signal.Samples.Count; i++)
            {
                m_WriterParameter.WriteLine(signal.Samples[i]);
            }
           
            m_WriterParameter.Flush();
            m_WriterParameter.Close();
        }
    }

}

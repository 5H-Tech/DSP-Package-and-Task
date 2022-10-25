using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }
        public struct level
        {
            public float value;
            public int no;
            public string code;
        }
        List<level> levels;

        public override void Run()
        {
            //init
            OutputQuantizedSignal = new Signal(new List<float>(),false);
            OutputSamplesError = new List<float>();
            OutputIntervalIndices = new List<int>();
            OutputEncodedSignal = new List<string>();

            // Declerations for some vars
            float inputMin = InputSignal.Samples.Min(),
                  inputMax=InputSignal.Samples.Max();

            int numberOfLevels = InputLevel > 0 ? InputLevel : Convert.ToInt32( Math.Pow(2,InputNumBits));
            float intervalWidth = (inputMax - inputMin) / numberOfLevels;
            levels = new List<level>();
            float start;            
            // Createin
            for (start = inputMin; start < inputMax; start += intervalWidth)
            {
                start = (float)Math.Round(start, 2);
                level tmpl = new level();
                tmpl.value = (start + start+intervalWidth) / 2;
                tmpl.no = levels.Count+1;
                tmpl.code = Convert.ToString(tmpl.no - 1, 2).PadLeft(Convert.ToInt32((Math.Log(numberOfLevels,2))), '0');
                levels.Add(tmpl);
            }
            foreach (var sampl in InputSignal.Samples)
	        {
                int levelIdx =Convert.ToInt32(Math.Floor(Math.Abs(sampl - inputMin)/intervalWidth));
                level currLevel = levels[levelIdx>=levels.Count ? levels.Count-1:levelIdx ];
                OutputQuantizedSignal.Samples.Add(currLevel.value);
                OutputSamplesError.Add(currLevel.value - sampl);
                OutputIntervalIndices.Add(currLevel.no);
                OutputEncodedSignal.Add(currLevel.code); 
            }
            
        }

    }
}

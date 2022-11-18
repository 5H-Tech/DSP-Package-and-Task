using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }

        public static Signal lastOutputFoldedSignal ;

        public static int folded = -1;

        public override void Run()
        {
            int n = InputSignal.Samples.Count;
            OutputFoldedSignal = new Signal(new List<float>(), InputSignal.SamplesIndices, false);

            for (int i = n - 1; i >= 0; i--)
            {
                OutputFoldedSignal.Samples.Add(InputSignal.Samples[i]);
            }
            if(InputSignal == lastOutputFoldedSignal)
            {
                folded = folded * -1;
            }
            else
            {
                lastOutputFoldedSignal = OutputFoldedSignal;
               
                folded = 1;
            }
           
        }
    }
}

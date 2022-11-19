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
            InputSignal.Samples.Reverse();
            InputSignal.SamplesIndices.Reverse();
            OutputFoldedSignal = InputSignal;
            
            OutputFoldedSignal.Folded = !OutputFoldedSignal.Folded;
            for (int i = 0; i < OutputFoldedSignal.SamplesIndices.Count; i++)
            {
                OutputFoldedSignal.SamplesIndices[i] *= -1;
            }
            //for (int i = n - 1; i >= 0; i--)
            //{
            //    OutputFoldedSignal.Samples.Add(InputSignal.Samples[i]);
            //}
           
            //if(InputSignal == lastOutputFoldedSignal)
            //{
            //    folded = folded * -1;
            //}
            //else
            //{
            //    lastOutputFoldedSignal = OutputFoldedSignal;
               
            //    folded = 1;
            //}
           
        }
    }
}

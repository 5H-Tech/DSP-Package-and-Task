using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            float mean = InputSignal.Samples.Average();
            OutputSignal = new Signal(new List<float>(), false);
            foreach (var item in InputSignal.Samples)
            {
                OutputSignal.Samples.Add(item - mean);
            }


        }
    }
}

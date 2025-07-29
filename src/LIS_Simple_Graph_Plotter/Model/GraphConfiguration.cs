using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LIS_Simple_Graph_Plotter.Model;

namespace LIS_Simple_Graph_Plotter.Model
{
    /// <summary>
    /// Model containing all relevant plot parameters
    /// </summary>
    public class PlotConfiguration
    {
        /// <summary>
        /// Hierarchy of settings with its corresponding parameters
        /// </summary>
        public Dictionary<FunctionTypes, FunctionParameters> PlotParameterSets { get; set; }

        /// <summary>
        /// Mathematical function type that was selected previously (e.g. Sine, Cosine)
        /// </summary>
        public FunctionTypes SelectedFunction { get; set; } = FunctionTypes.Sin;
    }
}

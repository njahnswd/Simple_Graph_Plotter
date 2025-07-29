using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace LIS_Simple_Graph_Plotter.Model
{
    /// <summary>
    /// List of all available plotting functions
    /// Future functions are to be added here
    /// </summary>
    public enum FunctionTypes
    {
        /// <summary>
        /// Sine function (y = a*sin(f * x + p))
        /// </summary>
        Sin,

        /// <summary>
        /// Cosine function (y = a*cos(f * x + p))
        /// </summary>
        [Description("Cosine function: y = a·cos(x)")]
        Cos,

        /// <summary>
        /// Distribution function (y= a*sin(f * x + p)/x)
        /// </summary>
        [Description("Sinc function: y = a·sin(f*x + p) / x")]
        Sinc
    }
}

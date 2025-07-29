using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LIS_Simple_Graph_Plotter.Model;

namespace LIS_Simple_Graph_Plotter.Services
{
    /// <summary>
    /// General interface for all data point generators for each mathematical function
    /// </summary>
    public interface IFunctionGenerator
    {
        /// <summary>
        /// Generates the points for the corresponding mathematical function to be displayed as a graph (sin, cos sinc)
        /// </summary>
        /// <param name="parameters">In: Parameterset that characterizes the graph (e.g. frequency)</param>
        /// <param name="resolution">In: The number of data points to generate for the graph (x-axis)</param>
        /// <returns>Collection of <see cref="Point"/> instances representing the mathematical function</returns>
        IEnumerable<Point> GenerateGraph(FunctionParameters parameters, int resolution = 500);
    }
}
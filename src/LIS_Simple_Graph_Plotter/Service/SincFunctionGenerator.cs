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
    /// Generator of the points to be displayed on graph for sine function
    /// </summary>
    public class SincFunctionGenerator : IFunctionGenerator
    {
        /// <summary>
        /// Generates the points for the sinc function
        /// </summary>
        /// <param name="parameters">In: Parameterset that characterizes the graph (e.g. frequency)</param>
        /// <param name="resolution">In: The number of data points to generate for the graph (x-axis)</param>
        /// <returns>Collection of <see cref="Point"/> instances representing the sine function</returns>
        public IEnumerable<Point> GenerateGraph(FunctionParameters parameters, int resolution = 500)
        {
            double step = (parameters.XMax - parameters.XMin) / (resolution - 1);

            for (int i = 0; i < resolution; i++)
            {
                double x = parameters.XMin + i * step;
                double y;

                if (Math.Abs(x) < double.Epsilon)
                {
                    y = parameters.Amplitude; // sinc(0) = 1
                }
                else
                {
                    y = parameters.Amplitude * Math.Sin(2 * Math.PI * parameters.Frequency * x + parameters.Phase) / (2 * Math.PI * x);
                }

                yield return new Point(x, y);
            }
        }
    }
}
using LIS_Simple_Graph_Plotter.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace LIS_Simple_Graph_Plotter.Service
{
    /// <summary>
    /// Handler for saving and storing plot configurations using a local JSon file  
    /// </summary>
    public class ConfigurationManager
    {
        /// <summary>
        /// Container for the settings of all graphs. Is only updated during the save interactions
        /// </summary>
        private PlotConfiguration _graphConfigs;

        /// <summary>
        /// Generate default settings for all graphs in case json file is missing
        /// </summary>
        /// <returns>Full configuration with default values</returns>
        private PlotConfiguration GetDefaultGraphConfiguration()
        {
            var parameterSets = new Dictionary<FunctionTypes, FunctionParameters>();

            foreach (FunctionTypes type in Enum.GetValues(typeof(FunctionTypes)))
            {
                parameterSets[type] = GetDefaultFunction();
            }

            return new PlotConfiguration
            {
                SelectedFunction = FunctionTypes.Sin,
                PlotParameterSets = parameterSets
            };
        }

        /// <summary>
        /// Generate default settings for a single function (e.g. Sine)
        /// </summary>
        /// <returns>New default set of <see cref="FunctionParameters"/></returns>
        private FunctionParameters GetDefaultFunction() => new FunctionParameters
        {
            Amplitude = 1,
            Frequency = 1,
            Phase = 0,
            XMin = -2,
            XMax = 2
        };

        /// <summary>
        /// Load all configurations for each plottable function (e.g. Sine) from local Json file
        /// </summary>
        /// <param name="configPath">In: Path to Json file containing configurations</param>
        public void LoadPlotConfigurationToFile(string configPath)
        {
            if (File.Exists(configPath))
            {
                var json = File.ReadAllText(configPath);
                //Todo: Add try catch block in order to prevent file manipulation
                _graphConfigs = JsonSerializer.Deserialize<PlotConfiguration>(json);
            }
            else 
            {
                _graphConfigs = GetDefaultGraphConfiguration();
            }
        }

        /// <summary>
        /// Save all configurations for each plottable function (e.g. Sine) from local Json file
        /// </summary>
        /// <param name="configPath">In: Path to Json file containing configurations</param>
        public void SavePlotConfigurationToFile(string configPath)
        {
            var json = JsonSerializer.Serialize(_graphConfigs, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(configPath, json);
        }

        /// <summary>
        /// Get a copy of the configuration for the selected function type
        /// </summary>
        /// <returns>Current function parameter values</returns>
        public FunctionParameters GetPlotConfiguration()
        {
            return _graphConfigs.PlotParameterSets[_graphConfigs.SelectedFunction].Clone();
        }

        /// <summary>
        /// Set the configuration for the selected function type
        /// </summary>
        /// <param name="updatedParameters">In: New function parameters to be used as default</param>
        public void SetPlotConfiguration(FunctionParameters updatedParameters)
        {
            _graphConfigs.PlotParameterSets[_graphConfigs.SelectedFunction] = updatedParameters;
        }

        /// <summary>
        /// Get a the current selected function type
        /// </summary>
        /// <returns>Selected function (e.g. Sine)</returns>
        public FunctionTypes GetSelectedFunction()
        {
            return _graphConfigs.SelectedFunction;
        }

        /// <summary>
        /// Set a the selected mathematical function
        /// </summary>
        ///<param name="selectedFunction">In: Next function to plot</param>
        public void SetSelectedFunction(FunctionTypes selectedFunction)
        {
            _graphConfigs.SelectedFunction = selectedFunction;
        }
    }
}

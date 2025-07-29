using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using LIS_Simple_Graph_Plotter.Model;
using LIS_Simple_Graph_Plotter.Services;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Annotations;
using System.IO;
using System.Windows.Media;
using LIS_Simple_Graph_Plotter.Service;
using OxyPlot.Axes;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LIS_Simple_Graph_Plotter.ViewModel
{
    /// <summary>
    /// ViewModel for plotting mathematical functions using OxyPlot
    /// Generating and updates the function plot
    /// </summary>
    public class PlotViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Relative path to the settings file containing plot parameters 
        /// </summary>
        private string JsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Settings\\app.json");

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Defines the content and structure of the plot
        /// </summary>
        public PlotModel PlotModel { get; private set; }

        /// <summary>
        /// Handles plot interactions (updating values and ranges)
        /// </summary>
        public PlotController PlotController { get; private set; }

        /// <summary>
        /// Handles loading and write the graph configurations like amplitude etc
        /// </summary>
        private readonly ConfigurationManager _configurationManager;

        /// <summary>
        /// Constructor of the PlotViewModel class
        /// Initializes the plot model, controller and default function parameters and property updating
        /// </summary>
        public PlotViewModel()
        {
            PlotModel = new PlotModel();
            PlotController = new PlotController();

            //Settings for the plot
            PlotController.UnbindMouseWheel();                      //disable mouse wheel
            PlotController.UnbindMouseDown(OxyMouseButton.Right);   //disable panning
            // Show x- and y-crosshair for the user convinience
            var axisLineX = new LineAnnotation
            {
                Type = LineAnnotationType.Horizontal,
                Y = 0,
                Color = OxyColors.Black,
                LineStyle = LineStyle.Solid,
                StrokeThickness = 1.5
            };
            var axisLineY = new LineAnnotation
            {
                Type = LineAnnotationType.Vertical,
                X = 0,
                Color = OxyColors.Black,
                LineStyle = LineStyle.Solid,
                StrokeThickness = 1.5
            };

            PlotModel.Annotations.Add(axisLineX);
            PlotModel.Annotations.Add(axisLineY);

            // Use dotted grid lines for y-axis (ToDo: Make adjustable)
            PlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "f(x)",
                TitleFontSize = 24,
                MinorGridlineStyle = LineStyle.Solid,
                MajorGridlineStyle = LineStyle.Solid,
            });
            // Use dotted grid lines for x-axis (ToDo: Make adjustable)
            PlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "x",
                TitleFontSize = 24,
                MinorGridlineStyle = LineStyle.Solid,
                MajorGridlineStyle = LineStyle.Solid
            });

            _configurationManager = new ConfigurationManager();
            //Read and apply previous graph settings
            InitializeConfiguration();
            SelectedFunction = _configurationManager.GetSelectedFunction();
        }

        /// <summary>
        /// Value of the <see cref="SelectedFunction"/> property. Defaulting to a not achievable value
        /// </summary>
        private FunctionTypes _selectedFunction = (FunctionTypes)(-1);

        /// <summary>
        /// Gets or sets the currently selected function type (sine, cosine, sinc) and regenerates the plot if altered
        /// </summary>
        public FunctionTypes SelectedFunction
        {
            get => _selectedFunction;
            set
            {
                if (_selectedFunction != value)
                {
                    _selectedFunction = value;
                    _configurationManager.SetSelectedFunction(_selectedFunction);
                    CurrentParameters = _configurationManager.GetPlotConfiguration();
                }
            }
        }

        /// <summary>
        /// Value of the <see cref="CurrentParameters"/> property
        /// </summary>
        private FunctionParameters _currentParameters;

        /// <summary>
        /// Container with all current settings. Is initialized with default values
        /// </summary>
        public FunctionParameters CurrentParameters
        {
            get => _currentParameters;
            set
            {
                if (_currentParameters != value)
                {
                    if (_currentParameters != null)
                        _currentParameters.PropertyChanged -= (s, e) => GeneratePlot();
                    _currentParameters = value;
                    OnPropertyChanged(nameof(CurrentParameters));
                    if (_currentParameters != null)
                        _currentParameters.PropertyChanged += (s, e) => GeneratePlot();
                    GeneratePlot();
                }
            }
        }

        /// <summary>
        /// Generates the graph for the selected function using the given parameters by updating the plot model
        /// </summary>
        public void GeneratePlot()
        {
            IFunctionGenerator generator = new SinFunctionGenerator();
            switch (SelectedFunction)
            {
                case FunctionTypes.Sin:
                    {
                        PlotModel.Title = "Sine Function";
                        break;
                    }
                case FunctionTypes.Cos:
                    {
                        PlotModel.Title = "Cosine Function";
                        generator = new CosFunctionGenerator();
                        break;
                    }
                case FunctionTypes.Sinc:
                    {
                        PlotModel.Title = "Sinc Function";
                        generator = new SincFunctionGenerator();
                        break;
                    }
                default:
                    throw new NotSupportedException("Unsupported function type");
            }

            var series = new LineSeries { Title = "f(x)" };
            //custom series settings
            series.TrackerFormatString = "x: {2:0.0}\ny: {4:0.0}";  //only 1 digit for tooltip
            series.Color = OxyColors.Blue;                          //color of graph (default is green)
            series.MarkerType = MarkerType.None;                    // No static markers
            foreach (var point in generator.GenerateGraph(CurrentParameters))
            {
                series.Points.Add(new DataPoint(point.X, point.Y));
            }
            PlotModel.Series.Clear();
            PlotModel.Series.Add(series);
            PlotModel.InvalidatePlot(true);
        }

        /// <summary>
        /// Save the entire plot including the axis to vector format image (.svg)
        /// </summary>
        /// <param name="filePath">In: Path to store image</param>
        public void SavePlotToSVG(string filePath)
        {
            using (var stream = File.Create(filePath))
            {
                var exporter = new SvgExporter { Width = 640, Height = 480 };
                exporter.Export(PlotModel, stream);
            }
        }

        /// <summary>
        /// Provides a collection of available mathematical functions that can be selected in UI via Data Binding
        /// </summary>
        public ObservableCollection<FunctionTypes> AvailableFunctions { get; } = new ObservableCollection<FunctionTypes>(Enum.GetValues(typeof(FunctionTypes)).Cast<FunctionTypes>());

        /// <summary>
        /// Load plot configurations from local file (Json)
        /// </summary>
        public void InitializeConfiguration()
        {
            _configurationManager.LoadPlotConfigurationToFile(JsonFilePath);
        }

        /// <summary>
        /// Save the parameters and the selected function to JSON file
        /// </summary>
        public void SaveConfiguration()
        {
            if(ValidateCurrentParameters(out string ErrorText))
            {
                _configurationManager.SetSelectedFunction(SelectedFunction);
                _configurationManager.SetPlotConfiguration(CurrentParameters);
                _configurationManager.SavePlotConfigurationToFile(JsonFilePath);
            }
            else
            {
                MessageBox.Show(ErrorText);
            }

        }

        /// <summary>
        /// Check if all functionparameters are valid
        /// </summary>
        /// <param name="errorMessage">Out: Error string in case not all parameters are valid</param>
        /// <returns>True when all parameters are valid</returns>
        private bool ValidateCurrentParameters(out string errorMessage)
        {
            //(ToDo: Add limits to model instead of hardcoding, Min-Max values for all parameters)
            if (CurrentParameters == null)
            {
                errorMessage = "Function Parameters are not valid!";
                return false;
            }

            if (CurrentParameters.Amplitude < 0)
            {
                errorMessage = "Amplitude must not be smaller than 0!";
                return false;
            }

            if (CurrentParameters.Frequency < 0)
            {
                errorMessage = "Frequency must not be smaller than 0!";
                return false;
            }


            if (CurrentParameters.Phase < 0)
            {
                errorMessage = "Phase must not be smaller than 0!";
                return false;
            }

            errorMessage = null;
            return true;
        }

        /// <summary>
        /// Event raised when properties change <see cref="PropertyChanged"/>
        /// </summary>
        /// <param name="propertyName">In: The name of the property that changed</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
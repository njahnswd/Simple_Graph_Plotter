using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace LIS_Simple_Graph_Plotter.Model
{
    /// <summary>
    /// Container of configurable parameters used for plotting mathematical functions
    /// Includes amplitude, frequency, phase shift, and the visible x-axis range
    /// </summary>
    public class FunctionParameters : INotifyPropertyChanged
    {
        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Value of the <see cref="Amplitude"/> property
        /// </summary>
        private double _amplitude = 1.0;

        /// <summary>
        /// Value of the <see cref="Frequency"/> property
        /// </summary>
        private double _frequency = 1.0;

        /// <summary>
        /// Value of the <see cref="Phase"/> property
        /// </summary>
        private double _phase = 0;

        /// <summary>
        /// Value of the <see cref="XMin"/> property
        /// </summary>
        private double _xMin = -10;

        /// <summary>
        /// Value of the <see cref="XMax"/> property
        /// </summary>
        private double _xMax = 10;

        /// <summary>
        /// Get or set the amplitude of the function
        /// </summary>
        public double Amplitude
        {
            get => _amplitude;
            set
            {
                if (_amplitude != value)
                {
                    _amplitude = value;
                    OnPropertyChanged(nameof(Amplitude));
                }
            }
        }

        /// <summary>
        /// Get or set the frequency of the function
        /// </summary>
        public double Frequency
        {
            get => _frequency;
            set
            {
                if (_frequency != value)
                {
                    _frequency = value;
                    OnPropertyChanged(nameof(Frequency));
                }
            }
        }

        /// <summary>
        /// Get or set the phase of the function. To be set in radians
        /// </summary>
        public double Phase
        {
            get => _phase;
            set
            {
                if (_phase != value)
                {
                    _phase = value;
                    OnPropertyChanged(nameof(Phase));
                }
            }
        }

        /// <summary>
        /// Get or set the minimum value of the x-axis in the plot
        /// </summary>
        public double XMin
        {
            get => _xMin;
            set
            {
                if (_xMin != value)
                {
                    _xMin = value;
                    OnPropertyChanged(nameof(XMin));
                }
            }
        }

        /// <summary>
        /// Get or set the maximum value of the x-axis in the plot
        /// </summary>
        public double XMax
        {
            get => _xMax;
            set
            {
                if (_xMax != value)
                {
                    _xMax = value;
                    OnPropertyChanged(nameof(XMax));
                }
            }
        }

        /// <summary>
        /// Event raised when properties change <see cref="PropertyChanged"/>
        /// </summary>
        /// <param name="PropertyName">In: The name of the property that changed</param>
        protected void OnPropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        /// <summary>
        /// Create a copy of all parameters to be used independently
        /// </summary>
        /// <returns>Deep copy of functionparameters</returns>
        public FunctionParameters Clone()
        {
            return new FunctionParameters
            {
                Amplitude = this.Amplitude,
                Frequency = this.Frequency,
                Phase = this.Phase,
                XMin = this.XMin,
                XMax = this.XMax
            };
        }

    }
}
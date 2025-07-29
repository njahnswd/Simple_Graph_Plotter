using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LIS_Simple_Graph_Plotter.View
{
    /// <summary>
    /// Interaktionslogik für ParameterSlider.xaml
    /// </summary>
    public partial class ParameterSlider : UserControl
    {
        /// <summary>
        /// Constructor of the ParameterSlider user control
        /// </summary>
        public ParameterSlider()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Register tick frequency as available property
        /// </summary>
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string), typeof(ParameterSlider), new PropertyMetadata("Parameter"));

        /// <summary>
        /// Register tick frequency as available property. This parameter can only be set within the range of the minimum and maximum
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register( nameof(Value), typeof(double), typeof(ParameterSlider),
                new FrameworkPropertyMetadata(0.0,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    null,
                    CheckValue));
        /// <summary>
        /// Register tick frequency as available property
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(ParameterSlider), new PropertyMetadata(0.0));

        /// <summary>
        /// Register tick frequency as available property
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(ParameterSlider), new PropertyMetadata(10.0));

        /// <summary>
        /// Register tick frequency as available property
        /// </summary>
        public static readonly DependencyProperty TickFrequencyProperty = DependencyProperty.Register(nameof(TickFrequency), typeof(double), typeof(ParameterSlider), new PropertyMetadata(0.1));

        /// <summary>
        /// Check input value for user element whether its between minimum and maximum
        /// </summary>
        /// <param name="dependencyObj">In: ParameterSlider as sender</param>
        /// <param name="baseValue">In: Value that needs to be checked</param>
        /// <returns>Checked value</returns>
        private static object CheckValue(DependencyObject dependencyObj, object baseValue)
        {
            var control = (ParameterSlider)dependencyObj;
            double value = (double)baseValue;

            double min = control.Minimum;
            double max = control.Maximum;
            //below minimum --> value is set to min
            if (value < min) return min;
            //above max --> value is set to max
            if (value > max) return max;
            return value;
        }

        /// <summary>
        /// Header of the Slider and name of the parameter that is influenced
        /// </summary>
        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        /// <summary>
        /// Current value and position of slider
        /// </summary>
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        /// Minimum value allowed on Slider
        /// </summary>
        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        /// <summary>
        /// Minimum value allowed on Slider
        /// </summary>
        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        /// <summary>
        /// Slider scaling set via ticks
        /// </summary>
        public double TickFrequency
        {
            get => (double)GetValue(TickFrequencyProperty);
            set => SetValue(TickFrequencyProperty, value);
        }

        /// <summary>
        /// Check if the user input is numeric and valid
        /// </summary>
        /// <param name="sender">In: Textbox as sender</param>
        /// <param name="e">In: Text changed arguments</param>
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string s = ((TextBox)sender).Text.Insert(((TextBox)sender).SelectionStart, e.Text);
            if (!double.TryParse(((TextBox)sender).Text.Insert(((TextBox)sender).SelectionStart, e.Text), out _))
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Check if copied content is numeric and valid
        /// </summary>
        /// <param name="sender">In: Textbox as sender</param>
        /// <param name="e">In: Text changed arguments</param>
        private void NumericOnly_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string pastedText = (string)e.DataObject.GetData(typeof(string));
                if (!double.TryParse(((TextBox)sender).Text.Insert(((TextBox)sender).SelectionStart, pastedText), out _))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
     }
}

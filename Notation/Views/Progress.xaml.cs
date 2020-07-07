using System.Windows;
using System.Windows.Controls;

namespace Notation.Views
{
    /// <summary>
    /// Logique d'interaction pour ProgressForm.xaml
    /// </summary>
    public partial class Progress : Window
    {
        private delegate void UpdateDelegate(object value);
        private UpdateDelegate _updateText;
        private UpdateDelegate _updateValue;

        private void UpdateText(object value)
        {
            Text = (string)value;
        }

        private void UpdateValue(object value)
        {
            Value = (int)value;
            Percentage = $"{(Value / ProgressBar.Maximum * 100).ToString("0.0")}%";
        }

        public void UpdateText(string text)
        {
            Dispatcher.Invoke(_updateText, System.Windows.Threading.DispatcherPriority.Background, new object[] { text });
        }

        public void UpdateValue()
        {
            Dispatcher.Invoke(_updateValue, System.Windows.Threading.DispatcherPriority.Background, new object[] { Value + 1 });
        }

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(Progress), new PropertyMetadata(0));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Progress), new PropertyMetadata(""));

        public string Percentage
        {
            get { return (string)GetValue(PercentageProperty); }
            set { SetValue(PercentageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Percentage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register("Percentage", typeof(string), typeof(Progress), new PropertyMetadata(""));

        public Progress(int count)
        {
            DataContext = this;

            InitializeComponent();

            ProgressBar.Maximum = count;

            _updateText = new UpdateDelegate(UpdateText);
            _updateValue = new UpdateDelegate(UpdateValue);
        }
    }
}

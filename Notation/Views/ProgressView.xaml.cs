using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Notation.Views
{
    /// <summary>
    /// Logique d'interaction pour ProgressForm.xaml
    /// </summary>
    public partial class ProgressView : Window
    {
        private delegate void UpdateProgressBarDelegate(DependencyProperty dp, object value);

        public int ProgressValue
        {
            get { return (int)GetValue(ProgressValueProperty); }
            set { SetValue(ProgressValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProgressValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressValueProperty =
            DependencyProperty.Register("ProgressValue", typeof(int), typeof(ProgressView), new PropertyMetadata(0));

        public string TextLabel
        {
            get { return (string)GetValue(TextLabelProperty); }
            set { SetValue(TextLabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextLabel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextLabelProperty =
            DependencyProperty.Register("TextLabel", typeof(string), typeof(ProgressView), new PropertyMetadata(""));

        public string ProgressLabel
        {
            get { return (string)GetValue(ProgressLabelProperty); }
            set { SetValue(ProgressLabelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProgressLabel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProgressLabelProperty =
            DependencyProperty.Register("ProgressLabel", typeof(string), typeof(ProgressView), new PropertyMetadata(""));

        public string Text
        {
            set
            {
                TextLabel = value;
                ProgressLabel = string.Format("{0}%", (int)((ProgressBar.Value + 1) / ProgressBar.Maximum * 100));
                Dispatcher.Invoke(_updatePbDelegate,
                           System.Windows.Threading.DispatcherPriority.Background,
                           new object[] { RangeBase.ValueProperty, ProgressBar.Value + 1 });
            }
        }

        private int _count;

        public int Count
        {
            set
            {
                _count = value;
                ProgressBar.Minimum = 0;
                ProgressBar.Maximum = value;
                ProgressBar.Value = 0;
                ProgressValue = 0;
            }
            private get
            {
                return _count;
            }
        }

        private UpdateProgressBarDelegate _updatePbDelegate;

        public ProgressView()
        {
            DataContext = this;

            InitializeComponent();

            _updatePbDelegate = new UpdateProgressBarDelegate(ProgressBar.SetValue);
        }
    }
}

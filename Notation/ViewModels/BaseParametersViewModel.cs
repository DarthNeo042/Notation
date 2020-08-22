using System.Windows;
using System.Windows.Media;

namespace Notation.ViewModels
{
    public class BaseParametersViewModel : DependencyObject
    {
        public byte ColorR
        {
            get { return (byte)GetValue(ColorRProperty); }
            set { SetValue(ColorRProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorR.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorRProperty =
            DependencyProperty.Register("ColorR", typeof(byte), typeof(BaseParametersViewModel), new PropertyMetadata((byte)0, ColorChanged));

        public byte ColorG
        {
            get { return (byte)GetValue(ColorGProperty); }
            set { SetValue(ColorGProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorG.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorGProperty =
            DependencyProperty.Register("ColorG", typeof(byte), typeof(BaseParametersViewModel), new PropertyMetadata((byte)125, ColorChanged));

        public byte ColorB
        {
            get { return (byte)GetValue(ColorBProperty); }
            set { SetValue(ColorBProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColorB.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorBProperty =
            DependencyProperty.Register("ColorB", typeof(byte), typeof(BaseParametersViewModel), new PropertyMetadata((byte)250, ColorChanged));

        private static void ColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BaseParametersViewModel baseParameters = (BaseParametersViewModel)d;
            baseParameters.Color = new Color() { R = baseParameters.ColorR, G = baseParameters.ColorG, B = baseParameters.ColorB, A = 255 };
        }

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(BaseParametersViewModel), new PropertyMetadata(Colors.White));

        public string AdminLogin
        {
            get { return (string)GetValue(AdminLoginProperty); }
            set { SetValue(AdminLoginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AdminLogin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AdminLoginProperty =
            DependencyProperty.Register("AdminLogin", typeof(string), typeof(BaseParametersViewModel), new PropertyMetadata("admin"));

        public string AdminPassword
        {
            get { return (string)GetValue(AdminPasswordProperty); }
            set { SetValue(AdminPasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AdminPassword.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AdminPasswordProperty =
            DependencyProperty.Register("AdminPassword", typeof(string), typeof(BaseParametersViewModel), new PropertyMetadata("martin"));

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(BaseParametersViewModel), new PropertyMetadata(""));

        public BaseParametersViewModel()
        {
            ColorChanged(this, new DependencyPropertyChangedEventArgs());
        }
    }
}

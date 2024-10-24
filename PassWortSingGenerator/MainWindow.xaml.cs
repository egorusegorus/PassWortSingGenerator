using System.Text.RegularExpressions;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace PassWortSingGenerator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NumericTextBoxBehavior.Attach(txtBox);
        }

        private void Zahlen_Click(object sender, RoutedEventArgs e)
        {
            if (Zahlen.IsChecked == true)
            {
                Sylaben.IsEnabled = false;
                Speziale_zeichnen.IsEnabled = true;
            }
            else
            {
                Sylaben.IsEnabled = true;
                Speziale_zeichnen.IsEnabled = true;
            }
        }

        private void Sylaben_Click(object sender, RoutedEventArgs e)
        {
            if (Sylaben.IsChecked == true)
            {
                Speziale_zeichnen.IsEnabled = false;
                Zahlen.IsEnabled = false;
            }
            else
            {
                Speziale_zeichnen.IsEnabled = true;
                Zahlen.IsEnabled = true;
            }
        }

        private void Speziale_zeichnen_Click(object sender, RoutedEventArgs e)
        {
            if (Speziale_zeichnen.IsChecked == true)
            {
                Sylaben.IsEnabled = false;
                Zahlen.IsEnabled = true;
            }
            else
            {
                Sylaben.IsEnabled = true;
                Zahlen.IsEnabled = true;
            }
        }

        private void btn_Generieren_Click(object sender, RoutedEventArgs e)
        {
            int laenge = Convert.ToInt32(txtBox.Text);
            bool sylaben = Sylaben.IsChecked ?? false;
            bool speziale_zeichnen = Speziale_zeichnen.IsChecked ?? false;
            bool zahlen = Zahlen.IsChecked ?? false;
            bool großebuchstaben = GroßeBuchstaben.IsChecked ?? false;
            bool kleinebuchstaben = kleineBuchstaben.IsChecked ?? false;

            Generation generation = new Generation(sylaben, speziale_zeichnen, zahlen, großebuchstaben, kleinebuchstaben);
            txtKennwort.Text = generation.Asci127Random(laenge);
        }
    }

    public class Generation
    {
        private bool[] Samlung = new bool[5];

        public Generation(bool sylaben, bool speziale_zeichnen, bool zahlen, bool großebuchstaben, bool kleinebuchstaben)
        {
            Samlung[0] = sylaben;
            Samlung[1] = speziale_zeichnen;                   //33-47,58-64,91-96,123-127
            Samlung[2] = zahlen;                              //48-57
            Samlung[3] = großebuchstaben;                       //65-90
            Samlung[4] = kleinebuchstaben;                      //97-122
        }
        private int zieht(int a) {
            Random random = new Random();
            a = random.Next(a);
            return a;
        }
        public string Asci127Random(int laenge)
        {
            int a = 0, b = 0; 

            if (!Samlung[0] && Samlung[1] && Samlung[2] && Samlung[3] && Samlung[4])
            {
                a = 33;
                b = 127; 
            }


            Random random = new Random();
            StringBuilder chain = new StringBuilder();

            for (int i = 0; i < laenge; i++)
            {
                char c = Convert.ToChar(random.Next(a, b));
                chain.Append(c);
            }

            return chain.ToString();
        }
    }

    public class NumericTextBoxBehavior
    {
        public static void Attach(TextBox textBox)
        {
            textBox.PreviewTextInput += TextBox_PreviewTextInput;
            DataObject.AddPastingHandler(textBox, OnPaste);
        }

        private static void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string pastedText = (string)e.DataObject.GetData(typeof(string));
                if (!IsTextAllowed(pastedText))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return !regex.IsMatch(text);
        }
    }
}

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

            if (Sylaben.IsChecked == true) SylabenChecked();
            else
            if(Sylaben.IsChecked==false&&Speziale_zeichnen.IsChecked==false&&Zahlen.IsChecked==false&&GroßeBuchstaben.IsChecked==false && 
                kleineBuchstaben.IsChecked == false)
            {
                MessageBox.Show("Bitte mindestens eine Checkbox markieren.");
            }
            else { Generation generation = new Generation();
                generation.GeneratePassword(Speziale_zeichnen, Zahlen, GroßeBuchstaben, kleineBuchstaben,txtBox, txtKennwort);
            }
        }

        public void SylabenChecked()
        {
            Sylaben sylaben = new Sylaben();
            string input = txtBox.Text;
            sylaben.KennwortLange = sylaben.PruefeAufZahl(input);
            int WievielSylaben = sylaben.KennwortLange / 3;
            int WasIstDerRest = sylaben.KennwortLange % 3;
            input = sylaben.GenerateKenwort(WievielSylaben, WasIstDerRest).ToString();
            if (kleineBuchstaben.IsChecked == true && GroßeBuchstaben.IsChecked == true) { input = ChangeSize(input); }
            else if (kleineBuchstaben.IsChecked == true && GroßeBuchstaben.IsChecked == false) { input = input.ToLower(); }
            else if (kleineBuchstaben.IsChecked == false && GroßeBuchstaben.IsChecked == true) { input = input.ToUpper(); }
            else if (kleineBuchstaben.IsChecked == false && GroßeBuchstaben.IsChecked == false) { input = input.ToUpper(); }
            txtKennwort.Text = input;
        }
        public string ChangeSize(string text)
        {
            Random random = new Random();
            string ergebnis = "";
            foreach (char letter in text)
            {
                if (random.NextDouble() < 0.5)
                {
                    ergebnis += letter.ToString().ToUpper();
                }
                else
                {
                    ergebnis += letter.ToString().ToLower();
                }
            }
            return ergebnis;
        }
      
        private void btn_Kopieren_Click(object sender, RoutedEventArgs e)
        {
            
            if (!string.IsNullOrEmpty(txtKennwort.Text))
            {
                Clipboard.SetText(txtKennwort.Text);

            }
            else
            {
                MessageBox.Show("Kein Text zum Kopieren.");
            }
        }
    }




    public class Generation
    {
        char[] specialCharacters = new char[]
        {
        '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '=', '+', '[', ']', '{', '}',
        '\\', '|', ';', ':', '\'', '\"', ',', '<', '.', '>', '/', '?', '~'};
        char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        char[] letters = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };



        public void GeneratePassword(CheckBox checkBoxSymbols, CheckBox checkBoxZahlen, CheckBox checkBoxGrosseBuchstaben, CheckBox checkBoxKleineBuchstaben, TextBox textBox, TextBox textBox1)
        {
            
            Random random = new Random();
            Sylaben sylaben = new Sylaben();
            string input = textBox.Text;

            
            int passwordLength = sylaben.PruefeAufZahl(input);
            if (passwordLength <= 0)
            {
                textBox.Text = "Invalid password length";
                return;
            }

           
            List<char> possibleCharacters = new List<char>();

            
            if (checkBoxSymbols.IsChecked == true)
            {
                possibleCharacters.AddRange("!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~");
            }

            
            if (checkBoxZahlen.IsChecked == true)
            {
                for (char c = '0'; c <= '9'; c++)
                {
                    possibleCharacters.Add(c);
                }
            }

            
            if (checkBoxGrosseBuchstaben.IsChecked == true)
            {
                for (char c = 'A'; c <= 'Z'; c++)
                {
                    possibleCharacters.Add(c);
                }
            }

           
            if (checkBoxKleineBuchstaben.IsChecked == true)
            {
                for (char c = 'a'; c <= 'z'; c++)
                {
                    possibleCharacters.Add(c);
                }
            }

          
            if (possibleCharacters.Count == 0)
            {
                textBox.Text = "No character types selected";
                return;
            }

            
            StringBuilder password = new StringBuilder();
            for (int i = 0; i < passwordLength; i++)
            {
                int index = random.Next(possibleCharacters.Count);
                password.Append(possibleCharacters[index]);
            }

            
            textBox1.Text = password.ToString();
        }
    }


        public class NumericTextBoxBehavior
        {

        }
        class Sylaben
        {
            private char[] vokale = { 'a', 'e', 'i', 'o', 'u', 'y' };
            private char[] konsonanten = {'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm',
                             'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'y', 'z'};
            public int SylabeLange = GetRandomIndex(1, 4);
            public int KennwortLange { get; set; }



            public string GenerateKenwort(int WieVielSylabem, int WasIstRest)
            {
                string kennwort = "";
                Random random = new Random();
                for (int i = 0; i < WieVielSylabem; i++)
                {
                    int a = random.Next(0, 20);
                    int b = random.Next(0, 5);
                    int c = random.Next(0, 20);

                    if (b == 2)
                    {
                        int d = random.Next(0, 2);
                        if (d == 0)
                        {
                            kennwort += konsonanten[a].ToString() + vokale[b].ToString() + konsonanten[c].ToString();
                        }
                        else if (d == 1)
                        {
                            c = random.Next(0, 5);
                            kennwort += konsonanten[a].ToString() + vokale[b].ToString() + vokale[c].ToString();
                        }
                    }
                    else
                    {
                        kennwort += konsonanten[a].ToString() + vokale[b].ToString() + konsonanten[c].ToString();
                    }
                }
                if (WasIstRest == 2)
                {
                    int a = random.Next(0, 20);
                    int b = random.Next(0, 5);
                    kennwort += konsonanten[a].ToString() + vokale[b].ToString();
                }
                else if (WasIstRest == 1)
                {

                    int b = random.Next(0, 5);
                    kennwort += vokale[b].ToString();
                }
                return kennwort;
            }
            private static int GetRandomIndex(int a, int b)
            {
                Random random = new Random();
                return random.Next(a, b);
            }

            public int PruefeAufZahl(string input)
            {
                while (true)
                {
                    if (int.TryParse(input, out int zahl))
                    {
                        return zahl;
                    }
                    else
                    {
                        MessageBox.Show("Das ist keine gültige Zahl. Bitte geben Sie eine neue Zahl ein:");

                    }
                }
            }

        }

    }

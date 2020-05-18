/*Необходимо сделать программу, в которой должен быть TextBox и Button.
            1. Пользователь должен вставить в поле любую статью или цитату из интернета.
            2. Нажатие на кнопку исправит все слова еще на ещё, все равно на всё равно.
            3. Кнопка Исправить должна писать Исправлено после исправления.
            4. Спустя пару секунд на кнопке должно быть написано Скопировать исправленный текст.
            5. Текст, который исправлен в поле TextBox должен лететь в буфер обмена.
            6. Кнопка пишет Скопировано в буфер обмена.*/

/*За пример возьмем две строки:
  Вовсе все равноправны все равно, все равно
  Еще все равно, еще.*/
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Correction_of_letters_in_words
{
    public partial class Form1 : Form
    {
        byte counterClick;
        string tempStringForButton1_ClickForeachCh = "", tempStringForTextBox1_TextChangedEqual = "";

        public Form1()
        {
            InitializeComponent();
        }

        async void Button1_Click(object sender, System.EventArgs e)
        {
            if (counterClick == 1 && button1.Text == "Скопировать исправленный текст")
            {
                Clipboard.SetDataObject(textBox1.Text, true);

                if (Clipboard.ContainsText())
                {
                    button1.BackColor = Color.Bisque;
                    button1.Text = "Скопировано в буфер обмена";
                    counterClick = 0;
                    await Task.Delay(2000);
                    button1.BackColor = Color.LightGreen;
                    button1.Text = "Исправлено";
                    return;
                }
            }
            else
                counterClick = 0;

            counterClick++;

            string[] lines = textBox1.Lines, stringToTextBox = new string[lines.Length];
            byte counter = 0;

            foreach (string line in lines)
            {
                stringToTextBox[counter] = "";

                if (line.Contains(" еще") || line.Contains("Еще"))
                {
                    if (line.StartsWith("еще "))
                        stringToTextBox[counter] = line.Replace(" еще", " ещё").Replace("еще ", "ещё ").Replace("Еще", "Ещё");
                    else
                        stringToTextBox[counter] = line.Replace(" еще", " ещё").Replace("Еще", "Ещё");
                }
                else
                    stringToTextBox[counter] = line;

                if (line.Contains(" все равно") || line.Contains("Все равно") || line.StartsWith("все равно"))
                {
                    int indexer = 0;
                    char[] chars = line.ToCharArray();

                    foreach (char ch in chars)
                    {
                        indexer++;

                        switch (ch)
                        {
                            case 'В':
                            case 'в':
                                if (tempStringForButton1_ClickForeachCh.StartsWith("все ра"))
                                    tempStringForButton1_ClickForeachCh += ch.ToString();
                                else
                                    tempStringForButton1_ClickForeachCh += ch.ToString();
                                break;
                            case 'с':
                                if (tempStringForButton1_ClickForeachCh.StartsWith("в") || tempStringForButton1_ClickForeachCh.StartsWith("В"))
                                    tempStringForButton1_ClickForeachCh += ch.ToString();
                                else
                                    tempStringForButton1_ClickForeachCh = "";
                                break;
                            case 'е':
                                if (tempStringForButton1_ClickForeachCh.StartsWith("вс") || tempStringForButton1_ClickForeachCh.StartsWith("Вс"))
                                    tempStringForButton1_ClickForeachCh += ch.ToString();
                                else
                                    tempStringForButton1_ClickForeachCh = "";
                                break;
                            case ' ':
                            case '.':
                            case ',':
                            case 'о':
                                if (ch == 'о' && (tempStringForButton1_ClickForeachCh.StartsWith("все равн") || tempStringForButton1_ClickForeachCh.StartsWith("Все равн")))
                                {
                                    tempStringForButton1_ClickForeachCh += ch.ToString();

                                    if (chars.Length == indexer)
                                    {
                                        if (stringToTextBox[counter].Contains("все") || stringToTextBox[counter].Contains("Все"))
                                        {
                                            stringToTextBox[counter] = stringToTextBox[counter].Remove(indexer - 9, 9);
                                            stringToTextBox[counter] = stringToTextBox[counter].Insert(indexer - 9, tempStringForButton1_ClickForeachCh.Replace("все равно", "всё равно").Replace("Все равно", "Всё равно"));
                                            tempStringForButton1_ClickForeachCh = "";
                                        }
                                        else
                                        {
                                            stringToTextBox[counter] = stringToTextBox[counter].Remove(indexer - 9, 9);
                                            stringToTextBox[counter] = stringToTextBox[counter].Insert(indexer - 10, tempStringForButton1_ClickForeachCh.Replace("все равно", "всё равно").Replace("Все равно", "Всё равно"));
                                            tempStringForButton1_ClickForeachCh = "";
                                        }
                                    }
                                }

                                else if (tempStringForButton1_ClickForeachCh.StartsWith("все равно") || tempStringForButton1_ClickForeachCh.StartsWith("Все равно"))
                                {
                                    if (stringToTextBox[counter].Contains("всё") || stringToTextBox[counter].Contains("Всё"))
                                    {
                                        stringToTextBox[counter] = stringToTextBox[counter].Remove(indexer - 10, 9);
                                        stringToTextBox[counter] = stringToTextBox[counter].Insert(indexer - 10, tempStringForButton1_ClickForeachCh.Replace("все равно", "всё равно").Replace("Все равно", "Всё равно"));
                                        tempStringForButton1_ClickForeachCh = "";
                                    }
                                    else
                                    {
                                        stringToTextBox[counter] = stringToTextBox[counter].Remove(indexer - 10, 9);
                                        stringToTextBox[counter] = stringToTextBox[counter].Insert(indexer - 10, tempStringForButton1_ClickForeachCh.Replace("все равно", "всё равно").Replace("Все равно", "Всё равно"));
                                        tempStringForButton1_ClickForeachCh = "";
                                    }
                                }

                                else if ((tempStringForButton1_ClickForeachCh.StartsWith("все") || tempStringForButton1_ClickForeachCh.StartsWith("Все")) && char.IsWhiteSpace(ch))
                                    tempStringForButton1_ClickForeachCh += ch.ToString();
                                else
                                    tempStringForButton1_ClickForeachCh = "";
                                break;
                            case 'р':
                                if (tempStringForButton1_ClickForeachCh.StartsWith("все ") || tempStringForButton1_ClickForeachCh.StartsWith("Все "))
                                    tempStringForButton1_ClickForeachCh += ch.ToString();
                                else
                                    tempStringForButton1_ClickForeachCh = "";
                                break;
                            case 'а':
                                if (tempStringForButton1_ClickForeachCh.StartsWith("все р") || tempStringForButton1_ClickForeachCh.StartsWith("Все р"))
                                    tempStringForButton1_ClickForeachCh += ch.ToString();
                                else
                                    tempStringForButton1_ClickForeachCh = "";
                                break;
                            case 'н':
                                if (tempStringForButton1_ClickForeachCh.StartsWith("все рав") || tempStringForButton1_ClickForeachCh.StartsWith("Все рав"))
                                    tempStringForButton1_ClickForeachCh += ch.ToString();
                                else
                                    tempStringForButton1_ClickForeachCh = "";
                                break;
                            default:
                                tempStringForButton1_ClickForeachCh = "";
                                break;
                        }
                    }
                }

                if (lines.Length - 1 != counter)
                {
                    counter++;
                }
            }

            tempStringForTextBox1_TextChangedEqual = string.Join("\r\n", stringToTextBox);

            if (tempStringForTextBox1_TextChangedEqual.Contains("Всё равно") || tempStringForTextBox1_TextChangedEqual.Contains("всё равно") || tempStringForTextBox1_TextChangedEqual.Contains("Ещё") || tempStringForTextBox1_TextChangedEqual.Contains("ещё"))
            {
                if (button1.Text != "Исправлено")
                {
                    button1.BackColor = Color.LightGreen;
                    button1.Text = "Исправлено";
                    textBox1.Lines = stringToTextBox;
                    stringToTextBox = null;
                    await Task.Delay(2000);
                    button1.BackColor = Color.LightGreen;
                    button1.Text = "Скопировать исправленный текст";
                }
            }

            else
            {
                button1.BackColor = Color.IndianRed;
                button1.Text = "Нечего исправлять";
            }
        }

        void TextBox1_TextChanged(object sender, System.EventArgs e)
        {
            if (textBox1.Text != tempStringForTextBox1_TextChangedEqual)
            {
                button1.BackColor = SystemColors.ControlLight;
                button1.Text = "Исправить";
            }
        }
    }
}
/*  
Задание:

1. необходимо сделать программу, в которой должен быть TextBox и Button;
2. программа должна уметь сворачиваться в трей и разворачиваться обратно;
3. пользователь должен вставить в поле любую статью или цитату из интернета;
4. нажатие на кнопку «исправить» исправит все слова: еще на ещё / все равно на всё равно;
5. кнопка «исправить» должна писать «исправлено» после исправления.
6. спустя пару секунд или миллисекунд, на кнопке должно быть написано «скопировать исправленный текст»;
7. текст, исправленный в поле TextBox, должен лететь в буфер обмена;
8. кнопка пишет «скопировано в буфер обмена»;
9. если пользователь после исправления текста лупит по Button, или TextBox пуст или содержит пробел, или пользователь вбил набор символов, 
которые не соответствуют «еще / все равно», то Button должен поменять значение на «нечего исправлять»
10. при выходе из программы возникает окно, позволяющее при нажатии на «yes» сохранить текст из TextBox в файл, а на «no» закрывает программу.
*/

/*  
За пример возьмем строки:

Вовсе все равноправны все равно, все равно еще 
Вовсе все равноправны все равно, все равно еще
Вовсе все равноправны все равно, все равно еще,
Вовсе все равноправны все равно, все равно еще.
Еще еще
еще Еще
Еще все равно еще
еще все равно Еще
еще.
еще 
все равно
*/

using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Correction_of_letters_in_words
{
    public partial class Form1 : Form
    {
        string tempStringForButton1_ClickForeachCh = "", stringForTextBox1_TextChangedEqual = "";
        bool confirmsTextSaving = false;

        public Form1()
        {
            InitializeComponent();

            FormClosing += async (s, e) =>
            {
                string path = @"D:\1\CorrectionOfLettersInWords.txt";

                if (button1.Text.Contains("Исправлено") || button1.Text.Contains("Скопировать исправленный текст") || confirmsTextSaving)
                {
                    DialogResult saveOrClose = MessageBox.Show($"Сохранить исправленный текст по пути {path}?", "Сохранение текста в файл", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);

                    if (saveOrClose == DialogResult.Yes)
                    {
                        if (!Directory.Exists(path.Replace(@"1\CorrectionOfLettersInWords.txt", @"1")))
                        {
                            Directory.CreateDirectory(path.Replace(@"1\CorrectionOfLettersInWords.txt", @"1"));
                        }

                        using (StreamWriter streamWriter = new StreamWriter(path, false))
                        {
                            await streamWriter.WriteAsync(textBox1.Text);
                        }
                    }
                }
            };

            Load += (s, e) =>
             {
                 notifyIcon1.BalloonTipTitle = "Замена \"все равно / еще\" на  \"всё равно / ещё\"";
                 notifyIcon1.BalloonTipText = "Cвернуто";
                 notifyIcon1.Text = "Замена \"все равно / еще\" на  \"всё равно / ещё\"";
             };

            Resize += (s, e) =>
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    Hide();
                    notifyIcon1.Visible = true;
                    notifyIcon1.ShowBalloonTip(1000);
                }

                else if (WindowState == FormWindowState.Normal)
                    notifyIcon1.Visible = true;
            };

            notifyIcon1.MouseDoubleClick += (s, e) =>
            {
                Show();
                notifyIcon1.Visible = true;
                WindowState = FormWindowState.Normal;
            };

            textBox1.DragEnter += (s, e) =>
            {
                e.Effect = DragDropEffects.Copy;
                textBox1.Text = e.Data.GetData(DataFormats.Text).ToString();
            };

            textBox1.TextChanged += (s, e) =>
              {
                  if (textBox1.Text != stringForTextBox1_TextChangedEqual)
                  {
                      button1.BackColor = SystemColors.ControlLight;
                      button1.Text = "Исправить";
                  }
              };
        }

        async void Button1_Click(object sender, System.EventArgs e)
        {
            if (button1.Text.Contains("Скопировать исправленный текст"))
            {
                Clipboard.SetDataObject(textBox1.Text, true);

                if (Clipboard.ContainsText())
                {
                    button1.BackColor = Color.Bisque;
                    button1.Text = "Скопировано в буфер обмена";
                    await Task.Delay(800);
                    button1.BackColor = Color.LightGreen;
                    button1.Text = "Исправлено";
                    return;
                }
            }

            string[] stringToTextBox = new string[textBox1.Lines.Length];
            byte counter = 0;

            foreach (string line in textBox1.Lines)
            {
                stringToTextBox[counter] = "";

                if (line.Contains(" еще") || line.Contains("Еще") || line.StartsWith("еще"))
                {
                    if (line.StartsWith("еще") || line.StartsWith("Еще"))
                    {
                        if (line.StartsWith("еще"))
                        {
                            stringToTextBox[counter] = line.Remove(0, 3);
                            stringToTextBox[counter] = stringToTextBox[counter].Insert(0, "ещё").Replace(" Еще", " Ещё").Replace(" еще", " ещё");
                        }

                        else
                        {
                            stringToTextBox[counter] = line.Remove(0, 3);
                            stringToTextBox[counter] = stringToTextBox[counter].Insert(0, "Ещё").Replace(" Еще", " Ещё").Replace(" еще", " ещё");
                        }
                    }

                    else
                        stringToTextBox[counter] = line.Replace(" еще", " ещё").Replace(" Еще", " Ещё");

                    confirmsTextSaving = true;
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

                    confirmsTextSaving = true;
                }

                if (textBox1.Lines.Length - 1 != counter)
                    counter++;
            }

            stringForTextBox1_TextChangedEqual = string.Join("\r\n", stringToTextBox);

            if (string.IsNullOrWhiteSpace(stringForTextBox1_TextChangedEqual) || (string.Join("\r\n", stringToTextBox) == string.Join("\r\n", textBox1.Lines)))
            {
                button1.BackColor = Color.IndianRed;
                button1.Text = "Нечего исправлять";

                if (((string.Join("\r\n", stringToTextBox) == string.Join("\r\n", textBox1.Lines))
                    && !string.IsNullOrWhiteSpace(stringForTextBox1_TextChangedEqual))
                    && (stringForTextBox1_TextChangedEqual.Contains("ещё")
                    || stringForTextBox1_TextChangedEqual.Contains("Ещё")
                    || stringForTextBox1_TextChangedEqual.Contains("всё равно")
                    || stringForTextBox1_TextChangedEqual.Contains("Всё равно")))
                    confirmsTextSaving = true;
                else
                    confirmsTextSaving = false;
            }

            else
            {
                button1.BackColor = Color.LightGreen;
                button1.Text = "Исправлено";
                textBox1.Lines = stringToTextBox;
                await Task.Delay(800);
                button1.BackColor = Color.Bisque;
                button1.Text = "Скопировать исправленный текст";
                confirmsTextSaving = true;
            }
        }
    }
}
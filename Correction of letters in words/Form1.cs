/*  
Задание:

1. необходимо сделать программу, в которой должен быть TextBox и Button;
2. Пользователь должен вставить в поле любую статью или цитату из интернета;
3. Нажатие на кнопку исправит все слова: еще на ещё / все равно на всё равно;
4. Кнопка Исправить должна писать Исправлено после исправления.
5. Спустя пару секунд на кнопке должно быть написано Скопировать исправленный текст;
6. Текст, который исправлен в поле TextBox должен лететь в буфер обмена;
7. Кнопка пишет Скопировано в буфер обмена;
8. при выходе из программы возникает окно, которое позволяет при нажатии на Yes сохранить текст из TextBox в файл.
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
        byte counterClick;
        string tempStringForButton1_ClickForeachCh = "", StringForTextBox1_TextChangedEqual = "";

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
                }

                else
                {
                    stringToTextBox[counter] = line;
                }

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

            StringForTextBox1_TextChangedEqual = string.Join("\r\n", stringToTextBox);

            if (string.Join("\r\n", stringToTextBox) == string.Join("\r\n", lines))
            {
                button1.BackColor = Color.IndianRed;
                button1.Text = "Нечего исправлять";
            }
            
            else
            {
                button1.BackColor = Color.LightGreen;
                button1.Text = "Исправлено";
                textBox1.Lines = stringToTextBox;
                stringToTextBox = null;
                await Task.Delay(2000);
                button1.BackColor = Color.Bisque;
                button1.Text = "Скопировать исправленный текст";
            }
        }

        void TextBox1_TextChanged(object sender, System.EventArgs e)
        {
            if (textBox1.Text != StringForTextBox1_TextChangedEqual)
            {
                button1.BackColor = SystemColors.ControlLight;
                button1.Text = "Исправить";
            }
        }

        void TextBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
            textBox1.Text = e.Data.GetData(DataFormats.Text).ToString();
        }

        async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string path = @"D:\1\CorrectionOfLettersInWords.txt";

            if (button1.Text.Contains("Исправлено") || button1.Text.Contains("Скопировать исправленный текст"))
            {
                DialogResult saveOrClose = MessageBox.Show($"Сохранить исправленный текст по пути {path}?", "Сохранение текста в файл", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);

                if (saveOrClose == DialogResult.Yes)
                {
                    using (StreamWriter streamWriter = new StreamWriter(path, false))
                    {
                        await streamWriter.WriteAsync(textBox1.Text);
                    }
                }
            }
        }
    }
}
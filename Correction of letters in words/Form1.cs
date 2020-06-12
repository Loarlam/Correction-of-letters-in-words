/*За пример возьмем строки ниже:
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

using Correction_of_letters_in_words.Properties;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Correction_of_letters_in_words
{
    public partial class Form1 : Form
    {
        bool confirmsTextSaving;
        byte digitsInTheFileName;
        string tempStringForButton1_ClickForeachCh, stringForTextBox1_TextChangedEqual, path;
        string[] filesInFolder;
        OpenFileDialog openFileDialog;

        public Form1()
        {
            InitializeComponent();

            confirmsTextSaving = false;
            tempStringForButton1_ClickForeachCh = "";
            stringForTextBox1_TextChangedEqual = "";
            path = $@"C:\Users\{Environment.UserName}\Desktop\Fixed.txt";
            openFileDialog = new OpenFileDialog { InitialDirectory = $@"C:\Users\{Environment.UserName}\Desktop" };
        }

        async void Form1_Load(object sender, EventArgs e)
        {
            for (Opacity = 0; Opacity <= 1; Opacity += .02)
            {
                if (Opacity == 1)
                    break;
                await Task.Delay(10);
            }
        }

        void Panel1_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(SystemColors.ActiveCaption, 1);
            pen.DashStyle = DashStyle.Dash;
            e.Graphics.DrawRectangle(pen, -1, 0, panel1.Width + 1, panel1.Height);
        }

        void TextBox1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
            textBox1.Text = $"{e.Data.GetData(DataFormats.Text)}";
        }

        void Panel1_DragEnter(object sender, DragEventArgs e)
        {
            string fileName = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];

            if (fileName.Substring(fileName.IndexOf(".")) == ".txt")
            {
                panel1.BackgroundImage = Resources.Drop;
                e.Effect = DragDropEffects.Move;
            }

            else
            {
                panel1.BackgroundImage = Resources.Error;
                e.Effect = DragDropEffects.None;
            }
        }

        void Panel1_DragLeave(object sender, EventArgs e) => panel1.BackgroundImage = Resources.Drag;

        void Panel1_DragDrop(object sender, DragEventArgs e)
        {
            panel1.BackgroundImage = Resources.Drag;
            using (StreamReader streamReader = new StreamReader(((string[])e.Data.GetData(DataFormats.FileDrop))[0]))
                textBox1.Text = streamReader.ReadToEnd();
        }

        async void Panel1_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = "Открытие текстового файла";
            openFileDialog.Filter = "Текстовый файл (*.txt)|*.txt";
            openFileDialog.CheckFileExists = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(openFileDialog.FileName);
                using (StreamReader streamReader = new StreamReader(openFileDialog.OpenFile()))
                    textBox1.Text = await streamReader.ReadToEndAsync();
            }
        }

        void FixTextButton_Click(object sender, EventArgs e)
        {
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
                                    tempStringForButton1_ClickForeachCh += $"{ch}";
                                else if (tempStringForButton1_ClickForeachCh != null)
                                    tempStringForButton1_ClickForeachCh = $"{ch}";
                                else
                                    tempStringForButton1_ClickForeachCh += $"{ch}";
                                break;
                            case 'с':
                                if (tempStringForButton1_ClickForeachCh.StartsWith("в") || tempStringForButton1_ClickForeachCh.StartsWith("В"))
                                    tempStringForButton1_ClickForeachCh += $"{ch}";
                                else
                                    tempStringForButton1_ClickForeachCh = "";
                                break;
                            case 'е':
                                if (tempStringForButton1_ClickForeachCh.StartsWith("вс") || tempStringForButton1_ClickForeachCh.StartsWith("Вс"))
                                    tempStringForButton1_ClickForeachCh += $"{ch}";
                                else
                                    tempStringForButton1_ClickForeachCh = "";
                                break;
                            case ' ':
                            case '.':
                            case ',':
                            case 'о':
                                if (ch == 'о' && (tempStringForButton1_ClickForeachCh.StartsWith("все равн") || tempStringForButton1_ClickForeachCh.StartsWith("Все равн")))
                                {
                                    tempStringForButton1_ClickForeachCh += $"{ch}";

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
                                    tempStringForButton1_ClickForeachCh += $"{ch}";
                                else
                                    tempStringForButton1_ClickForeachCh = "";
                                break;
                            case 'р':
                                if (tempStringForButton1_ClickForeachCh.StartsWith("все ") || tempStringForButton1_ClickForeachCh.StartsWith("Все "))
                                    tempStringForButton1_ClickForeachCh += $"{ch}";
                                else
                                    tempStringForButton1_ClickForeachCh = "";
                                break;
                            case 'а':
                                if (tempStringForButton1_ClickForeachCh.StartsWith("все р") || tempStringForButton1_ClickForeachCh.StartsWith("Все р"))
                                    tempStringForButton1_ClickForeachCh += $"{ch}";
                                else
                                    tempStringForButton1_ClickForeachCh = "";
                                break;
                            case 'н':
                                if (tempStringForButton1_ClickForeachCh.StartsWith("все рав") || tempStringForButton1_ClickForeachCh.StartsWith("Все рав"))
                                    tempStringForButton1_ClickForeachCh += $"{ch}";
                                else
                                    tempStringForButton1_ClickForeachCh = "";
                                break;
                            default:
                                tempStringForButton1_ClickForeachCh = "";
                                break;
                        }
                    }
                }

                if (textBox1.Lines.Length - 1 != counter)
                    counter++;
            }

            stringForTextBox1_TextChangedEqual = string.Join("\r\n", stringToTextBox);

            if (string.IsNullOrWhiteSpace(stringForTextBox1_TextChangedEqual) || (stringForTextBox1_TextChangedEqual == string.Join("\r\n", textBox1.Lines)))
            {
                if (!string.IsNullOrWhiteSpace(stringForTextBox1_TextChangedEqual)
                    && (textBox1.Text.Contains("ещё") || textBox1.Text.Contains("Ещё") || textBox1.Text.Contains("всё равно") || textBox1.Text.Contains("Всё равно")))
                    MaybeSaveTextToClipboardAndChangeForm(true, Color.IndianRed, "Нечего исправлять");
                else
                    MaybeSaveTextToClipboardAndChangeForm(false, Color.IndianRed, "Нечего исправлять");
            }
            else
                SaveTextToClipboardAndChangeForm(stringToTextBox);
        }

        void MaybeSaveTextToClipboardAndChangeForm(bool yesOrNo, Color colorToBackColor, string textToButtonText)
        {
            confirmsTextSaving = yesOrNo;
            BackColor = colorToBackColor;
            fixTextButton.BackColor = Color.White;
            fixTextButton.Text = textToButtonText;
        }

        async void SaveTextToClipboardAndChangeForm(string[] arrayOfStringAfterReplacement)
        {
            confirmsTextSaving = true;
            BackColor = Color.LightGreen;
            fixTextButton.BackColor = Color.White;
            fixTextButton.Text = "Исправлено";
            textBox1.Lines = arrayOfStringAfterReplacement;
            Clipboard.SetDataObject(textBox1.Text, true);

            await Task.Delay(800);

            if (textBox1.Text == stringForTextBox1_TextChangedEqual)
            {
                fixTextButton.BackColor = Color.Bisque;
                fixTextButton.Text = "Скопировано в буфер";
            }

            await Task.Delay(800);

            if (textBox1.Text == stringForTextBox1_TextChangedEqual)
            {
                BackColor = Color.LightGreen;
                fixTextButton.BackColor = Color.White;
                fixTextButton.Text = "Исправлено";
            }
        }

        void TextBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != stringForTextBox1_TextChangedEqual)
            {
                BackColor = SystemColors.Window;
                fixTextButton.BackColor = SystemColors.ControlLight;
                fixTextButton.Text = "Исправить";
                return;
            }
        }

        void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;
        }

        void CloseToolStripMenuItem_Click(object sender, EventArgs e) => Close();

        async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((fixTextButton.Text.Contains("Исправлено")
                || (fixTextButton.Text.Contains("Нечего исправлять") && confirmsTextSaving))
                || (textBox1.Text.Contains("ещё") || textBox1.Text.Contains("Ещё") || textBox1.Text.Contains("всё равно") || textBox1.Text.Contains("Всё равно")))
            {
                filesInFolder = Directory.GetFiles($@"C:\Users\{Environment.UserName}\Desktop", "Fixed*");

                if (filesInFolder.Any())
                {
                    if (filesInFolder[filesInFolder.Length - 1].Contains("Fixed.txt"))
                        path = $@"C:\Users\{Environment.UserName}\Desktop\Fixed1.txt";
                    else
                    {
                        filesInFolder[filesInFolder.Length - 1] = filesInFolder[filesInFolder.Length - 1].Substring(filesInFolder[filesInFolder.Length - 1].IndexOf('d') + 1);
                        digitsInTheFileName = Convert.ToByte(filesInFolder[filesInFolder.Length - 1].Remove(filesInFolder[filesInFolder.Length - 1].IndexOf('.')));
                        path = $@"C:\Users\{Environment.UserName}\Desktop\Fixed{++digitsInTheFileName}.txt";
                    }
                }

                DialogResult saveOrClose = MessageBox.Show($"Сохранить текст по пути {path}?", "Сохранение текста в файл", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);

                if (saveOrClose == DialogResult.Yes)
                {
                    using (StreamWriter streamWriter = new StreamWriter(path))
                    {
                        FixTextButton_Click(sender, e);
                        Clipboard.SetDataObject(textBox1.Text, true);
                        await streamWriter.WriteAsync(Clipboard.GetText(TextDataFormat.UnicodeText));
                    }

                    Process.Start(path);
                }
            }
        }
    }
}
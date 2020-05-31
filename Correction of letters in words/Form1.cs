﻿/*  
Задание:

1. сделать плавное появление окна при запуске программы;
2. необходимо сделать программу, в которой должен быть TextBox, Button, NotifyIcon, ContextMenuStrip, Icon, ScrollBar;
3. программа должна уметь сворачиваться в трей и разворачиваться обратно;
4. пользователь должен вставить в поле любую статью или цитату из интернета;
5. нажатие на кнопку «Исправить» заменяет слова "еще на ещё" / "все равно на всё равно";
6. кнопка «Исправить» должна менять текст на «Исправлено» после исправления, а также меняются цвет Button на White и Form.BackColor на LightGreen;
7. спустя пару секунд или миллисекунд на кнопке должно быть написано «Скопировать исправленный текст», а цвет Button меняется на Bisque;
8. текст, исправленный в поле TextBox, должен лететь в буфер обмена;
9. кнопка пишет «Скопировано в буфер обмена», при этом цвет Button остаётся Bisque;
10. Button должен поменять значение на «Нечего исправлять», а также меняются цвет Button на White и Form.BackColor на IndianRed, если: текст уже исправлен, 
а пользователь лупит по Button повторно; TextBox пуст или содержит только пробел/ пробелы; 
пользователь вбил набор символов, которые не соответствуют «еще / все равно»;
11. если пользователь решает внести изменения в TextBox, то Button меняет текст на "Исправить", а также меняются цвет Button на ColorLight и Form.BackColor на White;
12. выход из программы возможен по нажатию на кнопку закрытия формы, либо же кликом по полю "Закрыть" иконки в трей, либо же по ALT+F4;
13. перед закрытием программы возникает окно, позволяющее при нажатии на «Да» сохранить текст из TextBox в файл на рабочий стол пользователя, а на «Нет» закрывает программу;
14. если же пользователь ранее пользовался программой и имел сохраненный Fixed.txt на рабочем столе, 
то при повторном использовании программы, и попытке сохранения результатов, создается новый файл, но с инкрементальной нумерацией в один шаг в названии файла.
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

using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Correction_of_letters_in_words
{
    public partial class Form1 : Form
    {
        bool confirmsTextSaving = false;
        byte digitsInTheFileName;
        string tempStringForButton1_ClickForeachCh = "", stringForTextBox1_TextChangedEqual = "", path = $@"C:\Users\{Environment.UserName}\Desktop\Fixed.txt";
        string[] filesInFolder;

        public Form1()
        {
            InitializeComponent();

            Load += async (s, e) =>
             {
                 for (Opacity = 0; Opacity <= 1; Opacity += .02)
                     await Task.Delay(10);

                 notifyIcon1.BalloonTipTitle = "Замена \"все равно / еще\" на  \"всё равно / ещё\"";
                 notifyIcon1.BalloonTipText = "Cвернуто";
                 notifyIcon1.Text = "Замена \"все равно / еще\" на  \"всё равно / ещё\"";

             };

            FormClosing += async (s, e) =>
            {
                if (button1.Text.Contains("Исправлено") || button1.Text.Contains("Скопировать исправленный текст") || confirmsTextSaving)
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

                    DialogResult saveOrClose = MessageBox.Show($"Сохранить исправленный текст по пути {path}?", "Сохранение текста в файл", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);

                    if (saveOrClose == DialogResult.Yes)
                    {
                        using (StreamWriter streamWriter = new StreamWriter(path, false))
                            await streamWriter.WriteAsync(textBox1.Text);
                    }
                }
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
                     BackColor = SystemColors.Window;
                     button1.BackColor = SystemColors.ControlLight;
                     button1.Text = "Исправить";
                 }
             };

            closeToolStripMenuItem.Click += (s, e) => Close();
        }

        async void Button1_Click(object sender, EventArgs e)
        {
            if (button1.Text.Contains("Скопировать исправленный текст"))
            {
                Clipboard.SetDataObject(textBox1.Text, true);

                if (Clipboard.ContainsText())
                {
                    button1.BackColor = Color.Bisque;
                    button1.Text = "Скопировано в буфер обмена";
                    await Task.Delay(800);
                    button1.BackColor = Color.White;
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
                                else if (tempStringForButton1_ClickForeachCh != null)
                                    tempStringForButton1_ClickForeachCh = ch.ToString();
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
                BackColor = Color.IndianRed;
                button1.BackColor = Color.White;
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
                BackColor = Color.LightGreen;
                button1.BackColor = Color.White;
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
# Correction-of-letters-in-words
Задание:
>1. создать программу, в которой должен быть TextBox, Button, NotifyIcon, ContextMenuStrip, Icon, ScrollBar, Panel, OpenFileDialog;
>2. реализовать плавное появление окна при запуске программы;
>3. программа должна уметь разворачиваться  из трей;
>4. по нажатию на кнопку сворачивания, окно должно оставаться в панели задач;
>5. пользователь должен иметь возможность добавить в TextBox любой текст, следующими методами: копипастой;  перетаскиванием текста в TextBox; перетаскиванием в Panel файла с текстом; нажатием на Panel с последующим выбором файла в OpenFileDialog, в формате .txt;
>6. во время вхождения файла в границы Panel, графическая подсказка в Panel заменяется на иную, а при выходе за границы - возвращается на дефолтную;
>7. если пользователь отпустил файл внутри Panel, или же выбрал файл посредством клик в Panel, то текст из файла загружается в TextBox, при этом предыдущий текст, расположенный в TextBox, заменяется текстом из файла;
>8. пользователь должен наблюдать графическое предупреждение о неверном формате файла при перетаскивании в Panel;
>9. нажатие на кнопку «Исправить» заменяет слова "еще на ещё" / "все равно на всё равно";
>10. после исправления текста кнопка «Исправить» должна менять текст на «Исправлено», а также меняются цвет Button на White и Form.BackColor на LightGreen;
>11. после задержки времени текст в Button должен замениться на «Скопировано в буфер», а цвет Button на Bisque;
>12. текст, исправленный в поле TextBox, должен лететь в буфер обмена;
>13. после задержки времени текст в Button должен замениться на «Исправлено», а цвет Button на White;
>14. Button должен поменять значение на «Нечего исправлять», а также меняются цвет Button на White и Form.BackColor на IndianRed, если: TextBox пуст или содержит только пробел / пробелы; пользователь вбил набор символов, которые не соответствуют «еще / все равно»;
>15. если пользователь решает внести изменения в TextBox, то Button меняет текст на "Исправить", а также меняются цвет Button на ColorLight и Form.BackColor на White;
>16. выход из программы возможен по нажатию на кнопку закрытия формы, либо кликом по полю "Закрыть" иконки в трей, либо по ALT+F4;
>17. перед закрытием программы возникает окно, позволяющее при нажатии на «Да» запустить повторную проверку текста, введенного в TextBox, и сохранить текст в файл на рабочем столе пользователя, а на «Нет» закрывает программу;
>18. если же пользователь ранее пользовался программой и имел сохраненный Fixed.txt на рабочем столе, то при повторном использовании программы, и попытке сохранения результатов, создается новый файл, но с инкрементальной нумерацией в один шаг в названии файла.

Ограничение:
>1. требуется .NET Framework 4.7.2 и выше.

Подытог: вот такенный майндфак на выходе
>![](Correction%20of%20letters%20in%20words/Program_output.jpg)

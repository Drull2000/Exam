using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        Console.WriteLine("Выберите режим:");
        Console.WriteLine("1 - GUI (окно)");
        Console.WriteLine("2 - Консоль");

        string choice = Console.ReadLine();

        if (choice == "1")
        {
            // 👉 запуск интерфейса
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        else if (choice == "2")
        {
            RunConsoleMode();
        }
        else
        {
            Console.WriteLine("Неверный выбор");
        }
    }
    static void RunConsoleMode()
    {
        Console.WriteLine("Введите путь к файлу или папке:");
        string path = Console.ReadLine();

        Console.WriteLine("Введите запрещенные слова через запятую:");
        string[] words = Console.ReadLine().Split(',');

        string output = @"C:\ForbiddenResults";
        Directory.CreateDirectory(output);

        Console.WriteLine("Сканирование...");

        Scan(path, words, output);

        Console.WriteLine("Готово!");
    }
    static void Scan(string path, string[] words, string output)
    {
        if (File.Exists(path))
        {
            ProcessFile(path, words, output);
        }
        else if (Directory.Exists(path))
        {
            foreach (var file in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
            {
                ProcessFile(file, words, output);
            }
        }
    }
    static void ProcessFile(string file, string[] words, string output)
    {
        try
        {
            if (!file.EndsWith(".txt")) return;

            string text = File.ReadAllText(file);
            bool found = false;

            foreach (var word in words)
            {
                if (text.Contains(word, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    text = Regex.Replace(text, word, "*******", RegexOptions.IgnoreCase);
                }
            }

            if (found)
            {
                string name = Path.GetFileName(file);

                File.Copy(file, Path.Combine(output, name), true);
                File.WriteAllText(Path.Combine(output, "mod_" + name), text);

                Console.WriteLine($"Найдено: {file}");
            }
        }
        catch { }
    }
}

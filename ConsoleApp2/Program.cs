using System;
using System.IO;
using System.Text.RegularExpressions;
using ForbiddenWordsScanner; // 👈 твой UI проект

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("=== MENU ===");
            Console.WriteLine("1 - Консоль");
            Console.WriteLine("2 - Интерфейс");
            Console.WriteLine("0 - Выход");
            Console.Write("Выбор: ");

            string choice = Console.ReadLine();

            if (choice == "1")
            {
                RunConsole();
            }
            else if (choice == "2")
            {
                RunUI(); // 👈 ВОТ ОНА КНОПКА
            }
            else if (choice == "0")
            {
                break;
            }
        }
    }

    static void RunUI()
    {
        System.Windows.Forms.Application.EnableVisualStyles();
        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

        System.Windows.Forms.Application.Run(new Form1());
    }

    static void RunConsole()
    {
        Console.WriteLine("Console mode...");

        Console.Write("Path: ");
        string path = Console.ReadLine();

        Console.Write("Words: ");
        string[] words = Console.ReadLine().Split(',');

        string output = @"C:\ForbiddenResults";
        Directory.CreateDirectory(output);

        Scan(path, words, output);

        Console.WriteLine("Done!");
        Console.ReadLine();
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

                Console.WriteLine("Found: " + file);
            }
        }
        catch { }
    }
}
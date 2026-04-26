using ForbiddenWordsScanner;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace YourProject
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            
            Console.WriteLine("=== ВЫБЕРИ РЕЖИМ ===");
            Console.WriteLine("1 - Интерфейс (GUI)");
            Console.WriteLine("2 - Консольный режим");
            Console.Write("Ваш выбор: ");

            string choice = Console.ReadLine();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (choice == "1")
            {
                // 👉 GUI режим
                Application.Run(new Form1());
            }
            else if (choice == "2")
            {
                // 👉 консольный режим
                RunConsole();
            }
            else
            {
                Console.WriteLine("Неверный выбор");
                Console.ReadLine();
            }
        }

        static void RunConsole()
        {
            Console.WriteLine("\n=== CONSOLE MODE ===");

            Console.Write("Путь к файлу/папке: ");
            string path = Console.ReadLine();

            Console.Write("Запрещённые слова (через ,): ");
            string[] words = Console.ReadLine().Split(',');

            string output = @"C:\ForbiddenResults";
            Directory.CreateDirectory(output);

            Scan(path, words, output);

            Console.WriteLine("\nГотово!");
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

                    Console.WriteLine("Найден: " + file);
                }
            }
            catch { }
        }
    }
}
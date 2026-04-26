using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ForbiddenWordsScanner
{// главная форма
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        CancellationTokenSource cts;
        ManualResetEventSlim pauseEvent = new(true);

        int processedFiles = 0;
        int currentFiles = 0;
        int totalFiles = 0;

        object lockObj = new();

        Dictionary<string, int> wordStats = new();
        // кнопка старта
        private async void knopkaStart_Click(object sender, EventArgs e)
        {
            cts = new CancellationTokenSource();

            string[] words = txtWords.Text
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            string path = txtPath.Text;

            if (!Directory.Exists(path) && !File.Exists(path))
            {
                MessageBox.Show("Неверный путь!");
                return;
            }

            string outputFolder = @"C:\Resultati";
            Directory.CreateDirectory(outputFolder);

            lblStatus.Text = "Работаем Братья!...";

            // очистка логов и статистики перед стартом
            listBoxLog.Items.Clear();
            wordStats.Clear();
            processedFiles = 0;
            currentFiles = 0;

            // подсчет всех файлов для прогресс бара
            totalFiles = File.Exists(path)
                ? 1
                : CountFiles(path, cts.Token);

            progressBar1.Minimum = 0;
            progressBar1.Maximum = totalFiles;
            progressBar1.Value = 0;
           
            await Task.Run(() =>
            {
                if (File.Exists(path))
                {
                    ProcessFile(path, words, outputFolder);
                }
                else
                {
                    ScanDirectory(path, words, outputFolder, cts.Token);
                }
            });
            
            GenerateReport(outputFolder);
            // обновление статуса и логов после завершения
            this.Invoke((Delegate)(() =>
            {
                lblStatus.Text = "Готово!";
                listBoxLog.Items.Add("✔ Завершено");
            }));
        }
        // кнопка паузы и возобновления
        private void knopkaPaysa_Click(object sender, EventArgs e)
        {
            if (pauseEvent.IsSet)
            {
                pauseEvent.Reset();
                lblStatus.Text = "Пауза";
            }
            else
            {
                pauseEvent.Set();
                lblStatus.Text = "Работаем!";
            }
        }
        // кнопка чтобы остановить братву
        private void knopkaOstan_Click(object sender, EventArgs e)
        {
            cts.Cancel();
            lblStatus.Text = "Остановлено!";
        }
        // подсчет всех файлов для прогресс бара
        int CountFiles(string path, CancellationToken token)
        {
            int count = 0;

            try
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    if (file.EndsWith(".txt") || file.EndsWith(".log"))
                        count++;
                }

                foreach (var dir in Directory.GetDirectories(path))
                {
                    count += CountFiles(dir, token);
                }
            }
            catch { }

            return count;
        }
        // обход всех папок, который чел изначально написал в txtbox
        void ScanDirectory(string path, string[] words, string output, CancellationToken token)
        {
            if (token.IsCancellationRequested) return;

            pauseEvent.Wait();

            try
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    ProcessFile(file, words, output);
                }

                foreach (var dir in Directory.GetDirectories(path))
                {
                    ScanDirectory(dir, words, output, token);
                }
            }
            catch { }
        }
        // обработка каждого файла
        void ProcessFile(string file, string[] words, string output)
        {
            try
            {
                if (!file.EndsWith(".txt") && !file.EndsWith(".log"))
                    return;

                string text = File.ReadAllText(file);
                bool found = false;

                foreach (var word in words)
                {
                    if (text.Contains(word, StringComparison.OrdinalIgnoreCase))
                    {
                        found = true;

                        int count = Regex.Matches(text, word, RegexOptions.IgnoreCase).Count;
                        // замена слов на звезды фраерские
                        text = Regex.Replace(text, word, "*******", RegexOptions.IgnoreCase);

                        lock (lockObj)
                        {
                            if (!wordStats.ContainsKey(word))
                                wordStats[word] = 0;

                            wordStats[word] += count;
                        }
                    }
                }

                if (found)
                {
                    string name = Path.GetFileName(file);

                    string copyPath = Path.Combine(output, name);
                    File.Copy(file, copyPath, true);

                    string modifiedPath = Path.Combine(output, "mod_" + name);
                    File.WriteAllText(modifiedPath, text);
                }

                lock (lockObj)
                {
                    processedFiles++;
                    currentFiles++;
                }

                this.Invoke((Delegate)(() =>
                {
                    listBoxLog.Items.Add($"Проверен: {file}");

                    if (currentFiles <= progressBar1.Maximum)
                        progressBar1.Value = currentFiles;
                }));
            }
            catch { }
        }
        // генерация отчета
        void GenerateReport(string output)
        {
            string reportPath = Path.Combine(output, "report.txt");

            var topWords = wordStats
                .OrderByDescending(x => x.Value)
                .Take(10);

            List<string> lines = new();
            // добавляем статистику по каждому слову
            lines.Add("ТОП слова:");

            foreach (var w in topWords)
            {
                lines.Add($"{w.Key} - {w.Value}");
            }

            File.WriteAllLines(reportPath, lines);

            this.Invoke((Delegate)(() =>
            {
                listBoxLog.Items.Add("📄 Отчёт создан. Его можно найти в C:Resultati");
            }));
        }
        // браузер папок и фалов
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    txtPath.Text = folderDialog.SelectedPath;
                }
            }

            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtPath.Text = fileDialog.FileName;
                }
            }
        }
        // лог выбора файла или папки
        private void listBoxFiles_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // инициал при форме 
        }

        // спасибо за внимание, Володимир! Где то что то подсмотрел, говорю сразу, но как и обещал, я все нейронкой не пишу, есть своя логика в бащке. Хороших вам выходных, и спасибо за то что читаете это!


    }
}

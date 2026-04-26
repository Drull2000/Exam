using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ForbiddenWordsScanner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        CancellationTokenSource cts;
        ManualResetEventSlim pauseEvent = new(true);

        int processedFiles = 0;
        object lockObj = new();

        Dictionary<string, int> wordStats = new();

        private void Form1_Load(object sender, EventArgs e)
        {

        }

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

            string outputFolder = @"C:\ForbiddenResults";
            Directory.CreateDirectory(outputFolder);

            lblStatus.Text = "Працює...";

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

            lblStatus.Text = "Готово!";
        }

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
                lblStatus.Text = "Работаем братва!";
            }
        }

        private void knopkaOstan_Click(object sender, EventArgs e)
        {
            cts.Cancel();
            lblStatus.Text = "Остановили негадяя!";
        }
        void ScanAllDrives(string[] words, string output, CancellationToken token)
        {
            var drives = DriveInfo.GetDrives()
                .Where(d => d.IsReady);

            Parallel.ForEach(drives, drive =>
            {
                ScanDirectory(drive.RootDirectory.FullName, words, output, token);
            });
        }
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
        void ProcessFile(string file, string[] words, string output)
        {
            try
            {
                // эт фильтр, тут ии кста дополняет текст, вот (и не нужно его менять, он просто отсекает все файлы, которые не являются текстовыми) <-- ИИ в Скобках
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

                    UpdateUI(file);
                }

                lock (lockObj)
                {
                    processedFiles++;
                }
            }
            catch { }
        }
        void UpdateUI(string file)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateUI(file)));
                return;
            }

            listBoxFiles.Items.Add(file);

            if (progressBar1.Value < 100)
                progressBar1.Value++;
        }
        void GenerateReport(string output)
        {
            string reportPath = Path.Combine(output, "report.txt");

            var topWords = wordStats
                .OrderByDescending(x => x.Value)
                .Take(10);

            List<string> lines = new();

            lines.Add("ТОП 10 словечек:");

            foreach (var w in topWords)
            {
                lines.Add($"{w.Key} - {w.Value}");
            }

            File.WriteAllLines(reportPath, lines);
        }

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

        private void knopkaConsole_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ExecutablePath);
        }
    }
}


using System.Text.Json;
using System.Text.RegularExpressions;

using System.Net;
using System.Web;
using MetroFramework.Controls;
using System.Speech.Synthesis;
using System.Globalization;
using System.Media;
using BitMiracle.Docotic.Pdf;
using GemBox.Document;

namespace Eesti_Keel
{
    public partial class Main : MetroFramework.Forms.MetroForm
    {
        bool shift = false;
        bool caps = false;
        bool isruRnd = false;
        string wordRnd;
        int indexRnd;
        bool RussiaKeyBoard;
        bool SoundKeys = true;
        bool Writer = true;//true et , false ru
        List<Words> words = new List<Words>();
        Dictionary<string, string> Language_ET_RU = new Dictionary<string, string>(){
            {"й","q"}, {"ц","w"},{"у","e"}, {"к","r"}, {"е","t"},{"н","y"},{"г","u"},{"ш","i"},{"щ","o"},{"з","p"},{"х","ü"},{"ъ","õ"},{"ф","a"},
            {"ы","s"},{"в","d"},{"а","f"},{"п","g"},{"р","h"},{"о","j"},{"л","k"},{"д","l"},{"ж","ö"},{"э","ä"},{"я","z"},{"ч","x"},{"с","c"},
            {"м","v"},{"и","b"},{"т","n"},{"ь","m"},{"б",""},{"ю",""}};
        Dictionary<string, string> Small_Big = new Dictionary<string, string>(){
            {"Q","q"}, {"W","w"},{"E","e"}, {"R","r"}, {"T","t"},{"Y","y"},{"U","u"},{"I","i"},
            {"O","o"},{"P","p"},{"Ü","ü"},{"Õ","õ"},{"A","a"},
            {"S","s"},{"D","d"},{"F","f"},{"G","g"},{"H","h"},
            {"J","j"},{"K","k"},{"L","l"},{"Ö","ö"},{"Ä","ä"},{"Z","z"},{"X","x"},{"C","c"},
            {"V","v"},{"B","b"},{"N","n"},{"M","m"} };
        public MetroButton[] RB;
        public MetroButton[] Buttons;
        public MetroButton[] RndButtons;
        public MetroButton[] ReiterationButtons;
        //qwertyuiopüõasdfghjklöäzxcvbnm
        public Main()
        {
            InitializeComponent();
            LoadWords("Words.json");
            FillListButtons();
            //metroCheckBox4.Checked = true;
            metroCheckBox5.Checked = true;
            SetStartBackgroundImage();
            button1.BackColor = System.Drawing.Color.FromArgb(34, 34, 34);
            button2.BackColor = System.Drawing.Color.FromArgb(34, 34, 34);
            button3.BackColor = System.Drawing.Color.FromArgb(34, 34, 34);
            button4.BackColor = System.Drawing.Color.FromArgb(34, 34, 34);
            //First();
            metroTextBox2.KeyPress += TextBoxExamination;
            metroTextBox3.KeyPress += TextBoxExaminationru;
            metroTextBox6.KeyPress += TextBoxFalse;
            metroTextBox7.KeyPress += TextBoxFalse;
            metroTextBox9.KeyPress += TextBoxFalse;
            metroCheckBox1.CheckedChanged += metroCheckBox1CheckedChanged;
            metroTabControl1.Selected += SelectTabDictionary;
            metroTextBox4.TextChanged += SearchWordinDictionary;
            metroTextBox1.TextChanged += Translate;
            listBox1.BackColor = System.Drawing.Color.FromArgb(34, 34, 34);
        }
        private void FillListButtons()
        {
            RB = new[] { RB1 , RB2 , RB3 , RB4 , RB5 , RB6 , RB7, RB8 , RB9 , RB10 , RB11 , RB12, RB13, RB14, RB15,
            RB16, RB17,RB18,RB19 , RB20,RB21,RB22,RB23,RB24,RB25,RB26,RB27,RB28 ,RB29 , RB30,RB31,RB32};

            Buttons = new[] { metroButton1 , metroButton2 , metroButton3 , metroButton4 , metroButton5 ,
                metroButton6 , metroButton7, metroButton8 , metroButton9 , metroButton10
                , metroButton11 , metroButton12, metroButton13, metroButton14, metroButton15,
            metroButton16, metroButton17,metroButton18,metroButton19 , metroButton20,metroButton21,
                metroButton22,metroButton23,metroButton24,metroButton25,metroButton26,metroButton27,metroButton28
                ,metroButton29 , metroButton30,metroButton31,metroButton32};
            RndButtons = new[] { metroButton76, metroButton77, metroButton78, metroButton79, metroButton80
            ,metroButton81 , metroButton82 , metroButton83,metroButton84,metroButton85,metroButton86,metroButton87,
            metroButton88,metroButton89,metroButton90,metroButton91,metroButton92,metroButton93,metroButton94
            ,metroButton95,metroButton96,metroButton97,metroButton98,metroButton99,metroButton100,metroButton101,
            metroButton102,metroButton103,metroButton104,metroButton105};
            ReiterationButtons = new[] { metroButton108, metroButton109, metroButton110, metroButton111,
            metroButton112,metroButton113};
        }
        private void TextBoxFalse(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void Bigkeys()
        {
            foreach (var c in Buttons)
            {
                foreach (var b in Small_Big)
                {
                    if (c.Text == b.Value)
                    {
                        c.Text = b.Key;
                        break;
                    }
                }
            }
        }
        void metroCheckBox1CheckedChanged(object sender, EventArgs e)
        {
            if (metroCheckBox1.Checked == true)
            {
                RussiaKeyBoard = true;
                RussiaKeys();
            }
            else
            {
                RussiaKeyBoard = false;
                EestiKeysRB();
            }

        }
        private void EestiKeysRB()
        {
            foreach (var b in RB)
            {
                foreach (var c in Language_ET_RU)
                {
                    if (b.Text == c.Key)
                    {
                        b.Text = c.Value;
                        break;
                    }
                }
            }
            RB31.Visible = false;
            RB32.Visible = false;
        }
        private void RussiaKeys(/*object sender, EventArgs e*/)
        {
            foreach (var b in RB)
            {
                foreach (var c in Language_ET_RU)
                {
                    if (b.Text == c.Value)
                    {
                        b.Text = c.Key;
                        break;
                    }
                }
            }
            RB31.Visible = true;
            RB32.Visible = true;
        }
        private void SelectTabDictionary(object sender, EventArgs e)
        {

            if (metroTabControl1.SelectedIndex == 2)
            {
                FullListBox();
            }
            if (metroTabControl1.SelectedIndex == 3)
            {
                RndGame();
            }
            if (metroTabControl1.SelectedIndex == 4)
            {
                ReiterationGame();
            }
        }
        void ReiterationBut(object sender, EventArgs e)
        {
            Random rnd = new Random();
            int rndid;
            Button btn = (Button)sender;
            SoundButtonClick();
            if (btn.Text != string.Empty)
            {
                if (Convert.ToChar(btn.Text) == wordRnd[indexRnd])
                {
                    metroTextBox8.Text += btn.Text;

                    if (indexRnd == wordRnd.Length - 1)
                    {
                        metroTextBox8.Text = string.Empty;
                        metroLabel11.Visible = false;
                        ReiterationGame();
                    }
                    else
                    {
                        indexRnd++;
                        rndid = rnd.Next(0, wordRnd.Length - 1);
                        ReiterationButtons[rndid].Text = wordRnd[indexRnd].ToString();
                        foreach (var b in ReiterationButtons)
                        {
                            rndid = rnd.Next(0, Language_ET_RU.Count - 3);
                            var element = Language_ET_RU.ElementAt(rndid).Value;
                            if (Convert.ToChar(element) != wordRnd[indexRnd])
                            {
                                if (b.Text != wordRnd[indexRnd].ToString() || b.Text == string.Empty)
                                    b.Text = element;
                            }
                        }
                        metroLabel11.Text = "Correctly!";
                        metroLabel11.Style = MetroFramework.MetroColorStyle.Green;
                        metroLabel11.Visible = true;
                    }

                }
                else
                {
                    metroLabel11.Text = "Wrong!";
                    metroLabel11.Style = MetroFramework.MetroColorStyle.Red;
                    metroLabel11.Visible = true;
                }
            }

        }
        void ReiterationGame()
        {
            Random rnd = new Random();
            int rndid;
            indexRnd = 0;
            metroLabel11.Visible = false;
            if (words.Count > 0)
            {
                rndid = rnd.Next(0, words.Count - 1);
                wordRnd = words[rndid].word;
                string translation = words[rndid].translate;
                metroTextBox9.Text = translation;
                rndid = rnd.Next(0, 6);
                ReiterationButtons[rndid].Text = wordRnd[0].ToString();
                foreach (var b in ReiterationButtons)
                {
                    rndid = rnd.Next(0, Language_ET_RU.Count - 3);
                    var element = Language_ET_RU.ElementAt(rndid).Value;
                    if (Convert.ToChar(element) != wordRnd[indexRnd])
                    {
                        if (b.Text != wordRnd[indexRnd].ToString() || b.Text == string.Empty)
                        {
                            rndid = rnd.Next(0, 6);
                            b.Text = element;
                        }
                    }
                }
            }

        }
        void RndGame()
        {
            Random rnd = new Random();
            if (words.Count > 0)
            {
                int idex = rnd.Next(0, words.Count - 1);
                if (metroCheckBox2.Checked == false)
                {
                    metroTextBox6.Text = words[idex].translate;
                }
                else
                {

                    isruRnd = true;
                    metroTextBox7.Text = words[idex].word;
                }
            }

        }
        private void FullListBox()
        {
            listBox1.Items.Clear();
            foreach (var w in words)
            {
                listBox1.Items.Add($"{w.word} - {w.translate}");
            }
        }
        private void SearchWordinDictionary(object sender, EventArgs e)
        {
            string str = metroTextBox4.Text;
            int i = 0;
            foreach (string s in listBox1.Items)
            {
                i++;
                if (s.Contains(str))
                {
                    break;
                }
                else
                {
                    //listBox1.Items.Remove(str);
                }

            }
            if (i > 0)
                listBox1.SetSelected(i - 1, true);
            i = 0;
        }
        private void LoadWords(string name)
        {
            try
            {
                using (FileStream fs = new FileStream(name, FileMode.OpenOrCreate))
                {
                    words = JsonSerializer.Deserialize<List<Words>>(fs);
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void RndKeyBoard(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            metroLabel8.Visible = false;
            if (isruRnd == false)
            {

                metroTextBox7.Text += btn.Text;
                foreach (var c in words)
                {
                    if (c.translate == metroTextBox6.Text)
                    {
                        if (c.word == metroTextBox7.Text)
                        {
                            metroTextBox7.Text = string.Empty;
                            metroTextBox6.Text = string.Empty;
                            metroLabel8.Visible = true;
                            RndGame();
                        }
                    }
                }
            }
            else
            {
                metroTextBox6.Text += btn.Text;
                foreach (var c in words)
                {
                    if (c.word == metroTextBox7.Text)
                    {
                        if (c.translate == metroTextBox6.Text)
                        {
                            metroTextBox7.Text = string.Empty;
                            metroTextBox6.Text = string.Empty;
                            metroLabel8.Visible = true;
                            RndGame();
                        }
                    }
                }
            }
            SoundButtonClick();
        }

        private void KeyboardClick(object sender, EventArgs e)
        {
            Writer = true;
            Button btn = (Button)sender;
            metroTextBox1.Text += btn.Text;
            if (shift)
            {
                SmallKeys();
                shift = false;
            }
            SoundButtonClick();
        }

        private void metroButton33_Click(object sender, EventArgs e)
        {
            metroTextBox1.Text += " ";
            SoundButtonClick();
        }

        private void metroButton32_Click(object sender, EventArgs e)
        {
            shift = true;
            Bigkeys();
            SoundButtonClick();
        }

        private void SmallKeys()
        {
            foreach (var c in Buttons)
            {
                foreach (var b in Small_Big)
                {
                    if (c.Text == b.Key)
                    {
                        c.Text = b.Value;
                        break;
                    }
                }
            }
        }


        private void metroButton13_Click(object sender, EventArgs e)
        {
            if (caps)
            {
                SmallKeys();
                caps = false;
            }
            else
            {
                Bigkeys();
                caps = true;
            }
            SoundButtonClick();
        }
        private void KeyBoardAddword(object sender, EventArgs e)
        {
            Button btn = (Button)sender;

            if (!RussiaKeyBoard)
            {
                metroTextBox2.Text += btn.Text;

                metroTextBox2.Select(metroTextBox2.Text.Length, 0);
            }
            else
                metroTextBox3.Text += btn.Text;
            SoundButtonClick();
        }
        private void TextBoxExaminationru(object sender, KeyPressEventArgs e)
        {
            string Symbol = e.KeyChar.ToString();
            char Symbol1 = e.KeyChar;
            if (!Regex.Match(Symbol, @"[а-яА-Я]").Success)
            {
                e.Handled = true;
            }
            if (Symbol1 == 8 || Symbol1 == 32)
            {
                e.Handled = false;
            }
        }
        private void TextBoxExamination(object sender, KeyPressEventArgs e)
        {
            string Symbol = e.KeyChar.ToString();
            char Symbol1 = e.KeyChar;
            if (Symbol == "\u0016" || Symbol1 == 8 || Symbol1 == 32)
            {
                e.Handled = false;
            }
            else
                e.Handled = true;

        }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void metroButton38_Click(object sender, EventArgs e)
        {
            if (metroTextBox2.Text != string.Empty && metroTextBox3.Text != string.Empty)
            {
                metroTextBox2.Text = System.Text.RegularExpressions.Regex.Replace(metroTextBox2.Text, @"\s+", " ");
                metroTextBox3.Text = System.Text.RegularExpressions.Regex.Replace(metroTextBox3.Text, @"\s+", " ");
                if (metroTextBox2.Text.StartsWith(" "))
                {
                    metroTextBox2.Text = metroTextBox2.Text.Remove(0, 1);
                }
                if (metroTextBox3.Text.StartsWith(" "))
                {
                    metroTextBox3.Text = metroTextBox3.Text.Remove(0, 1);
                }
                if (metroTextBox2.Text.EndsWith(" "))
                {
                    metroTextBox2.Text = metroTextBox2.Text.Remove(metroTextBox2.Text.Length - 1, 1);
                }
                if (metroTextBox3.Text.EndsWith(" "))
                {
                    metroTextBox3.Text = metroTextBox3.Text.Remove(metroTextBox3.Text.Length - 1, 1);
                }
                string n1 = metroTextBox2.Text.ToLower();
                string n2 = metroTextBox3.Text.ToLower();
                words.Add(new Words(n1, n2));
                //Serialize();
                metroTextBox3.Text = string.Empty;
                metroTextBox2.Text = string.Empty;
                SoundButtonClick();
            }


        }

        private void metroButton43_Click(object sender, EventArgs e)
        {
            if (RussiaKeyBoard)
                metroTextBox3.Text += " ";
            else
                metroTextBox2.Text += " ";
            SoundButtonClick();
        }

        private void metroButton39_Click(object sender, EventArgs e)
        {
            if (metroTextBox1.Text != string.Empty)
                metroTextBox1.Text = metroTextBox1.Text.Remove(metroTextBox1.Text.Length - 1, 1);
            SoundButtonClick();
        }

        private void metroButton40_Click(object sender, EventArgs e)
        {
            if (RussiaKeyBoard)
            {
                if (metroTextBox3.Text != string.Empty)
                    metroTextBox3.Text = metroTextBox3.Text.Remove(metroTextBox3.Text.Length - 1, 1);
            }
            else
            {
                if (metroTextBox2.Text != string.Empty)
                    metroTextBox2.Text = metroTextBox2.Text.Remove(metroTextBox2.Text.Length - 1, 1);
            }
            SoundButtonClick();

        }
        void Serialize()
        {
            using var stream = File.Create("Words.json");
            JsonSerializer.SerializeAsync(stream, words);
        }
        void DictionaryTextBox(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            metroTextBox4.Text += btn.Text;
            SoundButtonClick();
        }
        async void SoundButtonClick()
        {
            if (SoundKeys)
            {
                SoundPlayer simpleSound = new SoundPlayer(@"Button_Click1.wav");
                simpleSound.Play();
            }

        }
        private void metroButton41_Click(object sender, EventArgs e)
        {
            if (metroTextBox4.Text != string.Empty)
                metroTextBox4.Text = metroTextBox4.Text.Remove(metroTextBox4.Text.Length - 1, 1);
            SoundButtonClick();
        }

        private void metroButton45_Click(object sender, EventArgs e)
        {
            metroTextBox4.Text += " ";
            SoundButtonClick();
        }
        private void Translate(object sender, EventArgs e)
        {
            if (Writer)
                metroTextBox5.Text = TranslateText(metroTextBox1.Text, "ru", "et");

        }
        private void AddWordsFromFile(object sender, EventArgs e)
        {
            SoundButtonClick();
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.ShowDialog();
            string FileName = openFile.FileName;
            string text = string.Empty;
            List<char> charsToRemove = new List<char>() { '@', '_', ',', '.' ,'`','"','~','!','@','#','$',
            '%','^','&','*','(',')','-','_','=','"','№',';',':','?','>','<' , '”' , '„' , '1','2','3','4','5',
            '6','7','8','9','0', '–' , '\\','/','\r' , '\n' , ''};

            if (FileName.Contains(".pdf"))
            {
                using (PdfDocument doc = new PdfDocument(FileName))
                {
                    text = doc.GetTextWithFormatting();
                }
            }
            else if (FileName.Contains(".txt"))
            {

                text = File.ReadAllText(FileName);

            }
            else if (FileName.Contains(".docx"))
            {
                DocumentModel doc = DocumentModel.Load(FileName);
                text = doc.Content.ToString();

            }
            metroLabel15.Visible = true;
            text = Filter(text, charsToRemove);
            string[] wrds = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            metroProgressSpinner1.Maximum = wrds.Count();
            metroProgressSpinner1.Visible = true;
            foreach (var w in wrds)
            {
                string n = w.ToLower();
                if (TranslateText(n, "ru", "et") != string.Empty)
                {
                    words.Add(new Words(n, TranslateText(n, "ru", "et").ToLower()));
                }
                metroProgressSpinner1.Value++;
            }
            metroProgressSpinner1.Visible = false;
            metroLabel15.Visible = false;
        }
        public static string Filter(string str, List<char> charsToRemove)
        {
            foreach (char c in charsToRemove)
            {
                str = str.Replace(c.ToString(), String.Empty);
            }

            return str;
        }
        public string TranslateText(string input, string toLanguage, string fromLanguage)
        {
            if (input != string.Empty)
            {
                //var toLanguage = "ru";//English
                //var fromLanguage = "et";//Estonian
                var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={fromLanguage}&tl={toLanguage}&dt=t&q={HttpUtility.UrlEncode(input)}";
                var webClient = new WebClient
                {
                    Encoding = System.Text.Encoding.UTF8
                };
                var result = webClient.DownloadString(url);
                try
                {
                    result = result.Substring(4, result.IndexOf("\"", 4, StringComparison.Ordinal) - 4);
                    return result;
                }
                catch
                {

                }
            }

            return string.Empty;
        }

        private void metroCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            metroTextBox6.Text = string.Empty;
            metroTextBox7.Text = string.Empty;
            if (metroCheckBox2.Checked == false)
            {
                foreach (var b in RndButtons)
                {
                    foreach (var c in Language_ET_RU)
                    {
                        if (b.Text == c.Key)
                            b.Text = c.Value;
                    }
                }
                metroButton106.Visible = false;
                metroButton107.Visible = false;
                isruRnd = false;
                RndGame();
            }
            else
            {
                foreach (var b in RndButtons)
                {
                    foreach (var c in Language_ET_RU)
                    {
                        if (b.Text == c.Value)
                            b.Text = c.Key;
                    }
                }
                metroButton106.Visible = true;
                metroButton107.Visible = true;
                isruRnd = true;
                RndGame();
            }
        }

        private void metroButton42_Click(object sender, EventArgs e)
        {
            if (metroCheckBox2.Checked == false)
            {
                if (metroTextBox7.Text != string.Empty)
                    metroTextBox7.Text = metroTextBox7.Text.Remove(metroTextBox7.Text.Length - 1, 1);
            }
            else
                if (metroTextBox6.Text != string.Empty)
                metroTextBox6.Text = metroTextBox6.Text.Remove(metroTextBox6.Text.Length - 1, 1);
            SoundButtonClick();
        }

        private void metroButton44_Click(object sender, EventArgs e)
        {
            if (metroCheckBox2.Checked == false)
            {
                metroTextBox7.Text += " ";
            }
            else
                metroTextBox6.Text += " ";
            SoundButtonClick();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SpeechSynthesizer speaker = new SpeechSynthesizer();
            SoundButtonClick();
            speaker.Rate = 1;
            speaker.Volume = 100;
            speaker.Speak(metroTextBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            SpeechSynthesizer speaker = new SpeechSynthesizer();
            SoundButtonClick();
            speaker.Rate = 1;
            speaker.Volume = 100;
            speaker.Speak(metroTextBox5.Text);
        }


        private void button3_Click(object sender, EventArgs e)
        {
            SoundButtonClick();
            if(metroTextBox1.Text != string.Empty)
            Clipboard.SetText(metroTextBox1.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SoundButtonClick();
            if (metroTextBox5.Text != string.Empty)
                Clipboard.SetText(metroTextBox5.Text);

        }

        private void metroButton114_Click(object sender, EventArgs e)
        {
            SoundButtonClick();
            ListBox.SelectedObjectCollection selectedItems = new ListBox.SelectedObjectCollection(listBox1);
            selectedItems = listBox1.SelectedItems;

            if (listBox1.SelectedIndex != -1)
            {
                for (int i = selectedItems.Count - 1; i >= 0; i--)
                    listBox1.Items.Remove(selectedItems[i]);
            }
        }

        private void metroTextBox10_DoubleClick(object sender, EventArgs e)
        {
            SoundButtonClick();
            OpenFileDialog openFile = new OpenFileDialog();// создаем диалоговое окно
            openFile.ShowDialog();// открываем окно
            string FileName = openFile.FileName;// берем полный адрес картинки
            try
            {
                ChangeBackgroundImage(FileName);
                metroTextBox10.Text = FileName;
                metroCheckBox4.Checked = false;

            }
            catch
            {

                metroTextBox10.Text = "Default";
                metroCheckBox4.Checked = true;
            }
            try
            {
                string name = FileName.Remove(0, FileName.LastIndexOf(@"\") + 1);
                System.IO.File.WriteAllText("BackgroundImage.txt", @"image/" + name);
                System.IO.File.Copy(FileName, @"image/" + name);
            }
            catch
            {

            }
        }
        private void SetStartBackgroundImage()
        {
            string tmp = System.IO.File.ReadAllText("BackgroundImage.txt");
            if (tmp == "Default")
            {
                metroCheckBox4.Checked = true;
            }
            else
            {
                metroTextBox10.Text = tmp;
                metroCheckBox4.Checked = false;
                ChangeBackgroundImage(tmp);
            }
        }
        private void metroCheckBox4_CheckedChanged(object sender, EventArgs e)
        {

            if (metroCheckBox4.Checked == true)
            {
                metroTextBox10.Text = "Default";
                ChangeBackgroundImage(@"image\\1161810.jpg");
                System.IO.File.WriteAllText("BackgroundImage.txt", "Default");

            }
            else if (metroTextBox10.Text == "Default")
            {
                OpenFileDialog openFile = new OpenFileDialog();// создаем диалоговое окно
                openFile.ShowDialog();// открываем окно
                string FileName = openFile.FileName;// берем полный адрес картинки
                try
                {
                    ChangeBackgroundImage(FileName);
                    metroTextBox10.Text = FileName;

                }
                catch
                {
                    metroTextBox10.Text = "Default";
                    metroCheckBox4.Checked = true;
                    System.IO.File.WriteAllText("BackgroundImage.txt", "Default");
                }
                try
                {
                    string name = FileName.Remove(0, FileName.LastIndexOf(@"\") + 1);
                    System.IO.File.WriteAllText("BackgroundImage.txt", @"image/" + name);
                    System.IO.File.Copy(FileName, @"image/" + name);
                }
                catch
                {

                }
            }
        }
        void ChangeBackgroundImage(string path)
        {
            metroTabControl1.TabPages[0].BackgroundImage = Image.FromFile(path);
            metroTabControl1.TabPages[1].BackgroundImage = Image.FromFile(path);
            metroTabControl1.TabPages[2].BackgroundImage = Image.FromFile(path);
            metroTabControl1.TabPages[3].BackgroundImage = Image.FromFile(path);
            metroTabControl1.TabPages[4].BackgroundImage = Image.FromFile(path);
            metroTabControl1.TabPages[5].BackgroundImage = Image.FromFile(path);
            metroTabControl1.TabPages[0].BackgroundImageLayout = ImageLayout.Stretch;
            metroTabControl1.TabPages[1].BackgroundImageLayout = ImageLayout.Stretch;
            metroTabControl1.TabPages[2].BackgroundImageLayout = ImageLayout.Stretch;
            metroTabControl1.TabPages[3].BackgroundImageLayout = ImageLayout.Stretch;
            metroTabControl1.TabPages[4].BackgroundImageLayout = ImageLayout.Stretch;
            metroTabControl1.TabPages[5].BackgroundImageLayout = ImageLayout.Stretch;

        }

        private void metroTextBox10_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void metroCheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            SoundButtonClick();
            if (metroCheckBox3.Checked == true)
            {
                metroStyleManager1.Theme = MetroFramework.MetroThemeStyle.Light;
                listBox1.BackColor = System.Drawing.Color.White;
                button1.BackColor = System.Drawing.Color.White;
                button2.BackColor = System.Drawing.Color.White;
                button3.BackColor = System.Drawing.Color.White;
                button4.BackColor = System.Drawing.Color.White;
            }
            if (metroCheckBox3.Checked == false)
            {
                metroStyleManager1.Theme = MetroFramework.MetroThemeStyle.Dark;
                listBox1.BackColor = System.Drawing.Color.FromArgb(34, 34, 34);
                button1.BackColor = System.Drawing.Color.FromArgb(34, 34, 34);
                button2.BackColor = System.Drawing.Color.FromArgb(34, 34, 34);
                button3.BackColor = System.Drawing.Color.FromArgb(34, 34, 34);
                button4.BackColor = System.Drawing.Color.FromArgb(34, 34, 34);
            }
        }

        private void metroTextBox11_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void metroTextBox11_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.ShowDialog();
            string FileName = openFile.FileName;
            try
            {
                LoadWords(FileName);

            }
            catch
            {

            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Serialize();
        }

        private void metroButton116_Click(object sender, EventArgs e)
        {
            SoundButtonClick();
            if(metroTextBox1.Text != string.Empty && metroTextBox5.Text != string.Empty)
            {
                string n1 = metroTextBox1.Text.ToLower();
                string n2 = metroTextBox5.Text.ToLower();
                Words n = new Words(n1, n2);
                words.Add(n);
            }
            

        }

        private void metroCheckBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (metroCheckBox5.Checked == true)
            {
                SoundKeys = true;
            }
            else
                SoundKeys = false;
        }

        private void metroTextBox12_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void metroTextBox5_TextChanged(object sender, EventArgs e)
        {
            if (!Writer)
                metroTextBox1.Text = TranslateText(metroTextBox5.Text, "et", "ru");
        }

        private void metroTextBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            Writer = false;
        }


        private void metroTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Writer = true;
        }

    }
}
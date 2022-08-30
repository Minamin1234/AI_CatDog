using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Win32;
using System.IO;
using System.Diagnostics;

namespace AI_DogCat
{
    /// <summary>
    /// 状態を表す列挙体
    /// </summary>
    public enum STATE
    {
        /// <summary>
        /// 未操作/実行中
        /// </summary>
        NONE,
        /// <summary>
        /// 実行に成功した
        /// </summary>
        SUCCESS,
        /// <summary>
        /// 実行に失敗
        /// </summary>
        FAILURE
    };

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        const string PICTURE = "Picture|*.jpg";
        const string PICTURE_EXT = ".jpg";
        const string MODEL = "Model|*.h5";
        const string MODEL_EXT = ".h5";
        const string AI_PATH = "C:/MyFile/Minamin1234/AI_CatDog/main.py";
        const string RESULT_PATH = @"C:\MyFile\Minamin1234\AI_CatDog\result.txt";
        const string PER_FMT = "0.00";

        const string RESULT_SUCCESS = "Success";
        const string RESULT_FAILURE = "Failure";
        List<string> RESULT_LIST = new List<string>() 
        { 
            "Dog",
            "Cat"
        };
        const int RESULT_DOG = 0;
        const int RESULT_CAT = 1;
        Color STATE_NONE = Color.FromRgb(130, 130, 130);
        Color STATE_SUCCESS = Color.FromRgb(0, 225, 100);
        Color STATE_FAILURE = Color.FromRgb(210, 30, 0);

        string PicPath = "./pic.jpg";
        string ModelPath = "./cnn.h5";
        STATE Status = STATE.NONE;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ファイルダイアログを表示し、選択されたファイルのパスを返します。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string SelectFile(string type)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = type;
            if(dialog.ShowDialog() == true)
            {
                //Console.Write(dialog.FileName);
            }
            return dialog.FileName;
        }

        /// <summary>
        /// 指定した画像を読み込みます。
        /// </summary>
        /// <param name="path"></param>
        public void LoadImage(string path)
        {
            TB_PicPath.Text = path;
            this.PicPath = path;
        }

        /// <summary>
        /// 指定したモデルを読み込みます。
        /// </summary>
        /// <param name="path"></param>
        public void LoadModel(string path)
        {
            TB_ModelPath.Text = path;
            this.ModelPath = path;
        }

        /// <summary>
        /// 指定した画像ファイルをウィンドウ上に表示させます。
        /// </summary>
        /// <param name="path"></param>
        public void ShowImage(string path)
        {
            this.SetStatusColor(STATE.NONE);
            this.SetStatusText("");
            var ms = new MemoryStream();
            using(var fs = new FileStream(path,FileMode.Open))
            {
                fs.CopyTo(ms);
            }
            ms.Seek(0,SeekOrigin.Begin);
            var img = new BitmapImage();
            img.BeginInit();
            img.StreamSource = ms;
            img.EndInit();

            this.Img_Imgbox.Source = img;
        }

        /// <summary>
        /// 状態(背景)を更新します。
        /// </summary>
        /// <param name="newstatus"></param>
        public void SetStatusColor(STATE newstatus)
        {
            this.Status = newstatus;
            switch (this.Status)
            {
                case STATE.NONE:
                    this.BR_Status.Background = new SolidColorBrush(STATE_NONE);
                    break;
                case STATE.SUCCESS:
                    this.BR_Status.Background = new SolidColorBrush(STATE_SUCCESS);
                    break;
                case STATE.FAILURE:
                    this.BR_Status.Background = new SolidColorBrush(STATE_FAILURE);
                    break;
            }
        }

        public void SetStatusText(string newtext)
        {
            this.T_Status.Content = newtext;
        }

        /// <summary>
        /// 予測プログラムを実行し、予測結果を返します。
        /// </summary>
        /// <returns></returns>
        public List<string> Predict()
        {
            this.SetStatusColor(STATE.NONE);
            this.SetStatusText("");
            ProcessStartInfo p = new ProcessStartInfo();
            p.FileName = "python";
            p.Arguments = $"{AI_PATH} {this.PicPath} {this.ModelPath} {RESULT_PATH}";
            p.UseShellExecute = true;

            Process ps = Process.Start(p);
            this.Topmost = true;
            ps.WaitForExit();
            this.Topmost = false;

            StreamReader r = new StreamReader(RESULT_PATH, Encoding.GetEncoding("Shift-JIS"));
            List<string> result = new List<string>(0);
            while(r.Peek() != -1)
            {
                var res = r.ReadLine();
                result.Add(res);
                Console.WriteLine(res);
            }
            r.Close();

            return result;
        }

        /// <summary>
        /// 予測結果からウィンドウに結果を表示します。
        /// </summary>
        /// <param name="result"></param>
        public void ShowResult(in List<string> result)
        {
            string res = "";
            List<float> values = new List<float>();
            float dog = float.Parse(result[1]) * 100; //dog
            float cat = float.Parse(result[2]) * 100; //cat
            values.Add(dog);
            values.Add(cat);
            res += " Dog: ";
            res += values[0].ToString(PER_FMT) + "%" + Environment.NewLine;

            res += " Cat: ";
            res += values[1].ToString(PER_FMT) + "%";
            T_Result.Text = res;

            var mx = values.Max();
            int most = values.IndexOf(mx);
            switch(result[0])
            {
                case RESULT_SUCCESS:
                    this.SetStatusColor(STATE.SUCCESS);
                    this.SetStatusText(RESULT_LIST[most]);
                    break;
                case RESULT_FAILURE:
                default:
                    this.SetStatusColor(STATE.FAILURE);
                    break;
            }
        }

        public void ClearResult()
        {
            T_Result.Text = "";
        }

        private void Start_Clicked(object sender, RoutedEventArgs e)
        {
            var result = Predict();
            ShowResult(result);
            //var args = Environment.GetCommandLineArgs();
            //Console.WriteLine(args[0]);
        }

        private void SelectModel_Clicked(object sender, RoutedEventArgs e)
        {
            var path = this.SelectFile(MODEL);
            this.LoadModel(path);
        }

        private void SelectPicture_Clicked(object sender, RoutedEventArgs e)
        {
            var path = this.SelectFile(PICTURE);
            LoadImage(path);
            ShowImage(path);
        }

        private void File_Dropped(object sender, DragEventArgs e)
        {
            
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            var ext = System.IO.Path.GetExtension(files[0]);
            if (String.Compare(ext,".jpg",true) == 0)
            {
                this.LoadImage(files[0]);
                this.ShowImage(files[0]);
            }
            else if(String.Compare(ext,".h5",true) == 0)
            {
                this.LoadModel(files[0]);
            }
        }

        private void File_OnOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = false;
        }
    }
}

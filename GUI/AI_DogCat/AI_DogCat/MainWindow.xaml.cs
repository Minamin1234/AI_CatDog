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

namespace AI_DogCat
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        const string PICTURE = "Picture|*.jpg";
        const string MODEL = "Model|*.h5";

        string PicPath = "./pic.jpg";
        string ModelPath = "./cnn.h5";
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
                Console.Write(dialog.FileName);
            }
            return dialog.FileName;
        }

        /// <summary>
        /// 指定した画像ファイルをウィンドウ上に表示させます。
        /// </summary>
        /// <param name="path"></param>
        public void ShowImage(string path)
        {
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

        private void Start_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void SelectModel_Clicked(object sender, RoutedEventArgs e)
        {
            var path = this.SelectFile(MODEL);
            TB_ModelPath.Text = path;
            this.ModelPath = path;
        }

        private void SelectPicture_Clicked(object sender, RoutedEventArgs e)
        {
            var path = this.SelectFile(PICTURE);
            TB_PicPath.Text = path;
            this.PicPath = path;
            ShowImage(path);
        }
    }
}

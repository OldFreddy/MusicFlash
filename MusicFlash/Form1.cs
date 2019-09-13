using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; //Подключаем пространство имен для работы с файлами
using System.Collections; //подключаем коллекции для создания динамического массива


namespace MusicFlash
{

    public partial class Form1 : Form
    {
        public string HomeFolder; //Переменная, содержащая в себе директорию с музыкой пользователя
        public string FlashFolder; //Переменная, содержащая в себе директорию назначения
        public string[] filenameswithdir; //массив имен файлов с директориями
        public string[] filenames; //массив имен файлов без директорий
        public double allsize; //переменная для рассчета объема музыки в дирректории
        public double FreeSpace; //переменная для определения свободного места на съемном диске
        public double FilesSize; //переменная содержащая объем, занимаемый всеми выбранными файлами
        public string[] randfiles; //массив созданный случайно из массива имен файлов с путем
        public int[] massrnd; //массив для хранения пройденных циклов рандома
        public string[] filenamesSORT; //массив имен файлов без путей отсортированный
        public double FolderCount;
        public int filesInDir; //переменная количества файлов в директории
        public int FolderName;
        public string[] testmass;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear(); //очистка первого листбокса

            /////Выбор директории с музыкой/////
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Выберите папку с музыкой";
            dialog.ShowNewFolderButton = false;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                HomeFolder = dialog.SelectedPath;
                textBox1.Text = HomeFolder;

                /////Получение списка файлов в директории/////

                filenameswithdir = Directory.GetFiles(HomeFolder, "*.mp3", SearchOption.AllDirectories); //получение списка файлов в указанной папке и подпапках
                filenames = new string [filenameswithdir.Length]; //создание массива, в котором будут содержаться имена файлов и указание его длины
                label1.Visible = true;
                label1.Text = ((filenameswithdir.Length) + " Files"); //подсчет количества файлов в директории
                allsize = 0;
                for (int i = 0; i < filenameswithdir.Length; i++)
                {
                    filenames[i] = Path.GetFileName(filenameswithdir[i]); //получение имени файла без пути
                    listBox1.Items.Add(filenames[i]); //запись имени файла в листбокс
                    FileInfo size = new FileInfo(filenameswithdir[i]); //объявление обработки размера файла
                    allsize = allsize + size.Length; //рассчет общего объема файлов в директории и поддиректориях



                }
                button2.Enabled = true;
            }
            /////////////////////////////////////

           

            double fullsize = allsize/1024/1024/1024; //перевод из байтов в гигабайты
            label2.Visible = true;
            label2.Text = fullsize.ToString("F") + " Gb"; //запись в лейбл в гигабайтах

            //////////////////////////////////////////////
            /////Вычисление свободного места на съемном диске и выбор директории/////


            FolderBrowserDialog dialog1 = new FolderBrowserDialog();
            dialog1.Description = "Выберите свою флешку";
            dialog1.ShowNewFolderButton = false;
            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                FlashFolder = dialog1.SelectedPath;
                textBox2.Text = FlashFolder;

                DriveInfo di = new DriveInfo(FlashFolder);
                FreeSpace = di.AvailableFreeSpace;// 1024 / 1024 / 1024;
                // MessageBox.Show(FreeSpace.ToString() + " Gb");
                label3.Visible = true;
                label3.Text = Math.Ceiling(FreeSpace/1024/1024/1024).ToString() + " Gb Free" ;
                //label3.Text = Convert.ToString(FreeSpace);
            }

           

            //////////////////////////////////////////////////////

        }
       
        private void button2_Click(object sender, EventArgs e)
        {
           

            //////////////////////////Генератор случайных уникальных чисел////////////////////
            listBox2.Items.Clear(); //очистка листбокса
            massrnd = new int [filenameswithdir.Length]; //присвоение массиву случайных чисел определенной длины
            var rand = new Random(); //переменная, которая будет генерировать случайное число

            bool alreadyThere; //переменная для проверки на существование похожего числа
            for (int i = 0; i < filenameswithdir.Length; )
            {
                alreadyThere = false;
                int newRandomValue = rand.Next(filenameswithdir.Length); // процедура получения случайного числа
                for (int j = 0; j < i; j++)
                {
                    if (massrnd[j] == newRandomValue)
                    {
                        alreadyThere = true; //проверка на присутствие такого же числа
                        break;

                    }
                }
                if (alreadyThere != true)
                {
                    massrnd[i] = newRandomValue; //заполнение массива уникальным числом
                    

                    i++;
                }

                numericUpDown1.Enabled = true;
            }
            
            /////Запись в листбокс отсортированных случайным образом имен файлов/////
            filenamesSORT = new string[filenameswithdir.Length];
            listBox2.Items.Clear();
            FilesSize = 0;
            

            List<string> list = new List<string>(); //задаем динамический массив

            for (int i = 0; i < filenameswithdir.Length; i++)
            {
                filenamesSORT[i] = Path.GetFileName(filenameswithdir[massrnd[i]]);
                listBox2.Items.Add(filenamesSORT[i]);
                FileInfo selectfile = new FileInfo(filenameswithdir[massrnd[i]]); // доступ к информации о выбранном в данный момент файле
                FilesSize = FilesSize + selectfile.Length; //подсчет занимаемого файлами объема
                

                if (FilesSize < FreeSpace)
                {
            
                    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!Спросить у Жеки, почему тут появляется ошибка, если FileSize подсчет запихнуть сюда, а так же ,почему считает чуть больше, чем свободное место
                    list.Add(filenameswithdir[massrnd[i]]); //записываем в динамический массив выбранный элемент
                }
                else
                {
                    label8.Text = Convert.ToString(FilesSize-selectfile.Length);
                    break;
                }
                
                
            }
            testmass = list.ToArray(); //конвертирование динамического массива в обычный
            button3.Enabled = true;
            progressBar1.Minimum = 0; //задаем минимум для прогрессбара
            progressBar1.Maximum = testmass.Length; //задаем максимальное значение прогрессбара

            //int g = filenamesSORT.Length;
            //MessageBox.Show(filenamesSORT.Length.ToString());
            ////////////////////////////////////////////////////////////////////////


        }
            ///////////////////Копирование отсортированных случайным образом файлов в директорию файлов в директорию/////////////////
        private void button3_Click(object sender, EventArgs e)
        {
            filesInDir = (int)numericUpDown1.Value; //считывание значения количества файлов в директории из переключателя
            label6.Visible = true;
            label7.Visible = true;
            progressBar1.Value = 0;
                                                  

            for (int i = 0; i < testmass.Length; i++)
            {

                FolderName = 1 + i / filesInDir;
                if (!Directory.Exists(FlashFolder + "/" + FolderName)) //проверка на существование директории
                {
                    Directory.CreateDirectory(FlashFolder + "/" + FolderName); //создание директории
                }
                if (!File.Exists(FlashFolder + "/" + FolderName + "/" + Path.GetFileName(testmass[i])))

                {
                    if (!File.Exists(FlashFolder + "/" + FolderName + "/" + i + " " + Path.GetFileName(testmass[i])))
                    {
                        File.Copy(testmass[i], FlashFolder + "/" + FolderName + "/" + i + Path.GetFileName(testmass[i])); // копирование файлов
                        progressBar1.Value = progressBar1.Value + 1; //увеличение прогресбара
                        int x = progressBar1.Value * 100 / testmass.Length;
                        label6.Text = x.ToString() + " %";  //проценты прогресса
                        label7.Text = Path.GetFileName(testmass[i]); //отображение текущего копируемого файла
                        Application.DoEvents(); //прерывание, чтобы не зависала оболочка
                    }
                    else continue;


                }


            }
            MessageBox.Show("Копирование завершено");
            ////////////////////////////////////////////////////////////////////////

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            string a = filenamesSORT.Length.ToString(); //общее число файлов
            string b = numericUpDown1.Value.ToString(); //указанное пользователем число файлов в директории
            FolderCount = Math.Ceiling(Convert.ToDouble(a) / Convert.ToDouble(b)); //рассчет количества директорий 
            label5.Text = (Convert.ToString(FolderCount) + " директорий");
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill(); //принудительная остановка процесса
        }
    }
}

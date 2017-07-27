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
using System.Windows.Shapes;

namespace DearJacob
{
    /// <summary>
    /// Логика взаимодействия для WindowStepTwo.xaml
    /// </summary>
    public partial class WindowStepTwo : Window
    {
        Matrix matrixOne;
        Matrix matrixTwo;

        public WindowStepTwo(Matrix matrix)
        {
            InitializeComponent();
            this.matrixOne = matrix;
            this.matrixTwo = CreateMatrix();

            Loaded += WindowStepTwo_Loaded;           
        }

        private void WindowStepTwo_Loaded(object sender, RoutedEventArgs e)
        {
            this.ToCenterScreen();

            Action action = new Action(() =>
            {
                try
                {
                    while (true)
                    {
                        Dispatcher.Invoke((Action)(() => labelTime.Content = DateTime.Now.ToLongTimeString().ToString()));
                        Task.Delay(1000);
                    }
                }
                catch (Exception)
                {
                    return;
                }
            });
            Task.Factory.StartNew(action);

            richTextBoxE1.PrintMassiveE(matrixOne.ArrE);
            richTextBoxM1.PrintMassiveK(matrixOne.ArrK);

            richTextBoxE2.PrintMassiveE(matrixTwo.ArrE);
            richTextBoxM2.PrintMassiveK(matrixTwo.ArrK);

            richTextBoxInversOne.PrintMassiveE(matrixOne.ArrE.Inverse());
            richTextBoxInversTwo.PrintMassiveE(matrixTwo.ArrE.Inverse());
        }

        private Matrix CreateMatrix()
        {
            Matrix matrix = new Matrix
            {
                ArrE = new float[8, 8]
                {
                { 4.792F, 4.417F, 4.244F, 2.406F, 1.798F, 0.790F, 0.785F, 2.993F},
                { 4.417F, 5.074F, 4.636F, 2.798F, 1.824F, 0.639F, 0.644F, 2.799F},
                { 4.244F, 4.636F, 5.428F, 3.224F, 2.111F, 0.903F, 1.131F, 2.943F},
                { 2.406F, 2.798F, 3.224F, 5.287F, 3.006F, 1.326F, 1.897F, 2.648F},
                { 1.798F, 1.824F, 2.111F, 3.006F, 3.574F, 2.229F, 2.471F, 1.915F},
                { 0.790F, 0.639F, 0.903F, 1.326F, 2.229F, 4.008F, 2.405F, 1.106F},
                { 0.785F, 0.644F, 1.131F, 1.897F, 2.471F, 2.405F, 4.507F, 1.727F},
                { 2.993F, 2.799F, 2.943F, 2.648F, 1.915F, 1.106F, 1.727F, 3.972F},
                },
                ArrK = new float[8]
                {5.760F, 5.715F, 5.705F, 4.150F, 6.225F, 6.960F, 6.750F, 3.910F}
            };

            WorkWithMatrix worker = new WorkWithMatrix(matrix, 0.0001F);

            matrix.ArrSum = worker.SummaTerm();

            return matrix;
        }

        private void ButClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButGoBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWin = new MainWindow();
            mainWin.Show();
            this.Close();
        }

        private async void ButStartBayesClassification_Click(object sender, RoutedEventArgs e)
        {
            Bayes bay = new Bayes(matrixOne, matrixTwo);

            bay.progressBarChange += ChangeProgressBar;

            await Task.Factory.StartNew(() =>
            {
                bay.Classification(out int classOne, out int classTwo, out float oshibka);

                Dispatcher.Invoke(() =>
                {
                    textClassOne.Text = string.Format($"Принадлежность первому классу: {classOne}");
                    textClassTwo.Text = string.Format($"Принадлежность второму классу: {classTwo}");
                    textOshibka.Text = string.Format($"Ошибка: {oshibka}");
                });
            });            
        }

        private void ChangeProgressBar(int i)
        {
            Dispatcher.Invoke(() =>
            {
                progressBar.Value = i;
            });
        }
    }
}

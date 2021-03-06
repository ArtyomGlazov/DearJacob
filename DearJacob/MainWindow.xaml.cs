﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;

namespace DearJacob
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string pathToFile;
        private Matrix matrix;
        private float accuracy = 0.001F;
        private bool canGiveYouMatrixY = false;
        private List<RichTextBox> listRiches;
        private ColumnDefinition column;
        private Point windowsPosition;
        private RowDefinition row;

        public MainWindow()
        {
            InitializeComponent();

            ExitMenu.Click += ExitMenu_Click;

            Loaded += MainWindow_Loaded;
            
            butClearTheFields.Click += ButClearTheFields_Click;
            butGiveYMatrix.Click += ButGiveYMatrix_Click;
            butDefaultSizeWindow.Click += ButDefaultSizeWindow_Click;
            butExit.Click += ButExit_Click;

            //labelAccuracy.MouseWheel += MainWindow_MouseWheel;
            labelAccuracy.MouseLeftButtonDown += LabelAccuracy_MouseLeftButtonDown;
            labelAccuracy.MouseRightButtonDown += LabelAccuracy_MouseRightButtonDown;
            labelAccuracy.Content = string.Format("Точность: {0}", accuracy);

            windowsPosition = new Point(Top, Left);
        }

        private void LabelAccuracy_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (accuracy > 0.0001)
            {
                accuracy /= 10;
                labelAccuracy.Content = string.Format("Точность: {0:0.########}", accuracy);
            }

            if (canGiveYouMatrixY)
            {
                ButGiveYMatrix_Click(sender, e as RoutedEventArgs);
            }
        }

        private void LabelAccuracy_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (accuracy < 1)
            {
                accuracy *= 10;
                labelAccuracy.Content = string.Format("Точность: {0:0.########}", accuracy);
            }

            if (canGiveYouMatrixY)
            {
                ButGiveYMatrix_Click(sender, e as RoutedEventArgs);
            }
        }

        private void ButExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();            
        }

        private void ButDefaultSizeWindow_Click(object sender, RoutedEventArgs e)
        {
            SetDefaultSizeWindow();
        }

        private RichTextBox CreateRichTextBox(string name, int height)
        {
            RichTextBox richTextBox = new RichTextBox
            {
                Name = name,
                Height = height,
                Style = (Style)FindResource("RichTextBoxStyle")
            };

            return richTextBox;
        }

        private Button CreateButton(string name, string content, RoutedEventHandler butAction)
        {
            Button butNextStep = new Button
            {
                Name = name,
                Content = content,
                VerticalAlignment = VerticalAlignment.Top
            };

            butNextStep.Click += butAction;

            return butNextStep;
        }

        private void WindowExtension()
        {
            Width = 1060;
            Height = 575;

            canGiveYouMatrixY = true;

            column = new ColumnDefinition
            {
                Width = GridLength.Auto
            };

            row = new RowDefinition
            {
                Height = GridLength.Auto
            };

            MyGrid.ColumnDefinitions.Add(column);

            RichTextBox richTextBoxSumm = CreateRichTextBox("richTextBoxSumm", 467);

            RichTextBox richTextBoxA = CreateRichTextBox("richTextBoxA", 140);

            RichTextBox richTextBoxF = CreateRichTextBox("richTextBoxF", 140);

            Button butNextStep = CreateButton("butNextStep", "Следующий шаг", (object sender, RoutedEventArgs e) =>
            {
                WindowStepTwo twoStep = new WindowStepTwo(matrix);
                twoStep.Show();
                this.Close();
            });

            MyGrid.Children.Add(richTextBoxSumm);
            Grid.SetRow(richTextBoxSumm, 1);
            Grid.SetColumn(richTextBoxSumm, 2);
            Grid.SetRowSpan(richTextBoxSumm, 4);

            listRiches.Add(richTextBoxSumm);

            MyGrid.Children.Add(richTextBoxA);
            Grid.SetRow(richTextBoxA, 3);
            Grid.SetColumn(richTextBoxA, 0);

            listRiches.Add(richTextBoxA);

            MyGrid.Children.Add(richTextBoxF);
            Grid.SetRow(richTextBoxF, 4);
            Grid.SetColumn(richTextBoxF, 0);

            listRiches.Add(richTextBoxF);

            MyGrid.Children.Add(butNextStep);
            Grid.SetRow(butNextStep, 3);
            Grid.SetColumn(butNextStep, 1);

            Background = (LinearGradientBrush)FindResource("LinearBackGroundAfter");

            this.ToCenterScreen();
        }

        private void SetDefaultSizeWindow()
        {
            Width = 600;
            Height = 275;

            canGiveYouMatrixY = false;

            MyGrid.Children.Remove(GetRichBox("richTextBoxSumm"));
            MyGrid.Children.Remove(GetRichBox("richTextBoxF"));
            MyGrid.Children.Remove(GetRichBox("richTextBoxA"));

            MyGrid.ColumnDefinitions.Remove(column);

            listRiches.RemoveRange(2, listRiches.Count - 2);

            textBlockKolIter.Text = string.Format("Итераций: 0");

            Background = (LinearGradientBrush)FindResource("LinearBackGroundBefore");

            butDefaultSizeWindow.Visibility = Visibility.Collapsed;

            this.ToCenterScreen();
        }

        private async void ButGiveYMatrix_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (matrix.ArrE == null || matrix.ArrK == null)
                {
                    PrintingMassives.GiveMeMatrixFromTextBoxes(ref matrix, richTextBoxE, richTextBoxK);
                    richTextBoxE.PrintMassiveE(matrix.ArrE, true);
                    richTextBoxK.PrintMassiveK(matrix.ArrK);
                }
                
                WorkWithMatrix worker = new WorkWithMatrix(matrix, accuracy);

                matrix.ArrEPlus = await Task<float[,]>.Factory.StartNew(worker.MatricaKPlusOne);
                matrix.ArrSum = await Task<float[,]>.Factory.StartNew(worker.SummaTerm);
                matrix.ArrF = worker.MyMatrix.ArrF;


                if (listRiches.Count < 3)
                    WindowExtension();

                GetRichBox("richTextBoxSumm").PrintMassiveE(matrix.ArrSum);
                GetRichBox("richTextBoxA").PrintMassiveE(matrix.ArrEPlus);
                GetRichBox("richTextBoxF").PrintMassiveE(matrix.ArrF);

                textBlockKolIter.Text = string.Format("Итераций: {0}", worker.KolIter.ToString());

                butDefaultSizeWindow.Visibility = Visibility.Visible;

                this.ToCenterScreen();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show(string.Format("Отсутвуют матрицы в полях."), "Ошибочка.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(FormatException)
            {
                MessageBox.Show(string.Format("Неверно введена матрица."), "Ошибочка.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(IndexOutOfRangeException)
            {
                MessageBox.Show(string.Format("Матрицы введены криво."), "Ошибочка.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(Exception ex)
            {
                MessageBox.Show(string.Format(ex.Message), "Ошибочка.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButClearTheFields_Click(object sender, RoutedEventArgs e)
        {
            foreach (var listRich in listRiches)
            {
                listRich.Document.Blocks.Clear();
            }

            canGiveYouMatrixY = false;

            pathToFile = null;
            matrix = new Matrix();            
            labelPathToFile.Content = "Файл не выбран.";
            textBlockKolIter.Text = string.Format("Итераций: 0");

            if (butDefaultSizeWindow.IsVisible)
            {
                SetDefaultSizeWindow();
            }
        }

        private RichTextBox GetRichBox(string nameRich)
        {
            try
            {
                RichTextBox rich = listRiches[listRiches.IndexOf(listRiches.Find((richBox) => richBox.Name == nameRich))];

                return rich;
            }
            catch (NullReferenceException)
            {
                throw;
            }
        }

        private void ExitMenu_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            listRiches = new List<RichTextBox> { richTextBoxE, richTextBoxK };

            butDefaultSizeWindow.Visibility = Visibility.Collapsed;

            labelPathToFile.Content = "Файл не выбран";

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
        }

        private async void OpenMenuClick(object sender, ExecutedRoutedEventArgs e)
        {            
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "(Текстовые файлы)|*.txt"
            };

            if (ofd.ShowDialog() == true)
            {
                pathToFile = ofd.FileName;
                labelPathToFile.Content = pathToFile;
            }
            else
            {
                return;
            }

            richTextBoxE.Document.Blocks.Clear();
            richTextBoxK.Document.Blocks.Clear();

            try
            {
                matrix = await Task<Matrix>.Factory.StartNew(() => WorkWithFiles.OpenMatrix(pathToFile));

                richTextBoxE.PrintMassiveE(matrix.ArrE, true);
                richTextBoxK.PrintMassiveK(matrix.ArrK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void SaveMenuClick(object sender, ExecutedRoutedEventArgs e)
        {
            if (pathToFile != null)
            {
                try
                {
                    matrix.ArrE.GiveMeMatrixFromTextBox(richTextBoxE);
                    matrix.ArrK.GiveMeMatrixFromTextBox(richTextBoxK);

                    await Task.Factory.StartNew(() => WorkWithFiles.SaveMatrixAs(pathToFile, matrix));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                SaveAsMenuClick(sender, e);
            }            
        }

        private async void SaveAsMenuClick(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "(Текстовые файлы)|*.txt"
            };

            if (sfd.ShowDialog() == true)
            {
                pathToFile = sfd.FileName;
                labelPathToFile.Content = pathToFile;
            }
            else
            {
                return;
            }

            try
            {
                matrix.ArrE.GiveMeMatrixFromTextBox(richTextBoxE);
                matrix.ArrK.GiveMeMatrixFromTextBox(richTextBoxK);

                await Task.Factory.StartNew(() => WorkWithFiles.SaveMatrixAs(pathToFile, matrix));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

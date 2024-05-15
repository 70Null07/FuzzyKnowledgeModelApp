using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FuzzyKnowledgeModelApp
{
    public partial class MainWindow : Window
    {
        private double[,] tempValues, mhzValues, rpmValues;

        private double[] calculatedRPM;

        public MainWindow()
        {
            InitializeComponent();

            tempValues = new double[90, 5];
            mhzValues = new double[1810, 5];
            rpmValues = new double[2090, 4];
            calculatedRPM = new double[2090];

            // Заполнение tempValues
            for (int i = 0; i < 90; i++)
            {
                tempValues[i, 0] = i;
                double tempLow = 0, tempMiddle = 0, tempHigh = 0, tempCritical = 0;

                if (i <= 30)
                {
                    tempLow = 1;
                }
                if (i > 30 && i < 45)
                {
                    tempLow = (45 - i) / 15;
                    if (i <= 40)
                    {
                        tempMiddle = 0;
                        tempHigh = 0;
                        tempCritical = 0;
                    }
                }
                if (i >= 45)
                {
                    tempLow = 0;
                }
                if (i >= 40 & i < 51)
                {
                    tempMiddle = (double)(i - 40) / 10;
                }
                if (i >= 51 && i < 70)
                {
                    tempMiddle = (double)(70 - i) / 20;
                }
                if (i >= 70)
                    tempMiddle = 0;
                if (i > 40 && i < 61)
                    tempHigh = 0;
                if (i >= 61 && i < 69)
                    tempHigh = (double)(i - 60) / 8;
                if (i >= 69)
                    tempHigh = 1;
                if (i > 40 && i < 80)
                    tempCritical = 0;
                if (i >= 80 & i < 87)
                    tempCritical = (double)(i - 79) / 8;
                if (i >= 87)
                    tempCritical = 1;
                tempValues[i, 1] = tempLow;
                tempValues[i, 2] = tempMiddle;
                tempValues[i, 3] = tempHigh;
                tempValues[i, 4] = tempCritical;
            }

            // Заполнение rpmValues
            for (int i = 0; i < 2090; i++)
            {
                rpmValues[i, 0] = i;
                if (i <= 700)
                {
                    rpmValues[i, 1] = 1;
                    rpmValues[i, 2] = 0;
                    rpmValues[i, 3] = 0;
                }
                if (i > 700 && i < 1100)
                {
                    rpmValues[i, 1] = (double)(1100 - i) / 395;
                }
                if (i >= 1100)
                    rpmValues[i, 1] = 0;
                if (i > 700 && i < 960)
                    rpmValues[i, 2] = 0;
                if (i >= 960 & i <= 1300)
                    rpmValues[i, 2] = (double)(i - 950) / 350;
                if (i > 1300 && i < 1600)
                    rpmValues[i, 2] = (double)(1600 - i) / 300;
                if (i >= 1600)
                    rpmValues[i, 2] = 0;
                if (i > 700 && i < 1540)
                    rpmValues[i, 3] = 0;
                if (i >= 1540 && i < 1690)
                    rpmValues[i, 3] = (double)(i - 1530) / 160;
                if (i >= 1690)
                    rpmValues[i, 3] = 1;
            }

            // Заполнение mhzValues
            for (int i = 0; i < 1810; i++)
            {
                rpmValues[i, 0] = i;
                if (i <= 210)
                    mhzValues[i, 1] = 1;
                else
                    mhzValues[i, 1] = 0;
                if (i < 310)
                    mhzValues[i, 2] = 1;
                else if (i >= 310 && i < 720)
                    mhzValues[i, 2] = (double)(720 - i) / 413;
                else
                    mhzValues[i, 2] = 0;
                if (i < 710)
                    mhzValues[i, 3] = 0;
                else if (i >= 710 && i <= 1160)
                    mhzValues[i, 3] = (double)(i - 700) / 460;
                else if (i > 1160 && i < 1600)
                    mhzValues[i, 3] = (double)(1600 - i) / 430;
                else
                    mhzValues[i, 3] = 0;
                if (i < 1310)
                    mhzValues[i, 4] = 0;
                else if (i >= 1310 && i < 1600)
                    mhzValues[i, 4] = (double)(i - 1300) / 300;
                else
                    mhzValues[i, 4] = 1;
            }
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            Polygon myPolygon = new()
            {
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            PointCollection points = new();

            int userTemp = int.Parse(TempTextBox.Text);
            int userMHz = int.Parse(MHzTextBox.Text);

            double MFt1 = tempValues[userTemp, 1], MFt2 = tempValues[userTemp, 2],
                MFt3 = tempValues[userTemp, 3], MFt4 = tempValues[userTemp, 4];
            double MFmhz1 = mhzValues[userMHz, 1], MFmhz2 = mhzValues[userMHz, 2],
                MFmhz3 = mhzValues[userMHz, 3], MFmhz4 = mhzValues[userMHz, 4];

            MFt1TextBlock.Text += MFt1; MFt2TextBlock.Text += MFt2;
            MFt3TextBlock.Text += MFt3; MFt4TextBlock.Text += MFt4;

            MFmhz1TextBlock.Text += MFmhz1; MFmhz2TextBlock.Text += MFmhz2;
            MFmhz3TextBlock.Text += MFmhz3; MFmhz4TextBlock.Text += MFmhz4;

            double MFy1 = Math.Min(MFt1, MFmhz1);
            double MFy2 = Math.Min(MFt1, MFmhz2);
            double MFy3 = Math.Max(MFt2, MFmhz3);
            double MFy4 = Math.Min(MFt3, MFmhz4);
            MFy3 = Math.Max(MFy3, MFy4);

            double maxCounter = 0, multCounter = 0, scalarMax = 0;

            for (int i = 0; i < 2090; i++)
            {
                rpmValues[i, 1] = rpmValues[i, 1] * MFy1;
                rpmValues[i, 2] = rpmValues[i, 2] * MFy2;
                rpmValues[i, 3] = rpmValues[i, 3] * MFy3;

                double maxValue = Math.Max(Math.Max(rpmValues[i, 1], rpmValues[i, 2]), rpmValues[i, 3]);
                maxCounter += maxValue;
                multCounter += i * maxValue;

                Point p = new(i / 10, -maxValue * MainCanvas.ActualHeight);
                points.Add(p);

                if (maxValue > 0 && i > scalarMax)
                {
                    scalarMax = i;
                }
            }
            if (gravityRadioButton.IsChecked == true)
                GravityCenter.Text += (double)multCounter / maxCounter;
            if (maxRadioButton.IsChecked == true)
                MaxMethod.Text += scalarMax;

            myPolygon.Points = points;
            MainCanvas.Children.Add(myPolygon);
        }
    }
}

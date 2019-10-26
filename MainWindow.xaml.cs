using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;

namespace StretchImage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //all constants
        const int MIN_ROLL = 0;
        const int MAX_ROLL = 6;
        const string IMMEDIATE = "0";
        const string FASTER = "1";
        const string FAST = "2";
        const string MODERATE = "3";
        const string SLOW = "4";
        const string SLOWER = "5";
        const int SPEED_IMMEDIATE = 0;
        const int SPEED_FASTER = 10;
        const int SPEED_FAST = 20;
        const int SPEED_MODERATE = 45;
        const int SPEED_SLOW = 70;
        const int SPEED_SLOWER = 95;
        const int NUMBER_OF_ROLLS = 20;
        const string SOUND_FILE = "sounds\\tick.mp3";
        const string OUTPUT_FILE = "DiceOutput.txt";
        const string IMAGE_FOLDER = "img";
        const double SPEED_MULTIPLIER = 1.1;

        List<string> imageLocations = new List<string>();
        DispatcherTimer stretchTimer = new DispatcherTimer();
        DispatcherTimer multipleRollsTimer = new DispatcherTimer();
        int leftToRightMargin = 0;
        int rightToLeftMargin = 0;
        int topToBottomMargin = 0;
        int bottomToTopMargin = 0;
        int randDirection = 0;
        int prevRand = 0;
        int prevRand2 = 0;
        int speed = 25;

        int waitVar = 0;

        public MainWindow()
        {
            InitializeComponent();
            Random rand = new Random();
            int diceRoll1 = rand.Next(MIN_ROLL, MAX_ROLL);
            prevRand = diceRoll1;
            stretchTimer.Tick += stretchTimer_Tick;
            stretchTimer.Interval = new TimeSpan(0, 0, 0, 0, 5);

            multipleRollsTimer.Tick += multipleRollsTimer_Tick;
            multipleRollsTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);

            imageLocations = Directory.GetFiles(IMAGE_FOLDER).ToList<string>();
            BitmapImage newImg = new BitmapImage();
            newImg.BeginInit();
            newImg.UriSource = new Uri(imageLocations[diceRoll1], UriKind.RelativeOrAbsolute);
            newImg.EndInit();
            imgToStretch.Source = newImg;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            multipleRollsTimer.Start();
        }

        private void multipleRollsTimer_Tick(object sender, EventArgs e)
        {
            if (waitVar < 20)
            {
                btnStr.IsEnabled = false;
                leftToRightMargin = 10;
                rightToLeftMargin = 210;
                topToBottomMargin = 10;
                bottomToTopMargin = 210;

                //set up roll random info
                Random rand = new Random();
                do
                {
                    randDirection = rand.Next(0, 4);
                } while (randDirection == prevRand2);
                prevRand2 = randDirection;
                int diceRoll1;
                do
                {
                    diceRoll1 = rand.Next(MIN_ROLL, MAX_ROLL);
                } while (diceRoll1 == prevRand);
                prevRand = diceRoll1;

                switch (randDirection)
                {
                    //top to bottom
                    case 0:
                        imgToStretch.Width = 200;
                        imgToStretch.Height = 0;
                        break;

                    //right to left
                    case 1:
                        imgToStretch.Width = 0;
                        imgToStretch.Height = 200;
                        break;

                    //bottom to top
                    case 2:
                        imgToStretch.Width = 200;
                        imgToStretch.Height = 0;
                        break;

                    //left to right
                    case 3:
                        imgToStretch.Width = 0;
                        imgToStretch.Height = 200;
                        break;
                }

                imgToStretch2.Width = 200;
                imgToStretch2.Height = 200;
                imgToStretch2.Source = imgToStretch.Source;
                imgToStretch2.Margin = imgToStretch.Margin;

                BitmapImage newImg = new BitmapImage();
                newImg.BeginInit();
                newImg.UriSource = new Uri(imageLocations[diceRoll1], UriKind.RelativeOrAbsolute);
                newImg.EndInit();
                imgToStretch.Source = newImg;

                stretchTimer.Start();
            }
            else
            {
                multipleRollsTimer.Stop();
                stretchTimer.Stop();
                btnStr.IsEnabled = true;
            }
        }

        private void stretchTimer_Tick(object sender, EventArgs e)
        {
            switch (randDirection)
            {
                //top to bottom
                case 0:
                    if (imgToStretch.ActualHeight <= 200 && imgToStretch2.Height > 0)
                    {
                        imgToStretch.Height += speed;
                        imgToStretch2.Height -= speed;
                        topToBottomMargin += speed;
                        imgToStretch2.Margin = new Thickness(10, topToBottomMargin, 0, 0);
                    }
                    else
                    {
                        stretchTimer.Stop();
                        waitVar++;
                    }
                    break;

                //right to left
                case 1:
                    if (imgToStretch.ActualWidth <= 200 && imgToStretch2.Width > 0)
                    {
                        imgToStretch.Width += speed;
                        imgToStretch2.Width -= speed;
                        rightToLeftMargin -= speed;
                        imgToStretch.Margin = new Thickness(rightToLeftMargin, 10, 0, 0);
                    }
                    else
                    {
                        stretchTimer.Stop();
                        waitVar++;
                    }
                    break;

                //bottom to top
                case 2:
                    if (imgToStretch.ActualHeight <= 200 && imgToStretch2.Height > 0)
                    {
                        imgToStretch.Height += speed;
                        imgToStretch2.Height -= speed;
                        bottomToTopMargin -= speed;
                        imgToStretch.Margin = new Thickness(10, bottomToTopMargin, 0, 0);
                    }
                    else
                    {
                        stretchTimer.Stop();
                        waitVar++;
                    }
                    break;

                //left to right
                case 3:
                    if (imgToStretch.ActualWidth <= 200 && imgToStretch2.Width > 0)
                    {
                        imgToStretch.Width += speed;
                        imgToStretch2.Width -= speed;
                        leftToRightMargin += speed;
                        imgToStretch2.Margin = new Thickness(leftToRightMargin, 10, 0, 0);
                    }
                    else
                    {
                        stretchTimer.Stop();
                        waitVar++;
                    }
                    break;
                default:
                    stretchTimer.Stop();
                    waitVar++;
                    break;
            }

            
        }
    }
}

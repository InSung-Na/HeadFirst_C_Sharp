using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace _1.MatchGame
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();
        private int m_nTimeElapsed_Deciseconds;
        private int m_nMatchesFound;
        private TextBlock m_tbLastSelectedTextBlock;
        private bool m_bFindingMatch = false;

        public MainWindow()
        {
            InitializeComponent();
            SetUpGame();

            timer.Interval = TimeSpan.FromSeconds(0.1);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Task.Run(() => Timer_Refresh());
        }
        private void Timer_Refresh()
        {
            Dispatcher.Invoke(() =>
            {
                m_nTimeElapsed_Deciseconds--;
                TimeTextBlock.Text = (m_nTimeElapsed_Deciseconds / 10F).ToString("0.0s");

                if (m_nMatchesFound >= 8 || m_nTimeElapsed_Deciseconds <= 0)
                {
                    TimeTextBlock.Text = $"{TimeTextBlock.Text} - Play again?";
                    timer.Stop();
                }
            });
            //if (m_nMatchesFound >= 8 || m_nTimeElapsedMillisecond <= 0)
            //{
            //    Dispatcher.Invoke(() =>
            //    {
            //        TimeTextBlock.Text = $"{TimeTextBlock.Text} - Play again?";
            //        timer.Stop();
            //    });
            //}

        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>()
            {
                "🐯", "🐯",
                "🐕", "🐕",
                "🦄", "🦄",
                "🐇", "🐇",
                "🦘", "🦘",
                "🐖", "🐖",
                "🐂", "🐂",
                "🐒", "🐒",
            };

            Random random = new Random();
            foreach(TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if(textBlock.Name != "TimeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    int index = random.Next(animalEmoji.Count);
                    string nextemoji = animalEmoji[index];
                    textBlock.Text = nextemoji;
                    animalEmoji.RemoveAt(index);
                }
            }

            m_nTimeElapsed_Deciseconds = 100;
            m_nMatchesFound = 0;
            timer.Start();
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if(m_bFindingMatch == false)
            {
                m_tbLastSelectedTextBlock = textBlock;
                textBlock.Visibility = Visibility.Hidden;
                m_bFindingMatch = true;
            }
            else if(m_tbLastSelectedTextBlock.Text == textBlock.Text)
            {
                textBlock.Visibility = Visibility.Hidden;
                m_bFindingMatch = false;
            }
            else
            {
                m_tbLastSelectedTextBlock.Visibility = Visibility.Visible;
                m_bFindingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(m_nMatchesFound >= 8 || m_nTimeElapsed_Deciseconds <= 0)
                SetUpGame();
        }
    }
}

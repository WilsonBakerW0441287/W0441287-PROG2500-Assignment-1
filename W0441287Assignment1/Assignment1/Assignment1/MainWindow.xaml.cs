using Microsoft.Win32;
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

namespace Assignment1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isPlaying = false;
        TagLib.File? file;
        string NameOfFileNoExtention;
        public MainWindow()
        {
            InitializeComponent();
            isPlaying = false;
            btnTagSave.IsEnabled = false;
            btnEditTags.IsEnabled = false;
            btnNowPlaying.IsEnabled = false;
            btnPlay.IsEnabled = false;
            btnStop.IsEnabled = false;
            btnPause.IsEnabled = false;

            myMediaElement.LoadedBehavior = MediaState.Manual;
        }
        private void Play_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = (myMediaElement.Source != null);
        }

        private void Play_Executed(object sender, RoutedEventArgs e)
        {
            myMediaElement.Play();
            isPlaying = true;
        }

        private void Pause_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isPlaying;
        }

        private void Pause_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            myMediaElement.Pause();
        }

        private void Stop_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = isPlaying;
        }

        private void Stop_Executed(object sender, RoutedEventArgs e)
        {
            myMediaElement.Stop();
            isPlaying = false;
        }
        private void btnFileChoice_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDLG = new OpenFileDialog();
            fileDLG.Filter = "MP3-Files|*.mp3|WMA-Files|*.wma";
            //showdialog method shows the user a file choosing dialog box
            //if true than the user selected a file
            if (fileDLG.ShowDialog() == true) //this is technincally error handling
            {
                //set the current file to the file the user selected
                file = TagLib.File.Create(fileDLG.FileName);
                //set the text of the label to the name of the file
                lblSongInfo.Content = file.Tag.Title + "\n" + file.Tag.Album + "\n" + file.Tag.Year + "\n" + file.Tag.AlbumArtists[0];

                //Allow the media element to play the song
                myMediaElement.Source = new Uri(fileDLG.FileName);

                //attempt to find image
                try
                {
                    //set the image source to the image in the file
                    //using fileDLG get the NAME of the file ONLY
                    NameOfFileNoExtention = System.IO.Path.GetFileNameWithoutExtension(fileDLG.FileName);
                    imgAbulmCover.Source = new BitmapImage(
                        new Uri("E:\\IT programming LAST SEM\\Windows programming\\Assignment1Github\\W0441287-PROG2500-Assignment-1\\W0441287Assignment1\\Mp3s\\" + NameOfFileNoExtention + ".png"));
                }
                catch (Exception)
                {
                    //if no image is found, set the image source to a default image
                    imgAbulmCover.Source = null;
                }

                //Edit the Tag Editor stackpannel to display song infromation
                txtSongName.Text = file.Tag.Title;
                txtAlbumName.Text = file.Tag.Album;
                txtYear.Text = file.Tag.Year.ToString();
                //re-enable the buttons
                btnTagSave.IsEnabled = true;
                btnEditTags.IsEnabled = true;
                btnNowPlaying.IsEnabled = true;
                btnPlay.IsEnabled = true;
                btnStop.IsEnabled = true;
                btnPause.IsEnabled = true;
                //allow the user to use the menu items 
                menuMedia.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Please select a valid file");
            }
        }

        private void fileExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Close this window
            Close();
        }

        private void btnNowPlaying_Click(object sender, RoutedEventArgs e)
        {
            if (lblSongInfo.Visibility == Visibility.Visible)
            {
                lblSongInfo.Visibility = Visibility.Hidden;
            }
            else
            {
                lblSongInfo.Visibility = Visibility.Visible;
            }
        }

        private void btnEditTags_Click(object sender, RoutedEventArgs e)
        {
            if (spEditScreen.Visibility == Visibility.Visible)
            {
                spEditScreen.Visibility = Visibility.Hidden;
            }
            else
            {
                spEditScreen.Visibility = Visibility.Visible;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (file != null)
                {
                    file.Tag.Title = txtSongName.Text;
                    file.Tag.Album = txtAlbumName.Text;
                    file.Tag.Year = Convert.ToUInt32(txtYear.Text);
                    if (txtSongName.Text == "" || txtAlbumName.Text == "" || txtYear.Text == "")
                    {
                        MessageBox.Show("Please fill in all fields");
                    }
                    else
                    {
                        myMediaElement.Source = null;
                        Thread.Sleep(100);
                        file.Save();
                        lblSongInfo.Content = file.Tag.Title + "\n" + file.Tag.Album + "\n" + file.Tag.Year;
                        spEditScreen.Visibility = Visibility.Hidden;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "Error saving info!");
            }
        }
    }
}
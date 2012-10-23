﻿/*
 * Copyright © 2012 Nokia Corporation. All rights reserved.
 * Nokia and Nokia Connecting People are registered trademarks of Nokia Corporation. 
 * Other product and company names mentioned herein may be trademarks
 * or trade names of their respective owners. 
 * See LICENSE.TXT for license information.
 */

using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework.Media;
using System;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace CameraExplorer
{
    /// <summary>
    /// Preview page displays the captured photo from DataContext.ImageStream and
    /// has a button to save the image to phone's camera roll.
    /// </summary>
    public partial class PreviewPage : PhoneApplicationPage
    {
        private CameraExplorer.DataContext _dataContext = CameraExplorer.DataContext.Singleton;
        private BitmapImage _bitmap = new BitmapImage();

        public PreviewPage()
        {
            InitializeComponent();

            DataContext = _dataContext;
        }

        /// <summary>
        /// When navigating to this page, if camera has not been initialized (for example returning from
        /// tombstoning), application will navigate directly back to the main page. Otherwise the
        /// DataContext.ImageStream will be set as the source for the Image control in XAML.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (_dataContext.Device == null)
            {
                NavigationService.GoBack();
            }
            else
            {
                _bitmap.SetSource(_dataContext.ImageStream);

                image.Source = _bitmap;
            }

            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Clicking on the save button saves the photo in DataContext.ImageStream to media library
        /// camera roll. Once image has been saved, the application will navigate back to the main page.
        /// </summary>
        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Reposition ImageStream to beginning, because it has been read already in the OnNavigatedTo method.
                _dataContext.ImageStream.Position = 0;

                MediaLibrary library = new MediaLibrary();

                library.SavePictureToCameraRoll("CameraExplorer_" + DateTime.Now.ToString() + ".jpg", _dataContext.ImageStream);
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Saving picture to camera roll failed");
            }

            NavigationService.GoBack();
        }
    }
}
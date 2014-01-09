using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows;


namespace Baby_Chatter
{
    public class Utilities
    {
        /*
         * Save a child's image to disk and return the filepath of the saved image
         * If childImage is null, null will be returned.
         */
        public static string SaveChildImageToDisk(string childName, BitmapImage childImage)
        {
            if (childImage == null)
            {
                return null;
            }

            string filename = childName + "_photo.jpg";
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream fileStream = isf.CreateFile(filename))
                {
                    // generate file to save
                    WriteableBitmap wb = new WriteableBitmap(childImage);
                    Extensions.SaveJpeg(wb, fileStream, wb.PixelWidth, wb.PixelHeight, 0, 100);
                    fileStream.Close();
                }
            }

            return filename;
        }

        /*
         * Save a BitmapImage to the disk as a JPEG.  Return a unique filename for the image or
         * null if the image is null.
         */
        public static string SavePhotoToDisk(BitmapImage image)
        {
            if (image == null)
            {
                return null;
            }

            string filename = "photo_" + DateTime.Today.Ticks + ".jpg";

            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream fileStream = isf.CreateFile(filename))
                {
                    // generate file to save
                    WriteableBitmap wb = new WriteableBitmap(image);
                    Extensions.SaveJpeg(wb, fileStream, wb.PixelWidth, wb.PixelHeight, 0, 100);
                    fileStream.Close();
                }
            }

            return filename;
        }

        /*
         * Get the image associated with the filename from disk.
         */
        public static BitmapImage GetImageFromDisk(string filename)
        {
            if (String.IsNullOrEmpty(filename))
            {
                return null;
            }

            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!isf.FileExists(filename))
                {
                    return null;
                }

                using (IsolatedStorageFileStream fs = new IsolatedStorageFileStream(filename, FileMode.Open, isf))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(fs);
                    return bitmapImage;
                }
            }
        }

        /*
         * Delete the image with the given filename from the disk.  Return true if successful, false if not.
         */
        public static Boolean DeleteImageFromDisk(string filename)
        {
            if (String.IsNullOrEmpty(filename))
            {
                return false;
            }
            using (IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!isf.FileExists(filename))
                {
                    return false;
                }

                isf.DeleteFile(filename);
                return true;
            }
        }

        /*
         * Capitalize the first letter of the string
         */
        public static string CapitalizeString(string s)
        {
            switch (s.Length)
            {
                case 0:
                    return "";
                case 1:
                    return Char.ToUpper(s[0]) + "";
                default:
                    return Char.ToUpper(s[0]) + s.Substring(1);
            }
        }

        /*
         * Returns a cropped, square image from the center of the supplied bitmap.  The returned image retains the same resolution
         * as the original image.
         */
        public static WriteableBitmap GetThumbnail(BitmapSource original)
        {
            if (original != null)
            {
                // Crop bitmap and create a square image to display.  The resulting cropped image will have height and width
                // equal to the minimum of either the height or width of the image and will be centered based on the original image.
                WriteableBitmap writeable = new WriteableBitmap(original);
                double cropRectHeightWidth = Math.Min(writeable.PixelWidth, writeable.PixelHeight);
                double x = .5 * (writeable.PixelWidth - cropRectHeightWidth);
                double y = .5 * (writeable.PixelHeight - cropRectHeightWidth);
                Rect cropRect = new Rect(x, y, cropRectHeightWidth, cropRectHeightWidth);
                writeable = writeable.Crop(cropRect);
                return writeable;
            }

            return null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ImageStenography
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private static String str = AppDomain.CurrentDomain.BaseDirectory + "images\\";
        private String IMAGE_UPLOAD_DIRECTORY = str;
        private const String IMAGE_PATH_SHORT = "images\\";
        private int count = 0;
        private const int IMAGE_WIDTH = 500;
        protected void Page_Load(object sender, EventArgs e)
        {
            imageTest.Visible = false;
            imageTwo.Visible = false;

        }

        //When the upload button is pressed, try and get the two images from either the URL entered or the upload file object for each. The files are saved 
        //to the server and their file names are stored as labels on the page, making it easy to reference them later.
        protected void UploadButtonPressed(object sender, EventArgs e)
        {
            //Get the first image from upload
            if (concealingFileUpload.HasFile)
            {
                try
                {
                    //file must be an image less than 20 MB in size
                    if (concealingFileUpload.PostedFile.ContentType.StartsWith("image") && concealingFileUpload.PostedFile.ContentLength < 104857600)
                    {

                        string filename = Path.GetFileName(concealingFileUpload.FileName);
                        concealingFileUpload.SaveAs(IMAGE_UPLOAD_DIRECTORY + filename);
                        concealingImageUploadedText.Text = filename;

                        
                    }
                }
                catch (Exception ex)
                {
                    concealingImageUploadedText.Text = ex.ToString();
                }
            }
            //otherwise try and get it from the URL
            else
            {
                if (concealingImageURLTextBox.Text.StartsWith("http"))
                {
                    getImageFromURL(concealingImageURLTextBox.Text, concealingImageURLTextBox);
                }
                else if (concealingImageURLTextBox.Text.StartsWith("www"))
                {
                    getImageFromURL("http://" + concealingImageURLTextBox.Text, concealingImageURLTextBox);
                }
            }

            //get the second image from a file upload
            if (concealedFileUpload.HasFile)
            {
                try
                {
                    //file must be an image less than 20 MB in size
                    if (concealedFileUpload.PostedFile.ContentType.StartsWith("image") && concealedFileUpload.PostedFile.ContentLength < 104857600)
                    {
                        string filename = Path.GetFileName(concealedFileUpload.FileName);
                        concealedFileUpload.SaveAs(IMAGE_UPLOAD_DIRECTORY + filename);
                        concealedImageUploadedText.Text = filename;
                    }
                }
                catch (Exception ex)
                {

                }
            }
            //otherwise get it from the URL
            else
            {
                if (concealedImageURL.Text.StartsWith("http"))
                {
                    getImageFromURL(concealedImageURL.Text, concealedImageURL);
                }
                else if (concealedImageURL.Text.StartsWith("www"))
                {
                    getImageFromURL("http://" + concealedImageURL.Text, concealedImageURL);
                }
            }

            //the first image must be uploaded regardless of the option selected
            if (!(concealingImageUploadedText.Text.Equals("upload an image")))
            {
                //if the option is set to conceal an image, we need both images uploaded
                if (typeOfOperation.SelectedIndex == 0)
                {
                    //to conceal an image we need the second image as well 
                    if (!(concealedImageUploadedText.Text.Equals("upload an image")))
                    {
                        //get the two images
                        Bitmap concealingBitmap = new Bitmap(IMAGE_UPLOAD_DIRECTORY + concealingImageUploadedText.Text);
                        Bitmap concealedBitmap = new Bitmap(IMAGE_UPLOAD_DIRECTORY + concealedImageUploadedText.Text);

                        //find the width and height to use (has to be global maximum between both images)
                        int width = (concealingBitmap.Width < concealedBitmap.Width) ? concealingBitmap.Width : concealedBitmap.Width;
                        int height = (concealingBitmap.Height < concealedBitmap.Height) ? concealingBitmap.Height : concealedBitmap.Height;

                        //get the number of bits to use to hide the image
                        int numberOfBits = (encodingBits.SelectedIndex);

                        //algorithm re-implemented with lockbits
                        Rectangle rect1 = new Rectangle(0, 0, concealingBitmap.Width, concealingBitmap.Height);
                        System.Drawing.Imaging.BitmapData concealingBMPData = concealingBitmap.LockBits(rect1, System.Drawing.Imaging.ImageLockMode.ReadWrite, concealingBitmap.PixelFormat);
                        rect1 = new Rectangle(0, 0, concealedBitmap.Width, concealedBitmap.Height);
                        System.Drawing.Imaging.BitmapData concealedBMPData = concealedBitmap.LockBits(rect1, System.Drawing.Imaging.ImageLockMode.ReadWrite, concealedBitmap.PixelFormat);
                        
                        //get an array of bytes for each image
                        int concealingBytesLength = Math.Abs(concealingBMPData.Stride) * concealingBitmap.Height;
                        byte[] concealingBytes = new byte[concealingBytesLength];
                        int concealedBytesLength = Math.Abs(concealedBMPData.Stride) * concealedBitmap.Height;
                        byte[] concealedBytes = new byte[concealedBytesLength];
                        System.Runtime.InteropServices.Marshal.Copy(concealingBMPData.Scan0, concealingBytes, 0, concealingBytesLength);
                        System.Runtime.InteropServices.Marshal.Copy(concealedBMPData.Scan0, concealedBytes, 0, concealedBytesLength);

                        for (int column = 0; column < 3*width; column++)
                        {
                            for (int row = 0; row < height; row++)
                            {
                                int concealingImageByte = row*concealingBMPData.Stride + column;
                                int concealedImageByte = row * concealedBMPData.Stride + column;
                                //The Stenography part, on each individual pixel
                                byte encodingCoefficient = (byte)((255 << numberOfBits));
                                //AND this with the concealing Byte
                                concealingBytes[concealingImageByte] = (byte)(concealingBytes[concealingImageByte] & encodingCoefficient);
                                //Now get the compressed image to hide
                                concealedBytes[concealedImageByte] = (byte)(concealedBytes[concealedImageByte] >> (8 - numberOfBits));
                                //finally combine the to with an OR
                                concealingBytes[concealingImageByte] = (byte)(concealingBytes[concealingImageByte] | concealedBytes[concealedImageByte]);
                            }
                        }


                        //unlock everything
                        System.Runtime.InteropServices.Marshal.Copy(concealingBytes, 0, concealingBMPData.Scan0, concealingBytesLength);
                        concealingBitmap.UnlockBits(concealingBMPData);
                        concealedBitmap.UnlockBits(concealedBMPData);


                        //Save the output
                        concealingBitmap.Save(IMAGE_UPLOAD_DIRECTORY + "output.png", ImageFormat.Png);
                        //concealingBitmap.Save(IMAGE_UPLOAD_DIRECTORY + "output.png");
                        //show the output at the bottom of the page
                        imageTest.Visible = true;
                        imageTwo.ImageUrl = IMAGE_PATH_SHORT + "output.png";
                        imageTest.ImageUrl = IMAGE_PATH_SHORT+ concealingImageUploadedText.Text;
                        imageTwo.Visible = true;
                        imageTest.Width = IMAGE_WIDTH;
                        imageTest.Height = IMAGE_WIDTH * concealingBitmap.Height / concealingBitmap.Width;
                        imageTwo.Width = imageTest.Width;
                        imageTwo.Height = imageTest.Height;

                        concealingBitmap.Dispose();
                        concealedBitmap.Dispose();


                    }
                }

                //for recovering a concealed image
                else if (typeOfOperation.SelectedIndex == 1)
                {
                    //get the image
                    Bitmap concealingBitmap = new Bitmap(IMAGE_UPLOAD_DIRECTORY + concealingImageUploadedText.Text);
                    int numberOfBits = (encodingBits.SelectedIndex);
                    
                    //re-implemented with lockbits
                    //takes image into an array of RGB byte values
                    Rectangle rect1 = new Rectangle(0, 0, concealingBitmap.Width, concealingBitmap.Height);
                    System.Drawing.Imaging.BitmapData concealingBMPData = concealingBitmap.LockBits(rect1, System.Drawing.Imaging.ImageLockMode.ReadWrite, concealingBitmap.PixelFormat);
                    int concealingBytesLength = Math.Abs(concealingBMPData.Stride) * concealingBitmap.Height;
                    byte[] concealingBytes = new byte[concealingBytesLength];
                    System.Runtime.InteropServices.Marshal.Copy(concealingBMPData.Scan0, concealingBytes, 0, concealingBytesLength);
                    for (int i = 0; i < concealingBytesLength; i++)
                    {
                        concealingBytes[i] = (byte)(concealingBytes[i] << (8 - numberOfBits));
                    }
                    //unlock and save
                    System.Runtime.InteropServices.Marshal.Copy(concealingBytes, 0, concealingBMPData.Scan0, concealingBytesLength);
                    concealingBitmap.UnlockBits(concealingBMPData);
                    concealingBitmap.Save(IMAGE_UPLOAD_DIRECTORY + "output1.png", ImageFormat.Png);

                    //concealingBitmap.Save(IMAGE_UPLOAD_DIRECTORY + "output1.png");
                    //show the images
                    imageTest.Visible = true;
                    imageTwo.Visible = true;
                    imageTest.ImageUrl = IMAGE_PATH_SHORT + concealingImageUploadedText.Text;
                    imageTwo.ImageUrl = IMAGE_PATH_SHORT + "output1.png";
                    imageTest.Width = IMAGE_WIDTH;
                    imageTest.Height = concealingBitmap.Height *IMAGE_WIDTH / concealingBitmap.Width;
                    imageTwo.Width = imageTest.Width;
                    imageTwo.Height = imageTest.Height;

                    concealingBitmap.Dispose();

                }
                
            }
        }
        //downloads an image from a url
        protected void getImageFromURL(String urlStr, object sender)
        {
            Uri remoteImgPathUri = new Uri(urlStr);
            string remoteImgPathWithoutQuery = remoteImgPathUri.GetLeftPart(UriPartial.Path);
            string fileName = Path.GetFileName(remoteImgPathWithoutQuery);
            string localPath = IMAGE_UPLOAD_DIRECTORY + fileName;
            WebClient webClient = new WebClient();
            //create a temp file in case someone wants to download a file already on the server
            webClient.DownloadFile(urlStr, IMAGE_UPLOAD_DIRECTORY+"temp.png");
            FileStream tempFile = new FileStream(IMAGE_UPLOAD_DIRECTORY + "temp.png", FileMode.Open);
            FileStream finalFile = new FileStream(localPath, FileMode.Create);
            tempFile.CopyTo(finalFile);
            tempFile.Close();
            finalFile.Close();
            if (sender.Equals(concealingImageURLTextBox))
            {
                concealingImageUploadedText.Text = fileName;

            }
            else
            {
                concealedImageUploadedText.Text = fileName;
            }
        }

        //make the second image upload visible only when the conceal option is selected
        protected void optionSelectedChanged(object sender, EventArgs e)
        {
            if (typeOfOperation.SelectedIndex == 0)
            {
                secondImageUploadPanel.Visible = true;
            }
            else
            {
                secondImageUploadPanel.Visible = false;
            }

        }

        //when a url is added, release the file selected for upload
        protected void concealingImageURLAdded(object sender, EventArgs e)
        {
            if (concealingImageURLTextBox.Text.StartsWith("http"))
            {
                //imageTest.ImageUrl = concealingImageURLTextBox.Text;
                //getImageFromURL(concealingImageURLTextBox.Text, sender);
                //concealingImageUploadedText.Text = "upload an image";
                //clear the file upload, as the file should now come from the URL
                concealingFileUpload = new FileUpload();
            }
            else if (concealingImageURLTextBox.Text.StartsWith("www"))
            {
                // imageTest.ImageUrl = "http://" + concealingImageURLTextBox.Text;
                //getImageFromURL("http://" + concealingImageURLTextBox.Text, sender);
                //concealingImageUploadedText.Text = "upload an image";
                //clear the file upload, as the file should now come from the URL
                concealingFileUpload = new FileUpload();
            }
        }

        //same as the previous method but for the second file upload
        protected void concealedImageURLAdded(object sender, EventArgs e)
        {
            if (concealedImageURL.Text.StartsWith("http"))
            {
                //imageTest.ImageUrl = concealingImageURLTextBox.Text;
                //getImageFromURL(concealedImageURL.Text, sender);
                //concealingImageUploadedText.Text = "upload an image";
                //clear the file upload, as the file should now come from the URL
                concealedFileUpload = new FileUpload();
            }
            else if (concealedImageURL.Text.StartsWith("www"))
            {
                // imageTest.ImageUrl = "http://" + concealingImageURLTextBox.Text;
                //getImageFromURL("http://" + concealedImageURL.Text, sender);
                //concealingImageUploadedText.Text = "upload an image";
                //clear the file upload, as the file should now come from the URL
                concealedFileUpload = new FileUpload();
            }
        }
        
        //delete the uploaded images when done.
        protected void disposeOfUploadedImages(object sender, EventArgs e)
        {
            try
            {
                File.Delete(IMAGE_UPLOAD_DIRECTORY + concealingImageUploadedText.Text);
            }
            catch
            {

            }
            try
            {
                File.Delete(IMAGE_UPLOAD_DIRECTORY + concealedImageUploadedText.Text);
            }
            catch
            {

            }
        }

    }
}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Image_Processing_Application
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Mouse Functions
        /// </summary>

        private bool isGetXYRGBValuesActivated = false;

        private void mainPictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (isGetXYRGBValuesActivated)
            {
                // Checks if the main picture box is empty or not
                bool isMainPictureBoxNullOrEmpty = mainPictureBox == null || mainPictureBox.Image == null;
                if (!isMainPictureBoxNullOrEmpty)
                {
                    // Variables for the x, y coordinates, the bitmap for the main picture box, color value of the coordinate
                    int xCoordinate = e.X, yCoordinate = e.Y;
                    Color coordinatePixelValue = this.bitMapOriginal.GetPixel(xCoordinate, yCoordinate);

                    // Set the x, y coordinate textbox to where the mouse is hovering
                    xCoordinateTextBox.Text = xCoordinate.ToString();
                    yCoordinateTextBox.Text = yCoordinate.ToString();

                    // Set the RGB value textbox to where the mouse is hovering
                    rValueTextBox.Text = coordinatePixelValue.R.ToString();
                    gValueTextBox.Text = coordinatePixelValue.G.ToString();
                    bValueTextBox.Text = coordinatePixelValue.B.ToString();
                    
                    // Get the RGB value of the specific coordinate in the picture
                    int redValue = coordinatePixelValue.R;
                    int greenValue = coordinatePixelValue.G;
                    int blueValue = coordinatePixelValue.B;

                    // Display the RGB value in the RGB text boxes
                    rValueTextBox.Text = redValue.ToString();
                    gValueTextBox.Text = greenValue.ToString();
                    bValueTextBox.Text = blueValue.ToString();

                    isGetXYRGBValuesActivated = false;
                }

            }
        }

        /// <summary>
        /// Button Functions by Calculation
        /// </summary>

        // Variables for the Bitmap's row and column
        int row, column;

        // Bitmap variables for original and result pictures
        Bitmap bitMapOriginal, bitMapResult;

        /**
         * Function to save the picture uploaded from the file explorer to the global original bit map variable
         */
        private void saveBitMapOriginal(Bitmap bitMapResult)
        {
            this.bitMapOriginal = bitMapResult;
        }

        /**
         * Function to save the bitmap of resulting calculation to the global bit map result variable
         */
        private void saveAndDisplayBitMapResult(Bitmap bitMapOriginal)
        {
            this.bitMapResult = bitMapOriginal;
            resultPictureBox.Image = this.bitMapResult;
        }

        /**
         * Function to setup the original and result Bitmap
         */
        private void bitMapSetup()
        {
            // Get the width and height of the original picture
            row = this.bitMapOriginal.Width;
            column = this.bitMapOriginal.Height;

            // Create a new Bitmap with the size of the original picture
            this.bitMapResult = new Bitmap(row, column);
        }

        /**
         * Function to change the brightness of the picture based on the brightness value input
         */
        private void changeBrightnessButton_Click(object sender, EventArgs e)
        {
            // Get the brightness value from brightness text box
            int brightnessValue = Convert.ToInt16(brightnessTextBox.Text);
            int redValue, greenValue, blueValue;

            // Setup the original and result Bitmap
            bitMapSetup();

            // Display the loading animation for the cursor
            Cursor = Cursors.WaitCursor;

            // For how tall the original picture is
            for (int i = 0; i < row; i++)
            {
                // For how wide the original picture is
                for (int j = 0; j < column; j++)
                {
                    Color coordinatePixelValue = this.bitMapOriginal.GetPixel(i, j);

                    // Add the brightness value to the original RGB value 
                    redValue = coordinatePixelValue.R + brightnessValue;
                    greenValue = coordinatePixelValue.G + brightnessValue;
                    blueValue = coordinatePixelValue.B + brightnessValue;

                    // If the resulting RGB value exceeds maximum brightness value
                    if (redValue > 255) redValue = 255;
                    if (greenValue > 255) greenValue = 255;
                    if (blueValue > 255) blueValue = 255;

                    // If the resulting RGB value exceeds minimum brightness value
                    if (redValue < 0) redValue = 0;
                    if (greenValue < 0) greenValue = 0;
                    if (blueValue < 0) blueValue = 0;

                    // Set the pixel of Coordinate (i, j) of the resulting picture to the resulting RGB value 
                    this.bitMapResult.SetPixel(i, j, Color.FromArgb(redValue, greenValue, blueValue));
                }
            }

            // Display the resulting 
            resultPictureBox.Image = this.bitMapResult;

            // Stop the cursor loading animation
            Cursor = Cursors.Default;
        }

        private void changePictureToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            // Open a Windows dialog
            OpenFileDialog open = new OpenFileDialog();

            // Fiter the format that can be displayed by the dialog
            open.Filter = "Image Files(*.jpg; *.bmp)|*.jpg; *.bmp";

            // Change the picture in the main picture box
            if (open.ShowDialog() == DialogResult.OK)
            {
                saveBitMapOriginal(new Bitmap(open.FileName));
                mainPictureBox.Image = this.bitMapOriginal;
            }
        }

        /**
         * Function to get the RGB values based on the coordinates input
         */
        private void getCoordinatesRGBValue_Click(object sender, EventArgs e)
        {
            isGetXYRGBValuesActivated = true;
        }

        /**
         * Function that returns the greyscale value based on averaging method
         */ 
        private int getGreyscaleAverageValue(Bitmap bitmap, int row, int column)
        {
            Color pixel = bitmap.GetPixel(row, column);
            int redValue = pixel.R;
            int greenValue = pixel.G;
            int blueValue = pixel.B;

            return (redValue + greenValue + blueValue) / 3;
        }

        /**
         * Function to greyscale a picture using averaging method
         */ 
        private void greyscaleAveragingButton_Click(object sender, EventArgs e)
        {
            // Greyscaled RGB values
            int greyscaledValue;

            // Setup the original and result Bitmap
            bitMapSetup();

            // Display the loading animation for the cursor
            Cursor = Cursors.WaitCursor;

            // For how tall the original picture is
            for (int i = 0; i < row; i++)
            {
                // For how wide the original picture is
                for (int j = 0; j < column; j++)
                {
                    // Convert the original RGB to inverted value
                    greyscaledValue = getGreyscaleAverageValue(bitMapOriginal, i, j);

                    // Set the pixel of Coordinate (i, j) of the resulting picture to the inverted RGB value 
                    this.bitMapResult.SetPixel(i, j, Color.FromArgb(greyscaledValue, greyscaledValue, greyscaledValue));
                }
            }

            // Display the resulting 
            resultPictureBox.Image = this.bitMapResult;

            // Stop the cursor loading animation
            Cursor = Cursors.Default;
        }

        /**
         * Function to greyscale a picture using luma method
         */
        private void greyscaleLumaButton_Click(object sender, EventArgs e)
        {
            // Greyscaled RGB values
            int redValue, greenValue, blueValue, roundedGreyscaledValue;
            double greyscaledValue;

            // Setup the original and result Bitmap
            bitMapSetup();

            // Display the loading animation for the cursor
            Cursor = Cursors.WaitCursor;

            // For how tall the original picture is
            for (int i = 0; i < row; i++)
            {
                // For how wide the original picture is
                for (int j = 0; j < column; j++)
                {
                    Color coordinatePixelValue = this.bitMapOriginal.GetPixel(i, j);

                    // Get the original RGB value
                    redValue = coordinatePixelValue.R;
                    greenValue = coordinatePixelValue.G;
                    blueValue = coordinatePixelValue.B;

                    // Calculate greyscale value based on Luma method
                    greyscaledValue = 0.2126 * redValue + 0.7152 * greenValue + 0.0722 * blueValue;

                    // Convert the double value to integer value
                    roundedGreyscaledValue = Convert.ToInt16(greyscaledValue);

                    // Set the pixel of Coordinate (i, j) of the resulting picture to the inverted RGB value 
                    this.bitMapResult.SetPixel(i, j, Color.FromArgb(roundedGreyscaledValue, roundedGreyscaledValue, roundedGreyscaledValue));
                }
            }

            // Display the resulting 
            resultPictureBox.Image = this.bitMapResult;

            // Stop the cursor loading animation
            Cursor = Cursors.Default;
        }

        /**
         * Function to invert the color of the picture by calculation
         */
        private void invertPictureButton_Click(object sender, EventArgs e)
        {
            // Inverted RGB values
            int invertRedValue, invertGreenValue, invertBlueValue;

            // Setup the original and result Bitmap
            bitMapSetup();

            // Display the loading animation for the cursor
            Cursor = Cursors.WaitCursor;

            // For how tall the original picture is
            for (int i = 0; i < row; i++)
            {
                // For how wide the original picture is
                for (int j = 0; j < column; j++)
                {
                    Color coordinatePixelValue = this.bitMapOriginal.GetPixel(i, j);

                    // Convert the original RGB to inverted value
                    invertRedValue = 255 - coordinatePixelValue.R;
                    invertGreenValue = 255 - coordinatePixelValue.G;
                    invertBlueValue = 255 - coordinatePixelValue.B;

                    // Set the pixel of Coordinate (i, j) of the resulting picture to the inverted RGB value 
                    this.bitMapResult.SetPixel(i, j, Color.FromArgb(invertRedValue, invertGreenValue, invertBlueValue));
                }
            }

            // Display the resulting 
            resultPictureBox.Image = this.bitMapResult;

            // Stop the cursor loading animation
            Cursor = Cursors.Default;
        }

        /**
         * Returns 0 OR 255 based on the rgbValue and thresholdValue
         */
        private int getThresholdValue(int rgbValue, int thresholdValue)
        {
            if(rgbValue > thresholdValue)
            {
                return 255;
            }

            return 0;
        }


        /**
         * Function to greyscale an image based on a specific threshold
         */
        private void thresholdPictureButton_Click(object sender, EventArgs e)
        {
            // Get the threshold value from threshold text box
            int thresholdValue = Convert.ToInt16(thresholdValueTextBox.Text);

            // Variables for the RGB and thresholded RGB value
            int redValue, greenValue, blueValue;
            int thresholdedRedValue, thresholdedGreenValue, thresholdedBlueValue, thresholdedFinalValue;

            // Setup the original and result Bitmap
            bitMapSetup();

            // Display the loading animation for the cursor
            Cursor = Cursors.WaitCursor;

            // For how tall the original picture is
            for (int i = 0; i < row; i++)
            {
                // For how wide the original picture is
                for (int j = 0; j < column; j++)
                {
                    Color coordinatePixelValue = this.bitMapOriginal.GetPixel(i, j);

                    // Get the original RGB value
                    redValue = coordinatePixelValue.R;
                    greenValue = coordinatePixelValue.G;
                    blueValue = coordinatePixelValue.B;

                    // Get the thresholded RGB value
                    thresholdedRedValue = getThresholdValue(redValue, thresholdValue);
                    thresholdedGreenValue = getThresholdValue(greenValue, thresholdValue);
                    thresholdedBlueValue = getThresholdValue(blueValue, thresholdValue);

                    // Calculate for the final threshold value
                    if(thresholdedRedValue > thresholdValue && thresholdedGreenValue > thresholdValue && thresholdedBlueValue > thresholdValue)
                    {
                        thresholdedFinalValue = 255;
                    }
                    else
                    {
                        thresholdedFinalValue = 0;
                    }

                    // Set the pixel of Coordinate (i, j) of the resulting picture to the resulting RGB value 
                    this.bitMapResult.SetPixel(i, j, Color.FromArgb(thresholdedFinalValue, thresholdedFinalValue, thresholdedFinalValue));
                }
            }

            // Display the resulting 
            resultPictureBox.Image = this.bitMapResult;

            // Stop the cursor loading animation
            Cursor = Cursors.Default;
        }

        /**
         * Function to show picture histogram
         */ 
        private void showPictureHistogramButton_Click(object sender, EventArgs e)
        {
            // Array to hold the greyscale values and their amount
            int[] histogramValue = new int[256];
            int max = 0, histogramValueMaxIndex = 0;

            // Setup the original and result Bitmap
            bitMapSetup();

            // Display the loading animation for the cursor
            Cursor = Cursors.WaitCursor;

            // Variable for the greyscale value
            int greyscaleValue;
            
            // For how tall the original picture is
            for (int i = 0; i < row; i++)
            {
                // For how wide the original picture is
                for (int j = 0; j < column; j++)
                {
                    // Convert the original RGB to inverted value
                    greyscaleValue = getGreyscaleAverageValue(bitMapOriginal, i, j);

                    // Set the pixel of Coordinate (i, j) of the resulting picture to the inverted RGB value 
                    this.bitMapResult.SetPixel(i, j, Color.FromArgb(greyscaleValue, greyscaleValue, greyscaleValue));

                    // Increment the amount of that specific greyscaleValue in the array
                    histogramValue[greyscaleValue]++;

                    // Change the max value
                    if (max < histogramValue[greyscaleValue])
                    {
                        max = histogramValue[greyscaleValue];
                        histogramValueMaxIndex = greyscaleValue;
                    }
                }
            }

            // Display the resulting 
            resultPictureBox.Image = this.bitMapResult;

            // Stop the cursor loading animation
            Cursor = Cursors.Default;

            // Variables regarding histogram
            int histogramHeight = 236;
            float histogramValuePercentage;
            Bitmap histogramData = new Bitmap(256, histogramHeight + 10);

            // Draw the histogram
            using (Graphics g = Graphics.FromImage(histogramData))
            {
                for (int v = 0; v < 256; v++)
                {
                    histogramValuePercentage = (histogramValue[v] * histogramHeight) / max;   // What percentage of the max is this value
                    g.DrawLine(
                        Pens.Black,
                        new Point(v, histogramHeight),
                        new Point(v, histogramHeight - (int)Math.Round(histogramValuePercentage))  // Use that percentage of the height
                    );
                }
            }

            // Variable to display the histogram in a form
            var histogramForm = new Form
            {
                ShowInTaskbar = false,
                TopMost = true,
                Size = histogramData.Size,
                Location = new Point(0, 0),
                StartPosition = FormStartPosition.Manual,
                BackgroundImage = histogramData
            };

            // Display the form
            histogramForm.Show();
        }

        /// <summary>
        /// Button Functions by Pointer
        /// </summary>

        /**
         * Unlock and lock the addresses of each pixels in order to be used for manipulation
         */
        private Bitmap manipulatePictureByPointer(Bitmap bitMap, Action<BitmapData> calculationFunction)
        {
            BitmapData bitMapData = bitMap.LockBits(new Rectangle(0, 0, bitMap.Width, bitMap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {
                calculationFunction(bitMapData);
            }

            bitMap.UnlockBits(bitMapData);

            return bitMap;
        }

        /**
         * Unlock and lock the addresses of each pixels in order to be used for manipulation with an extra param for text box variable
         */
        private Bitmap manipulatePictureByPointerWithValue(Bitmap bitMap, Action<BitmapData, int, int, int> calculationFunction, int textBoxValue)
        {
            BitmapData bitMapData = bitMap.LockBits(new Rectangle(0, 0, bitMap.Width, bitMap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int numOffset = bitMapData.Stride - bitMapData.Width * 3;
            int numWidth = bitMapData.Width * 3;

            unsafe
            {
                calculationFunction(bitMapData, textBoxValue, numWidth, numOffset);
            }

            bitMap.UnlockBits(bitMapData);
            Bitmap bitMapCopy = (Bitmap)bitMap.Clone();

            return bitMapCopy;
        }

        /**
         * Calculations to brighten the pixel of the image
         */
        unsafe void changeBrightnessByPointer(BitmapData bitMapData, int brightnessValue, int numWidth, int numOffset)
        {
            int x;
            byte* p = (byte*)(void*)bitMapData.Scan0.ToPointer();
            for (int i = 0; i < bitMapData.Height; i++)
            {
                for (int j = 0; j < numWidth; j++)
                {
                    x = (int)(p[0] + brightnessValue);
                    if (x < 0) x = 0;
                    if (x > 255) x = 255;

                    p[0] = (byte)x;
                    ++p;
                }
                p += numOffset;
            }
        }

        /**
         * Function to brighten the color of the picture by pointer
         */
        private void changeBrightnessByPointerButton_Click(object sender, EventArgs e)
        {
            // Get the brightness value from brightness text box
            int brightnessValue = Convert.ToInt16(brightnessByPointerTextBox.Text);

            if (brightnessValue < 0) brightnessValue = 0;
            if (brightnessValue > 255) brightnessValue = 255;

            Bitmap bitMapOriginalCopy = new Bitmap(this.bitMapOriginal);
            Bitmap bitMapResult = manipulatePictureByPointerWithValue(bitMapOriginalCopy, changeBrightnessByPointer, brightnessValue);

            // Save the resulting bit map to the global bit map result variable
            saveAndDisplayBitMapResult(bitMapResult);
        }

        /**
         * Calculations to invert picture's color by pointer
         */
        unsafe void invertColorByPointer(BitmapData bitMapData)
        {
            byte* p = (byte*)(void*)bitMapData.Scan0.ToPointer();
            int stopAddress = (int)p + bitMapData.Stride * bitMapData.Height;
            while ((int)p != stopAddress)
            {
                p[0] = (byte)(255 - p[0]);
                p[1] = (byte)(255 - p[1]);
                p[2] = (byte)(255 - p[2]);
                p += 3;
            }
        }

        /**
         * Function to invert the color of the picture by pointer
         */
        private void invertPictureByPointerButton_Click(object sender, EventArgs e)
        {
            Bitmap bitMapOriginalCopy = new Bitmap(this.bitMapOriginal);
            Bitmap invertedBitMap = manipulatePictureByPointer(bitMapOriginalCopy, invertColorByPointer);

            // Save the resulting bit map to the global bit map result variable
            saveAndDisplayBitMapResult(invertedBitMap);
        }

        /**
         * Calculations to convert picture to greyscale using BT.601 method by pointer
         */
        unsafe void convertBt601GreyscaleByPointer(BitmapData bitMapData)
        {
            byte* p = (byte*)(void*)bitMapData.Scan0.ToPointer();
            int stopAddress = (int)p + bitMapData.Stride * bitMapData.Height;
            while ((int)p != stopAddress)
            {
                p[0] = (byte)(.299 * p[2] + .587 * p[1] + .114 * p[0]);
                p[1] = p[0];
                p[2] = p[0];
                p += 3;
            }
        }

        /**
         * Function to greyscale an image using BT.601 method by pointer
         */
        private void greyscalePointerButton_Click(object sender, EventArgs e)
        {
            Bitmap bitMapOriginalCopy = new Bitmap(this.bitMapOriginal);
            Bitmap greyscaledBitMap = manipulatePictureByPointer(bitMapOriginalCopy, convertBt601GreyscaleByPointer);

            // Save the resulting bit map to the global bit map result variable
            saveAndDisplayBitMapResult(greyscaledBitMap);
        }
    }
}

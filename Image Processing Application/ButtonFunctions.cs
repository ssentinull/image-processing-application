﻿using System;
using System.Drawing;

namespace Image_Processing_Application
{
    internal class ButtonFunctions
    {
        internal Bitmap changeBrightness(int row, int column, int brightnessValue, Bitmap bitMapSource)
        {
            // RGB values for the calculated pixel value
            int redValue, greenValue, blueValue;

            // Create a new empty bitmap with the same dimensions as the source bitmap
            Bitmap bitMapResult = new Bitmap(bitMapSource.Width, bitMapSource.Height);

            // For how tall the original picture is
            for (int i = 0; i < row; i++)
            {
                // For how wide the original picture is
                for (int j = 0; j < column; j++)
                {
                    Color coordinatePixelValue = bitMapSource.GetPixel(i, j);

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
                    bitMapResult.SetPixel(i, j, Color.FromArgb(redValue, greenValue, blueValue));
                }
            }

            // Returns the resulting bitmap
            return bitMapResult;
        }
    }
}
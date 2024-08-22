using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Drawing;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace _Utilities
{
    public static class ImgCalculations
    {
        
        public static (double,bool) GetDifferenceForAdjust(string ImagePath , double OriginalSize)
        {
            Bitmap bitmap = new Bitmap(ImagePath);
            double imageWidth = bitmap.Width;
            double imageHeigth = bitmap.Height;

            double DifferenceInPixels;
            bool isLandscape;

            if (imageHeigth < imageWidth)
            {
                isLandscape = true;
                DifferenceInPixels = Math.Abs((imageWidth * (OriginalSize / imageHeigth)) - OriginalSize);
            }
            else
            {
                isLandscape = false;
                DifferenceInPixels = Math.Abs((imageHeigth * (OriginalSize / imageWidth)) - OriginalSize);
            }
            bitmap.Dispose();
            return (DifferenceInPixels,isLandscape);
        }

        public static bool RequireCropping(string ImagePath)
        {
            Bitmap bitmap = new Bitmap(ImagePath);
            double imageWidth = bitmap.Width;
            double imageHeigth = bitmap.Height;

            double nonDominantSide = imageHeigth > imageWidth ? imageWidth : imageHeigth;
            double DifferenceInPercentage =Math.Abs(imageWidth - imageHeigth) / nonDominantSide * 100;

            bool requireCropping = DifferenceInPercentage > 6.5 ? true : false;

            bitmap.Dispose();
            return requireCropping;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMU_LibPerspective_Sprite_Generator
{
    internal class Program
    {
        static void Main(string[] args) // A Utility For Converting .PNG Files To LibPerspective Sprites. Thanks To Kresna And Walter For LibPerspective And WaterBear, Respectively! Thanks Also NeoGeoFreak2004 And ProgrammingDude2004 For The Threads And Idea, + To Everyone & All Of The VMU Community!
        {
            string inputFilePath = args[0]; // Input .PNG File Passed In As A Paramter In The Command Line.
            int threshold = 128; // Grayscale Threshold (0-255). Thanks, Microsoft Bing & CoPilot For The Help With Image Bit Array Generation And File Writing!

            try
            {
                // Load the image
                Bitmap bitmap = new Bitmap(inputFilePath);

                // Create a 2D array to store the binary representation
                int[,] binaryArray = new int[bitmap.Height, bitmap.Width];

                for (int y = 0; y < bitmap.Height; y++)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        // Get the pixel color
                        Color pixelColor = bitmap.GetPixel(x, y);

                        // Convert to grayscale using the average method
                        int grayscale = (pixelColor.R + pixelColor.G + pixelColor.B) / 3;

                        // Apply the threshold to determine 1 or 0
                        binaryArray[y, x] = grayscale >= threshold ? 0 : 1;
                    }
                }

                // Print the binary array (optional)
                /*for (int y = 0; y < binaryArray.GetLength(0); y++)
                {
                    for (int x = 0; x < binaryArray.GetLength(1); x++)
                    {
                        Console.Write(binaryArray[y, x]);
                    }
                    Console.WriteLine();
                }*/

                Console.WriteLine("Conversion complete!");

                string finalOutput = args[0].TrimEnd(".png".ToCharArray()) + ":\n.byte " + bitmap.Width + ", " + bitmap.Height + "\n"; // Output File Will Have The Same Name As The Input, With The .ASM Extension.
                for (int y = 0; y < bitmap.Height; y++)
                {
                    finalOutput+= "  .byte %";
                    int currentModulo = 0;
                    bool firstColumn = true;
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        if (x %8  == 0 && !firstColumn)
                        {
                            finalOutput += ",%";
                            currentModulo = 0;
                        }
                        finalOutput += binaryArray[y, x];
                        currentModulo++;
                        firstColumn = false;
                    }
                    for (int i = currentModulo; i < 8; i++)
                    {
                        finalOutput += "0";
                    }
                    finalOutput += "\n";
                }
                // Console.WriteLine(finalOutput);  //  Thanks GitHub For Generated Repo Name: Fantastic-Funicular!
                string destinationFilePath = args[0].TrimEnd(".png".ToCharArray()) + ".asm";
                File.WriteAllText(destinationFilePath, finalOutput);
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}

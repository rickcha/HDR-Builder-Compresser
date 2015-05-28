using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Main : Form
    {
        /*
         * Variables to store header information
         */

        // File header from byte number 0 to 11
        private string ByteOrder;
        private int FileType;
        private int OffsetIFD1;
        private int NumOfDirectories;

        // Tag information from byte number 0 to 11
        private int SubfileType;
        private int ImageWidth;
        private int ImageLength;
        private int BitsPerSample;
        private int Compression;
        private int PhotometricInterpretation;
        private int StripOffsets;
        private int SamplesPerPixel;
        private int XResolution;
        private int YResolution;
        private int PlanarConfiguration;

        // Necessarily, the following variables are also set global
        private string fileDirectory;    // File location
        private bool file1 = false, file2 = false, file3 = false;
        private int[] buffImage1, buffImage2, buffImage3;
        private byte[] allImageBytes, image1byte, image2byte, image3byte;
        private int pixLength;
        
        private int[] arrGray;                  // Sequential values of the grayscale image
        private int countArrGray;               // Counts the number of elements in the array
        private int fileLength;                 // The length of a file in byte
        private int red = 0;                    // Indicates the red scale from 0 to 255
        private int green = 0;                  // Sets Green Colour Index
        private int blue = 0;                   // Sets Blue Colour Index
        private int countRow = 1;               // Y value of the single pixel
        private int countColumn = 1;            // X value of the single pixel
        private int[] sorted_histogram;

        public int last_i;
        public int test_value;
        public int maxCodewordLength;

        // Gloval variables required for compression
        private StringBuilder compFile;
        private Dictionary<double, string> codeword = new Dictionary<double, string>();
        private Node root;

        public Main()
        {
            InitializeComponent();
            // btnGrayscale.Hide();
            // btnHistogram.Hide();
            // btnDithering.Hide();

            // Adjust form1 window size
            this.Width = 1000;           
            this.Height = 700;
            labelInstruction.Text = "Follow The Instructions:";
        }

        public int getMaxCodewordLength() {
            return maxCodewordLength;
        }

        private class Node
        {
            // Properties
            public double symbol { get; set; }
            public int countSymbol { get; set; }
            public Node left { get; set; }
            public Node right { get; set; }
            public List<bool> code { get; set; }
            // Constructor with no parameter
            public Node()
            {
                this.symbol = new double();
                this.countSymbol = -1;
                left = null;
                right = null;
            }
            // Constructor with four parameters
            public Node(double symbol, int count, Node node1, Node node2)
            {
                this.symbol = symbol;
                this.countSymbol = count;
                left = node1;
                right = node2;
            }
        }

        // Generate codeword for every different sample value
        private void GenerateCode(Node parentNode, string code)
        {
            if (parentNode != null)
            {
                GenerateCode(parentNode.left, code + "0");

                if (parentNode.left == null && parentNode.right == null)
                    codeword.Add(parentNode.symbol, code);

                GenerateCode(parentNode.right, code + "1");
            }
        }

        // Returns a encoded list of boolean values
        public void Encode(byte[] buff)
        {
            // Use StringBuilder for concatenation in tight loops.
            compFile = new System.Text.StringBuilder();

            // Concatentate all the strings
            for (int i = 0; i < buff.Length; i++)
            {
                if (codeword.ContainsKey(buff[i]) == true)
                {
                    compFile.Append(codeword[buff[i]]);
                }
            }

            int numList = codeword.Count;
            maxCodewordLength = 0;
            for (int i = 0; i < numList; i++)
            {
                if (maxCodewordLength < codeword.Values.ElementAt(i).Length)
                {
                    maxCodewordLength = codeword.Values.ElementAt(i).Length;
                }
            }

            // int compFileLength = compFile.Length / 8;
            // double CompressionRatio = (double)fileLength / (double)compFileLength;
            // CompressionRatio = Math.Round(CompressionRatio, 4);
            // label2.Text = "Compressed File Size: " + compFileLength + " Byte";
            // label3.Text = "Compression Ratio: " + CompressionRatio;
        }

        private void identify_tag(int number, byte[] buff)
        {
            int tagStarts = OffsetIFD1 + 2;
            int tagEnds = OffsetIFD1 + 2 + number * 12; // 142 in our sample file
            while(tagStarts < tagEnds)
            {
                // Extracts Four Field Informations From 12 Byte IFD
                int tagID;
                int Type;
                int Count;
                int Value;

                int i = tagStarts;
                byte[] bTagID = { buff[i], buff[i + 1] };
                byte[] bType = { buff[i + 2], buff[i + 3] };
                byte[] bCount = { buff[i + 4], buff[i + 5], buff[i + 6], buff[i + 7] };
                byte[] bValue = { buff[i + 8], buff[i + 9], buff[i + 10], buff[i + 11] };

                if (ByteOrder == "II")
                {
                    bTagID.Reverse();
                    bType.Reverse();
                    bCount.Reverse();
                    bValue.Reverse();
                }

                tagID = BitConverter.ToInt16(bTagID, 0);
                Type = BitConverter.ToInt16(bType, 0);
                Count = BitConverter.ToInt32(bCount, 0);
                Value = BitConverter.ToInt32(bValue, 0);

                if (tagID == 256)
                    ImageWidth = Value;
                if (tagID == 257)
                    ImageLength = Value;
                if (tagID == 273)
                    StripOffsets = Value;

                tagStarts = tagStarts + 12;
            }
        }

        private void drawRGB(int red, int green, int blue, int xPixOffset, int yPixOffset) {
            // Declares colour variable
            Color myRgbColor = new Color();

            // Create color and brush
            myRgbColor = Color.FromArgb(red, green, blue);
            System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(myRgbColor);
            System.Drawing.Graphics formGraphics;
            formGraphics = this.CreateGraphics();

            // Choose the location where you would like to draw a rectangle
            // And pick the size of a rectangle (one pixel)
            formGraphics.FillRectangle(myBrush, new Rectangle((countColumn + xPixOffset), (countRow + yPixOffset), 1, 1));
            countColumn++;
            myBrush.Dispose();
            formGraphics.Dispose();

            // If one row is filled with ImageWidth number of rectangles with one pixel, go to next row
            if (countColumn % ImageWidth == 1)
            {
                countRow++;
                countColumn = 1;
            }
        }

        private void buildHDR_RGB(int xPixOffset, int yPixOffset)
        {
            int rgbLength = pixLength * 3;
            for (int i = 0; i < rgbLength; i = i + 3)
            {
                red = (buffImage1[i] + buffImage2[i] + buffImage3[i]) / 3;
                green = (buffImage1[i + 1] + buffImage2[i + 1] + buffImage3[i + 1]) / 3;
                blue = (buffImage1[i + 2] + buffImage2[i + 2] + buffImage3[i + 2]) / 3;
                drawRGB(red, green, blue, xPixOffset, yPixOffset);
            }
        }

        private void drawBuff(int nImage, byte[] buff, int xPixOffset, int yPixOffset)
        {
            /*
             *  From here, extract header information
             */
            if (nImage == 4)
                buildHDR_RGB(xPixOffset, yPixOffset);
            else
            {
                ASCIIEncoding ascii = new ASCIIEncoding();

                // Identify whether the order of bytes are little-endian or big-endian
                // If "II", then it is little-endian
                byte[] bByteOrder = { buff[0], buff[1] };
                ByteOrder = ascii.GetString(bByteOrder);

                // Identify whether the order of bytes are little-endian or big-endian
                // If 42, then it is TIFF file format
                byte[] bFileType = { buff[2], buff[3] };
                if (ByteOrder == "II")
                    bFileType.Reverse();
                FileType = BitConverter.ToInt16(bFileType, 0);

                // Identify the first IFD offset and store the value into OffsetIFD1
                byte[] bOffsetIFD1 = { buff[4], buff[5], buff[6], buff[7] };
                if (ByteOrder == "II")
                    bOffsetIFD1.Reverse();
                OffsetIFD1 = BitConverter.ToInt32(bOffsetIFD1, 0);

                // Identify the first IFD offset and store the value into OffsetIFD1
                byte[] bNumOfDirectories = { buff[8], buff[9] };
                if (ByteOrder == "II")
                    bNumOfDirectories.Reverse();
                NumOfDirectories = BitConverter.ToInt16(bNumOfDirectories, 0);

                identify_tag(NumOfDirectories, buff); // Call the "identify_tag" methods to dig IFD information
                pixLength = ImageWidth * ImageLength;
                int pixEnd = StripOffsets + pixLength * 3;

                switch (nImage)
                {
                    case 1:
                        buffImage1 = new int[pixLength * 3];
                        file1 = true;
                        break;
                    case 2:
                        buffImage2 = new int[pixLength * 3];
                        file2 = true;
                        break;
                    case 3:
                        buffImage3 = new int[pixLength * 3];
                        file3 = true;
                        break;
                }

                // Reads file from buff
                int j = 0;
                for (int i = StripOffsets; i < pixEnd; i = i + 3)
                {
                    red = buff[i];
                    green = buff[i + 1];
                    blue = buff[i + 2];

                    switch (nImage)
                    {
                        case 1:
                            buffImage1[j] = red;
                            buffImage1[j + 1] = green;
                            buffImage1[j + 2] = blue;
                            break;
                        case 2:
                            buffImage2[j] = red;
                            buffImage2[j + 1] = green;
                            buffImage2[j + 2] = blue;
                            break;
                        case 3:
                            buffImage3[j] = red;
                            buffImage3[j + 1] = green;
                            buffImage3[j + 2] = blue;
                            break;
                    }
                    j = j + 3;
                    drawRGB(red, green, blue, xPixOffset, yPixOffset);
                }
            }
        }

        private void btnOpenFile1_Click(object sender, EventArgs e)
        {
            // Initially, no value is stored.
            red = 0;
            green = 0;
            blue = 0;
            countRow = 1;
            countColumn = 1;

            //// Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "TIFF Image Files (.tif)|*.tif|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                // Saves file location and reads it into a byte array
                fileDirectory = openFileDialog1.FileName;
                image1byte = File.ReadAllBytes(fileDirectory);
                fileLength = image1byte.Length;

                drawBuff(1, image1byte, 150, 20);
            }
        }

        private void btnOpenFile2_Click(object sender, EventArgs e)
        {
            // Initially, no value is stored.
            red = 0;
            green = 0;
            blue = 0;
            countRow = 1;
            countColumn = 1;

            //// Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "TIFF Image Files (.tif)|*.tif|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                // Saves file location and reads it into a byte array
                fileDirectory = openFileDialog1.FileName;
                image2byte = File.ReadAllBytes(fileDirectory);
                fileLength = image2byte.Length;

                drawBuff(2, image2byte, 550, 20);
            }
        }

        private void btnOpenFile3_Click(object sender, EventArgs e)
        {
            // Initially, no value is stored.
            red = 0;
            green = 0;
            blue = 0;
            countRow = 1;
            countColumn = 1;

            //// Create an instance of the open file dialog box.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            // Set filter options and filter index.
            openFileDialog1.Filter = "TIFF Image Files (.tif)|*.tif|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;

            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK) // Test result.
            {
                // Saves file location and reads it into a byte array
                fileDirectory = openFileDialog1.FileName;
                image3byte = File.ReadAllBytes(fileDirectory);
                fileLength = image3byte.Length;

                drawBuff(3, image3byte, 150, 340);
            }
        }

        private void btnHDR_Click(object sender, EventArgs e)
        {
            if (file1 == true && file2 == true && file3 == true)
            {
                labelInstruction.Text = "Jobs Done...";
                drawBuff(4, null, 550, 50);
                // labelInstruction.Text = "" + buffImage1[1] + " " + buffImage1[80] + " " + buffImage2[40] + " " + buffImage3[60];
            }
            else
            {
                labelInstruction.Text = "Upload All Three Images...";
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public static byte[] ConvertIntToByteArray(int I)
        {
            return BitConverter.GetBytes(I);
        }

        public static double ConvertByteArrayToInt32(byte[] b)
        {
            return BitConverter.ToInt32(b, 0);
        }

        public static int ConvertBinaryStringToInt32(string BS)
        {
            return Convert.ToInt32(BS, 2);
        }

        public static string ConvertByteToBinaryString(byte b)
        {
            return Convert.ToString(b, 2);
        }

        private void btnCompress_Click(object sender, EventArgs e)
        {
            // Number of bytes in each image file
            int lenImage1 = image1byte.Length;
            int lenImage2 = image2byte.Length;
            int lenImage3 = image3byte.Length;

            // Combine all three files into the allBytes array
            int lenAllBytes = lenImage1 + lenImage2 + lenImage3;
            allImageBytes = new byte[lenAllBytes];
            image1byte.CopyTo(allImageBytes, 0);
            image2byte.CopyTo(allImageBytes, image1byte.Length);
            image3byte.CopyTo(allImageBytes, image1byte.Length + image2byte.Length);
           
            // A list of nodes
            // root of the list
            root = new Node();
            List<Node> nodes = new List<Node>();
            Dictionary<double, int> frequencies = new Dictionary<double, int>();

            // Create a list of dictionaries and count of appearing for frequency
            for (int i = 0; i < allImageBytes.Length; i++)
            {
                // if dictionary does not contain key
                if (!frequencies.ContainsKey(allImageBytes[i]))
                {
                    // Add key
                    frequencies.Add(allImageBytes[i], 0);
                }
                // Add one frequency to current key
                frequencies[allImageBytes[i]]++;
            }

            // For each element in frequencies
            // create a new node of an element and add to the list
            foreach (KeyValuePair<double, int> element in frequencies)
                nodes.Add(new Node(element.Key, element.Value, null, null));

            // Find maixmum frequency
            // List<Node> orderedNodes3 = nodes.OrderBy(node => node.countSymbol).ToList<Node>();

            // While the nodes list contains more than one element
            while (nodes.Count > 1)
            {
                // Order nodes in the list by countSymbol
                List<Node> orderedNodes = nodes.OrderBy(node => node.countSymbol).ToList<Node>();
                if (nodes.Count >= 2)
                { // if greater than two
                    // Take the first two nodes from the list
                    List<Node> twoChildNodes = orderedNodes.Take(2).ToList<Node>();
                    Node Parent = new Node(new double(),
                        twoChildNodes[0].countSymbol + twoChildNodes[1].countSymbol,
                        twoChildNodes[0],
                        twoChildNodes[1]);

                    // Delete the least two elements and add the parent
                    nodes.Remove(twoChildNodes[0]);
                    nodes.Remove(twoChildNodes[1]);
                    nodes.Add(Parent);

                }
                // The first node in the list
                root = nodes.FirstOrDefault();
            }

            // Pass root and generate codeword
            GenerateCode(root, "");
            
            //////////////////////
            //
            //
            //      Create header
            //
            //
            //////////////////////

            // The number of images that are compressed 
            int numImages = 3;
            byte numImagesInByte = (byte)numImages;

            // The number of codewords in Huffman coding table
            int numCodeword = codeword.Count;
            byte[] numCodewordInByte = ConvertIntToByteArray(numCodeword);

            // Convert integer number of bytes in each image to the byte arrays
            byte[] lenImage1InByte = ConvertIntToByteArray(lenImage1);
            byte[] lenImage2InByte = ConvertIntToByteArray(lenImage2);
            byte[] lenImage3InByte = ConvertIntToByteArray(lenImage3);

            // Image 1,2,3 are converted into a series of binary strings
            // Encode/compress using huffman coding
            Encode(allImageBytes);

            // Huffman coding needs to go inside the header in bytes
            byte[] huffmanTable = new byte[9 * codeword.Count];
            for (int i = 0; i < codeword.Count; i++)
            {
                byte[] leafNode = new byte[9];
                leafNode[0] = (byte)codeword.Keys.ElementAt(i);
                int lenBinaryHuffman = codeword.Values.ElementAt(i).Length;
                byte[] lenBinaryHuffmanInByte = ConvertIntToByteArray(lenBinaryHuffman);
                string huffmanCoding = codeword.Values.ElementAt(i).PadRight(maxCodewordLength, '0');
                //label2.Text = "" + maxCodewordLength;
                int lookupValue = ConvertBinaryStringToInt32(huffmanCoding);
                byte[] lookupValueInByte = ConvertIntToByteArray(lookupValue);
                lookupValueInByte.CopyTo(leafNode, 1);
                lenBinaryHuffmanInByte.CopyTo(leafNode, 5);
                leafNode.CopyTo(huffmanTable, i * 9);
            }

            //-------------For opening save File dialog---------------//
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "THR files (*.thr)|*.thr";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            //--------------------------------------------------------//

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // If failure, close the stream
                    myStream.Close();
                }

                string saveFileDirectory = saveFileDialog1.FileName;       // Save file location\
                string compFileInString = compFile.ToString();

                // Original combined binary length of all the three images
                // This one also goes inside the header information
                int lenBinaryBeforePadding = compFileInString.Length;
                byte[] lenBinaryBeforePaddingInByte = ConvertIntToByteArray(lenBinaryBeforePadding);

                // Because we need a binary string with multiple of eight
                int numOfBytesAllBinary = compFileInString.Length / 8;
                if (compFileInString.Length % 8 != 0) 
                {
                    compFileInString = compFileInString.PadRight((numOfBytesAllBinary + 1) * 8, '0');
                    numOfBytesAllBinary = numOfBytesAllBinary + 1;
                }
                
                // Method 3
                // It is time to combine all the information and genarate one file
                byte[] allBytes = new byte[5 + huffmanTable.Length
                    + lenImage1InByte.Length + lenImage2InByte.Length + lenImage3InByte.Length
                    + lenBinaryBeforePaddingInByte.Length
                    + numOfBytesAllBinary];

                ///////////////////////////////////
                //                               //
                //                               //
                //        Built the header       //
                //                               //
                //                               //
                ///////////////////////////////////

                int allByteIndex = 0;
                // Copy how many images
                allBytes[0] = numImagesInByte;
                // Copy Huffman Table Length
                numCodewordInByte.CopyTo(allBytes, 1);
                // Copy Huffman Table
                huffmanTable.CopyTo(allBytes, 1+numCodewordInByte.Length);
                allByteIndex = 1 + numCodewordInByte.Length + huffmanTable.Length;
                // Copy three image lengths
                lenImage1InByte.CopyTo(allBytes, allByteIndex);
                lenImage2InByte.CopyTo(allBytes, allByteIndex + lenImage1InByte.Length);
                lenImage3InByte.CopyTo(allBytes, allByteIndex + lenImage1InByte.Length + lenImage2InByte.Length);
                allByteIndex = allByteIndex + lenImage1InByte.Length + lenImage2InByte.Length + lenImage3InByte.Length;
                // Copy Original binary string length of three images before padding
                lenBinaryBeforePaddingInByte.CopyTo(allBytes, allByteIndex);
                allByteIndex = allByteIndex + lenBinaryBeforePaddingInByte.Length;

                ///////////////////////////////////
                //                               //
                //                               //
                //        Enter Images Now       //
                //                               //
                //                               //
                ///////////////////////////////////

                int count = 0;

                // label2.Text = compFileInString.Substring(0, 8);
                for (int i = allByteIndex; i < allBytes.Length; i++)
                {
                    allBytes[i] = Convert.ToByte(compFileInString.Substring(8 * count, 8), 2);
                    count++;
                }
                File.WriteAllBytes(saveFileDirectory, allBytes);

                label1.Text = "Location of The Compressed: " + saveFileDirectory;
            }
        }

        private void btnHuffmanCoding_Click(object sender, EventArgs e)
        {
            Detail properties = new Detail(codeword);
            properties.ShowDialog();
        }
    }
}
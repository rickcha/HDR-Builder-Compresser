using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Detail : Form
    {
        private int maxCodewordLength;
        public Detail(Dictionary<double, string> codeword)
        {
            InitializeComponent();

            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

            //Add column header
            listView1.Columns.Add("Symbol", 100);
            listView1.Columns.Add("Huffman Coding", 120);

            //Add items in the listview
            string[] arr = new string[4];
            ListViewItem item;

            foreach (KeyValuePair<double, string> pair in codeword)
            {
                //Add first item
                arr[0] = "" + pair.Key;
                arr[1] = pair.Value;
                item = new ListViewItem(arr);
                listView1.Items.Add(item);
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

            label1.Text = "Maximum Length of The Codeword: " + maxCodewordLength;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

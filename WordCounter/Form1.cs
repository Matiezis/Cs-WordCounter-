using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordCounter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            listViewUniqueWords.Items.Clear();
            List<string> uniqueWordsList = new List<string>();
            List<int> uniqueWordsCountList = new List<int>();
            string wholeText = richTextBox1.Text;
            wholeText = wholeText.ToLower();
            string[] separators = { "\n","\r\n","/","\\","|","-","+","(",")","<",">","[","]","$","#","!"," ", ",", ".", "?", "!", "    ", "...", "'", ";", ":","1","2","3","4","5","6","7","8","9","0","*","-","_","^","@","#" };
            string[] words = wholeText.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            foreach(string word in words)
            {
                if (uniqueWordsList.Contains(word))
                {
                    int index = uniqueWordsList.IndexOf(word);
                    uniqueWordsCountList[index]++;
                }
                else
                {
                    uniqueWordsList.Add(word);
                    uniqueWordsCountList.Add(1);
                }
            }
            if (uniqueWordsList.Count > 0)
            {
                int uniqueWordCount = 0;
                int wordCount = 0;
                foreach (string word in uniqueWordsList)
                {
                    uniqueWordCount++;
                    wordCount += uniqueWordsCountList[uniqueWordsList.IndexOf(word)];
                    ListViewItem tempItem = new ListViewItem(word);
                    tempItem.SubItems.Add(uniqueWordsCountList[uniqueWordsList.IndexOf(word)].ToString());
                    listViewUniqueWords.Items.Add(tempItem);
                }
                textBoxUniqueWordCount.Text = uniqueWordCount.ToString();
                textBoxWordCount.Text = wordCount.ToString(); 
            }

        }
        private int sortColumn = -1;
        public class ListViewItemComparer : IComparer
        {
            private int col;
            private System.Windows.Forms.SortOrder order;
            public ListViewItemComparer()
            {
                col = 0;
                order = System.Windows.Forms.SortOrder.Ascending;
            }
            public ListViewItemComparer(int column, System.Windows.Forms.SortOrder order)
            {
                col = column;
                this.order = order;
            }
            public int Compare(object x, object y)
            {
                int returnVal = -1;
                returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text,
                                ((ListViewItem)y).SubItems[col].Text);
                if (col == 1)
                {
                    returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text,
                                ((ListViewItem)y).SubItems[col].Text);
                }
                if (order == System.Windows.Forms.SortOrder.Descending)
                    returnVal *= -1;
                return returnVal;
            }
        }
        private void listViewUniqueWords_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column != sortColumn)
            {
                sortColumn = e.Column;
                listViewUniqueWords.Sorting = SortOrder.Ascending;
            }
            else
            {
                if (listViewUniqueWords.Sorting == SortOrder.Ascending)
                    listViewUniqueWords.Sorting = SortOrder.Descending;
                else
                    listViewUniqueWords.Sorting = SortOrder.Ascending;
            }

            listViewUniqueWords.Sort();
            this.listViewUniqueWords.ListViewItemSorter = new ListViewItemComparer(e.Column, listViewUniqueWords.Sorting);
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text files|*.txt";
            openFileDialog1.Title = "Select a Text File";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Text = File.ReadAllText(openFileDialog1.FileName);                
            }
        }
    }
}

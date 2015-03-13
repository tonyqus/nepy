using Nepy.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Nepy.Studio
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnParse_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            NamedEntityRecognizer ner = new NamedEntityRecognizer();
            var prc=ner.Recognize(rtbText.Text.Trim(), ParserPattern.China);
            sw.Stop();
            tsTime.Text = string.Format("消耗时间：{0}毫秒", sw.ElapsedMilliseconds);
            listView1.Items.Clear();
            foreach (var pr in prc)
            {
                AddParseResultToList(pr);
            }
        }
        void AddParseResultToList(ParseResult pr)
        {
            var item = listView1.Items.Add(pr.Text);
            item.SubItems.Add(pr.StartPos.ToString());
            item.SubItems.Add(pr.Type.Description());
            if (pr.Value != null)
            {
                item.SubItems.Add(pr.Value.GetType().Name);
                item.SubItems.Add(pr.Value.ToString());
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

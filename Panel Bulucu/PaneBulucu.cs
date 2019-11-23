using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;
using System.Data.OleDb;

namespace Panel_Bulucu
{
    public partial class PaneBulucu : Form
    {
        public PaneBulucu()
        {
            InitializeComponent();
        }
        int i;
        Thread islem;
        string metin;
        string kayıtlar;
        void Tarama()
        {
            int adet=listBox1.Items.Count;
            for (i = 0; i < adet; i++)
            {
                try
                {
                    listBox1.SelectedIndex = i;
                    HttpWebRequest istek = (HttpWebRequest)HttpWebRequest.Create(textBox1.Text + listBox1.Items[i].ToString());
                    HttpWebResponse cevap = (HttpWebResponse)istek.GetResponse();
                    string durum = cevap.StatusCode.ToString();
                    if (File.Exists(Application.StartupPath + "/log.txt") == false)
                        File.CreateText(Application.StartupPath + "/log.txt");
                    if (durum == "OK")
                    {
                        try
                        {
                            listBox2.Items.Add(listBox1.Items[i].ToString());
                            label5.Text = listBox1.Items[i].ToString();

                            label11.Text = textBox1.Text + listBox1.Items[i].ToString();
                            if (metin !="")
                            {
                                StreamReader oku = new StreamReader("log.txt");
                                oku.ReadLine();
                                kayıtlar = oku.ReadLine();

                                 metin += kayıtlar + Environment.NewLine + DateTime.Now.ToLocalTime() + "              Site : " + textBox1.Text + "    Panel : " + label11.Text + Environment.NewLine;
                                oku.Close();
                                File.WriteAllText("log.txt", metin);
                            }
                            else
                            {
                                StreamReader oku = new StreamReader("log.txt");
                                oku.ReadLine();
                                kayıtlar = oku.ReadLine();
                                metin += DateTime.Now.ToLocalTime() + "           Site : " + textBox1.Text + "    Panel : " + label11.Text + Environment.NewLine;
                                File.WriteAllText("log.txt", metin);
                                oku.Close();
                            }
                            
                        }
                        catch (Exception hata)
                        {
                            MessageBox.Show(hata.Message.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        label5.Text = listBox1.Items[i].ToString();
                        label6.ForeColor = Color.Red;
                        label6.Text = "Başarısız";
                    }
                }
                catch
                {
                    label5.Text = listBox1.Items[i].ToString();
                    label6.ForeColor = Color.Red;
                    label6.Text = "Başarısız";
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(Application.StartupPath + "/log.txt") == false)
                File.CreateText(Application.StartupPath + "/log.txt");
            if (File.Exists("log.txt") == false)
            {
                timer2.Enabled = true;
              

            }
            if (System.IO.File.Exists("panel.txt") == false)
            {
                groupBox1.Visible = false;
                groupBox2.Visible = true;

            }
            else
            {
                StreamReader oku = new StreamReader("panel.txt");
                string metin = oku.ReadLine();
                while (metin != null)
                {
                    listBox1.Items.Add(metin);
                    metin = oku.ReadLine();
                }
                CheckForIllegalCrossThreadCalls = false;
                label9.Text = "Panel : " + listBox1.Items.Count.ToString();
            }
            }
        
     
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Lütfen site url giriniz!", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                button8.Enabled = false;
                button1.Enabled = false;
                button8.Enabled = false;
                islem = new Thread(new ThreadStart(Tarama));
                islem.Start();
                textBox1.Enabled = false;
                button2.Enabled = true;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                islem.Abort();
                button1.Enabled = true;
                button8.Enabled = true;
                button2.Enabled = false;
            }
            catch 
            {
                MessageBox.Show("Veri girişi bulunamadı !", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.SelectedIndex = 0;
                listBox2.Items.Clear();
                button1.Enabled = true;
                button8.Enabled = true;
                button2.Enabled = true;
                textBox1.Enabled = true;
                label6.Text = "";
                label5.Text = "";
                textBox1.Text = "";
                label11.Text = "";
                islem.Abort();
                i = 0;
            }
            catch 
            {
                MessageBox.Show("Veri girişi bulunamadı !", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
           
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog panel = new OpenFileDialog();
            panel.Filter = "Metin dosyaları|*.txt|Tüm Dosyalar|*.*";
            panel.Title = "Panel Dosyasını Seçiniz";
            panel.InitialDirectory = "c:\\";
            panel.RestoreDirectory = true;
            if (panel.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = panel.FileName;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                File.Copy(textBox2.Text, @"panel.txt");
                Application.Restart();
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (label11.Text != "")
            {
                button7.Enabled = true;
            }
            else
            {
                button7.Enabled = false;
            }
             if (textBox2.Text != "")
            {
                button5.Enabled = true;
            }
            else
            {
                button5.Enabled = false;
            }
            if (label11.Text != "")
            {
                button4.Enabled = true;
            }
            else
            {
                button4.Enabled = false;
            }
           
          
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
             
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(label11.Text);
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message.ToString(), "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            label11.Text = textBox1.Text + listBox2.SelectedItem.ToString();
        }

        private void hakkındaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Hakkında info = new Hakkında();
            info.Show();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Clipboard.SetText(label11.Text);
        }

        private void arşivToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private void button8_Click(object sender, EventArgs e)
        {
          
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            
            File.CreateText("log.txt");
       
        }

        private void geçmişToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("log.txt");
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://forum.turksiberguvenlik.net/index.php");
        }

        private void linkLabel1_Move(object sender, EventArgs e)
        {
           

        }

        private void linkLabel1_Leave(object sender, EventArgs e)
        {
           
        }

        private void linkLabel1_MouseMove(object sender, MouseEventArgs e)
        {
            //linkLabel1.LinkColor = Color.Red;
        }

        private void linkLabel1_MouseLeave(object sender, EventArgs e)
        {
           // linkLabel1.LinkColor = Color.RoyalBlue;
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ac = new OpenFileDialog();
            ac.Title = "AÇ";
            ac.Filter = "Metin Dosyaları|*.txt|Tüm Dosyalar|*.*";
            if (ac.ShowDialog() == DialogResult.OK)
            {
                StreamReader oku = new StreamReader(ac.FileName);
                string satır = oku.ReadLine();
                while (satır != null)
                {
                    listBox1.Items.Add(satır);
                    satır = oku.ReadLine();
                }
                oku.Close();
            }
            label9.Text = "Panel : " + listBox1.Items.Count.ToString();
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem.ToString() != "")
                    silToolStripMenuItem.Enabled = true;
                else
                    silToolStripMenuItem.Enabled = false;
            }
            catch { silToolStripMenuItem.Enabled = false; }
            if (listBox1.Items.Count > 0)
                tümünüSilToolStripMenuItem.Enabled = true;
            else tümünüSilToolStripMenuItem.Enabled = false;
            
        }

        private void ekleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = -342; i <=0; i++)
            {
                panel1.Location = new Point(i, panel1.Location.Y);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                listBox1.Items.Add(textBox3.Text);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            for (int i = 0; i >= -342; i--)
            {
                panel1.Location = new Point(i, panel1.Location.Y);
            }
        }

        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Remove(listBox1.SelectedItem);
            }
            catch { }
        }

        private void tümünüSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            Random random=new Random();
            label4.ForeColor = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        }
        }
    
}

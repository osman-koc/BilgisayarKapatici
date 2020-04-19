using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;//bilgisayarikapat fonksiyonu için

namespace BilgisayarKapatici_v1
{
    public partial class Form1 : Form
    {
        bool baslaMi;//buton yazısı Başlat ise true İptal ise false verir
        Timer tmr = new Timer();
        DateTime dt = DateTime.Now;

        private void PCkapat()
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "C:\\Windows\\system32\\shutdown.exe";
            psi.Arguments = "-f -s -t 0";
            Process.Start(psi);
            this.Close();
        }

        public Form1()
        {
            InitializeComponent();

            pictureBox1.ImageLocation = "ustLogo.png";
            timer1.Start();
            label3.Visible = false;
            label4.Visible = false;
            textBox2.Visible = false;
            textBox3.Visible = false;
            ayarlanmadiLBL.Text = "Henüz ayarlanmadı.";
            baslaMi = true;

            for (int i = 0; i < 24; i++)
            {
                if (i < 10)
                    Ksaat.Items.Add("0" + i.ToString());
                else
                    Ksaat.Items.Add(i.ToString());
            }
            for (int i = 0; i < 60; i++)
            {
                if (i < 10)
                { Kdakika.Items.Add("0" + i.ToString()); Ksaniye.Items.Add("0" + i.ToString()); }
                else
                { Kdakika.Items.Add(i.ToString()); Ksaniye.Items.Add(i.ToString()); }
            }



            tmr.Interval = 1000;
            tmr.Tick += (sender, e) =>
            {
                TimeSpan diff = dt.Subtract(DateTime.Now);
                textBox3.Text = string.Format("{0:00}:{1:00}:{2:00}", diff.Hours, diff.Minutes, diff.Seconds);
                if (dt < DateTime.Now)
                    ((Timer)sender).Stop();
                if (tmr.Enabled == false)//geri sayım biterse
                    PCkapat();
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {//Başlat ve İptal Butonu
            try
            {
                int Ksa = Convert.ToInt16(Ksaat.Text);
                int Kdk = Convert.ToInt16(Kdakika.Text);
                int Ksn = Convert.ToInt16(Ksaniye.Text);
                DialogResult Kapat = new DialogResult();

                int Ksa_kalan = Ksa - DateTime.Now.Hour;
                int Kdk_kalan = Kdk - DateTime.Now.Minute;
                int Ksn_kalan = Ksn - DateTime.Now.Second;
                if (Ksa_kalan < 0 || (Ksa_kalan == 0 && Kdk_kalan < 0))
                    Ksa_kalan += 24;//şu anki saatten az bir zaman girilirse 24 saat eklenecek

                if (baslaMi)//Başlat
                {
                    if (Ksa == DateTime.Now.Hour && Kdk == DateTime.Now.Minute)
                    {
                        Kapat = MessageBox.Show("Bilgisayar şimdi kapatılacak, onaylıyor musunuz?", "KAPAT", MessageBoxButtons.YesNo);
                        if (Kapat == System.Windows.Forms.DialogResult.Yes)
                            PCkapat();
                    }
                    else
                    {
                        Ksaat.Enabled = false;
                        Kdakika.Enabled = false;
                        Ksaniye.Enabled = false;

                        label3.Visible = true;
                        label4.Visible = true;
                        textBox2.Visible = true;
                        textBox3.Visible = true;

                        ayarlanmadiLBL.Visible = false;
                        baslaMi = false;
                        button1.Text = "İptal Et";
                        button1.BackColor = System.Drawing.Color.FromName("Red");


                        textBox2.Text = Ksaat.Text + ":" + Kdakika.Text + ":" + Ksaniye.Text;
                        dt = DateTime.Now.AddHours(Ksa_kalan).AddMinutes(Kdk_kalan).AddSeconds(Ksn_kalan);
                        tmr.Start();

                        MessageBox.Show("Süre başladı. Program simge durumuna düşürülecek.\nAçıp işlemi iptal etmek için sağ alttan program simgesine çift tıklayın.", "BİLGİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Hide();
                    }
                }
                else if (baslaMi == false)//İptal Et
                {
                    tmr.Stop();
                    Ksaat.Enabled = true;
                    Kdakika.Enabled = true;
                    Ksaniye.Enabled = true;

                    label3.Visible = false;
                    label4.Visible = false;
                    textBox2.Visible = false;
                    textBox3.Visible = false;

                    ayarlanmadiLBL.Visible = true;
                    ayarlanmadiLBL.Text = "Henüz ayarlanmadı.";
                    baslaMi = true;
                    button1.Text = "Başlat";
                    button1.BackColor = System.Drawing.Color.FromName("ControlDarkDark");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Bilinmeyen bir hata oluştu! \nProgram yapımcısıyla iletişime geçin.", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            textBox1.Text = DateTime.Now.ToLongTimeString();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //textboxa veri girişini engelleme
            e.Handled = Char.IsLetterOrDigit(e.KeyChar) || Char.IsSymbol(e.KeyChar) || Char.IsPunctuation(e.KeyChar) || Char.IsWhiteSpace(e.KeyChar) || Char.IsControl(e.KeyChar) || Char.IsNumber(e.KeyChar);
        }

        private void Ksaat_KeyPress(object sender, KeyPressEventArgs e)
        {
            //textboxa sadece sayı girişi
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //formun boyutuyla oynanmaması için
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void programıAçToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void gizleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.Hide();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void hakkındaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bu program Osman KOÇ tarafından yapılmıştır. \nDestek için: info@osmkoc.com", "HAKKINDA", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult Cikis = new DialogResult();
            if (!baslaMi)
            {
                Cikis = MessageBox.Show("Bilgisayarı kapatma işlemi iptal edilecek, onaylıyor musunuz?", "KAPAT", MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (Cikis == System.Windows.Forms.DialogResult.Yes)
                {   tmr.Stop(); this.Close();     }
            }
            else
                this.Close();
        }
    }
}
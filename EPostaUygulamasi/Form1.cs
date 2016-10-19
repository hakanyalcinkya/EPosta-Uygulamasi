using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EPostaUygulamasi
{
    public partial class Form1 : Form
    {
        string eposta = string.Empty;
        string sifre = string.Empty;
        string sunucu = string.Empty;
        int port = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 frm = new Form2();
            frm.ShowDialog();
            eposta = frm.txtEpostaHesap.Text.Trim();
            sifre = frm.txtSifre.Text.Trim();
            sunucu = frm.txtSunucu.Text.Trim();
            port = (int)frm.txtPort.Value;
            if (eposta != "" && sifre != "" && sunucu != "")
            {
                btnGonder.Enabled = true;
                txtKonu.Enabled = true;
                txtMesaj.Enabled = true;
                cmbEPosta.Enabled = true;

                lblDurum.Text = "Hazır (" + eposta + " adresi ile hesap açıldı.)";

                btnHesapAc.Text = eposta;
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir hesap açınız.", "Uyarı",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            btnGonder.Enabled = false;
            txtKonu.Enabled = false;
            txtMesaj.Enabled = false;
            cmbEPosta.Enabled = false;
            lblDurum.Text = "Lütfen bir hesap açınız.";
        }

        private void btnGonder_Click(object sender, EventArgs e)
        {
            NetworkCredential kimlik = new NetworkCredential();
            kimlik.UserName = eposta;
            kimlik.Password = sifre;

            SmtpClient client = new SmtpClient();
            client.Host = sunucu;
            client.Port = port;

            client.Credentials = kimlik;
            client.EnableSsl = true;

            MailMessage posta = new MailMessage();
            posta.IsBodyHtml = true;
            posta.Subject = txtKonu.Text;
            posta.Body = txtMesaj.Text;
            try
            {
                posta.From = new MailAddress(eposta);
                posta.To.Add(new MailAddress(cmbEPosta.Text));

                // Sunucuya mesajı gönder komutu verilir.
                // http://www.yazilimindunyasi.com/c-windows-form-application/63-gmail-uzerinden-mail-gonderme-programi.html
                client.Send(posta);

                lblDurum.Text = "Mail gönderildi";
                lblDurum.ForeColor = Color.Green;

                MailAdresiKaydet(cmbEPosta.Text);

                timer1.Enabled = true;
                //timer1.Start();
            }
            catch (Exception ex)
            {
                lblDurum.Text = "Hata oluştu.";
                lblDurum.ForeColor = Color.Red;

                MessageBox.Show(ex.Message, "Hata Mesajı");
                MessageBox.Show(ex.Source, "Hata Kaynağı");
                MessageBox.Show(ex.StackTrace, "Hata Detay");
            }
        }
    }
}

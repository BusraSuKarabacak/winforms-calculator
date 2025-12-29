using System;
using System.Data;
using System.Windows.Forms;

namespace hesapmakinesi
{
    public partial class Form1 : Form
    {
        // Ekranda görünen matematiksel ifade
        string ifade = "";

        public Form1()
        {
            InitializeComponent();
            TumButonlaraClickAta();

            ekranLabel.Text = "0";
            ekranLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        }

        // Formdaki TÜM butonlara otomatik Click bağlar
        private void TumButonlaraClickAta()
        {
            foreach (Control c in this.Controls)
            {
                if (c is Button btn)
                {
                    btn.Click += OrtakButton_Click;
                }
            }
        }

        private void OrtakButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string text = btn.Text;

            // === TEMİZLE ===
            if (text == "C")
            {
                ifade = "";
                ekranLabel.Text = "0";
                return;
            }

            // === EŞİTTİR ===
            if (text == "=")
            {
                try
                {
                    double sonuc = Hesapla(ifade);
                    ekranLabel.Text = sonuc.ToString();
                    ifade = sonuc.ToString();
                }
                catch
                {
                    ekranLabel.Text = "Hata";
                    ifade = "";
                }
                return;
            }

            // === VİRGÜL ===
            if (text == ",")
            {
                if (ifade.Length == 0 || ifade.EndsWith(","))
                    return;

                // Aynı sayıda ikinci virgül olmasın
                int sonIslem = Math.Max(
                    ifade.LastIndexOf('+'),
                    Math.Max(
                        ifade.LastIndexOf('-'),
                        Math.Max(
                            ifade.LastIndexOf('X'),
                            ifade.LastIndexOf('/')
                        )
                    )
                );

                string sonSayi = ifade.Substring(sonIslem + 1);
                if (sonSayi.Contains(","))
                    return;

                ifade += ",";
                ekranLabel.Text = ifade;
                return;
            }

            // === İŞLEMLER ===
            if (text == "+" || text == "-" || text == "X" || text == "/")
            {
                if (ifade.Length == 0)
                    return;

                char son = ifade[ifade.Length - 1];
                if ("+-X/".Contains(son.ToString()))
                    return;

                ifade += text;
                ekranLabel.Text = ifade;
                return;
            }

            // === SAYILAR ===
            if (char.IsDigit(text[0]))
            {
                ifade += text;
                ekranLabel.Text = ifade;
                return;
            }
        }

        // === ÇOKLU İŞLEM + VİRGÜL DESTEKLİ HESAPLAMA ===
        double Hesapla(string ifade)
        {
            ifade = ifade.Replace("X", "*");
            ifade = ifade.Replace(",", ".");

            DataTable dt = new DataTable();
            var sonuc = dt.Compute(ifade, "");
            return Convert.ToDouble(sonuc);
        }

        // === KLAVYEDEN BACKSPACE (SİLME) ===
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Back)
            {
                if (ifade.Length > 0)
                {
                    ifade = ifade.Substring(0, ifade.Length - 1);
                    ekranLabel.Text = ifade == "" ? "0" : ifade;
                }
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        // Designer hata vermesin diye bilinçli boş
        private void ekranLabel_Click(object sender, EventArgs e) { }
    }
}


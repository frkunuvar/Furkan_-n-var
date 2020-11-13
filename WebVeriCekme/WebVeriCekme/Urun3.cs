using HtmlAgilityPack;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Timers;

namespace ConsoleAppVeriCekme
{
    class Urun3
    {
        internal static void Urun_3()
        {

            string farkdeger = "0";
            string UrunAdi = "0", EskiUrunGoruntulenmeSayisi = "", EskiUrunFiyati = "0";
            string YeniGoruntulenmeSayisi = "";
            string YeniUrunFiyati = "0";
          //  YeniGoruntulenmeSayisi = GoruntulenmeSayisiniOku().Replace("\n", "");
            YeniUrunFiyati = FiyatiniOku().Replace("\n", "");
            try
            {
                string connectionString = "Data Source=DESKTOP-SVRCSIC;Initial Catalog=WebVeri;Integrated Security=True";
                SqlConnection connection = new SqlConnection(connectionString);

                string sqlParametreFark = "Select Deger from Parametreler where DegiskenAdi='Fark'";

                SqlCommand command = new SqlCommand(sqlParametreFark, connection);
                SqlDataAdapter da = new SqlDataAdapter(sqlParametreFark, connection);
                DataSet dsParametreFark = new DataSet();
                connection.Open();
                da.Fill(dsParametreFark);
                if (DataSetKontrol(dsParametreFark))
                {
                    farkdeger = dsParametreFark.Tables[0].Rows[0]["Deger"].ToString();
                }
                command.Dispose();

                //Ürünlerden 3.si
                string sqlUrunBilgileri = "Select UrunAdi,GoruntulenmeSayisi,Fiyat from UrunBilgileri where UrunAdi='Rochas Man Edt Erkek Parfüm 100ml'";

                command = new SqlCommand(sqlUrunBilgileri, connection);
                da = new SqlDataAdapter(sqlUrunBilgileri, connection);
                DataSet dsUrunBilgileri = new DataSet();

                da.Fill(dsUrunBilgileri);
                if (DataSetKontrol(dsUrunBilgileri))
                {
                    UrunAdi = dsUrunBilgileri.Tables[0].Rows[0][0].ToString();
                    EskiUrunGoruntulenmeSayisi = dsUrunBilgileri.Tables[0].Rows[0][1].ToString();
                    EskiUrunFiyati = dsUrunBilgileri.Tables[0].Rows[0][2].ToString();
                    double EskiUrunFiyatiD = Convert.ToDouble(dsUrunBilgileri.Tables[0].Rows[0][2]);
                    double YeniUrunFiyatiD = Convert.ToDouble(YeniUrunFiyati.Replace(" ", "").Replace(",", "."));
                    if ((YeniUrunFiyatiD - EskiUrunFiyatiD) < Convert.ToDouble(farkdeger) || (YeniUrunFiyatiD - EskiUrunFiyatiD) > Convert.ToDouble(farkdeger))
                    {
                        command = new SqlCommand();

                        command.Connection = connection;
                        command.CommandText = "update UrunBilgileri set  GoruntulenmeSayisi= @GoruntulenmeSayisi, Fiyat=@Fiyat where UrunAdi='Rochas Man Edt Erkek Parfüm 100ml'";

                        command.Parameters.AddWithValue("@GoruntulenmeSayisi", YeniGoruntulenmeSayisi);
                        command.Parameters.AddWithValue("@Fiyat", YeniUrunFiyatiD);
                        command.ExecuteNonQuery();

                        // MailAt();
                    }
                }
                else
                {
                    command = new SqlCommand();

                    double Fiyati = Convert.ToDouble(YeniUrunFiyati.Replace(" ", "").Replace(",", "."));
                    command.Connection = connection;
                    command.CommandText = "insert into UrunBilgileri(UrunAdi,GoruntulenmeSayisi,Fiyat) values (@UrunAdi,@Goruntulenme,@Fiyat)";
                    command.Parameters.AddWithValue("@UrunAdi", "Rochas Man Edt Erkek Parfüm 100ml");
                    command.Parameters.AddWithValue("@Goruntulenme", YeniGoruntulenmeSayisi);
                    command.Parameters.AddWithValue("@Fiyat", Fiyati);

                    command.ExecuteNonQuery();


                    // MailAt();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata1.3: " + ex.Message);
            }
        }
        public static string FiyatiniOku()
        {
            try
            {
                var url = new Uri("https://www.rossmann.com.tr/rochas-man-edt-erkek-parfum-100ml-p-st10070080");
                var client = new WebClient();
                var html = client.DownloadString(url);
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(html);

                HtmlNode node = doc.DocumentNode.SelectNodes("//span[@class='price']")[1];

                if (node != null)
                {
                    return node.InnerText.Replace(" ", "").Replace("₺", "");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata2.3: " + ex.Message);
            }

            return "";
        }
        public static bool DataSetKontrol(DataSet dset)
        {
            if (dset != null)
            {
                if (dset.Tables.Count > 0)
                {
                    if (dset.Tables[0].Rows.Count > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static string GoruntulenmeSayisiniOku()
        {
            try
            {
                var url = new Uri("https://www.rossmann.com.tr/prokudent-dis-fircasi-complete-white-orta-1-adet-p-sr18020026"); // url oluştruduk
                var client = new WebClient(); // siteye erişim için client tanımladık
                var html = client.DownloadString(url); //sitenin html lini indirdik
                HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument(); //burada HtmlAgilityPack Kütüphanesini kullandık
                doc.LoadHtml(html); // indirdiğimiz sitenin html lini oluşturduğumuz dokumana dolduruyoruz


                //ins-selected-dynamic-element' burada görüntülenme sayısı var ancak okunmuyor
                HtmlNode node = doc.DocumentNode.SelectNodes("//span[@class='ins-selected-dynamic-element']")[0];
                if (node != null)
                {
                    return node.InnerText.Replace(" ", "");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }

            return "0";
        }
        private static void MailAt()
        {
            SmtpClient sc = new SmtpClient();
            sc.Port = 587;
            sc.Host = "smtp.gmail.com";

            sc.UseDefaultCredentials = false;
            //Buraya mail adres  ve şifre gerekli
            sc.Credentials = new NetworkCredential("frkunuvar@gmail.com", "AhFfG1257");
            sc.EnableSsl = true;

            MailMessage mail = new MailMessage();

            mail.From = new MailAddress("gsdede.1257@gmail.com", "AhFfG1453");

            mail.To.Add("haticeekenek@icloud.com");


            mail.Subject = "Fiyat Değişikliği";
            mail.IsBodyHtml = true;
            mail.Body = "Ürün fiyatı değişmiştir";
            sc.TargetName = "STARTTLS/smtp.gmail.com";
            sc.Send(mail);
            //mail.Attachments.Add(new Attachment(@"C:\Ornek.xlsx"));

        }
    }
}

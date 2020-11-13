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
    class Program
    {
        static System.Timers.Timer timer = new System.Timers.Timer();
        static void Main(string[] args)
        {
            TimerBaslat();
            Console.WriteLine("Çıkmak için \'ç\' harfine basınız!");
            while (Console.Read() != 'ç') ;
        }
        private static void TimerBaslat()
        {
            timer = new System.Timers.Timer
            {
                Interval = 10,
                Enabled = true
                //AutoReset=true
            };
            timer.Elapsed += TimerElapsedEvent;
            timer.Start();
        }
        static void TimerElapsedEvent(Object source, ElapsedEventArgs e)
        {
            timer.Stop();

            Console.WriteLine("Tarih ve saat -> {0:HH:mm:ss.fff}", e.SignalTime.ToString());
            
            Urun1.Urun_1();
            Urun2.Urun_2();
            Urun3.Urun_3();
            timer.Start();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using MySql.Data.MySqlClient;
using System.Threading;
namespace Chat
{
    class Program
    {
        public static string nickname;
        static MySqlConnection connection = new MySqlConnection();
        static void Main(string[] args)
        {
            Timer t = new Timer(TimerCallback, null, 0, 2000);
            connection = new MySqlConnection("SERVER=localhost; UID=root; PASSWORD=password; DATABASE=messages;");
            Console.WriteLine("Enter Nickname");
            nickname = Console.ReadLine();
            if (!string.IsNullOrEmpty(Console.ReadLine()))
            {
                onReadLine(Console.ReadLine());
            }
        }

        private static void TimerCallback(Object o)
        {

            GC.Collect();
        }

        static void checkForMessages()
        {
            connection.Close();
            connection.Open();
            Console.Clear();
            MySqlCommand command = new MySqlCommand("SELECT * FROM messages;",connection);
            MySqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                Console.WriteLine(dr.GetValue(0).ToString());
            }
            
        }

        static void onReadLine(string text)
        {
            connection.Close();
            connection.Open();
            MySqlCommand command = new MySqlCommand($"INSERT INTO messages (message) VALUES({text})", connection);
            int sent = command.ExecuteNonQuery();
            if(sent != 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Message not sent");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.WriteLine(text);
            }
        }
    }
}
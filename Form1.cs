using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FusionDB.Common;

namespace FusionDB
{
    public partial class Form1 : Form
    {
        RtData rtd;
        Image img_enable;
        Image img_disable;
        //System.Timers.Timer freshTimer = new System.Timers.Timer(1000);
        delegate void MsgRef(string msg);
        AsySocket listener = null;
        SortedList<string, AsySocket> clients = new SortedList<string, AsySocket>();

        public Form1()
        {
            InitializeComponent();
            rtd = new RtData();
            for (int i = 0; i < 6; i++)
            {
                rtd.AddSensorInfo(i);
            }
            timer1.Interval = 1000;
            //freshTimer.AutoReset = true;
            //freshTimer.Elapsed += new System.Timers.ElapsedEventHandler(timeUp);
            //freshTimer.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            img_enable = Image.FromFile(@"F:\FusionDB\FusionDB\FusionDB\Resources\enable.bmp");
            img_disable = Image.FromFile(@"F:\FusionDB\FusionDB\FusionDB\Resources\disable.bmp");

            //pictureBoxSensor1.Image = img_enable;
            //pictureBoxSensor2.Image = img_disable;

            labelSensor1.Text = "sensor1";
            labelSensor2.Text = "sensor2";
            labelSensor3.Text = "sensor3";
            labelSensor4.Text = "sensor4";
            labelSensor5.Text = "sensor5";
            labelSensor6.Text = "sensor6";

        }

        public void Data_Oprate()
        {

        }

        public void Data_Show()
        {
            for (int i = 0; i < 6; i++)
            {
                if (rtd.IsEnable(i) == true)
                {
                    switch (i)
                    {
                        case 0:
                            pictureBoxSensor1.Image = img_enable;
                            labelSensor1.Text = rtd.GetSensorID(i);
                            break;
                        case 1:
                            pictureBoxSensor2.Image = img_enable;
                            labelSensor2.Text = rtd.GetSensorID(i);
                            break;
                        case 2:
                            pictureBoxSensor3.Image = img_enable;
                            labelSensor3.Text = rtd.GetSensorID(i);
                            break;
                        case 3:
                            pictureBoxSensor4.Image = img_enable;
                            labelSensor4.Text = rtd.GetSensorID(i);
                            break;
                        case 4:
                            pictureBoxSensor5.Image = img_enable;
                            labelSensor5.Text = rtd.GetSensorID(i);
                            break;
                        case 5:
                            pictureBoxSensor6.Image = img_enable;
                            labelSensor6.Text = rtd.GetSensorID(i);
                            break;
                    }
                    comboBox1.Items.Add(rtd.GetSensorID(i));
                }
                else
                {
                    switch (i)
                    {
                        case 0:
                            pictureBoxSensor1.Image = img_disable;
                            labelSensor1.Text = rtd.GetSensorID(i);
                            break;
                        case 1:
                            pictureBoxSensor2.Image = img_disable;
                            labelSensor2.Text = rtd.GetSensorID(i);
                            break;
                        case 2:
                            pictureBoxSensor3.Image = img_disable;
                            labelSensor3.Text = rtd.GetSensorID(i);
                            break;
                        case 3:
                            pictureBoxSensor4.Image = img_disable;
                            labelSensor4.Text = rtd.GetSensorID(i);
                            break;
                        case 4:
                            pictureBoxSensor5.Image = img_disable;
                            labelSensor5.Text = rtd.GetSensorID(i);
                            break;
                        case 5:
                            pictureBoxSensor6.Image = img_disable;
                            labelSensor6.Text = rtd.GetSensorID(i);
                            break;
                    }
                }
            }
            //Console.WriteLine("2");
            label3.Text = rtd.GetFusionMode();
            label5.Text = rtd.GetFusionSensors();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Data_Oprate();
            Data_Show();
        }

        //public void timeUp(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    Data_Oprate();
        //    Console.WriteLine("1");
        //    this.Data_Show();
        //    //t.Start();
        //}

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Form FormOpen = new Form2();
            FormOpen.Show();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Form FormConfig = new Form3();
            FormConfig.Show();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            //Data_Show();
            timer1.Start();
            //t.Enabled = true;
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            //t.Enabled = false;
        }

        private void ButtonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonRecord_Click(object sender, EventArgs e)
        {
             int flag = 0;
            flag = ~flag;

            if (flag == 0)
            {
                rtd.SetFusionMode("single mode");
            } 
            else
            {
                rtd.SetFusionMode("multi mode");
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            listener = new AsySocket();
            listener.OnAccept += new AcceptEventHandler(listener_OnAccept);
            listener.Listen(10);

            toolStripButton4.Enabled = false;
        }

        void listener_OnAccept(AsySocket AcceptedSocket)
        {
            //注册事件
            AcceptedSocket.OnStringDataAccept += new StringDataAcceptHandler(AcceptedSocket_OnStringDataAccept);
            AcceptedSocket.OnClosed += new AsySocketClosedEventHandler(AcceptedSocket_OnClosed);
            AcceptedSocket.BeginAcceptData();
            //加入
            //listBox1.Items.Add(AcceptedSocket.ID);
            clients.Add(AcceptedSocket.ID, AcceptedSocket);
        }

        void AcceptedSocket_OnClosed(string SocketID, string ErrorMessage)
        {
            //客户端关闭
            clients.Remove(SocketID);
        }

        void AddMsg(string msg)
        {
            textBox1.Text += Environment.NewLine + msg + Environment.NewLine;
        }

        void AcceptedSocket_OnStringDataAccept(string AccepterID, string AcceptData)
        {
            try
            {
                //数据填充

                MsgRef m = new MsgRef(AddMsg);
                this.Invoke(m, new object[] { AcceptData });
                //转发
                //for (int i = 0; i < clients.Count; i++)
                //{
                //    if (clients.Values[i].ID != AccepterID)
                //    {
                //        clients.Values[i].ASend(AcceptData);
                //    }
                //}
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


    }
}
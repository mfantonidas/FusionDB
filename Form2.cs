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
    public partial class Form2 : Form
    {
        DataFlagInfo dfi;
        RtData rtd;
        RecordData rd;
        DataTable dt;
        int index;

        public Form2()
        {
            DataTable dtIndex;
            InitializeComponent();
            dfi = new DataFlagInfo();
            rtd = new RtData();
            rd = new RecordData();
            rd.GetIndexCount();
            dtIndex = rd.LoadData();

            comboBox1.DataSource = dtIndex;
            comboBox1.DisplayMember = "StartTime2EndTime";
            comboBox1.ValueMember = "RecordIndex";

            //string[] sensors = new string[8] {"All", "Fusion Data", "sensor1", "sensor2", "sensor3", "sensor4", "sensor5", "sensor6" };
            //for (int i = 0; i < 8; i++ )
            //{
            //    comboBox2.Items.Add(sensors[i]);
            //}
            //comboBox2.SelectedIndex = 0;
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView drv = comboBox1.SelectedItem as DataRowView;
            DataRow dr = drv.Row;
            
            index = Convert.ToInt32(dr[1]);

            rd.SetCurrentRecord(comboBox1.SelectedText, index);
            rd.LoadData(index);
            rd.SetDataUnit();

            string[] str = DataRecordOpt.GetSensors(index);
            int count = str.Length;
            comboBox2.Items.Clear();
            comboBox2.Items.Add("ALL");
            comboBox2.Items.Add("Fusion Data");
            for (int i = 0; i < count; i++ )
            {
                comboBox2.Items.Add(str[i]);
            }
            comboBox2.SelectedIndex = 0;
            return;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
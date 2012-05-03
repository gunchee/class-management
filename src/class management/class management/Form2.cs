using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Npgsql;

namespace class_management
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            string str = "Uid=postgres; Password=taaldaa; server=localhost; port=5432; Database=classmanagement;";
            NpgsqlConnection conn = new NpgsqlConnection(str);
            conn.Open();
            String select = "SELECT name FROM subject";
            NpgsqlCommand select_command = new NpgsqlCommand(select, conn);
            NpgsqlDataReader reader = select_command.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader[0]);
                comboBox3.Items.Add(reader[0]);
            }
            dataGridView2.Refresh();
            dataGridView2.ColumnCount = 2;
            dataGridView2.Columns[0].Name = "Name";
            dataGridView2.Columns[1].Name = "Surname";
            reader.Close();
            conn.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = "Uid=postgres; Password=taaldaa; server=localhost; port=5432; Database=classmanagement;";
            NpgsqlConnection conn = new NpgsqlConnection(str);
            conn.Open();
            String insertin = "INSERT INTO subject VALUES('" + textBox1.Text + "')";
            NpgsqlCommand command = new NpgsqlCommand(insertin, conn);
            command.ExecuteNonQuery();
            comboBox1.Items.Clear();
            String select = "SELECT name FROM subject";
            NpgsqlCommand select_command = new NpgsqlCommand(select, conn);
            NpgsqlDataReader reader = select_command.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader[0]);
                comboBox3.Items.Add(reader[0]);
            }
            reader.Close();

            MessageBox.Show("Success.", "", MessageBoxButtons.OK);
            


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string str = "Uid=postgres; Password=taaldaa; server=localhost; port=5432; Database=classmanagement;";
            NpgsqlConnection conn = new NpgsqlConnection(str);
            conn.Open();
            dataGridView1.Refresh();
            DataSet ds = new DataSet();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("SELECT * FROM student", conn);
            da.Fill(ds, "result");


            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "result";
           
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string str = "Uid=postgres; Password=taaldaa; server=localhost; port=5432; Database=classmanagement;";
            NpgsqlConnection conn = new NpgsqlConnection(str);
            conn.Open();
            int i = dataGridView2.RowCount;
            i--;
            int n = 0;
            for (n=0; n<i; n++)
            {
                String insert="INSERT into student VALUES('"+dataGridView2.Rows[n].Cells[0].Value+"','"+dataGridView2.Rows[n].Cells[1].Value+"','"+comboBox2.Text+"','"+numericUpDown1.Value+"-"+numericUpDown2.Value+"')";
                NpgsqlCommand command = new NpgsqlCommand(insert, conn);
                command.ExecuteNonQuery();
                
            }
            conn.Close();
            MessageBox.Show("success.");
        }

        private void Show(int i)
        {
            throw new NotImplementedException();
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            string str = "Uid=postgres; Password=taaldaa; server=localhost; port=5432; Database=classmanagement;";
            NpgsqlConnection conn = new NpgsqlConnection(str);
            conn.Open();
           
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {

                if (dr.Cells[0].Value != null) //Cells[0] Because in cell 0th cell we have added checkbox
                {
                    int n = dr.Index;

                    String insert = "INSERT into substudent VALUES('" + comboBox1.Text.ToString() + "','" + dataGridView1.Rows[n].Cells[1].Value + "',' 0 ',' 0','" + dataGridView1.Rows[n].Cells[2].Value + "')";
                    NpgsqlCommand command = new NpgsqlCommand(insert, conn);
                    command.ExecuteNonQuery();
                    String insert1 = "INSERT into attendance VALUES('"+dataGridView1.Rows[n].Cells[1].Value+"','"+dataGridView1.Rows[n].Cells[2].Value+"','"+comboBox1.Text.ToString()+"')";
                    command = new NpgsqlCommand(insert1, conn);
                    command.ExecuteNonQuery();
                }

            }

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
   
            string str = "Uid=postgres; Password=taaldaa; server=localhost; port=5432; Database=classmanagement;";
            NpgsqlConnection conn = new NpgsqlConnection(str);
            conn.Open();
            DataSet ds = new DataSet();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("SELECT * FROM attendance WHERE subjectname='"+comboBox3.Text.ToString()+"' ", conn);
            da.Fill(ds, "result");
            dataGridView3.DataSource = ds;
            dataGridView3.DataMember = "result";
            conn.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string str = "Uid=postgres; Password=taaldaa; server=localhost; port=5432; Database=classmanagement;";
            NpgsqlConnection conn = new NpgsqlConnection(str);
            conn.Open();
            int i = dataGridView3.RowCount;
            int n = 0, j = 3 ;
            int k = dataGridView3.ColumnCount;
            int h = 0;
            i--;
            String update;
            NpgsqlCommand command;

        
            for(n=0; n<i; n++)
            {
                for (j = 3; j<k; j++)
                {
                    h++;
                    update = "UPDATE attendance SET "+h+"='" + dataGridView3.Rows[n].Cells[j].Value.ToString() + "' WHERE studentname='" + dataGridView3.Rows[n].Cells[0].Value.ToString() + "'";
                    command = new NpgsqlCommand(update, conn);
                    command.ExecuteNonQuery();
                    
                }
                h = 0;
            }


            MessageBox.Show("success");
            conn.Close();
        }

    }
}

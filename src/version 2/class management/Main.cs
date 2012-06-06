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
    /// <add version 2 code>
    /// I added the part "remove lessonm, remove student, total grade"
    /// When choosing a lesson, display students who is studying the lesson. Also not displaying students 
    /// who is not able to study the lesson. Also it is possible that cancel the students' lessons.
    /// Calculates start time and end time.
    /// Database Single quote error is fixed.
    /// Calculating all grades.
    /// </summary>
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            string str = "Uid=postgres; Password=123; server=localhost; port=5432; Database=classmanagement;";
            NpgsqlConnection conn = new NpgsqlConnection(str);
            conn.Open();
            String select = "SELECT name FROM subject";
            NpgsqlCommand select_command = new NpgsqlCommand(select, conn);
            NpgsqlDataReader reader = select_command.ExecuteReader();
            
            while (reader.Read())
            {
                comboBox1.Items.Add(reader[0]);
                comboBox3.Items.Add(reader[0]);
                comboBox4.Items.Add(reader[0]);
                comboBox6.Items.Add(reader[0]);
                comboBox7.Items.Add(reader[0]);
            }
            dataGridView2.Refresh();
            dataGridView2.ColumnCount = 2;
            dataGridView2.Columns[0].Name = "Name";
            dataGridView2.Columns[1].Name = "Surname";
            reader.Close();
            conn.Close();
        }
        int q = 0,yes=0;
        private NpgsqlCommand Connect(string s)
        {
            string str = "Uid=postgres; Password=123; server=localhost; port=5432; Database=classmanagement;";
            NpgsqlConnection conn = new NpgsqlConnection(str);
            conn.Open();
            NpgsqlCommand command = new NpgsqlCommand(s,conn);
            command.ExecuteNonQuery();
            conn.Close();
            return command;
        }
        /*
       function name AddLesson_Click
       inputs: query
       outputs: database insert subject
       errors:
       */
        private void AddLesson_Click(object sender, EventArgs e)
        {
            string str = "Uid=postgres; Password=123; server=localhost; port=5432; Database=classmanagement;";

            string str1 = "";
            string str2 = "";
            str1 = textBox1.Text;
            char ch1 ='\'';
            char[] ch = str1.ToCharArray();
            for (int j = 0; j < str1.Length; j++)
            {

                if (ch[j] == ch1)
                {
                    str2 += ch1.ToString() + ch[j];
                }
                else
                    str2 += ch[j];

            }
            textBox1.Text =str2;
            NpgsqlConnection conn = new NpgsqlConnection(str);
            conn.Open();
            
            String insertin = "INSERT INTO subject VALUES('" + textBox1.Text + "','"+numericUpDown3.Value+"','"+numericUpDown4.Value+"','"+numericUpDown6.Value+"','"+numericUpDown5.Value+"')";
            NpgsqlCommand command = new NpgsqlCommand(insertin, conn);
            command.ExecuteNonQuery();
            comboBox1.Items.Clear();
            comboBox3.Items.Clear();
            comboBox4.Items.Clear();
            String select = "SELECT name FROM subject";
            NpgsqlCommand select_command = new NpgsqlCommand(select, conn);
            NpgsqlDataReader reader = select_command.ExecuteReader();

            while (reader.Read())
            {
                comboBox1.Items.Add(reader[0]);
                comboBox3.Items.Add(reader[0]);
                comboBox4.Items.Add(reader[0]);
            }
            reader.Close();

            MessageBox.Show("Success.", "", MessageBoxButtons.OK);
         
        }
        /*
        function name comboBox1_SelectedIndexChanged
        inputs: query
        outputs: the checke indicates whether student take a lesson. If checked, student study a lesson. if unchecked, student do not study.
        errors:
        */
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int row = dataGridView1.RowCount;
            row--;
            while (row>0)
            {
                dataGridView1.Rows.RemoveAt(row - 1);
                row--;
            }
            string str = "Uid=postgres; Password=123; server=localhost; port=5432; Database=classmanagement;";
            NpgsqlConnection conn = new NpgsqlConnection(str);
            conn.Open();
           
            DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();
            {
                column.HeaderText = "";
                column.Name = "";
                column.AutoSizeMode =
                DataGridViewAutoSizeColumnMode.DisplayedCells;
                column.FlatStyle = FlatStyle.Standard;
                column.ThreeState = false;
                column.CellTemplate = new DataGridViewCheckBoxCell();
                column.CellTemplate.Style.BackColor = Color.Beige;
            }
            dataGridView1.Refresh();
            if (yes == 0)
            {
                dataGridView1.Columns.Insert(0, column); // This is to be a checkbox column
            }

            int n = 0;
            int helber = 0;
            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[2].Name = "Name";
            dataGridView1.Columns[3].Name = "Subname";
            dataGridView1.Columns[4].Name = "Class";
            dataGridView1.Columns[5].Name = "Level";

            string sel = "SELECT sname,surname,angi,level1 FROM student";
            int s1=0;
            NpgsqlCommand comm = new NpgsqlCommand(sel, conn);
            NpgsqlDataReader reader = comm.ExecuteReader();
            
            string[,] student = new string[100,4];
            while (reader.Read())
            {
                student[s1,0] = reader[0].ToString();
                student[s1,1] = reader[1].ToString();
                student[s1,2] = reader[2].ToString();
                student[s1,3] = reader[3].ToString();
                s1++;
            }
            reader.Close();
            
            string sel1 = "SELECT student_name,student_surname,start_time,start_minute,finish_time,finish_minute,lesson_name FROM startfinish";

            NpgsqlCommand comm1 = new NpgsqlCommand(sel1, conn);
            NpgsqlDataReader reader1 = comm1.ExecuteReader();
            string[,] start = new string[100, 8];
            int stat = 0;
            while (reader1.Read())
            {
                start[stat, 0] = reader1[0].ToString();
                start[stat, 1] = reader1[1].ToString();
                start[stat, 2] = reader1[2].ToString();
                start[stat, 3] = reader1[3].ToString();
                start[stat, 5] = reader1[4].ToString();
                start[stat, 6] = reader1[5].ToString();
                start[stat, 7] = reader1[6].ToString();
                stat++;
            }  
            reader1.Close();
            
            string sel2 = "SELECT start_time,start_minute,finish_time,finish_minute FROM subject WHERE name='"+comboBox1.Text+"'";

            NpgsqlCommand comm2 = new NpgsqlCommand(sel2, conn);
            NpgsqlDataReader reader2 = comm2.ExecuteReader();
            if (reader2.Read())
            {
               
                for (int i = 0; i < s1; i++)
                {

                    for (int sf = 0; sf < stat; sf++)
                    {
                        if ((student[i, 0] == start[sf, 0] && student[i, 1] == start[sf, 1])&&(start[sf,7]==comboBox1.Text))
                        {

                            if ((Convert.ToInt32(reader2[0]) < Convert.ToInt32(start[sf, 2]) && Convert.ToInt32(start[sf, 2]) < Convert.ToInt32(reader2[3])))
                            {
                                helber = 2;
                            }
                            if ((Convert.ToInt32(reader2[0]) < Convert.ToInt32(start[sf, 4]) && Convert.ToInt32(start[sf, 4]) < Convert.ToInt32(reader2[3])))
                            {
                                helber = 2;
                            }
                            else
                            {
                                dataGridView1.Rows.Add();
                                dataGridView1.Rows[n].Cells[0].Value = true;
                                dataGridView1.Rows[n].Cells[2].Value = student[i, 0];
                                dataGridView1.Rows[n].Cells[3].Value = student[i, 1];
                                dataGridView1.Rows[n].Cells[4].Value = student[i, 2];
                                dataGridView1.Rows[n].Cells[5].Value = student[i, 3];
                                n++;
                                helber = 1;
                            }
                        }
                        
                    }
                    if (helber == 0)
                    {
                        dataGridView1.Rows.Add();
                        dataGridView1.Rows[n].Cells[0].Value = null;
                        dataGridView1.Rows[n].Cells[2].Value = student[i, 0];
                        dataGridView1.Rows[n].Cells[3].Value = student[i, 1];
                        dataGridView1.Rows[n].Cells[4].Value = student[i, 2];
                        dataGridView1.Rows[n].Cells[5].Value = student[i, 3];
                        n++;
                    }
                    helber = 0;
                }
                yes = 1;
            }
            /*
                string str = "Uid=postgres; Password=123; server=localhost; port=5432; Database=classmanagement;";
                NpgsqlConnection conn = new NpgsqlConnection(str);
                conn.Open();
                dataGridView1.Refresh();
                DataSet ds = new DataSet();
                NpgsqlDataAdapter da = new NpgsqlDataAdapter("SELECT * FROM student ", conn);
                da.Fill(ds, "result");
                dataGridView1.DataSource = ds;
                dataGridView1.DataMember = "result";
                conn.Close();
             */

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
        /*
        function name AddNewStudents_Click_1
        inputs: query
        outputs: database insert lesson
        errors:
        */
        private void AddNewStudents_Click_1(object sender, EventArgs e)
        {
            string str = "Uid=postgres; Password=123; server=localhost; port=5432; Database=classmanagement;";
            NpgsqlConnection conn = new NpgsqlConnection(str);
            conn.Open();
            int i = dataGridView2.RowCount;
            i--;
            int n = 0;
            for (n=0; n<i; n++)
            {
                String insert="INSERT into student VALUES('"+dataGridView2.Rows[n].Cells[0].Value+"','"+dataGridView2.Rows[n].Cells[1].Value+"','"+comboBox2.Text.ToString()+"','"+numericUpDown1.Value+"-"+numericUpDown2.Value+"')";
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
        /*
        function name AddStudents_Click
        inputs: query
        outputs: add student studying lesson and cancel student studying lesson
        errors:
        */
        private void AddStudents_Click(object sender, EventArgs e)
        {
            string str = "Uid=postgres; Password=123; server=localhost; port=5432; Database=classmanagement;";
            NpgsqlConnection conn = new NpgsqlConnection(str);
            conn.Open();
 
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                int n = dr.Index;
                if (dr.Cells[0].Value != null)
                {

                    String insert = "";

                    String select1 = "SELECT studentname FROM attendance WHERE studentname='" + dataGridView1.Rows[n].Cells[1].Value + "'AND subjectname='" + comboBox1.Text + "'";
                    NpgsqlCommand select_command1 = new NpgsqlCommand(select1, conn);
                    NpgsqlDataReader read = select_command1.ExecuteReader();
                    if (read.Read())
                    {
                        MessageBox.Show("Error message", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        insert = "INSERT into attendance VALUES('" + dataGridView1.Rows[n].Cells[2].Value + "','" + dataGridView1.Rows[n].Cells[3].Value + "','" + comboBox1.Text + "')";
                        NpgsqlCommand command = new NpgsqlCommand(insert, conn);
                        command.ExecuteNonQuery();
                        command.Cancel();
                        String select = "SELECT name,start_time,start_minute,finish_time,finish_minute FROM subject WHERE name='" + comboBox1.Text + "'";
                        NpgsqlCommand select_command = new NpgsqlCommand(select, conn);
                        NpgsqlDataReader reader1 = select_command.ExecuteReader();
                        NpgsqlCommand baba = new NpgsqlCommand();
                        if (reader1.Read())
                        {
                            int num1 = Convert.ToInt32(reader1[1]);
                            int num2 = Convert.ToInt32(reader1[2]);
                            int num3 = Convert.ToInt32(reader1[3]);
                            int num4 = Convert.ToInt32(reader1[4]);
                            String ins = "INSERT into startfinish VALUES('" + comboBox1.Text.ToString() + "','" + dataGridView1.Rows[n].Cells[2].Value + "','" + dataGridView1.Rows[n].Cells[3].Value + "','" + num1 + "','" + num2 + "','" + num3 + "','" + num4 + "')";
                            baba = new NpgsqlCommand(ins, conn);

                        }
                        reader1.Close();
                        baba.ExecuteNonQuery();

                        insert = "INSERT into homework VALUES('" + dataGridView1.Rows[n].Cells[2].Value + "','" + dataGridView1.Rows[n].Cells[3].Value + "','" + comboBox1.Text.ToString() + "')";
                        NpgsqlCommand comm = new NpgsqlCommand(insert, conn);
                        comm.ExecuteNonQuery();

                        insert = "INSERT into grade VALUES('" + comboBox1.Text.ToString() + "','" + dataGridView1.Rows[n].Cells[2].Value + "','" + dataGridView1.Rows[n].Cells[3].Value + "')";
                        NpgsqlCommand comm1 = new NpgsqlCommand(insert, conn);
                        comm1.ExecuteNonQuery();

                        //MessageBox.Show("success.");
                    }
                    read.Close();
                }
                else
                {
                    String delete = "DELETE FROM attendance WHERE studentname='" + dataGridView1.Rows[n].Cells[2].Value + "' AND subjectname='"+comboBox1.Text+"'";
                    NpgsqlCommand comm = new NpgsqlCommand(delete, conn);
                    comm.ExecuteNonQuery();
                    
                    delete = "DELETE FROM startfinish WHERE student_name='" + dataGridView1.Rows[n].Cells[2].Value + "' AND lesson_name='"+comboBox1.Text+"'";
                    NpgsqlCommand comm1 = new NpgsqlCommand(delete, conn);
                    comm1.ExecuteNonQuery();

                    delete = "DELETE FROM homework WHERE studentname='" + dataGridView1.Rows[n].Cells[2].Value + "' AND subjectname='" + comboBox1.Text + "'";
                    NpgsqlCommand comm2 = new NpgsqlCommand(delete, conn);
                    comm2.ExecuteNonQuery();

                    delete = "DELETE FROM grade WHERE student_name='" + dataGridView1.Rows[n].Cells[2].Value + "' AND lesson='" + comboBox1.Text + "'";
                    NpgsqlCommand comm3 = new NpgsqlCommand(delete, conn);
                    comm3.ExecuteNonQuery();
                }

            }
            conn.Close();
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }
        /*
        function name AttendaceShow_Click
        inputs: query
        outputs: show in the datagridview3
        errors:
        */
        private void AttendaceShow_Click(object sender, EventArgs e)
        {
            
            dataGridView3.Refresh();
            string str = "Uid=postgres; Password=123; server=localhost; port=5432; Database=classmanagement;";
            NpgsqlConnection conn = new NpgsqlConnection(str);
            conn.Open();
            
            DataSet ds = new DataSet();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("SELECT studentname, studentsubname, week1, week2, week3, week4, week5, week6, week7, week8, week9, week10, week11, week12, week13, week14, week15, week16 FROM attendance WHERE subjectname='" + comboBox3.Text.ToString() + "' ", conn);
            
            da.Fill(ds, "result");
            dataGridView3.DataSource = ds;
            dataGridView3.DataMember = "result";
            q = 1;
            conn.Close();
        }
        /*
        function name HomeworkShow_Click
        inputs: query
        outputs: show in the datagridview3
        errors:
        */
        private void HomeworkShow_Click(object sender, EventArgs e)
        {
            dataGridView3.Refresh();
            string str = "Uid=postgres; Password=123; server=localhost; port=5432; Database=classmanagement;";
            NpgsqlConnection conn = new NpgsqlConnection(str);
            conn.Open();
            DataSet ds = new DataSet();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("SELECT studentname, studentsubname, week1, week2, week3, week4, week5, week6, week7, week8, week9, week10, week11, week12, week13, week14, week15, week16  FROM homework WHERE subjectname='" + comboBox4.Text.ToString() + "' ", conn);

            da.Fill(ds, "result");
            dataGridView3.DataSource = ds;
            dataGridView3.DataMember = "result";
            q = 2;
            conn.Close();
            
        }

        /*
         function name SaveAttendance_Click
         inputs: query
         outputs: database insert
         errors:
         */
        private void SaveAttendance_Click(object sender, EventArgs e)
        {
            string str = "Uid=postgres; Password=123; server=localhost; port=5432; Database=classmanagement;";
            NpgsqlConnection conn = new NpgsqlConnection(str);
            conn.Open();
            int i = dataGridView3.RowCount;
            int n = 0, j = 3 ;
            int k = dataGridView3.ColumnCount;
            int h = 0;
            i--;
            String update;
            NpgsqlCommand command;

            if (q == 1)
            {
                h = 0;
                
                for (n = 0; n < i; n++)
                {
                    int total = 0;
                    for (j = 2; j < k; j++)
                    {
                        h++;
                        update = "UPDATE attendance SET week" + h + "='" + dataGridView3.Rows[n].Cells[j].Value.ToString() + "' WHERE studentname='" + dataGridView3.Rows[n].Cells[0].Value.ToString() + "'";
                        command = new NpgsqlCommand(update, conn);
                        command.ExecuteNonQuery();
                        //MessageBox.Show(dataGridView3.Rows[n].Cells[j].Value.ToString());
                        if(dataGridView3.Rows[n].Cells[j].Value.ToString() == "irtsen")
                        {
                            total = total + 1;
                        }
                    }
                    update = "UPDATE grade SET total_attendance='" + total + "' WHERE student_name='" + dataGridView3.Rows[n].Cells[0].Value.ToString() + "'";
                    command = new NpgsqlCommand(update, conn);
                    command.ExecuteNonQuery();
                    h = 0;
                }
            }
            if (q == 2)
            {
                h = 0;
                for (n = 0; n < i; n++)
                {
                    int total_h = 0;
                    for (j = 2; j < k; j++)
                    {
                        h++;
                        update = "UPDATE homework SET week" + h + "='" + dataGridView3.Rows[n].Cells[j].Value.ToString() + "' WHERE studentname='" + dataGridView3.Rows[n].Cells[0].Value.ToString() + "'";

                        command = new NpgsqlCommand(update, conn);
                        command.ExecuteNonQuery();
                        String str1 = dataGridView3.Rows[n].Cells[j].Value.ToString();
                        if (str1 != "")
                        {
                            int f = Convert.ToInt32(str1);
                            total_h = total_h + f;
                            MessageBox.Show(f.ToString());
                        }
                        //int f = Convert.ToInt32(str1);
                        
                    }
                    update = "UPDATE grade SET total_homework='" + total_h + "' WHERE student_name='" + dataGridView3.Rows[n].Cells[0].Value.ToString() + "'";
                    command = new NpgsqlCommand(update, conn);
                    command.ExecuteNonQuery();
                    h = 0;
                }
            }
            if (q == 0)
            {
                MessageBox.Show("chooce attendance or homework pls.");
            }


            MessageBox.Show("success");
            conn.Close();
            
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }
        /*
         function name comboBox5_SelectedIndexChanged
         inputs: query
         outputs: show in the datagridview 
         errors:
         */
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = "Uid=postgres; Password=123; server=localhost; port=5432; Database=classmanagement;";
            NpgsqlConnection conn = new NpgsqlConnection(str);
            conn.Open();
            dataGridView4.Refresh();
            DataSet ds = new DataSet();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("SELECT * FROM student WHERE angi='"+comboBox5.Text+"'", conn);
            da.Fill(ds, "result");
            dataGridView4.DataSource = ds;
            dataGridView4.DataMember = "result";
            conn.Close();
        }
        /*
         function name removestudent_Click
         inputs: query
         outputs: database insert
         errors:
         */
        private void removestudent_Click(object sender, EventArgs e)
        {
            string str = "Uid=postgres; Password=123; server=localhost; port=5432; Database=classmanagement;";
            NpgsqlConnection conn = new NpgsqlConnection(str);
            conn.Open();

            foreach (DataGridViewRow dr in dataGridView4.Rows)
            {
                if (dr.Cells[0].Value != null)
                {
                    int n = dr.Index;

                    String insert = "";

                    insert = "DELETE FROM attendance WHERE studentname='" + dataGridView4.Rows[n].Cells[1].Value.ToString() + "'";
                    NpgsqlCommand comm = new NpgsqlCommand(insert, conn);
                    comm.ExecuteNonQuery();
                    insert = "DELETE FROM homework WHERE studentname='" + dataGridView4.Rows[n].Cells[1].Value.ToString() + "'";
                    NpgsqlCommand comm1 = new NpgsqlCommand(insert, conn);
                    comm1.ExecuteNonQuery();
                    insert = "DELETE FROM startfinish WHERE student_name='" + dataGridView4.Rows[n].Cells[1].Value.ToString() + "'";
                    NpgsqlCommand comm2 = new NpgsqlCommand(insert, conn);
                    comm2.ExecuteNonQuery();
                    insert = "DELETE FROM student WHERE sname='" + dataGridView4.Rows[n].Cells[1].Value.ToString() + "'";
                    NpgsqlCommand comm3 = new NpgsqlCommand(insert, conn);
                    comm3.ExecuteNonQuery();

                    MessageBox.Show("success.");
                }

            }
            conn.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView2.Refresh();
        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }
        /*
         function name button1_Click
         inputs: query
         outputs: show in the datagridview3
         errors:
         */
        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView3.Refresh();
            string str = "Uid=postgres; Password=123; server=localhost; port=5432; Database=classmanagement;";
            NpgsqlConnection conn = new NpgsqlConnection(str);
            conn.Open();
            dataGridView1.Rows.Add();
            DataSet ds = new DataSet();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("SELECT * FROM grade WHERE lesson='" + comboBox6.Text + "' ", conn);

            da.Fill(ds, "result");
            dataGridView3.DataSource = ds;
            dataGridView3.DataMember = "result";

            q = 3;
            conn.Close();
        }
        /*
         function name button2_Click
         inputs: query
         outputs: remove lesson
         errors:
         */
        private void button2_Click(object sender, EventArgs e)
        {
            string str = "Uid=postgres; Password=123; server=localhost; port=5432; Database=classmanagement;";
            NpgsqlConnection conn = new NpgsqlConnection(str);
            conn.Open();

            if (comboBox7.Text != null)
            {

                String insert = "";

                insert = "DELETE FROM attendance WHERE subjectname='" + comboBox7.Text + "'";
                NpgsqlCommand comm = new NpgsqlCommand(insert, conn);
                comm.ExecuteNonQuery();
                insert = "DELETE FROM grade WHERE lesson='" + comboBox7.Text + "'";
                NpgsqlCommand grade = new NpgsqlCommand(insert, conn);
                grade.ExecuteNonQuery();
                insert = "DELETE FROM homework WHERE subjectname='" + comboBox7.Text + "'";
                NpgsqlCommand comm1 = new NpgsqlCommand(insert, conn);
                comm1.ExecuteNonQuery();
                insert = "DELETE FROM startfinish WHERE lesson_name='" + comboBox7.Text + "'";
                NpgsqlCommand comm2 = new NpgsqlCommand(insert, conn);
                comm2.ExecuteNonQuery();
                insert = "DELETE FROM subject WHERE name='" + comboBox7.Text + "'";
                NpgsqlCommand comm3 = new NpgsqlCommand(insert, conn);
                comm3.ExecuteNonQuery();

                MessageBox.Show("success.");
            }

            conn.Close();
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }


        

       

    }
}

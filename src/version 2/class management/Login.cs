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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void UserNameChecker_Click(object sender, EventArgs e)
        {
            string str = "Uid=postgres; Password=123; server=localhost; port=5432; Database=classmanagement;";
            NpgsqlConnection conn = new NpgsqlConnection(str);
            conn.Open();
            String select = "SELECT name,password FROM admin";
            NpgsqlDataReader catcher;
            NpgsqlCommand command = new NpgsqlCommand(select, conn);
            command.ExecuteNonQuery();
            catcher = command.ExecuteReader();
            catcher.Read();
            if ((textBox1.Text == catcher[0].ToString()) && (textBox2.Text == catcher[1].ToString()))
            {
                catcher.Close();
                Main myform = new Main();
                myform.ShowDialog();
                this.Close();
            }
            else
                MessageBox.Show("Та хэрэглэгчийн нэр эсвэл нууц үгээ буруу оруулсан байна.", "Алдааны Мэдээлэл", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}

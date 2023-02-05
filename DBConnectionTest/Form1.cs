using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBConnectionTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static string DataSource = "";
        public static string OutputFolder = "";
        public static string Username = "";
        public static string Password = "";
        public static string AuthType = "";
        public static string connectionString ="";


        public void getValues() 
        {
             DataSource = severNameBox1.Text.Trim();
             OutputFolder = outputTextBox2.Text.Trim();
            Username = userNametextBox1.Text.Trim();
            Password = maskedTextBox1.Text.Trim();
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            outputTextBox2.Text = folderBrowserDialog1.SelectedPath;
        }

        public bool validatedata()
        {
           
            if (string.IsNullOrEmpty(severNameBox1.Text) || string.IsNullOrEmpty(comboBox1.Text) || string.IsNullOrEmpty(outputTextBox2.Text))
            {
                MessageBox.Show("Please fill all mandetory fields");
                return false;
            }
            else
            {
               
                return true;
            }
        
        }
        
        public bool validateserver()
        {
            if (AuthType == "Windows Authentication")
            {
                if (string.IsNullOrEmpty(severNameBox1.Text))
                {
                    MessageBox.Show("Please fill server name");
                    return false;
                }
                else
                {

                    return true;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(severNameBox1.Text))
                {
                    MessageBox.Show("Please fill server name");
                    return false;
                }
                else if (string.IsNullOrEmpty(userNametextBox1.Text) || string.IsNullOrEmpty(maskedTextBox1.Text))
                {
                    MessageBox.Show("Please enter username and password");
                    return false;
                }
                else
                {

                    return true;
                }
            }
           

        }

      
        private void button1_Click(object sender, EventArgs e)
        {
            if (validatedata())
            {
                MessageBox.Show("Test completed");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            getValues();
            if (validateserver())
            {
                if (AuthType == "Windows Authentication")
                {
                    connectionString = $"Data Source={DataSource};Integrated Security=True";        
                }
                else
                {
                    connectionString = $"Data Source={DataSource}; user id={Username};password={Password};";
                }
                //string connectionString = $"Data Source={DataSource};Integrated Security=True";            // LAPTOP-BQHQGI82
                SqlConnection conn = new SqlConnection(connectionString);
                try
                {

                    conn.Open();
                    MessageBox.Show("Sever Connection established");

                    var cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select name from sys.databases";

                    var adapter = new SqlDataAdapter(cmd);
                    var dataset = new DataSet();
                    adapter.Fill(dataset);
                    DataTable dtDatabases = dataset.Tables[0];
                    //comboBox1.Items.Add("----Choose database----");
                    for (int i = 0; i < dtDatabases.Rows.Count; i++)
                    {

                        comboBox1.Items.Add(dtDatabases.Rows[i][0].ToString());
                        conn.Close();
                    }
                    MessageBox.Show("Databases loaded.");
                    //comboBox1.SelectedText = "test1";
                    comboBox1.SelectedIndex = 0;
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void severNameBox1_TextChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
        }


        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            AuthType = comboBox2.SelectedItem.ToString();
            if (!(AuthType== "Windows Authentication"))
            {
                userNametextBox1.Visible = true;
                maskedTextBox1.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
            }
            else
            {
                userNametextBox1.Visible = false;
                maskedTextBox1.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox2.SelectedIndex = 0;
            AuthType = comboBox2.SelectedItem.ToString();
            userNametextBox1.Visible = false;
            maskedTextBox1.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
        }

      

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
        }

        private void userNametextBox1_TextChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
        }
    }
}

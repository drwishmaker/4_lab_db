using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace _4_lab_db
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;
            createDB();
        }
        
        public SQLiteConnection dbConnect;
        public string command = "";
        int count;

        private void createDB()
        {

            dbConnect = new SQLiteConnection("Data Source = library.db; Version = 3");

            if (!File.Exists("./library.db"))
                SQLiteConnection.CreateFile("library.db");

            command = "CREATE TABLE IF NOT EXISTS [books]([id] INTEGER PRIMARY KEY AUTOINCREMENT, [author] VARCHAR(45), [genre] VARCHAR(45), [publishingHouse] VARCHAR(45), [yearOfIssue] INTEGER, [number] INTEGER)";
            SQLiteCommand commandCreate = new SQLiteCommand(command, dbConnect);
            dbConnect.Open();
            commandCreate.ExecuteNonQuery();
            dbConnect.Close();

            dbConnect.Open();
            command = "INSERT INTO [books]([author], [genre], [publishingHouse], [yearOfIssue], [number]) VALUES ('jojo', 'golden wind', 'italia', 2000, 5)";
            SQLiteCommand commandAdd = new SQLiteCommand(command, dbConnect);
            commandAdd.ExecuteNonQuery();
            dbConnect.Close();

            dataGridView1.Rows.Clear();

            dbConnect.Open();
            command = "SELECT * FROM books";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command, dbConnect);

            DataTable table = new DataTable();
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    dataGridView1.Rows.Add(table.Rows[i].ItemArray);
                    count = i;
                }
            }
            dbConnect.Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            dbConnect.Open();
            string commandInsert = "INSERT INTO [books]([author], [genre], [publishingHouse], [yearOfIssue], [number]) VALUES ('"+ textBoxAuthor.Text +"', '"+ textBoxGenre.Text +"', '"+ textBoxPubhouse.Text +"', "+ textBoxYear.Text + ", " + textBoxNum.Text + ")";
            SQLiteCommand addCommand = new SQLiteCommand(commandInsert, dbConnect);
            addCommand.ExecuteNonQuery();

            command = "SELECT * FROM books";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command, dbConnect);

            DataTable table = new DataTable();
            adapter.Fill(table);

            //if (table.Rows.Count > 0)
            //{
            //    for (int i = 0; i < table.Rows.Count; i++)
            //        dataGridView1.Rows.Add(table.Rows[i].ItemArray);
            //}

            dataGridView1.Rows.Add(table.Rows[++count].ItemArray);
            dbConnect.Close();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            dbConnect.Open();
            command = "DELETE FROM books WHERE id = " + numericUpDownId.Value;
            SQLiteCommand commandDelete = new SQLiteCommand(command, dbConnect);
            commandDelete.ExecuteNonQuery();

            command = "SELECT * FROM books";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command, dbConnect);

            DataTable table = new DataTable();
            adapter.Fill(table);

            dataGridView1.Rows.Clear();

            if (table.Rows.Count > 0)
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    dataGridView1.Rows.Add(table.Rows[i].ItemArray);
                    count = i;
                }
            }
            dbConnect.Close();

            int row = dataGridView1.CurrentRow.Index;
            numericUpDownId.Value = Convert.ToDecimal(dataGridView1[0, row].Value);
            textBoxAuthor.Text = Convert.ToString(dataGridView1[1, row].Value);
            textBoxGenre.Text = Convert.ToString(dataGridView1[2, row].Value);
            textBoxPubhouse.Text = Convert.ToString(dataGridView1[3, row].Value);
            textBoxYear.Text = Convert.ToString(dataGridView1[4, row].Value);
            textBoxNum.Text = Convert.ToString(dataGridView1[5, row].Value);
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected == true)
            {
                dbConnect.Open();
                command = "UPDATE books SET author = '" + textBoxAuthor.Text + "', genre = '"+ textBoxGenre.Text +"', publishingHouse = '" + textBoxPubhouse.Text + "', yearOfIssue = '" + 
                    textBoxYear.Text + "', number = '" + textBoxNum.Text + "' where id = " + numericUpDownId.Value;
                SQLiteCommand commandUpdate = new SQLiteCommand(command, dbConnect);
                commandUpdate.ExecuteNonQuery();

                command = "SELECT * FROM books";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command, dbConnect);

                DataTable table = new DataTable();
                adapter.Fill(table);

                dataGridView1.Rows.Clear();

                if (table.Rows.Count > 0)
                {
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        dataGridView1.Rows.Add(table.Rows[i].ItemArray);
                        count = i;
                    }
                }
                dbConnect.Close();

            }
            else
                MessageBox.Show("Input error", "Error");
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            int row = dataGridView1.CurrentRow.Index;
            numericUpDownId.Value = Convert.ToDecimal(dataGridView1[0, row].Value);
            textBoxAuthor.Text = Convert.ToString(dataGridView1[1, row].Value);
            textBoxGenre.Text = Convert.ToString(dataGridView1[2, row].Value);
            textBoxPubhouse.Text = Convert.ToString(dataGridView1[3, row].Value);
            textBoxYear.Text = Convert.ToString(dataGridView1[4, row].Value);
            textBoxNum.Text = Convert.ToString(dataGridView1[5, row].Value);
        }
    }
}
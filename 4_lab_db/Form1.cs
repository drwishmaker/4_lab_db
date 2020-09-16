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

            command = "CREATE TABLE IF NOT EXISTS [books]([id] INTEGER PRIMARY KEY AUTOINCREMENT, [author] VARCHAR(45), [genre] VARCHAR(45), [publishing house] VARCHAR(45), [year of issue] INTEGER, [number] INTEGER)";
            SQLiteCommand commandCreate = new SQLiteCommand(command, dbConnect);
            dbConnect.Open();
            commandCreate.ExecuteNonQuery();
            dbConnect.Close();

            dbConnect.Open();
            command = "INSERT INTO [books]([author], [genre], [publishing house], [year of issue], [number]) VALUES ('jojo', 'golden wind', 'italia', 2000, 5)";
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
            string commandInsert = "INSERT INTO [books]([author], [genre], [publishing house], [year of issue], [number]) VALUES ('"+ textBoxAuthor.Text +"', '"+ textBoxGenre.Text +"', '"+ textBoxPubhouse.Text +"', "+ textBoxYear.Text + ", " + textBoxNum.Text + ")";
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
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            if (numericUpDownId.Value != 0)
            { 
                dbConnect.Open();
                command = "UPDATE books SET author = '" + textBoxAuthor.Text + "' where id = " + numericUpDownId.Value;
                SQLiteCommand commandUpdate = new SQLiteCommand(command, dbConnect);
                commandUpdate.ExecuteNonQuery();
                dbConnect.Close();
            }
        }
    }
}
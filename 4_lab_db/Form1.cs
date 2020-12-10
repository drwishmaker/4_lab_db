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
        public int count;

        private void createDB()
        {
            dbConnect = new SQLiteConnection("Data Source = library.db; Version = 3");

            if (!File.Exists("./library.db"))
                SQLiteConnection.CreateFile("library.db");

            command = "CREATE TABLE IF NOT EXISTS [books]([id] INTEGER PRIMARY KEY AUTOINCREMENT, [author] VARCHAR(45), [bookTitle] VARCHAR(60), [genre] VARCHAR(45), " +
                "[publishingHouse] VARCHAR(45), [yearOfIssue] INTEGER, [number] INTEGER)";
            SQLiteCommand commandCreate = new SQLiteCommand(command, dbConnect);
            dbConnect.Open();
            commandCreate.ExecuteNonQuery();
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
            if (numericUpDownNum.Value > 0)
            {
                dbConnect.Open();
                string commandInsert = "INSERT INTO [books]([author], [bookTitle], [genre], [publishingHouse], [yearOfIssue], [number]) VALUES ('" + textBoxAuthor.Text + "', '" + textBoxTitle.Text + "','" + textBoxGenre.Text + "', '" + textBoxPubhouse.Text + "', " + numericUpDownYear.Value + ", " + numericUpDownNum.Value + ")";
                SQLiteCommand addCommand = new SQLiteCommand(commandInsert, dbConnect);
                addCommand.ExecuteNonQuery();

                command = "SELECT * FROM books";
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command, dbConnect);

                DataTable table = new DataTable();
                adapter.Fill(table);

                if (table.Rows.Count == 1)
                    dataGridView1.Rows.Add(table.Rows[count].ItemArray);
                else
                    dataGridView1.Rows.Add(table.Rows[++count].ItemArray);
                dbConnect.Close();
            }
            else
                MessageBox.Show("Number of pages cant be equal 0", "Error");
        }

        public void AddRecord()
        {
            dbConnect.Open();
            string commandInsert = "INSERT INTO [books]([author], [bookTitle], [genre], [publishingHouse], [yearOfIssue], [number]) VALUES ('test', 'test', 'test', 'test', 2000, 20)";
            SQLiteCommand addCommand = new SQLiteCommand(commandInsert, dbConnect);
            addCommand.ExecuteNonQuery();
            dbConnect.Close();
        }

        public int GetCount()
        {
            dbConnect.Open();
            int counter;
            string command = "SELECT * FROM books;";
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command, dbConnect);

            DataTable table = new DataTable();
            adapter.Fill(table);
            counter = table.Rows.Count;
            dbConnect.Close();

            return counter;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Delete record?","Notification", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
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
                    fillAllTextBox();
                }
                else if (dataGridView1.Rows.Count == 0)
                    clearAllTextBox();
                dbConnect.Close();
            }
            
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            try
            {
                dbConnect.Open();
                command = "UPDATE books SET author = '" + textBoxAuthor.Text + "', bookTitle = '" + textBoxTitle.Text + "', genre = '" + textBoxGenre.Text +"', publishingHouse = '" + textBoxPubhouse.Text + "', yearOfIssue = '" + 
                    numericUpDownYear.Value + "', number = '" + numericUpDownNum.Value + "' where id = " + numericUpDownId.Value;
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
            catch
            {
                MessageBox.Show("Input error", "Error");
            }
                
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            fillAllTextBox();
        }

        private void fillAllTextBox()
        {
            int row = dataGridView1.CurrentRow.Index;
            numericUpDownId.Value = Convert.ToDecimal(dataGridView1[0, row].Value);
            textBoxAuthor.Text = Convert.ToString(dataGridView1[1, row].Value);
            textBoxTitle.Text = Convert.ToString(dataGridView1[2, row].Value);
            textBoxGenre.Text = Convert.ToString(dataGridView1[3, row].Value);
            textBoxPubhouse.Text = Convert.ToString(dataGridView1[4, row].Value);
            numericUpDownYear.Value = Convert.ToDecimal(dataGridView1[5, row].Value);
            numericUpDownNum.Value = Convert.ToDecimal(dataGridView1[6, row].Value);
        }

        private void clearAllTextBox()
        {
            
            foreach(TextBox tb in Controls.OfType<TextBox>())
            {
                tb.Clear();
            }

            foreach(NumericUpDown num in Controls.OfType<NumericUpDown>())
            {
                num.Value = 0;
            }
        }

        private void aboutProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Program developed by Krupsky Artemy, student of 484 gr.\n The program create or open data base of library.", "About program");
        }
    }
}
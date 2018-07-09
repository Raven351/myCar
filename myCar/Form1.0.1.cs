using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace myCar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)  //currently in work
        {

        }

        private void autorzyToolStripMenuItem_Click(object sender, EventArgs e) //Przycisk - autorzy
        {
            Form2 autorzy = new Form2();
            autorzy.Show();
        }

        private void button1_Click(object sender, EventArgs e) //FINISHED /// currently in work
        {
            string textbox_username;
            string[] users = new string[10];
            textbox_username = Convert.ToString(textBox1.Text);
                if (textbox_username != "")
                {
                    {
                    int r = Convert.ToInt32(listBox1.Items.Count);
                    if (r >= 6) MessageBox.Show("Osiągnięto limit użytkowników");
                    else
                    {
                        string user = Convert.ToString(textBox1.Text);
                        users_savetab(user);
                        textBox1.Clear();
                    }
                    textBox1.Clear();
                }
                }
                else MessageBox.Show("Wprowadź nazwe użytkownika");
        }

        private void textBox1_TextChanged(object sender, EventArgs e) //uaktywnienie przycisku "Zapisz" przy próbie wpisania nazwy użytkownika
        {
            string textbox_username;
            textbox_username = Convert.ToString(textBox1.Text);
            if (textbox_username != null) button1.Enabled = true;
        }

        private void zakończToolStripMenuItem_Click(object sender, EventArgs e)
        {
        } //todo

        private void Form1_Load(object sender, EventArgs e) //procedura wczytująca
        {
            if (File.Exists("users.xml"))
            {
                for (int i = 0; i <= 5; i++)
                {
                    if (userstab(i) != null) listBox1.Items.Add(userstab(i));
                    else break;
                }
                if (userstab(0)== null) MessageBox.Show("Brak zapamiętanych uzytkowników. Wprowadź nowego użytkownika");
            }
            else MessageBox.Show("Brak zapamiętanych uzytkowników. Wprowadź nowego użytkownika");
            if (textBox1.Text != null) this.AcceptButton = button1;
            else if (listBox1.SelectedIndex >= 0) this.AcceptButton = button2;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) //uaktywnienie przycisków kontrolnych listy uzytkownikow
        {
            if (listBox1.SelectedIndex >= 0) button3.Enabled = true;
            if (listBox1.SelectedIndex >= 0) button4.Enabled = true;
            if (listBox1.SelectedIndex >= 0) button2.Enabled = true;
        }
        public string userstab(int n) // FINISHED /// tablica odczytu użytkowników z pliku do pamieci programu
        {
            if (File.Exists("users.xml"))
            {
                string[] users = new string[10];
                XmlSerializer xizt = new XmlSerializer(typeof(Information));
                FileStream read = new FileStream("users.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
                Information info = (Information)xizt.Deserialize(read);
                users[0] = Convert.ToString(info.Data1);
                users[1] = Convert.ToString(info.Data2);
                users[2] = Convert.ToString(info.Data3);
                users[3] = Convert.ToString(info.Data4);
                users[4] = Convert.ToString(info.Data5);
                users[5] = Convert.ToString(info.Data6);
                read.Close();
                return users[n];
            }
            else return null;
            
        }
        public void users_savetab(string textbox_username)
        {
            string username;
            string[] users = new string[10];
            username = Convert.ToString(textBox1.Text);
            if (username != "")
            {
                int i;
                for (i =0; i<=5; i++)
                {
                    users[i] = userstab(i);
                }
                i = 0;
                do
                {
                    if (users[i] == username)
                    {
                        MessageBox.Show("Użytkownik o podanej nazwie już istnieje");
                        break;
                    }
                    else
                    {
                        if (users[i] == "" || users[i] == null)
                        {
                            users[i] = username;
                            listBox1.Items.Add(username);
                            break;
                        }
                    }
                    i++;
                } while (i <= 5);
                try
                {
                    Information info = new Information();
                    info.Data1 = users[0];
                    info.Data2 = users[1];
                    info.Data3 = users[2];
                    info.Data4 = users[3];
                    info.Data5 = users[4];
                    info.Data6 = users[5];
                    XMLSave.SaveData(info, "users.xml");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else MessageBox.Show("Wprowadź nazwe użytkownika");
        } //FINISHED ///tablica zapisu użytkowników
        }
    }


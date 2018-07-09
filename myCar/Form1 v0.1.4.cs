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
using System.Threading;

namespace myCar
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Userchange_FormClosed(object sender, FormClosedEventArgs e)
        {
            throw new NotImplementedException();
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

        private void button3_Click(object sender, EventArgs e)  //przycisk edycji nazwy
        {
            string username;
            username = listBox1.SelectedItem.ToString();
            try
            {
                Information info = new Information();
                info.Data1 = username;
                XMLSave.SaveData(info, "temp_username.xml");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            users_change(listBox1.SelectedIndex);
            userchangewindow openwindow = new userchangewindow();
            this.Hide();
            listBox1.Items.Clear();
            openwindow.ShowDialog();
            Form1_reload();
            this.Show();
        }
        public string user_change_number_sender()
        {
            return listBox1.SelectedIndex.ToString();
        }

        private void autorzyToolStripMenuItem_Click(object sender, EventArgs e) //FINISHED /// Przycisk - autorzy
        {
            Form2 autorzy = new Form2();
            autorzy.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e) //FINISHED ///
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

        private void textBox1_TextChanged(object sender, EventArgs e) //FINISHED /// uaktywnienie przycisku "Zapisz" przy próbie wpisania nazwy użytkownika
        {
            string textbox_username;
            textbox_username = Convert.ToString(textBox1.Text);
            if (textbox_username != null) button1.Enabled = true;
        }

        private void zakończToolStripMenuItem_Click(object sender, EventArgs e)
        {
        } //TODO

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
        private void Form1_reload()
        {
            listBox1.Items.Clear();
            if (File.Exists("users.xml"))
            {
                for (int i = 0; i <= 5; i++)
                {
                    if (userstab(i) != null) listBox1.Items.Add(userstab(i));
                    else break;
                }
                if (userstab(0) == null) MessageBox.Show("Brak zapamiętanych uzytkowników. Wprowadź nowego użytkownika");
            }
            else MessageBox.Show("Brak zapamiętanych uzytkowników. Wprowadź nowego użytkownika");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) // FINISHED /// uaktywnienie przycisków kontrolnych listy uzytkownikow
        {
            if (listBox1.SelectedIndex >= 0) button3.Enabled = true;
            if (listBox1.SelectedIndex >= 0) button4.Enabled = true;
            if (listBox1.SelectedIndex >= 0) button2.Enabled = true;
        }
        public static string userstab(int n) // FINISHED /// tablica odczytu użytkowników z pliku do pamieci programu
        {
            if (File.Exists("users.xml")) //sprawdzenie czy plik istnienie
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
                    users[i] = userstab(i); //wczytanie do tablicy nazw z pliku
                }
                i = 0;
                do
                {
                    if (users[i] == username) // sprawdzenie czy dana nazwa już istnieje
                    {
                        MessageBox.Show("Użytkownik o podanej nazwie już istnieje");
                        break;
                    }
                    else
                    {
                        if (users[i] == "" || users[i] == null) // przypisanie nazwy do tablicy na najbliższym wolnym miejscu
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
                    Information info = new Information(); //odwołanie do klasy przypisującej elementy tablicy do pliku xml
                    info.Data1 = users[0];
                    info.Data2 = users[1];
                    info.Data3 = users[2];
                    info.Data4 = users[3];
                    info.Data5 = users[4];
                    info.Data6 = users[5];
                    XMLSave.SaveData(info, "users.xml"); //odwołanie do klasy zapisującej dane w pliku xml
                }
                catch (Exception ex) //kontrola błedów w trakcie próby zapisu
                {
                    MessageBox.Show(ex.Message); //wiadomość wyświetlająca treść błedu
                }
            }
            else MessageBox.Show("Wprowadź nazwe użytkownika");
        } //FINISHED ///tablica zapisu użytkowników
        private void users_delete(int n)
        {
            listBox1.Items.RemoveAt(n);
            string[] users = new string[10];
            int i;
            for (i = 0; i <= 5; i++)
            {
                users[i] = userstab(i); //wczytanie do tablicy nazw z pliku
            }
            for (i = n; i <= 5; i++)
            {
                users[i] = users[i + 1];
            }
            try
            {
                Information info = new Information(); //odwołanie do klasy przypisującej elementy tablicy do pliku xml
                info.Data1 = users[0];
                info.Data2 = users[1];
                info.Data3 = users[2];
                info.Data4 = users[3];
                info.Data5 = users[4];
                info.Data6 = users[5];
                XMLSave.SaveData(info, "users.xml"); //odwołanie do klasy zapisującej dane w pliku xml
            }
            catch (Exception ex) //kontrola błedów w trakcie próby zapisu
            {
                MessageBox.Show(ex.Message); //wiadomość wyświetlająca treść błedu
            }
        }//FINISHED ///procedura usunięcia użytkownika

        private void button4_Click(object sender, EventArgs e)// FINISHED ///przycisk usunięcia użytkownika
        {
            int n = Convert.ToInt32(listBox1.SelectedIndex);
            users_delete(n);
        }
        private void users_change(int n)
        {
            string[] users = new string[10];
            int i;
            for (i = 0; i <= 5; i++)
            {
                users[i] = userstab(i); //wczytanie do tablicy nazw z pliku
            }
            users[n] = null;
            try
            {
                Information info = new Information(); //odwołanie do klasy przypisującej elementy tablicy do pliku xml
                info.Data1 = users[0];
                info.Data2 = users[1];
                info.Data3 = users[2];
                info.Data4 = users[3];
                info.Data5 = users[4];
                info.Data6 = users[5];
                XMLSave.SaveData(info, "users.xml"); //odwołanie do klasy zapisującej dane w pliku xml
            }
            catch (Exception ex) //kontrola błedów w trakcie próby zapisu
            {
                MessageBox.Show(ex.Message); //wiadomość wyświetlająca treść błedu
            }
        }//FINISHED ///procedura usunięcia użytkownika
    }
    }


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
using System.Text.RegularExpressions;

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
            if (listBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Wybierz użytkownika!");
            }
            else
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
            char[] allowed = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
            string textbox_username;
            string[] users = new string[10];
            textbox_username = Convert.ToString(textBox1.Text);
                if (textbox_username != "")
                {
                if (textBox1.Text.EndsWith(" ") || textBox1.Text.StartsWith(" ")) { MessageBox.Show("Nazwa nie może zawierać spacji na początku lub końcu");}
                else
                {
                    int r = Convert.ToInt32(listBox1.Items.Count);
                    if (r >= 6) MessageBox.Show("Osiągnięto limit użytkowników");
                    //else if (Regex.IsMatch(textBox1.Text, @"^[a-zA-Z0-9]+$"));
                    else if (textBox1.Text.All(Char.IsLetterOrDigit))
                    {
                        string user = Convert.ToString(textBox1.Text);
                        users_savetab(user);
                        textBox1.Clear();
                        label8.Text = Convert.ToString(listBox1.Items.Count + 1 - 1);
                    }
                    else
                    {
                        MessageBox.Show("Nazwa zawiera niedozwolone znaki");
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
            if (textbox_username != null)
            {
                button1.Enabled = true;
                AcceptButton = button1;
            }
        }

        private void zakończToolStripMenuItem_Click(object sender, EventArgs e)
        {
        } //TODO

        private void Form1_Load(object sender, EventArgs e) //procedura wczytująca
        {
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
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
            label8.Text = listBox1.Items.Count.ToString();
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
            label8.Text = listBox1.Items.Count.ToString();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) // FINISHED /// uaktywnienie przycisków kontrolnych listy uzytkownikow
        {
            if (listBox1.SelectedIndex >= 0) button3.Enabled = true;
            if (listBox1.SelectedIndex >= 0) button4.Enabled = true;
            if (listBox1.SelectedIndex >= 0) button2.Enabled = true;
            if (listBox1.SelectedIndex >= 0) AcceptButton = button2;
            else AcceptButton = button1;
            label8.Text = listBox1.Items.Count.ToString();

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
            if (listBox1.SelectedIndex < 0) MessageBox.Show("Wybierz użytkownika!");
            else
            {
                DialogResult result = MessageBox.Show(String.Format("Czy na pewno chcesz usunąć użytkownika o nazwie '{0}'?", listBox1.SelectedItem.ToString()), "Ostrzeżenie", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    delete_user_files(listBox1.SelectedItem.ToString());
                    int n = Convert.ToInt32(listBox1.SelectedIndex);
                    users_delete(n);
                }
                else
                {

                }
            }
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

        private void button2_Click(object sender, EventArgs e)
        {
            int user_index;
            user_index = listBox1.SelectedIndex;
            if (user_index == -1)
            {
                MessageBox.Show("Wybierz użytkownika");
            }
            else
            {
                string user_name;
                user_name = listBox1.SelectedItem.ToString();
                try
                {
                    Information info = new Information(); //odwołanie do klasy przypisującej elementy tablicy do pliku xml
                    info.Data1 = user_name;
                    info.Data2 = listBox1.SelectedItem.ToString();
                    XMLSave.SaveData(info, "identifier.xml"); //odwołanie do klasy zapisującej dane w pliku xml
                }
                catch (Exception ex) //kontrola błedów w trakcie próby zapisu
                {
                    MessageBox.Show(ex.Message); //wiadomość wyświetlająca treść błedu
                }
                this.Hide();
                MainMenu menu = new MainMenu();
                menu.ShowDialog();
                this.Show();
            }

        }

        private void zakończToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void delete_user_files(string username)
        {
            username_files_delete(username);
        }

        private void username_files_delete(string username)
        {
            if (File.Exists("carlist_user_" + username + ".xml"))
            {
                string[] carlist = new string[10];
                XmlSerializer carlist_serial = new XmlSerializer(typeof(Information));
                FileStream carlist_read = new FileStream("carlist_user_" + username + ".xml", FileMode.Open, FileAccess.Read, FileShare.Read);
                Information car = (Information)carlist_serial.Deserialize(carlist_read);
                carlist[0] = car.Data1;
                carlist[1] = car.Data2;
                carlist[2] = car.Data3;
                carlist[3] = car.Data4;
                carlist[4] = car.Data5;
                carlist[5] = car.Data6;
                carlist[6] = car.Data7;
                carlist[7] = car.Data8;
                carlist[8] = car.Data9;
                carlist[9] = car.Data10;
                carlist_read.Close();
                for (int i = 0; i <= 9; i++)
                {
                    if (File.Exists("carlist_user_" + username + "_" + carlist[i] + ".xml"))
                    {
                        string[] carname = new string[12];
                        XmlSerializer carname_serial = new XmlSerializer(typeof(Information));
                        FileStream carname_read = new FileStream("carlist_user_" + username + "_" + carlist[i] + ".xml", FileMode.Open, FileAccess.Read, FileShare.Read);
                        Information info = (Information)carname_serial.Deserialize(carname_read);
                        carname[0] = info.Data1;
                        carname[1] = info.Data2;
                        carname[2] = info.Data3;
                        carname[3] = info.Data4;
                        carname[4] = info.Data5;
                        carname[5] = info.Data6;
                        carname[6] = info.Data7;
                        carname[7] = info.Data8;
                        carname[8] = info.Data9;
                        carname[9] = info.Data10;
                        carname[10] = info.Data11;
                        carname[11] = info.Data12;
                        carname_read.Close();
                        username_events_files_delete(username, carlist[i]);
                        File.Delete("carlist_user_" + username + "_" + carlist[i] + ".xml");
                    }
                }
                File.Delete("carlist_user_" + username + ".xml");
            }

        }

        private void username_events_files_delete(string username, string carname)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + username + "_" + carname + ".txt"))
            {
                string[] events_filenames = new string[1000];
                string[] events_name = new string[1000];
                string[] events_date = new string[1000];
                FileStream read_file = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + username + "_" + carname + ".txt", FileMode.Open, FileAccess.Read, FileShare.None);
                StreamReader read = new StreamReader(read_file);
                for (int i = 0; i <= 999; i++)
                {
                    events_filenames[i] = read.ReadLine();
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\events\" + events_filenames[i]))
                    {
                        XmlSerializer events_data_serial = new XmlSerializer(typeof(Information));
                        FileStream events_data_read = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\" + events_filenames[i], FileMode.Open, FileAccess.Read, FileShare.Read);
                        Information events_data = (Information)events_data_serial.Deserialize(events_data_read);
                        events_date[i] = events_data.Data2;
                        events_name[i] = events_data.Data1;
                        events_data_read.Close();
                    }
                }
                read.Close();
                read_file.Close();
                FileStream new_event_list = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + username + "_" + carname + ".txt", FileMode.Create, FileAccess.Write, FileShare.None);
                StreamWriter write = new StreamWriter(new_event_list);
                for (int i = 0; i <= 999; i++)
                {
                    if (events_filenames[i] == null || events_filenames[i] == "") break;
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\events\" + events_filenames[i]);
                }
                write.Close();
                new_event_list.Close();
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + username + "_" + carname + ".txt");
            }
        }
    }
    }


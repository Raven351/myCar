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
    public partial class userchangewindow : Form
    {
        public userchangewindow()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string newusername;
            string existingusername;
            XmlSerializer xizt = new XmlSerializer(typeof(Information));
            FileStream read = new FileStream("temp_username.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
            Information info = (Information)xizt.Deserialize(read);
            existingusername = Convert.ToString(info.Data1);
            read.Close();
            newusername = textBox1.Text;
            int i = 0;
            for (i = 0; i <= 5; i++)
            {
                if (newusername == userstab(i) || newusername == existingusername)
                {
                    textBox1.Clear();
                    MessageBox.Show("Użytkownik o danej nazwie już istnieje");
                    break;
                }
                else if (textBox1.Text == null || textBox1.Text == "")
                {
                    textBox1.Clear();
                    MessageBox.Show("Wprowadź nazwe użytkownika");
                    break;
                }

                else if (newusername != null && newusername != "" && newusername != userstab(i) || newusername != existingusername)
                {
                    if ((textBox1.Text.All(Char.IsLetterOrDigit)))
                    {
                        users_savetab(newusername);
                        new_username_filename_change(existingusername, newusername);
                        this.Close();
                        break;
                    }
                    else { MessageBox.Show("Nazwa zawiera niedozwolone znaki"); textBox1.Clear(); break; }
                }

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string existingusername;
            XmlSerializer xizt = new XmlSerializer(typeof(Information));
            FileStream read = new FileStream("temp_username.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
            Information info = (Information)xizt.Deserialize(read);
            existingusername = Convert.ToString(info.Data1);
            read.Close();
            users_savetab(existingusername);
            this.Close();
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
            username = textbox_username;
            if (username != "")
            {
                int i;
                for (i = 0; i <= 5; i++)
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

        private void userchangewindow_Load(object sender, EventArgs e)
        {
            this.ActiveControl = textBox1;
        }

        private void new_username_filename_change(string old_username, string new_username)
        {
            if (File.Exists("carlist_user_" + old_username + ".xml"))
            {
                string[] carlist = new string[10];
                XmlSerializer carlist_serial = new XmlSerializer(typeof(Information));
                FileStream carlist_read = new FileStream("carlist_user_" + old_username + ".xml", FileMode.Open, FileAccess.Read, FileShare.Read);
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
                    if (File.Exists("carlist_user_" + old_username + "_" + carlist[i] + ".xml"))
                    {
                        string[] carname = new string[12];
                        XmlSerializer carname_serial = new XmlSerializer(typeof(Information));
                        FileStream carname_read = new FileStream("carlist_user_" + old_username + "_" + carlist[i] + ".xml", FileMode.Open, FileAccess.Read, FileShare.Read);
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
                        try
                        {
                            Information info_new = new Information();
                            info_new.Data1 = carname[0];
                            info_new.Data2 = carname[1];
                            info_new.Data3 = carname[2];
                            info_new.Data4 = carname[3];
                            info_new.Data5 = carname[4];
                            info_new.Data6 = carname[5];
                            info_new.Data7 = carname[6];
                            info_new.Data8 = carname[7];
                            info_new.Data9 = carname[8];
                            info_new.Data10 = carname[9];
                            info_new.Data11 = carname[10];
                            info_new.Data12 = carname[11];
                            XMLSave.SaveData(info_new, "carlist_user_" + new_username + "_" + carlist[i] + ".xml");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        new_username_events_filename_change(old_username, new_username, carlist[i]);
                        File.Delete("carlist_user_" + old_username + "_" + carlist[i] + ".xml");
                    }
                }
                try
                {
                    Information info_new = new Information();
                    info_new.Data1 = carlist[0];
                    info_new.Data2 = carlist[1];
                    info_new.Data3 = carlist[2];
                    info_new.Data4 = carlist[3];
                    info_new.Data5 = carlist[4];
                    info_new.Data6 = carlist[5];
                    info_new.Data7 = carlist[6];
                    info_new.Data8 = carlist[7];
                    info_new.Data9 = carlist[8];
                    info_new.Data10 = carlist[9];
                    XMLSave.SaveData(info_new, "carlist_user_" + new_username + ".xml");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                File.Delete("carlist_user_" + old_username + ".xml");
            }
        }

        private void new_username_events_filename_change(string old_username, string new_username, string carname)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + old_username + "_" + carname + ".txt"))
            {
                string[] events_filenames = new string[1000];
                string[] events_name = new string[1000];
                string[] events_date = new string[1000];
                FileStream read_file = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + old_username + "_" + carname + ".txt", FileMode.Open, FileAccess.Read, FileShare.None);
                StreamReader read = new StreamReader(read_file);
                for (int i = 0; i<=999; i++)
                {
                    events_filenames[i] = read.ReadLine();
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\events\" + events_filenames[i])) {
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
                FileStream new_event_list = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + new_username + "_" + carname + ".txt", FileMode.Create, FileAccess.Write, FileShare.None);
                StreamWriter write = new StreamWriter(new_event_list);
                for (int i = 0; i<=999; i++)
                {
                    if (events_filenames[i] == null || events_filenames[i] == "") break;
                    File.Move(AppDomain.CurrentDomain.BaseDirectory + @"\events\" + events_filenames[i], AppDomain.CurrentDomain.BaseDirectory + @"\events\" + new_username + "_" + carname + "_" + events_date[i] + "_" + events_name[i] + ".xml");
                    write.WriteLine(new_username + "_" + carname + "_" + events_date[i] + "_" + events_name[i] + ".xml");
                }
                write.Close();
                new_event_list.Close();
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + old_username + "_" + carname + ".txt");
            }
        }
    }
}

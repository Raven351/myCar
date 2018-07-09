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
                else if (newusername != null && newusername != "" && newusername != userstab(i) || newusername != existingusername)
                {
                    users_savetab(newusername);
                    this.Close();
                    break;
                }
                else
                {
                    textBox1.Clear();
                    MessageBox.Show("Wprowadź nazwe użytkownika");
                    break;
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
    }
}

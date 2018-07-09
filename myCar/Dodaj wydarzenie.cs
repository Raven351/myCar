using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;

namespace myCar
{
    public partial class Dodaj_wydarzenie : Form
    {
        public Dodaj_wydarzenie()
        {
            InitializeComponent();
        }
        private string current_user_name()
        {
            XmlSerializer xizt = new XmlSerializer(typeof(Information));
            FileStream read = new FileStream("identifier.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
            Information info = (Information)xizt.Deserialize(read);
            string username = Convert.ToString(info.Data2);
            read.Close();
            return username;
        }
        private string current_tab_name()
        {
            string current_tab_name;
            XmlSerializer xizt = new XmlSerializer(typeof(Information));
            FileStream read = new FileStream("identifier_current_tab.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
            Information info = (Information)xizt.Deserialize(read);
            current_tab_name = Convert.ToString(info.Data1);
            read.Close();
            return current_tab_name;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //AppDomain.CurrentDomain.BaseDirectory
            //txt -> tablica -> dodaj filename do tablicy -> tablica ->txt
            string filename;
            filename = current_user_name() + "_" + current_tab_name() + "_" + dateTimePicker3.Text + "_" + textBox3.Text + ".xml";
            if (textBox3.Text.EndsWith(" ") || textBox3.Text.StartsWith(" "))
            {
                MessageBox.Show("Opis skrócony nie może zawierać spacji na początku lub końcu");
                textBox3.Clear();
            }
            else if (textBox3.Text == null || textBox3.Text == "")
            {
                MessageBox.Show("Wprowadź nazwe!");
                textBox3.Clear();
            }
            else if (((textBox3.Text.All(Char.IsLetterOrDigit))))
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + current_user_name() + "_" + current_tab_name() + ".txt"))
                {
                    int i = -1;
                    string[] filenames = new string[1000];
                    FileStream open = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + current_user_name() + "_" + current_tab_name() + ".txt", FileMode.Open, FileAccess.Read, FileShare.None);
                    System.IO.StreamReader events_list = new StreamReader(open);
                    do
                    {
                        i++;
                        filenames[i] = events_list.ReadLine();
                    } while (filenames[i] != null);
                    events_list.Close();
                    open.Close();
                    i = 0;
                    do
                    {
                        if (filenames[i] == filename)
                        {
                            MessageBox.Show("Wydarzenie o podanej nazwie skróconej i dla podanej daty już istnieje");
                            break;
                        }
                        else
                        {
                            if (filenames[i] == "" || filenames[i] == null)
                            {
                                filenames[i] = filename;
                                MessageBox.Show(filenames[2]);
                                FileStream open2 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + current_user_name() + "_" + current_tab_name() + ".txt", FileMode.Open, FileAccess.Write, FileShare.None);
                                System.IO.StreamWriter events_list2 = new StreamWriter(open2);
                                i = 0;
                                while (filenames[i] != null)
                                {
                                    events_list2.WriteLine(filenames[i]);
                                    i++;
                                }
                                events_list2.Close();
                                open2.Close();
                                try
                                {
                                    Information info = new Information(); //odwołanie do klasy przypisującej elementy tablicy do pliku xml
                                    info.Data1 = Convert.ToString(textBox3.Text);
                                    info.Data2 = Convert.ToString(dateTimePicker3.Text);
                                    info.Data3 = Convert.ToString(textBox9.Text);
                                    info.Data4 = Convert.ToString(richTextBox1.Text);
                                    XMLSave.SaveData(info, AppDomain.CurrentDomain.BaseDirectory + @"\events\" + filename); //odwołanie do klasy zapisującej dane w pliku xml
                                }
                                catch (Exception ex) //kontrola błedów w trakcie próby zapisu
                                {
                                    MessageBox.Show(ex.Message); //wiadomość wyświetlająca treść błedu
                                }
                                textBox3.Clear();
                                textBox9.Clear();
                                richTextBox1.Clear();
                                break;
                            }
                        }
                        i++;
                    } while (i <= 999);

                }
                else
                {
                    FileStream open2 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + current_user_name() + "_" + current_tab_name() + ".txt", FileMode.Create, FileAccess.Write, FileShare.None);
                    System.IO.StreamWriter events_list2 = new StreamWriter(open2);
                    events_list2.Write(filename);
                    try
                    {
                        Information info = new Information(); //odwołanie do klasy przypisującej elementy tablicy do pliku xml
                        info.Data1 = Convert.ToString(textBox3.Text);
                        info.Data2 = Convert.ToString(dateTimePicker3.Text);
                        info.Data3 = Convert.ToString(textBox9.Text);
                        info.Data4 = Convert.ToString(richTextBox1.Text);
                        XMLSave.SaveData(info, AppDomain.CurrentDomain.BaseDirectory + @"\events\" + filename); //odwołanie do klasy zapisującej dane w pliku xml
                    }
                    catch (Exception ex) //kontrola błedów w trakcie próby zapisu
                    {
                        MessageBox.Show(ex.Message); //wiadomość wyświetlająca treść błedu
                    }
                    events_list2.Close();
                    open2.Close();
                    textBox3.Clear();
                    textBox9.Clear();
                    richTextBox1.Clear();
                }
            }
            else { MessageBox.Show("Nazwa zawiera niedozwolone znaki"); textBox3.Clear(); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Dodaj_wydarzenie_Load(object sender, EventArgs e)
        {
            dateTimePicker3.Format = DateTimePickerFormat.Custom;
            dateTimePicker3.CustomFormat = "yyyy.MM.dd";
        }
    }
}

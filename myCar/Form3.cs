using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;


namespace myCar
{
    public partial class Form3 : Form
    {
        public Form3()
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
        private string old_filename()
        {
            FileStream read = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\edited_event.txt", FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader readfile = new StreamReader(read);
            string filename = readfile.ReadLine();
            readfile.Close();
            read.Close();
            return filename;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string filename;
            filename = current_user_name() + "_" + current_tab_name() + "_" + dateTimePicker3.Text + "_" + textBox3.Text + ".xml";
            if (textBox3.Text.EndsWith(" ") || textBox3.Text.StartsWith(" "))
            {
                MessageBox.Show("Opis skrócony nie może zawierać spacji na początku lub końcu");
                textBox3.Clear();
            }
            else if ((textBox3.Text.All(Char.IsLetterOrDigit)))
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
                        if ((filenames[i] == filename) && (filename != old_filename()))
                    {
                            MessageBox.Show("Wydarzenie o podanej nazwie skróconej i dla podanej daty już istnieje");
                            break;
                    }
                    if ((filenames[i] == filename) && (filename==old_filename()))
                    {
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
                    else
                        {
                            if (filenames[i] == old_filename())
                            {
                                filenames[i] = filename;
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
                            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\events\" + old_filename())) File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\events\" + old_filename());
                            break;
                            }
                        }
                        i++;
                    } while (i <= 999);
                this.Close();
            }
            else { MessageBox.Show("Nazwa zawiera niedozwolone znaki"); textBox3.Clear(); }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            dateTimePicker3.Format = DateTimePickerFormat.Custom;
            dateTimePicker3.CustomFormat = "yyyy.MM.dd";
            XmlSerializer xizt = new XmlSerializer(typeof(Information));
            FileStream read = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\" + old_filename(), FileMode.Open, FileAccess.Read, FileShare.Read);
            Information info = (Information)xizt.Deserialize(read);
            dateTimePicker3.Text = info.Data2;
            textBox3.Text = info.Data1;
            textBox9.Text = info.Data3;
            richTextBox1.Text = info.Data4;
            read.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

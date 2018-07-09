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
    public partial class event_delete_komunikat : Form
    {
        public event_delete_komunikat()
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
        private string event_filename()
        {
            FileStream read = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\edited_event.txt", FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader readfile = new StreamReader(read);
            string filename = readfile.ReadLine();
            readfile.Close();
            read.Close();
            return filename;
        }

        private void button1_Click(object sender, EventArgs e)
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
                FileStream open2 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + current_user_name() + "_" + current_tab_name() + ".txt", FileMode.Create, FileAccess.Write, FileShare.None);
                System.IO.StreamWriter events_list2 = new StreamWriter(open2);
                while (filenames[i] != null)
                {
                    if (event_filename() == filenames[i]) i++;
                    else
                    {
                        events_list2.WriteLine(filenames[i]);
                        i++;
                    }
                }
                events_list2.Close();
                open2.Close();
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\events\" + event_filename());
                this.Close();
                }
            else
            {
                MessageBox.Show("Błąd! Plik nie istnieje!");
                this.Close();
            }

        }

        private void event_delete_komunikat_Load(object sender, EventArgs e)
        {
            XmlSerializer xizt = new XmlSerializer(typeof(Information));
            FileStream read = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\" + event_filename(), FileMode.Open, FileAccess.Read, FileShare.Read);
            Information info = (Information)xizt.Deserialize(read);
            textBox2.Text = info.Data1;
            textBox1.Text = info.Data2;
            read.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

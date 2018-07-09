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
    public partial class Dla_Daty_Listawydarzen : Form
    {
        public Dla_Daty_Listawydarzen()
        {
            InitializeComponent();
        }
        struct wydarzenie
        {
            //DateTime
            public string data, nazwa, lokacja, szczegoły;
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

        private void form_reload()
        {
            listView1.Items.Clear();
            button2.Enabled = false;
            button3.Enabled = false;
            FileStream datach = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\wybrana_data.txt", FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader datatach2 = new StreamReader(datach);
            string wybrana_data = datatach2.ReadLine();
            datatach2.Close();
            datach.Close();
            wydarzenie[] s = new wydarzenie[1000];
            ListViewItem lvi;
            label2.Text = current_user_name();
            label3.Text = current_tab_name();
            int i = 0;
            string[] filenames = new string[1000];
            FileStream open = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + current_user_name() + "_" + current_tab_name() + ".txt", FileMode.Open, FileAccess.Read, FileShare.None);
            System.IO.StreamReader events_list = new StreamReader(open);
            do
            {
                filenames[i] = events_list.ReadLine();
                if (filenames[i] == "" || filenames[i] == null) break;
                else if (filenames[i].Contains(wybrana_data))
                {
                    XmlSerializer xizt = new XmlSerializer(typeof(Information));
                    FileStream read = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\" + filenames[i], FileMode.Open, FileAccess.Read, FileShare.Read);
                    MessageBox.Show(filenames[i]);
                    Information info = (Information)xizt.Deserialize(read);
                    s[i].nazwa = info.Data1;
                    s[i].data = info.Data2;
                    s[i].lokacja = info.Data3;
                    s[i].szczegoły = info.Data4;
                    if (s[i].data == wybrana_data)
                    {
                        lvi = new ListViewItem(s[i].data);
                        lvi.SubItems.Add(s[i].nazwa);
                        lvi.SubItems.Add(s[i].lokacja);
                        lvi.SubItems.Add(s[i].szczegoły);
                        listView1.Items.Add(lvi);
                    }
                    read.Close();
                    i++;
                }
            } while (filenames[i] != null || filenames[i] != "");
            events_list.Close();
            open.Close();
        }

        private void Dla_Daty_Listawydarzen_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = false;
            FileStream datach = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\wybrana_data.txt", FileMode.Open, FileAccess.Read, FileShare.None);
            StreamReader datatach2 = new StreamReader(datach);
            string wybrana_data = datatach2.ReadLine();
            datatach2.Close();
            datach.Close();
            wydarzenie[] s = new wydarzenie[1000];
            ListViewItem lvi;
            label2.Text = current_user_name();
            label3.Text = current_tab_name();
            int i = 0;
            string[] filenames = new string[1000];
            FileStream open = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + current_user_name() + "_" + current_tab_name() + ".txt", FileMode.Open, FileAccess.Read, FileShare.None);
            System.IO.StreamReader events_list = new StreamReader(open);
            do
            {
                filenames[i] = events_list.ReadLine();
                if (filenames[i] == "" || filenames[i] == null) break;
                else if (filenames[i].Contains (wybrana_data))
                {
                    XmlSerializer xizt = new XmlSerializer(typeof(Information));
                    FileStream read = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\" + filenames[i], FileMode.Open, FileAccess.Read, FileShare.Read);
                    Information info = (Information)xizt.Deserialize(read);
                    s[i].nazwa = info.Data1;
                    s[i].data = info.Data2;
                    s[i].lokacja = info.Data3;
                    s[i].szczegoły = info.Data4;
                    if (s[i].data == wybrana_data)
                    {
                        lvi = new ListViewItem(s[i].data);
                        lvi.SubItems.Add(s[i].nazwa);
                        lvi.SubItems.Add(s[i].lokacja);
                        lvi.SubItems.Add(s[i].szczegoły);
                        listView1.Items.Add(lvi);
                    }
                    read.Close();
                    i++;
                }
            } while (filenames[i] != null || filenames[i] != "");
            events_list.Close();
            open.Close();
            label4.Text = wybrana_data;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Dodaj_wydarzenie dodaj = new Dodaj_wydarzenie();
            dodaj.ShowDialog();
            form_reload();
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string filename = label2.Text + "_" + label3.Text + "_" + listView1.SelectedItems[0].Text + "_" + listView1.SelectedItems[0].SubItems[1].Text + ".xml";
            MessageBox.Show(filename);
            FileStream save_event_name = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\edited_event.txt", FileMode.Create, FileAccess.Write, FileShare.None);
            System.IO.StreamWriter write = new StreamWriter(save_event_name);
            write.WriteLine(filename);
            write.Close();
            save_event_name.Close();
            this.Hide();
            Form3 edytuj_event = new Form3();
            edytuj_event.ShowDialog();
            form_reload();
            this.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string filename = label2.Text + "_" + label3.Text + "_" + listView1.SelectedItems[0].Text + "_" + listView1.SelectedItems[0].SubItems[1].Text + ".xml";
            MessageBox.Show(filename);
            FileStream save_event_name = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\edited_event.txt", FileMode.Create, FileAccess.Write, FileShare.None);
            System.IO.StreamWriter write = new StreamWriter(save_event_name);
            write.WriteLine(filename);
            write.Close();
            save_event_name.Close();
            this.Hide();
            event_delete_komunikat kasuj = new event_delete_komunikat();
            kasuj.ShowDialog();
            form_reload();
            this.Show();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count <= 0)
            {
                button2.Enabled = false;
                button3.Enabled = false;
            }
            else
            {
                button2.Enabled = true;
                button3.Enabled = true;
            }
        }
    }
}

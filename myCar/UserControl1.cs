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
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            MainMenu menu = new MainMenu();
            menu.tabControl1.Selected += new TabControlEventHandler(tabControl1_Selected);
        }
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            load_car_data(file_name());
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

        private string file_name()
        {
            string userindex;
            string part1_name = "carlist_user_";
            string part2_name = ".xml";
            XmlSerializer xizt = new XmlSerializer(typeof(Information));
            FileStream read = new FileStream("identifier.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
            Information info = (Information)xizt.Deserialize(read);
            userindex = Convert.ToString(info.Data1);
            read.Close();
            string carlist_index_carname = part1_name + userindex + "_" + current_tab_name() + part2_name;
            return carlist_index_carname;
        }
        private string file_name_on_load(string carname)
        {
            string userindex;
            string part1_name = "carlist_user_";
            string part2_name = ".xml";
            XmlSerializer xizt = new XmlSerializer(typeof(Information));
            FileStream read = new FileStream("identifier.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
            Information info = (Information)xizt.Deserialize(read);
            userindex = Convert.ToString(info.Data1);
            read.Close();
            string carlist_index_carname = part1_name + userindex + "_" + carname + part2_name;
            return carlist_index_carname;
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
        private void load_car_data(string filename)
        {
            XmlSerializer xizt = new XmlSerializer(typeof(Information));
            FileStream read = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            Information info = (Information)xizt.Deserialize(read);
            textBox2.Text = info.Data1;
            textBox3.Text = info.Data2;
            textBox4.Text = info.Data3;
            textBox5.Text = info.Data4;
            textBox6.Text = info.Data5;
            textBox7.Text = info.Data6;
            textBox11.Text = info.Data8;
            comboBox1.Text = info.Data10;
            textBox10.Text = info.Data13;
            dateTimePicker1.Text = info.Data11;
            dateTimePicker2.Text = info.Data12;
            read.Close();
        }
        private void save_car_data(string filename)
        {
            try
            {
                Information info = new Information(); //odwołanie do klasy przypisującej elementy tablicy do pliku xml
                info.Data1 = Convert.ToString(textBox2.Text);
                info.Data2 = Convert.ToString(textBox3.Text);
                info.Data3 = Convert.ToString(textBox4.Text);
                info.Data4 = Convert.ToString(textBox5.Text);
                info.Data5 = Convert.ToString(textBox6.Text);
                info.Data6 = Convert.ToString(textBox7.Text);
                info.Data7 = Convert.ToString(textBox8.Text);
                info.Data8 = Convert.ToString(textBox11.Text);
                info.Data9 = Convert.ToString(textBox13.Text);
                info.Data10 = Convert.ToString(comboBox1.Text);
                info.Data11 = Convert.ToString(dateTimePicker1.Text);
                info.Data12 = Convert.ToString(dateTimePicker2.Text);
                XMLSave.SaveData(info, filename); //odwołanie do klasy zapisującej dane w pliku xml
            }
            catch (Exception ex) //kontrola błedów w trakcie próby zapisu
            {
                MessageBox.Show(ex.Message); //wiadomość wyświetlająca treść błedu
            }
        }

        public void button5_Click(object sender, EventArgs e)
        {
            if (button5.Text == "Edytuj dane samochodu")
            {
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
                textBox5.Enabled = true;
                textBox6.Enabled = true;
                textBox7.Enabled = true;
                textBox11.Enabled = true;
                comboBox1.Enabled = true;
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = true;
                groupBox1.Enabled = false;
                groupBox7.Enabled = false;
                button5.Text = "Zapisz";
            }
            else
            {
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox6.Enabled = false;
                textBox7.Enabled = false;
                textBox11.Enabled = false;
                comboBox1.Enabled = false;
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
                groupBox1.Enabled = true;
                groupBox7.Enabled = true;
                button5.Text = "Edytuj dane samochodu";
                string filename = file_name();
                if (Parent.Name == "<wprowadź nazwę>") MessageBox.Show("Wprowadź nazwę samochodu w menu na dole i potwierdź");
                save_car_data(filename);
                textBox8.Text = time_differance_ubezpieczenie();
                textBox13.Text = time_differance_przeglad();
            }

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void upcoming_events()
        {
            string[] dates = new string[1000];
            int i = 0;
            string[] filenames = new string[1000];
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + current_user_name() + "_" + current_tab_name() + ".txt"))
            {
                FileStream open2 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + current_user_name() + "_" + current_tab_name() + ".txt", FileMode.Open, FileAccess.Read, FileShare.None);
                System.IO.StreamReader events_list2 = new StreamReader(open2);
                do
                {
                    filenames[i] = events_list2.ReadLine();
                    if (filenames[i] == "" || filenames[i] == null) break;
                    else
                    {
                        XmlSerializer xizt = new XmlSerializer(typeof(Information));
                        FileStream read = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\" + filenames[i], FileMode.Open, FileAccess.Read, FileShare.Read);
                        Information info = (Information)xizt.Deserialize(read);
                        dates[i] = info.Data2;
                        read.Close();
                        i++;
                    }
                } while (filenames[i] != null || filenames[i] != "");
                i = 0;
                events_list2.Close();
                open2.Close();
                string date1 = "";
                string date2 = "";
                Array.Sort(dates);
                for (i = 999; i >= 0; i--)
                {
                    if (dates[i] == null || dates[i] == "")
                    {
                        date1 = dates[i+1];
                        date2 = dates[i+2];
                        break;
                    }
                }
                FileStream open3 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + current_user_name() + "_" + current_tab_name() + ".txt", FileMode.Open, FileAccess.Read, FileShare.None);
                System.IO.StreamReader events_list3 = new StreamReader(open3);
                i = 0;
                do
                {
                    filenames[i] = events_list3.ReadLine();
                    if (filenames[i] == "" || filenames[i] == null) break;
                    else
                    {
                        XmlSerializer xizt = new XmlSerializer(typeof(Information));
                        FileStream read = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\" + filenames[i], FileMode.Open, FileAccess.Read, FileShare.Read);
                        Information info = (Information)xizt.Deserialize(read);
                        if (filenames[i].Contains(date1) && info.Data1 != textBox12.Text && textBox14.Text == "")
                        {
                            textBox14.Text = info.Data1;
                            dateTimePicker4.Text = info.Data2;
                            read.Close();
                        }
                        if (filenames[i].Contains(date2) && info.Data1 != textBox14.Text)
                        {
                            textBox12.Text = info.Data1;
                            dateTimePicker5.Text = info.Data2;
                            read.Close();
                        }
                    }
                    i++;
                } while (filenames[i] != null || filenames[i] != "");

                events_list3.Close();
                open3.Close();
            }
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            if(File.Exists(file_name_on_load(Parent.Text))) load_car_data(file_name_on_load(Parent.Text));
            textBox8.Text = time_differance_ubezpieczenie();
            textBox13.Text = time_differance_przeglad();
            dateTimePicker3.Format = DateTimePickerFormat.Custom;
            dateTimePicker3.CustomFormat = "yyyy.MM.dd";
            dateTimePicker4.Format = DateTimePickerFormat.Custom;
            dateTimePicker4.CustomFormat = "yyyy.MM.dd";
            dateTimePicker5.Format = DateTimePickerFormat.Custom;
            dateTimePicker5.CustomFormat = "yyyy.MM.dd";
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + current_user_name() + "_" + Parent.Text + ".txt"))
            {
                DateTime[] highlighted = new DateTime[1000];
                int i = -1;
                string[] filenames = new string[1000];
                FileStream open = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + current_user_name() + "_" + Parent.Text + ".txt", FileMode.Open, FileAccess.Read, FileShare.None);
                System.IO.StreamReader events_list = new StreamReader(open);
                do
                {
                    i++;
                    filenames[i] = events_list.ReadLine();
                } while (filenames[i] != null);
                events_list.Close();
                open.Close();
                i = 0;
                while (filenames[i] != null || filenames[i] != "")
                {
                    if (filenames[i] == null || filenames[i] == "") break;
                    XmlSerializer xizt5 = new XmlSerializer(typeof(Information));
                    FileStream read5 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\" + filenames[i], FileMode.Open, FileAccess.Read, FileShare.Read);
                    Information info5 = (Information)xizt5.Deserialize(read5);
                    highlighted[i] = DateTime.Parse(info5.Data2);
                    read5.Close();
                    i++;
                }
                if (i < 10)
                {
                    label28.Text = "   " + Convert.ToString(i);
                }
                else if (i < 100)
                {
                    label28.Text = "  " + Convert.ToString(i);
                }
                else if (i < 1000)
                {
                    label28.Text = " " + Convert.ToString(i);
                }
                else
                {
                    label28.Text = Convert.ToString(i);
                }
                progressBar1.Value = i;
                monthCalendar1.BoldedDates = highlighted;
            }
        }
        private void usercontrol_reload()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + current_user_name() + "_" + Parent.Text + ".txt"))
            {
                DateTime[] highlighted = new DateTime[1000];
                int i = -1;
                string[] filenames = new string[1000];
                FileStream open = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + current_user_name() + "_" + Parent.Text + ".txt", FileMode.Open, FileAccess.Read, FileShare.None);
                System.IO.StreamReader events_list = new StreamReader(open);
                do
                {
                    i++;
                    filenames[i] = events_list.ReadLine();
                } while (filenames[i] != null);
                events_list.Close();
                open.Close();
                i = 0;
                while (filenames[i] != null)
                {
                    if (filenames[i] == null || filenames[i] == "") break; ;
                    XmlSerializer xizt5 = new XmlSerializer(typeof(Information));
                    FileStream read5 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\" + filenames[i], FileMode.Open, FileAccess.Read, FileShare.Read);
                    Information info5 = (Information)xizt5.Deserialize(read5);
                    highlighted[i] = DateTime.Parse(info5.Data2);
                    read5.Close();
                    i++;
                }
                if (i < 10)
                {
                    label28.Text = "   " + Convert.ToString(i);
                }
                else if (i < 100)
                {
                    label28.Text = "  " + Convert.ToString(i);
                }
                else if (i < 1000)
                {
                    label28.Text = " " + Convert.ToString(i);
                }
                else
                {
                    label28.Text = Convert.ToString(i);
                }
                progressBar1.Value = i;
                monthCalendar1.BoldedDates = highlighted;
            }
        }
        private string time_differance_ubezpieczenie()
        {
            DateTime validate = dateTimePicker1.Value;
            DateTime today = DateTime.Today;
            TimeSpan difference = validate - today;
            return difference.Days.ToString();
        }
        private string time_differance_przeglad()
        {
            DateTime validate = dateTimePicker2.Value;
            DateTime today = DateTime.Today;
            TimeSpan difference = validate - today;
            return difference.Days.ToString();
        }
        private string event_file_name()
        {
            string userindex;
            string part1_name = "carlist_user_";
            string part2_name = ".xml";
            XmlSerializer xizt = new XmlSerializer(typeof(Information));
            FileStream read = new FileStream("identifier.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
            Information info = (Information)xizt.Deserialize(read);
            userindex = Convert.ToString(info.Data1);
            read.Close();
            string event_name = part1_name + userindex + "_" + current_tab_name() + "_" + textBox1.Text + "_"+ dateTimePicker3.Text + part2_name;
            if (File.Exists(event_name)) return "000000001";
            else return event_name;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //AppDomain.CurrentDomain.BaseDirectory
            //txt -> tablica -> dodaj filename do tablicy -> tablica ->txt
            string filename;
            filename = current_user_name() + "_"+ current_tab_name() + "_" + dateTimePicker3.Text + "_" + textBox1.Text + ".xml";
            if (textBox1.Text.EndsWith(" ") || textBox1.Text.StartsWith(" "))
            {
                MessageBox.Show("Opis skrócony nie może zawierać spacji na początku lub końcu");
                textBox1.Clear();
            }
            else if (textBox1.Text == null || textBox1.Text == "")
            {
                MessageBox.Show("Wprowadź nazwe!");
                textBox1.Clear();
            }
            else if ((textBox1.Text.All(Char.IsLetterOrDigit)))
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + current_user_name() + "_" + current_tab_name() + ".txt")) //lista wydarzen istnieje
                {
                    int i = -1;
                    string[] filenames = new string[1000];
                    FileStream open = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + current_user_name()+"_"+ current_tab_name() + ".txt", FileMode.Open, FileAccess.Read, FileShare.None);
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
                        else //dodanie wydarzenia do listy po sprawdzeniu czy nie istnieje juz wydarzenie o podanej nazwie
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
                                    Information info = new Information(); 
                                    info.Data1 = Convert.ToString(textBox1.Text);
                                    info.Data2 = Convert.ToString(dateTimePicker3.Text);
                                    info.Data3 = Convert.ToString(textBox9.Text);
                                    info.Data4 = Convert.ToString(richTextBox1.Text);
                                    XMLSave.SaveData(info, AppDomain.CurrentDomain.BaseDirectory + @"\events\" + filename); 
                                }
                                catch (Exception ex) 
                                {
                                    MessageBox.Show(ex.Message); 
                                }
                                usercontrol_reload();
                                textBox1.Clear();
                                textBox9.Clear();
                                richTextBox1.Clear();
                                break;
                            }
                        }
                        i++;
                    } while (i <= 999);

                }
                else //lista wydarzen nei istnieje, plik listy wydarzen zostaje utworzony a wydarzenie dodane
                {
                    FileStream open2 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + current_user_name() + "_" + current_tab_name() + ".txt", FileMode.Create, FileAccess.Write, FileShare.None);
                    System.IO.StreamWriter events_list2 = new StreamWriter(open2);
                    events_list2.Write(filename);
                    try
                    {
                        Information info = new Information(); //odwołanie do klasy przypisującej elementy tablicy do pliku xml
                        info.Data1 = Convert.ToString(textBox1.Text);
                        info.Data2 = Convert.ToString(dateTimePicker3.Text);
                        info.Data3 = Convert.ToString(textBox9.Text);
                        info.Data4 = Convert.ToString(richTextBox1.Text);
                        XMLSave.SaveData(info, AppDomain.CurrentDomain.BaseDirectory + @"\events\" + filename); //odwołanie do klasy zapisującej dane w pliku xml
                    }
                    catch (Exception ex) //kontrola błedów w trakcie próby zapisu
                    {
                        MessageBox.Show(ex.Message); //wiadomość wyświetlająca treść błedu
                    }
                    usercontrol_reload();
                    events_list2.Close();
                    open2.Close();
                    textBox1.Clear();
                    textBox9.Clear();
                    richTextBox1.Clear();
                }
            }
            else { MessageBox.Show("Nazwa zawiera niedozwolone znaki"); textBox1.Clear();}
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true) textBox16.Enabled = true;
            else textBox16.Enabled = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true) textBox17.Enabled = true;
            else textBox17.Enabled = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true) textBox18.Enabled = true;
            else textBox18.Enabled = false;
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox16_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox17_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox18_TextChanged(object sender, EventArgs e)
        {
        }
        private void znam_dystans()
        {
            double spalanie = Convert.ToDouble(textBox10.Text);
            double cena_paliwa = Convert.ToDouble(textBox15.Text);
            double dystans = Convert.ToDouble(textBox16.Text);
            double koszt = (dystans / 100) * spalanie * cena_paliwa;
            textBox17.Text = Convert.ToString(koszt);
            double ilość = (dystans / 100 * spalanie);
            textBox18.Text = Convert.ToString(ilość);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked == true) znam_dystans();
                if (radioButton2.Checked == true) znam_koszt();
                if (radioButton3.Checked == true) znam_ilość();
                try
                {
                    Information info = new Information(); //odwołanie do klasy przypisującej elementy tablicy do pliku xml
                    info.Data13 = Convert.ToString(textBox10.Text);
                    XMLSave.SaveData(info, file_name()); //odwołanie do klasy zapisującej dane w pliku xml
                }
                catch (Exception ex) //kontrola błedów w trakcie próby zapisu
                {
                    MessageBox.Show(ex.Message); //wiadomość wyświetlająca treść błedu
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR");
            }
        }
        private void znam_koszt()
        {
            double spalanie = Convert.ToDouble(textBox10.Text);
            double cena_paliwa = Convert.ToDouble(textBox15.Text);
            double koszt = Convert.ToDouble(textBox17.Text);
            double dystans = koszt / cena_paliwa * 100 / spalanie;
            textBox16.Text = Convert.ToString(dystans);
            double ilość = (dystans / 100 * spalanie);
            textBox18.Text = Convert.ToString(ilość);
        }
        private void znam_ilość()
        {
            double ilość = Convert.ToDouble(textBox18.Text);
            double spalanie = Convert.ToDouble(textBox10.Text);
            double cena_paliwa = Convert.ToDouble(textBox15.Text);
            double dystans = ilość * 100 / spalanie;
            double koszt = (dystans / 100) * spalanie * cena_paliwa;
            textBox16.Text = Convert.ToString(dystans);
            textBox17.Text = Convert.ToString(koszt);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + current_user_name() + "_" + current_tab_name() + ".txt"))
            {
                ListaWydarzeń listawydarze = new ListaWydarzeń();
                listawydarze.ShowDialog();
                usercontrol_reload();
            }
            else MessageBox.Show("Brak wydarzeń do wyświetlenia. Dodaj wydarzenie");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + current_user_name() + "_" + current_tab_name() + ".txt"))
            {
                DateTime selected;
                selected = monthCalendar1.SelectionRange.Start;
                string wybrana_data2 = selected.ToString("yyyy.MM.dd");
                //string wybrana_data = monthCalendar1.SelectionRange.Start.ToShortDateString();
                FileStream data = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\wybrana_data.txt", FileMode.Create, FileAccess.Write, FileShare.None);
                StreamWriter data2 = new StreamWriter(data);
                data2.WriteLine(wybrana_data2);
                data2.Close();
                data.Close();
                Dla_Daty_Listawydarzen show_for_date = new Dla_Daty_Listawydarzen();
                show_for_date.ShowDialog();
                usercontrol_reload();
            }
            else MessageBox.Show("Brak wydarzeń do wyświetlenia. Dodaj wydarzenie");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string filename = current_user_name() + "_" + current_tab_name() + "_" + dateTimePicker4.Text + "_" + textBox14.Text + ".xml";
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\events\" + filename))
            {
                XmlSerializer xizt = new XmlSerializer(typeof(Information));
                FileStream read = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\" + filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                Information info = (Information)xizt.Deserialize(read);
                string line1 = info.Data1;
                string line2 = info.Data2;
                string line3 = info.Data3;
                string line4 = info.Data4;
                read.Close();
                MessageBox.Show(line1 + Environment.NewLine + line2 + Environment.NewLine + line3 + Environment.NewLine + line4);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string filename = current_user_name() + "_" + current_tab_name() + "_" + dateTimePicker5.Text + "_" + textBox12.Text + ".xml";
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\events\" + filename))
            {
                XmlSerializer xizt = new XmlSerializer(typeof(Information));
                FileStream read = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\" + filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                Information info = (Information)xizt.Deserialize(read);
                string line1 = info.Data1;
                string line2 = info.Data2;
                string line3 = info.Data3;
                string line4 = info.Data4;
                read.Close();
                MessageBox.Show(line1 + Environment.NewLine + line2 + Environment.NewLine + line3 + Environment.NewLine + line4);
            }
        }
    }
}

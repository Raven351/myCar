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
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
            tabControl1.Selected += new TabControlEventHandler(tabControl1_Selected);
        }



        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            string current_tab = tabControl1.SelectedTab.Text;
            try
            {
                Information info = new Information(); //odwołanie do klasy przypisującej elementy tablicy do pliku xml
                info.Data1 = current_tab;
                XMLSave.SaveData(info, "identifier_current_tab.xml"); //odwołanie do klasy zapisującej dane w pliku xml
            }
            catch (Exception ex) //kontrola błedów w trakcie próby zapisu
            {
                MessageBox.Show(ex.Message); //wiadomość wyświetlająca treść błedu
            }

        }
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tabControl1.TabCount == 1)
            {
                car_delete_files(textBox14.Text, tabControl1.SelectedTab.Text);
                tabPage1.Name = "<wprowadź nazwe>";
                File.Delete(file_name_on_load(tabControl1.SelectedTab.Text));
                File.Delete("carlist_user_" + textBox14.Text + ".xml");
                this.Close();
            }
            else
            {
                car_delete_files(textBox14.Text, tabControl1.SelectedTab.Text);
                carname_delete(tabControl1.SelectedIndex);
                File.Delete(file_name_on_load(tabControl1.SelectedTab.Text));
            }
            mainmenu_reload();

        }

        private void car_delete_files(string username, string carname)
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
                }
                read.Close();
                read_file.Close();
                for (int i = 0; i <= 999; i++)
                {
                    if (events_filenames[i] == null || events_filenames[i] == "") break;
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\events\" + events_filenames[i]);
                }
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + username + "_" + carname + ".txt");
            }
        }

        private string carlist_x()
        {
            string[] data = new string[10];
            string userindex;
            string part1_name = "carlist_user_";
            string part2_name = ".xml";
            XmlSerializer xizt = new XmlSerializer(typeof(Information));
            FileStream read = new FileStream("identifier.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
            Information info = (Information)xizt.Deserialize(read);
            userindex = Convert.ToString(info.Data1);
            read.Close();
            string carlist_x = part1_name + userindex + part2_name;
            return carlist_x;
        }

        private void upcoming_events()
        {
            string[] dates = new string[1000];
            int i = 0;
            string[] filenames = new string[1000];
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + textBox14.Text + "_" + tabPage1.Text + ".txt"))
            {
                FileStream open2 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + textBox14.Text + "_" + tabPage1.Text + ".txt", FileMode.Open, FileAccess.Read, FileShare.None);
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
                        date1 = dates[i + 1];
                        date2 = dates[i + 2];
                        break;
                    }
                }
                FileStream open3 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + textBox14.Text + "_" + tabPage1.Text + ".txt", FileMode.Open, FileAccess.Read, FileShare.None);
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
                            textBox2.Text = info.Data1;
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

        private void MainMenu_Load(object sender, EventArgs e)
        {
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            dateTimePicker3.Format = DateTimePickerFormat.Custom;
            dateTimePicker3.CustomFormat = "yyyy.MM.dd";
            dateTimePicker4.Format = DateTimePickerFormat.Custom;
            dateTimePicker4.CustomFormat = "yyyy.MM.dd";
            dateTimePicker5.Format = DateTimePickerFormat.Custom;
            dateTimePicker5.CustomFormat = "yyyy.MM.dd";
            string[] data = new string[10];
            string userindex;
            string part1_name = "carlist_user_";
            string part2_name = ".xml";
            XmlSerializer xizt = new XmlSerializer(typeof(Information));
            FileStream read = new FileStream("identifier.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
            Information info = (Information)xizt.Deserialize(read);
            userindex = Convert.ToString(info.Data1);
            textBox14.Text = Convert.ToString(info.Data2);
            read.Close();
            string carlist_x = part1_name + userindex + part2_name;
            if (File.Exists(carlist_x))
            {
                if (carlist_tab(0) != null)
                {
                    tabPage1.Text = carlist_tab(0);
                }
                else MessageBox.Show("Wprowadź w menu na dole nazwe samochodu i zatwiedź");
                for (int i = 0; i <= 5; i++)
                {
                    if (carlist_tab(i) != null)
                    {
                        if (i == 0)
                        {
                            string tab_title = carlist_tab(i);
                            tabPage1.Text = carlist_tab(i);  // ============WAZNE============
                            if (File.Exists(file_name_on_load(tabPage1.Text))) load_car_data(file_name_on_load(tabPage1.Text));
                            {

                                textBox19.Text = time_differance_ubezpieczenie();
                                textBox13.Text = time_differance_przeglad();
                            }
                        }
                        else
                        {
                            string tab_title = carlist_tab(i);
                            AddTabOnLoad(tab_title);
                            //tabControl1.SelectedTab = new TabPage(carlist_tab(i-1));
                            //tabControl1.TabPages.Add(tab_title);
                        }

                    }
                    else break;

                }

                if (carlist_tab(0) == null || carlist_tab(0) == "")
                {

                }
            }
            else
            {
                MessageBox.Show("Brak zapamiętanych samochodów. Wprowadź nazwe samochodu w menu na dole.");
                groupBox9.Enabled = false;
                button5.Enabled = false;
                groupBox2.Enabled = false;
                groupBox7.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
            }
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + textBox14.Text + "_" + tabPage1.Text + ".txt"))
            {
                DateTime[] highlighted = new DateTime[1000];
                int i = -1;
                string[] filenames = new string[1000];
                FileStream open = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + textBox14.Text + "_" + tabPage1.Text + ".txt", FileMode.Open, FileAccess.Read, FileShare.None);
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
            string current_tab = tabControl1.SelectedTab.Text;
            try
            {
                Information info555 = new Information(); //odwołanie do klasy przypisującej elementy tablicy do pliku xml
                info555.Data1 = current_tab;
                XMLSave.SaveData(info555, "identifier_current_tab.xml"); //odwołanie do klasy zapisującej dane w pliku xml
            }
            catch (Exception ex) //kontrola błedów w trakcie próby zapisu
            {
                MessageBox.Show(ex.Message); //wiadomość wyświetlająca treść błedu
            }

        }

        private void mainmenu_reload()
        {
            {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + textBox14.Text + "_" + tabPage1.Text + ".txt"))
            {
                string[] data = new string[10];
                string userindex;
                string part1_name = "carlist_user_";
                string part2_name = ".xml";
                XmlSerializer xizt = new XmlSerializer(typeof(Information));
                FileStream read = new FileStream("identifier.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
                Information info = (Information)xizt.Deserialize(read);
                userindex = Convert.ToString(info.Data1);
                textBox14.Text = Convert.ToString(info.Data2);
                read.Close();
                string carlist_x = part1_name + userindex + part2_name;
                if (File.Exists(carlist_x))
                {
                    if (carlist_tab(0) != null)
                    {
                        tabPage1.Text = carlist_tab(0);
                    }
                    else MessageBox.Show("Wprowadź w menu na dole nazwe samochodu i zatwiedź");
                    for (int j = 0; j <= 5; j++)
                    {
                        if (carlist_tab(j) != null)
                        {
                            if (j == 0)
                            {
                                string tab_title = carlist_tab(j);
                                tabPage1.Text = carlist_tab(j);  // ============WAZNE============
                                if (File.Exists(file_name_on_load(tabPage1.Text))) load_car_data(file_name_on_load(tabPage1.Text));
                                {

                                    textBox19.Text = time_differance_ubezpieczenie();
                                    textBox13.Text = time_differance_przeglad();
                                }
                            }
                            else
                            {
                                string tab_title = carlist_tab(j);
                                AddTabOnLoad(tab_title);
                                //tabControl1.SelectedTab = new TabPage(carlist_tab(i-1));
                                //tabControl1.TabPages.Add(tab_title);
                            }

                        }
                        else break;

                    }

                    if (carlist_tab(0) == null || carlist_tab(0) == "")
                    {

                    }
                }
                else
                {
                        MessageBox.Show("Brak zapamiętanych samochodów. Wprowadź nazwe samochodu w menu na dole.");
                        tabPage1.Text = "<wprowadz nazwe>";
                        groupBox9.Enabled = false;
                        button5.Enabled = false;
                        groupBox2.Enabled = false;
                        groupBox7.Enabled = false;
                        button2.Enabled = false;
                        button3.Enabled = false;
                    }
            }
            // ====
            DateTime[] highlighted = new DateTime[1000];
                int i = 0;
            string[] filenames = new string[1000];
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + textBox14.Text + "_" + tabPage1.Text + ".txt"))
                {
                    i = -1;
                    FileStream open = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + textBox14.Text + "_" + tabPage1.Text + ".txt", FileMode.Open, FileAccess.Read, FileShare.None);
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

    public static string carlist_tab(int n) // FINISHED 
    {
        string userindex;
        string part1_name = "carlist_user_";
        string part2_name = ".xml";
        XmlSerializer xizt = new XmlSerializer(typeof(Information));
        FileStream read = new FileStream("identifier.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
        Information info = (Information)xizt.Deserialize(read);
        userindex = Convert.ToString(info.Data1);
        read.Close();
        string carlist_x = part1_name + userindex + part2_name;
        if (File.Exists(carlist_x)) //sprawdzenie czy plik istnienie
        {
            string[] cars = new string[10];
            XmlSerializer xizt2 = new XmlSerializer(typeof(Information));
            FileStream read2 = new FileStream(carlist_x, FileMode.Open, FileAccess.Read, FileShare.Read);
            Information info2 = (Information)xizt.Deserialize(read2);
            cars[0] = Convert.ToString(info2.Data1);
            cars[1] = Convert.ToString(info2.Data2);
            cars[2] = Convert.ToString(info2.Data3);
            cars[3] = Convert.ToString(info2.Data4);
            cars[4] = Convert.ToString(info2.Data5);
            cars[5] = Convert.ToString(info2.Data6);
            cars[6] = Convert.ToString(info2.Data7);
            cars[7] = Convert.ToString(info2.Data8);
            cars[8] = Convert.ToString(info2.Data9);
            cars[9] = Convert.ToString(info2.Data10);
            read2.Close();
            return cars[n];
        }
        else return null;
    }

    private void button3_Click(object sender, EventArgs e)
    {
        if (carname_check(textBox1.Text) == 0)
        {
            MessageBox.Show("Wprowadź nazwe samochodu");
        }
        else if (carname_check(textBox1.Text) == 1)
        {
            MessageBox.Show("Nie można wprowadzić samochodu o tej samej nazwie");
        }
        else if (carname_check(textBox1.Text) == 2)
        {
                if ((textBox1.Text.All(Char.IsLetterOrDigit)))
                {
                    string current_carname = tabControl1.SelectedTab.Text.ToString();
                    string new_name = textBox1.Text;
                    new_carname_events_filename_change(current_carname, new_name, textBox14.Text);
                    tabControl1.SelectedTab.Text = textBox1.Text.ToString();
                    carname_edit(current_carname, textBox1.Text);
                    if (File.Exists(file_name_on_load(current_carname)))System.IO.File.Move(file_name_on_load(current_carname), file_name_on_load(new_name));
                }
                else { MessageBox.Show("Nazwa zawiera niedozwolone znaki"); textBox1.Clear(); }
            }

        else if ((carname_check(textBox1.Text) == 3)) MessageBox.Show("BŁĄD");
        textBox1.Clear();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        string userindex;
        string part1_name = "carlist_user_";
        string part2_name = ".xml";
        XmlSerializer xizt = new XmlSerializer(typeof(Information));
        FileStream read = new FileStream("identifier.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
        Information info = (Information)xizt.Deserialize(read);
        userindex = Convert.ToString(info.Data1);
        textBox14.Text = Convert.ToString(info.Data2);
        read.Close();
        string carlist_x = part1_name + userindex + part2_name;
        if (File.Exists(carlist_x))
        {
            if (carname_check(textBox1.Text) == 0)
            {
                MessageBox.Show("Wprowadź nazwe samochodu");
            }
            else if (carname_check(textBox1.Text) == 1)
            {
                MessageBox.Show("Nie można wprowadzić samochodu o tej samej nazwie");
            }
            else if (carname_check(textBox1.Text) == 2)
            {
                if (textBox1.Text.EndsWith(" ") || textBox1.Text.StartsWith(" "))
                {
                    MessageBox.Show("Nazwa nie może zawierać spacji na początku lub końcu");
                    textBox1.Clear();
                }
                else if ((textBox1.Text.All(Char.IsLetterOrDigit)))
                    {
                    AddTab(textBox1.Text);
                    carname_save_to_file(textBox1.Text);
                }
                else { MessageBox.Show("Nazwa zawiera niedozwolone znaki"); textBox1.Clear(); }
            }
            else if ((carname_check(textBox1.Text) == 3)) MessageBox.Show("BŁĄD");
            textBox1.Clear();
        }
        else
        {
                if (textBox1.Text == "" || textBox1.Text == null)
                {
                    MessageBox.Show("Wprowadź nazwe!");
                }
            else if (textBox1.Text.EndsWith(" ") || textBox1.Text.StartsWith(" "))
            {
                MessageBox.Show("Nazwa nie może zawierać spacji na początku lub końcu");
                textBox1.Clear();
            }
            else if (((textBox1.Text.All(Char.IsLetterOrDigit))))
            {
                tabPage1.Text = textBox1.Text;
                groupBox9.Enabled = true;
                button5.Enabled = true;
                groupBox2.Enabled = true;
                groupBox7.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                try
                {
                    Information info2 = new Information(); //odwołanie do klasy przypisującej elementy tablicy do pliku xml
                    info2.Data1 = textBox1.Text.ToString();
                    XMLSave.SaveData(info2, carlist_x); //odwołanie do klasy zapisującej dane w pliku xml
                }
                catch (Exception ex) //kontrola błedów w trakcie próby zapisu
                {
                    MessageBox.Show(ex.Message); //wiadomość wyświetlająca treść błedu
                }
                textBox1.Clear();
            }
            else { MessageBox.Show("Nazwa zawiera niedozwolone znaki"); textBox1.Clear(); }
            }
    }
    private void button1_click_old()
    {
        if (carname_check(textBox1.Text) == 0)
        {
            MessageBox.Show("Wprowadź nazwe samochodu");
        }
        else if (carname_check(textBox1.Text) == 1)
        {
            MessageBox.Show("Nie można wprowadzić samochodu o tej samej nazwie");
        }
        else if (carname_check(textBox1.Text) == 2)
        {
            AddTab(textBox1.Text);
            carname_save_to_file(textBox1.Text);
        }
        else if ((carname_check(textBox1.Text) == 3)) MessageBox.Show("BŁĄD");
        textBox1.Clear();
    }
    private int carname_check(string carname_to_check)
    {
        int i = 0;
        do
        {
            if (textBox1.Text == "" || textBox1.Text == null)
            {
                return 0;
                break;
            }
            else if (carlist_tab(i) == textBox1.Text)
            {
                return 1;
                break;
            }
            else if (tabPage1.Text == "< wprowadź nazwe >")
            {
                return 4;
                break;
            }
            else if (i == 9)
            {
                return 2;
                break;
            }
            i++;
        } while (i <= 9);
        return 3;
    }
    private void AddTab(string tab_name) //works
    {
        UserControl1 uc = new UserControl1();
        uc.Dock = DockStyle.Fill;
        TabPage tbp = new TabPage(tab_name);
        uc.Name = "uc" + tbp.Name.ToString();
        tbp.Controls.Add(uc);
        tabControl1.TabPages.Add(tbp);
    }
    private void AddTabOnLoad(string tab_name) // works
    {
        UserControl1 uc = new UserControl1();
        TabPage tbp = new TabPage(tab_name);
        tbp.Text = tab_name;
        uc.Dock = DockStyle.Fill;
        uc.Name = "uc" + tbp.Name.ToString();
        tbp.Controls.Add(uc);
        tabControl1.TabPages.Add(tbp);
    }
    private void carname_save_to_file(string carname)
    {
        string[] cars = new string[10];
        if (carname != "")
        {
            int i;
            for (i = 0; i <= 9; i++)
            {
                cars[i] = carlist_tab(i); //wczytanie do tablicy nazw z pliku
            }
            i = 0;
            do
            {
                if (cars[i] == carname) // sprawdzenie czy dana nazwa już istnieje
                {
                    MessageBox.Show("Użytkownik o podanej nazwie już istnieje");
                    break;
                }
                else
                {
                    if (cars[i] == "" || cars[i] == null) // przypisanie nazwy do tablicy na najbliższym wolnym miejscu
                    {
                        cars[i] = carname;
                        break;
                    }
                }
                i++;
            } while (i <= 5);
            string userindex;
            string part1_name = "carlist_user_";
            string part2_name = ".xml";
            XmlSerializer xizt = new XmlSerializer(typeof(Information));
            FileStream read = new FileStream("identifier.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
            Information info = (Information)xizt.Deserialize(read);
            userindex = Convert.ToString(info.Data1);
            read.Close();
            string carlist_x = part1_name + userindex + part2_name;
            try
            {
                Information info2 = new Information(); //odwołanie do klasy przypisującej elementy tablicy do pliku xml
                info2.Data1 = cars[0];
                info2.Data2 = cars[1];
                info2.Data3 = cars[2];
                info2.Data4 = cars[3];
                info2.Data5 = cars[4];
                info2.Data6 = cars[5];
                info2.Data7 = cars[6];
                info2.Data8 = cars[7];
                info2.Data9 = cars[8];
                info2.Data10 = cars[9];
                XMLSave.SaveData(info2, carlist_x); //odwołanie do klasy zapisującej dane w pliku xml
            }
            catch (Exception ex) //kontrola błedów w trakcie próby zapisu
            {
                MessageBox.Show(ex.Message); //wiadomość wyświetlająca treść błedu
            }
        }
    } 
        private void carname_edit(string current_carname, string new_carname)
        {
            string[] cars = new string[10];
            if (new_carname != "")
            {
                int i;
                for (i = 0; i <= 9; i++)
                {
                    cars[i] = carlist_tab(i); //wczytanie do tablicy nazw z pliku
                }
                i = 0;
                do
                {
                    if (cars[i] == new_carname) // sprawdzenie czy dana nazwa już istnieje
                    {
                        MessageBox.Show("Użytkownik o podanej nazwie już istnieje");
                        break;
                    }
                    else
                    {
                        if (cars[i] == current_carname) // przypisanie nazwy do tablicy na najbliższym wolnym miejscu
                        {
                            cars[i] = new_carname;
                            break;
                        }
                    }
                    i++;
                } while (i <= 5);
                string userindex;
                string part1_name = "carlist_user_";
                string part2_name = ".xml";
                XmlSerializer xizt = new XmlSerializer(typeof(Information));
                FileStream read = new FileStream("identifier.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
                Information info = (Information)xizt.Deserialize(read);
                userindex = Convert.ToString(info.Data1);
                read.Close();
                string carlist_x = part1_name + userindex + part2_name;
                try
                {
                    Information info2 = new Information(); //odwołanie do klasy przypisującej elementy tablicy do pliku xml
                    info2.Data1 = cars[0];
                    info2.Data2 = cars[1];
                    info2.Data3 = cars[2];
                    info2.Data4 = cars[3];
                    info2.Data5 = cars[4];
                    info2.Data6 = cars[5];
                    info2.Data7 = cars[6];
                    info2.Data8 = cars[7];
                    info2.Data9 = cars[8];
                    info2.Data10 = cars[9];
                    XMLSave.SaveData(info2, carlist_x); //odwołanie do klasy zapisującej dane w pliku xml
                }
                catch (Exception ex) //kontrola błedów w trakcie próby zapisu
                {
                    MessageBox.Show(ex.Message); //wiadomość wyświetlająca treść błedu
                }
            }
        }
        private void carname_delete(int tabindex)
        {
            tabControl1.TabPages.Remove(tabControl1.SelectedTab);
            string[] cars = new string[10];
            int i;
            for (i = 0; i <= 9; i++)
            {
                cars[i] = carlist_tab(i); //wczytanie do tablicy nazw z pliku
            }
            for (i = tabindex; i <= 8; i++)
            {
                cars[i] = cars[i + 1];
            }
            string filename = carlist_filename();
            try
            {
                Information info2 = new Information(); //odwołanie do klasy przypisującej elementy tablicy do pliku xml
                info2.Data1 = cars[0];
                info2.Data2 = cars[1];
                info2.Data3 = cars[2];
                info2.Data4 = cars[3];
                info2.Data5 = cars[4];
                info2.Data6 = cars[5];
                info2.Data7 = cars[6];
                info2.Data8 = cars[7];
                info2.Data9 = cars[8];
                info2.Data10 = cars[9];
                XMLSave.SaveData(info2, filename); //odwołanie do klasy zapisującej dane w pliku xml
            }
            catch (Exception ex) //kontrola błedów w trakcie próby zapisu
            {
                MessageBox.Show(ex.Message); //wiadomość wyświetlająca treść błedu
            }
        }
        private string carlist_filename()
        {
            string userindex;
            string part1_name = "carlist_user_";
            string part2_name = ".xml";
            XmlSerializer xizt = new XmlSerializer(typeof(Information));
            FileStream read = new FileStream("identifier.xml", FileMode.Open, FileAccess.Read, FileShare.Read);
            Information info = (Information)xizt.Deserialize(read);
            userindex = Convert.ToString(info.Data1);
            read.Close();
            string carlist_x = part1_name + userindex + part2_name;
            return carlist_x;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (button5.Text == "Edytuj dane samochodu")
            {
                textBox8.Enabled = true;
                textBox4.Enabled = true;
                textBox5.Enabled = true;
                textBox6.Enabled = true;
                textBox7.Enabled = true;
                textBox11.Enabled = true;
                textBox20.Enabled = true;
                comboBox1.Enabled = true;
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = true;
                groupBox1.Enabled = false;
                groupBox7.Enabled = false;
                button5.Text = "Zapisz";
            }
            else
            {
                textBox8.Enabled = false;
                textBox4.Enabled = false;
                textBox5.Enabled = false;
                textBox6.Enabled = false;
                textBox7.Enabled = false;
                textBox11.Enabled = false;
                textBox20.Enabled = false;
                comboBox1.Enabled = false;
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
                groupBox1.Enabled = true;
                groupBox7.Enabled = true;
                button5.Text = "Edytuj dane samochodu";
                string filename = file_name();
                save_car_data(filename);
                textBox19.Text = time_differance_ubezpieczenie();
                textBox13.Text = time_differance_przeglad();
            }
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
            string carlist_index_carname = part1_name + userindex + "_" + tabPage1.Text + part2_name;
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
        private void load_car_data(string filename)
        {
            XmlSerializer xizt = new XmlSerializer(typeof(Information));
            FileStream read = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            Information info = (Information)xizt.Deserialize(read);
            textBox8.Text = info.Data1;
            textBox7.Text = info.Data2;
            textBox4.Text = info.Data3;
            textBox6.Text = info.Data4;
            textBox5.Text = info.Data5;
            textBox20.Text = info.Data6;
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
                info.Data1 = Convert.ToString(textBox8.Text);
                info.Data2 = Convert.ToString(textBox7.Text);
                info.Data3 = Convert.ToString(textBox4.Text);
                info.Data4 = Convert.ToString(textBox6.Text);
                info.Data5 = Convert.ToString(textBox5.Text);
                info.Data6 = Convert.ToString(textBox20.Text);
                info.Data8 = Convert.ToString(textBox11.Text);
                info.Data9 = Convert.ToString(textBox10.Text);
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
        public string carname
        {
            //not used
            get { return tabControl1.SelectedTab.Text; }
            set { tabControl1.SelectedTab.Text = value; }
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

        private void button10_Click(object sender, EventArgs e)
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

        private void autorzyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            {
                Form2 autorzy = new Form2();
                autorzy.ShowDialog();
            }
        }

        private void zmieńUżytkownikaToolStripMenuItem_Click(object sender, EventArgs e)
        {
                DialogResult result = MessageBox.Show("Czy na pewno chcesz wyjść z programu?", "Zamykanie aplikacji", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    Environment.Exit(0);
                }
                else
                {

                }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string filename;
            filename = textBox14.Text + "_" + tabPage1.Text + "_" + dateTimePicker3.Text + "_" + textBox3.Text + ".xml";
            if (textBox1.Text.EndsWith(" ") || textBox1.Text.StartsWith(" "))
            {
                MessageBox.Show("Opis skrócony nie może zawierać spacji na początku lub końcu");
                textBox1.Clear();
            }
            else if (textBox3.Text == null || textBox3.Text == "")
            {
                MessageBox.Show("Wprowadź nazwe!");
                textBox1.Clear();
            }
            else if (((textBox3.Text.All(Char.IsLetterOrDigit))))
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + textBox14.Text + "_" + tabPage1.Text + ".txt"))
                {
                    int i = -1;
                    string[] filenames = new string[1000];
                    FileStream open = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + textBox14.Text + "_" + tabPage1.Text + ".txt", FileMode.Open, FileAccess.Read, FileShare.None);
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
                                FileStream open2 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + textBox14.Text + "_" + tabPage1.Text + ".txt", FileMode.Open, FileAccess.Write, FileShare.None);
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
                                mainmenu_reload();
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
                    FileStream open2 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + textBox14.Text + "_" + tabPage1.Text + ".txt", FileMode.Create, FileAccess.Write, FileShare.None);
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
                    mainmenu_reload();
                    textBox3.Clear();
                    textBox9.Clear();
                    richTextBox1.Clear();
                }
            }
            else { MessageBox.Show("Nazwa zawiera niedozwolone znaki"); textBox3.Clear(); }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string current_tab = tabControl1.SelectedTab.Text;
            try
            {
                Information info = new Information(); //odwołanie do klasy przypisującej elementy tablicy do pliku xml
                info.Data1 = current_tab;
                XMLSave.SaveData(info, "identifier_current_tab.xml"); //odwołanie do klasy zapisującej dane w pliku xml
            }
            catch (Exception ex) //kontrola błedów w trakcie próby zapisu
            {
                MessageBox.Show(ex.Message); //wiadomość wyświetlająca treść błedu
            }
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + textBox14.Text + "_" + tabPage1.Text + ".txt"))
            {
                ListaWydarzeń listawydarze = new ListaWydarzeń();
                listawydarze.ShowDialog();
                mainmenu_reload();
            }
            else MessageBox.Show("Brak wydarzeń do wyświetlenia. Dodaj wydarzenie");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string current_tab = tabControl1.SelectedTab.Text;
            try
            {
                Information info = new Information(); //odwołanie do klasy przypisującej elementy tablicy do pliku xml
                info.Data1 = current_tab;
                XMLSave.SaveData(info, "identifier_current_tab.xml"); //odwołanie do klasy zapisującej dane w pliku xml
            }
            catch (Exception ex) //kontrola błedów w trakcie próby zapisu
            {
                MessageBox.Show(ex.Message); //wiadomość wyświetlająca treść błedu
            }
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + textBox14.Text + "_" + tabPage1.Text + ".txt"))
            {
                DateTime selected;
                selected = monthCalendar1.SelectionRange.Start;
                string wybrana_data2 = selected.ToString("yyyy.MM.dd");
                FileStream data = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\wybrana_data.txt", FileMode.Create, FileAccess.Write, FileShare.None);
                StreamWriter data2 = new StreamWriter(data);
                data2.WriteLine(wybrana_data2);
                data2.Close();
                data.Close();
                Dla_Daty_Listawydarzen show_for_date = new Dla_Daty_Listawydarzen();
                show_for_date.ShowDialog();
                mainmenu_reload();
                
            }
            else MessageBox.Show("Brak wydarzeń do wyświetlenia. Dodaj wydarzenie");
        }

        private void MainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void new_carname_events_filename_change(string old_carname, string new_carname, string username)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + username + "_" + old_carname + ".txt"))
            {
                string[] events_filenames = new string[1000];
                string[] events_name = new string[1000];
                string[] events_date = new string[1000];
                FileStream read_file = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + username + "_" + old_carname + ".txt", FileMode.Open, FileAccess.Read, FileShare.None);
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
                FileStream new_event_list = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + username + "_" + new_carname + ".txt", FileMode.Create, FileAccess.Write, FileShare.None);
                StreamWriter write = new StreamWriter(new_event_list);
                for (int i = 0; i <= 999; i++)
                {
                    if (events_filenames[i] == null || events_filenames[i] == "") break;
                    File.Move(AppDomain.CurrentDomain.BaseDirectory + @"\events\" + events_filenames[i], AppDomain.CurrentDomain.BaseDirectory + @"\events\" + username + "_" + new_carname + "_" + events_date[i] + "_" + events_name[i] + ".xml");
                    write.WriteLine(username + "_" + new_carname + "_" + events_date[i] + "_" + events_name[i] + ".xml");
                }
                write.Close();
                new_event_list.Close();
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\events\events_list_" + username + "_" + old_carname + ".txt");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

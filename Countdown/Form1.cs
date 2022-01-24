using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Timers;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Globalization;

namespace Countdown
{
    public partial class Form1 : Form
    {
        private static System.Timers.Timer calcTimer;
        private static System.Windows.Forms.Timer setTimer;
        double d = 0;
        double h = 0;
        double min = 0;
        double sec = 0;
        double time = 0;

        int selectedHour = 0;
        int selectedMinute = 0;
        int selectedSecond = 0;

        bool timeron = false;
        public void SetTimer()
        {
            calcTimer = new System.Timers.Timer(1000);
            calcTimer.Elapsed += Calculation;
            calcTimer.Start();

            setTimer = new System.Windows.Forms.Timer();
            setTimer.Tick += new EventHandler(TimerEventProcessor);
            setTimer.Interval = 1000;
            setTimer.Start();
            
        }
        public Form1()
        {
            InitializeComponent();
            DateTime n = DateTime.Today.AddDays(1);
            label1.Text = n.ToShortDateString();
            monthCalendar1.SelectionStart = n;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            SetTimer();
            LoadForm();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to save your timer?", "CodeJuggler", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                bool onlydays = false;

                if(checkBox1.Checked == true)
                {
                    onlydays = true;
                }
                else
                {
                    onlydays = false;
                }

                SaveForm(time, onlydays);
            }
        }


        public void LoadForm()
        {
            XmlDocument doc = new XmlDocument();

            if(File.Exists("Save.xml"))
            {
                Console.WriteLine("Loading!");
                doc.Load("Save.xml");
                XmlNode node = doc.SelectSingleNode("/nodes/variables[1]");
                time = float.Parse(node.InnerText);
                Console.WriteLine("Time " + time);

                node = doc.SelectSingleNode("/nodes/variables[2]");
                checkBox1.Checked = bool.Parse(node.InnerText);

                node = doc.SelectSingleNode("/nodes/variables[3]");
                textBox1.Text = node.InnerText;

                node = doc.SelectSingleNode("/nodes/variables[4]");
                monthCalendar1.SetDate(DateTime.Parse(node.InnerText));

                node = doc.SelectSingleNode("/nodes/variables[5]");
                selectedHour = Int32.Parse(node.InnerText);

                node = doc.SelectSingleNode("/nodes/variables[6]");
                selectedMinute = Int32.Parse(node.InnerText);

                node = doc.SelectSingleNode("/nodes/variables[7]");
                selectedSecond = Int32.Parse(node.InnerText);

                Console.WriteLine(monthCalendar1.SelectionEnd.ToString());

                if (textBox1.Text != string.Empty && monthCalendar1.TodayDateSet == false)
                {
                    label1.Visible = false;
                    label2.Visible = true;
                    label2.Text = "Aikaa tapahtuman " + textBox1.Text + " alkamiseen on";
                    label4.Visible = true;
                    label5.Visible = true;
                    label6.Visible = true;
                    label7.Visible = true;
                    label8.Visible = true;
                    label9.Visible = true;
                    label10.Visible = true;
                    label11.Visible = true;

                    textBox1.Enabled = false;

                    comboBox1.Visible = false;
                    comboBox2.Visible = false;
                    comboBox3.Visible = false;
                    checkBox1.Visible = false;
                    button1.Visible = false;
                    button2.Visible = true;
                    button2.Enabled = true;

                    label12.Visible = false;
                    label13.Visible = false;
                    label14.Visible = false;

                    timeron = true;
                }

                if (checkBox1.Checked == true)
                {
                    label6.Visible = false;
                    label9.Visible = false;
                    label5.Visible = false;
                    label10.Visible = false;
                    label11.Visible = false;
                    label4.Visible = false;
                }


            }
            else
            {
                XDocument doc2 = new XDocument();
                doc2 = new XDocument(new XElement("nodes",
                                           new XElement("variables", "Selected time"),
                                           new XElement("variables", "box"),
                                           new XElement("variables", "eventName"),
                                           new XElement("variables", "dateTime"),
                                           new XElement("variables", "selectedhour"),
                                           new XElement("variables", "selectedminute"),
                                           new XElement("variables", "selectedsecond")
                                           ));
                doc2.Save("Save.xml");
            }
        }

        public void SaveForm(double time, bool onlydays)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("Save.xml");
            XmlNode node = doc.SelectSingleNode("/nodes/variables[1]");
            node.InnerText = time.ToString();

            node = doc.SelectSingleNode("/nodes/variables[2]");
            node.InnerText = (string)checkBox1.Checked.ToString();

            node = doc.SelectSingleNode("/nodes/variables[3]");
            node.InnerText = textBox1.Text;

            node = doc.SelectSingleNode("/nodes/variables[4]");
            DateTime s = monthCalendar1.SelectionEnd;
            TimeSpan ts = new TimeSpan(selectedHour, selectedMinute, selectedSecond);
            s = s + ts;
            node.InnerText = s.ToString();

            node = doc.SelectSingleNode("/nodes/variables[5]");
            node.InnerText = selectedHour.ToString();

            node = doc.SelectSingleNode("/nodes/variables[6]");
            node.InnerText = selectedMinute.ToString();

            node = doc.SelectSingleNode("/nodes/variables[7]");
            node.InnerText = selectedSecond.ToString();

            doc.Save("Save.xml");
        }

        private void label1_Click(object sender, EventArgs e)
        {
            monthCalendar1.Visible = true;
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            Console.WriteLine("Valittu " + monthCalendar1.SelectionEnd);

        }
        private void monthCalendar1_DateSelected(object sender, System.Windows.Forms.DateRangeEventArgs e)
        {
            Console.WriteLine("TEST DATESELECTED!");

            string date = monthCalendar1.SelectionEnd.ToShortDateString();
            Console.WriteLine(date);
            monthCalendar1.Visible = false;
            label1.Text = date;
            label2.Visible = false;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty && monthCalendar1.TodayDateSet == false)
            {
                label1.Visible = false;
                label2.Visible = true;
                label2.Text = "Aikaa tapahtuman " + textBox1.Text + " alkamiseen on";
                label4.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
                label8.Visible = true;
                label9.Visible = true;
                label10.Visible = true;
                label11.Visible = true;

                textBox1.Enabled = false;

                comboBox1.Visible = false;
                comboBox2.Visible = false;
                comboBox3.Visible = false;
                checkBox1.Visible = false;
                button1.Visible = false;
                button2.Visible = true;
                button2.Enabled = true;

                label12.Visible = false;
                label13.Visible = false;
                label14.Visible = false;

                timeron = true;
            }

            if(checkBox1.Checked == true)
            {
                label6.Visible = false;
                label9.Visible = false;
                label5.Visible = false;
                label10.Visible = false;
                label11.Visible = false;
                label4.Visible = false;
            }

            if(comboBox1.Text != string.Empty)
            {
                string i = comboBox1.Text.Substring(0, comboBox1.Text.Length - 3);
                selectedHour = Int32.Parse(i);
                Console.WriteLine("selectedHour " + selectedHour);
            }

            if (comboBox2.Text != string.Empty)
            {
                selectedMinute = Int32.Parse(comboBox2.Text);
            }


            if (comboBox3.Text != string.Empty)
            {
                selectedSecond = Int32.Parse(comboBox3.Text);
            }

            Timer();
        }

        private void Timer()
        {
            int[] daysyears = new int[99];
            int years = 0;
            int months = 0;
            int days = 0;
            int wholedays = 0;
            int amountofleapyears = 0;
            bool moremonths = false;
            bool moredays = false;
            string date = monthCalendar1.SelectionEnd.ToShortDateString();
            //Console.WriteLine(date);
            for (int i = 0; i < daysyears.Length; i++)
            {
                //Console.WriteLine(i);
                if (i % 2 == 0 && i != 0)
                {
                    if (daysyears[2] != 0)
                    {
                        if (i % 4 == 0 && i != 4)
                        {
                            Console.WriteLine("Sisällä");
                            daysyears[i] = 366;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Sisällä2");
                        daysyears[i] = 366;
                    }
                }
                else
                {
                    daysyears[i] = 365;
                }
                Console.WriteLine(daysyears[i]);
                Console.WriteLine("I on " + i);
            }

            if (monthCalendar1.SelectionEnd.Year > monthCalendar1.TodayDate.Year)
            {
                if (monthCalendar1.SelectionEnd.Month > monthCalendar1.TodayDate.Month)
                {
                    years = (monthCalendar1.SelectionEnd.Year - monthCalendar1.TodayDate.Year);
                }
                else
                {
                    years = (monthCalendar1.SelectionEnd.Year - monthCalendar1.TodayDate.Year - 1);
                }
            }

            //if(monthCalendar1.SelectionEnd.Month >= monthCalendar1.TodayDate.Month)
            //{
            //    if(monthCalendar1.SelectionEnd.Month == monthCalendar1.TodayDate.Month)
            //    {
            //        if(monthCalendar1.SelectionEnd.Day > monthCalendar1.TodayDate.Day)
            //        {
            //            years = (monthCalendar1.SelectionEnd.Year - monthCalendar1.TodayDate.Year);
            //        }
            //        else
            //        {
            //            years = (monthCalendar1.SelectionEnd.Year - monthCalendar1.TodayDate.Year - 1);
            //            moremonths = true;
            //        }
            //    }
            //    else
            //    {
            //        years = (monthCalendar1.SelectionEnd.Year - monthCalendar1.TodayDate.Year);
            //    }
            //}
            //else
            //{
            //    years = (monthCalendar1.SelectionEnd.Year - monthCalendar1.TodayDate.Year - 1);
            //    moremonths = true;
            //}

            if (monthCalendar1.SelectionEnd.Month > monthCalendar1.TodayDate.Month)
            {
                months = (12 - (monthCalendar1.TodayDate.Month));
            }
            else
            {
                months = (12 - (monthCalendar1.TodayDate.Month)) + monthCalendar1.SelectionEnd.Month;

                //(12-(monthCalendar1.TodayDate.Month+1)) + monthCalendar1.SelectionEnd.Month;
            }

            if (monthCalendar1.SelectionEnd.Day > monthCalendar1.TodayDate.Day)
            {
                days = (int)Math.Round(30.41666 - monthCalendar1.TodayDate.Day + monthCalendar1.SelectionEnd.Day);
            }
            else
            {
                days = (int)Math.Round(30.41666 - monthCalendar1.TodayDate.Day + monthCalendar1.SelectionEnd.Day);
            }

            Console.WriteLine("Päivistä " + days);
            Console.WriteLine("Kuukausista " + months);

            for (int i = 0; i < years; i++)
            {
                wholedays += daysyears[i];
                Console.WriteLine(daysyears[i]);
                Console.WriteLine("Vuosista " + wholedays);

                if (daysyears[i] == 366)
                {
                    amountofleapyears++;
                    Console.WriteLine(amountofleapyears);
                }
            }
            Console.WriteLine("Vuosista " + years);
            wholedays += (int)Math.Round(months * 30.42);
            wholedays += days;
            time = (monthCalendar1.SelectionEnd.Date - DateTime.Now).TotalSeconds;
            Console.WriteLine("Aika " + time);
            double d = time / 86400;
            double m = time / 2629743.83;
            double y = time / 31556926;

            double h = 0;
            double min = 0;
            double sec = 0;


            Console.WriteLine("Päivät " + d);

            if (d > (int)Math.Round(d))
            {
                var leftd = d - (int)Math.Floor(d);
                Console.WriteLine("Left of days " + leftd);
                h = leftd * 24;
                var lefth = h - (int)Math.Floor(h);
                Console.WriteLine("Left of hours " + lefth);
                min = lefth * 60;
                var leftmin = min - (int)Math.Floor(min);
                Console.WriteLine("Left of minutes " + leftmin);
                sec = (int)Math.Round(leftmin * 60);

                d = Math.Floor(d);
                h = Math.Floor(h);
                min = Math.Floor(min);


                Console.WriteLine("Hours " + h);
                Console.WriteLine("Minutes " + min);
                Console.WriteLine("Seconds " + sec);
            }

            label4.Text = sec.ToString();


            //Console.WriteLine(time);
            //Console.WriteLine(d);
            //Console.WriteLine(m);
            //Console.WriteLine(y);

            //Console.WriteLine(wholedays);
        }

        private void Calculation(Object source, ElapsedEventArgs e)
        {
            Console.WriteLine(monthCalendar1.SelectionEnd.Date);
            Console.WriteLine(selectedHour);
            Console.WriteLine(selectedMinute);
            Console.WriteLine(selectedSecond);
            Console.WriteLine("d " + d.ToString());

            if (monthCalendar1.SelectionEnd.Date != null)
            {
                Console.WriteLine("SelectionEnd.Date != null");
                double time = (monthCalendar1.SelectionEnd.Date - DateTime.Now).TotalSeconds + (selectedHour*60*60) + (selectedMinute*60) + (selectedSecond);

                d = time / 86400;
                double m = time / 2629743.83;
                double y = time / 31556926;
            }

            if (d != null)
            {
                Console.WriteLine("d > (int)Math.Round(d)");
                var leftd = d - (int)Math.Floor(d);
                Console.WriteLine("Left of days " + leftd);
                h = leftd * 24;
                var lefth = h - (int)Math.Floor(h);
                Console.WriteLine("Left of hours " + lefth);
                min = lefth * 60;
                var leftmin = min - (int)Math.Floor(min);
                Console.WriteLine("Left of minutes " + leftmin);
                sec = (int)Math.Round(leftmin * 60);

                d = Math.Floor(d);
                h = Math.Floor(h);
                min = Math.Floor(min);

                Console.WriteLine("Days " + d);
                Console.WriteLine("Hours " + h);
                Console.WriteLine("Minutes " + min);
                Console.WriteLine("Seconds " + sec);
            }

        }

        private void TimerEventProcessor(object myObject, EventArgs myEventArgs)
        {
            label4.Text = sec.ToString();
            label5.Text = min.ToString();
            label6.Text = h.ToString();
            label7.Text = d.ToString();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;

            //label 7 == päivät numerona
            // label 8 == "päivät"

            label7.Font = new Font("Arial", 24, FontStyle.Bold);
            label7.Location = new Point(160, 123);
            label8.Font = new Font("Arial", 24, FontStyle.Bold);
            label8.Location = new Point(131, 80);
            label8.Size = new System.Drawing.Size(900, 26);
            label8.AutoSize = true;


        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(@"save.xml"))
            {
                File.Delete(@"save.xml");
            }
            Application.Restart();
            Environment.Exit(0);
            /*label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            label2.Visible = false;

            label1.Enabled = true;
            checkBox1.Enabled = true;*/
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
 }

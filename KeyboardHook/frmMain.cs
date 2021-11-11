using System;
using System.Windows.Forms;
using Hooks;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Threading;


namespace KeyboardHook
{
    public partial class frmMain : Form
    {
        static bool on = false;
        protected readonly GlobalHook hook = new GlobalHook();
        public static List<macros> macroses = new List<macros>();
        static int countmacros = -1;
        static List<int> indexcikl = new List<int>();
        static List<int> kolcikl = new List<int>();
        static string path = @"macros\macroses.xml";
        //static DirectoryInfo dirInfo = new DirectoryInfo(path);
        string strvvodkol="1";
        public static Stopwatch sw = new Stopwatch();
        bool temp = false;
        bool focuskey = false;
        public frmMain()
        {
            InitializeComponent();
            if (File.Exists(path))
            {
                iniMacros();
            }
            
            string str = File.ReadAllText("bind.txt");
            label8.Text = str;
            textBox3.Text = Convert.ToString(label8.Text);
            this.FormClosed += new FormClosedEventHandler(frmMain_FormClosed);
            MouseHook.MouseDown += new MouseEventHandler(MouseHook_MouseDown);
            //MouseHook.MouseMove += new MouseEventHandler(MouseHook_MouseMove);
            MouseHook.MouseUp += new MouseEventHandler(MouseHook_MouseUp);

            hook.KeyDown += (s, ev) =>
            {
                if (ev.KeyCode.ToString() == textBox3.Text) {
                    on = false;
                    button5.Enabled = false;
                    button4.Enabled = true;
                    sw.Stop();
                    this.WindowState = FormWindowState.Normal;
                }
                if (focuskey) {
                    label8.Text = ev.KeyCode.ToString();
                }
            };
            MouseHook.LocalHook = false;
            listBox1.Items.Clear();
            MouseHook.InstallHook();
            label1.Text = string.Format("Installed:{0}\r\nModule:{1}\r\nLocal{2}",
                MouseHook.IsHookInstalled, MouseHook.ModuleHandle, MouseHook.LocalHook);
            panelVisible(false, true, false);
            panel2.Location = panel1.Location;
            panel3.Location = panel1.Location;
            button5.Enabled = false;
           
        }

        void MouseHook_MouseMove(object sender, MouseEventArgs e)
        {
            //listBox1.Items.Add(e.Location);
            //listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }
        void MouseHook_MouseDown(object sender, MouseEventArgs e)
        {
            if (!MouseHook.onoff)
            {
                if (on)
                {
                    long i = sw.ElapsedMilliseconds / 10;
                    sw.Stop();
                    sw.Reset();
                    sw.Start();
                    macroses[countmacros].mousearray.Add(new MouseDate(Cursor.Position.X, Cursor.Position.Y, MouseHook.type_clickint, (int)i));
                    listBox1.Items.Add(MouseHook.type_click);
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                }
            }
            if (MouseHook.onoff && !temp)
            {
                listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
                macroses[countmacros].mousearray.RemoveAt(macroses[countmacros].mousearray.Count - 1);
                temp = true;
                sw.Stop();
                on = false;
            }
        }
        void MouseHook_MouseUp(object sender, MouseEventArgs e)
        {
            if (!MouseHook.onoff)
            {
                if (on)
                {
                    long i = sw.ElapsedMilliseconds / 10;
                    sw.Stop();
                    sw.Reset();
                    sw.Start();
                    listBox1.Items.Add(MouseHook.type_click);
                    macroses[countmacros].mousearray.Add(new MouseDate(Cursor.Position.X, Cursor.Position.Y, MouseHook.type_clickint, (int)i));
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                  
                }
            }
            if (MouseHook.onoff && !temp)
            {
                listBox1.Items.RemoveAt(listBox1.Items.Count-1);
                macroses[countmacros].mousearray.RemoveAt(macroses[countmacros].mousearray.Count-1);
                temp = true; 
                sw.Stop();
                on = false;
            }
        }
 
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            MouseHook.UnInstallHook(); // Обязательно !!!
            hook.Dispose();
            
        }
        private void frmMain_Load(object sender, EventArgs e)
        {

        }
        public void panelVisible(bool p1, bool p2, bool p3)
        {
            panel1.Visible = p1;
            panel2.Visible = p2;
            panel3.Visible = p3;
        }
        private void созданиеМакросовToolStripMenuItem_Click(object sender, EventArgs e)
        {

            panelVisible(false, true, false);


        }
        private void запускМакросовToolStripMenuItem_Click(object sender, EventArgs e)
        {

            panelVisible(true, false, false);
            listBox2.Items.Clear();
            for (int i = 0; i <= countmacros; i++)
                listBox2.Items.Add(macroses[i].name);
        }

        private void редактированиеМакросовToolStripMenuItem_Click(object sender, EventArgs e)
        {

            panelVisible(false, false, true);
            listBox4.Items.Clear();
            for (int i = 0; i <= countmacros; i++)
                listBox4.Items.Add(macroses[i].name);


        }
        private void button4_Click(object sender, EventArgs e)
        {
            temp = false;
            MouseHook.onoff = false; 
            sw.Reset();
            sw.Start();
            listBox1.Items.Clear();
            countmacros++;
            macroses.Add(new macros());
            macroses[countmacros].name = textBox2.Text;
            on = true;
            button4.Enabled = false;
            button5.Enabled = true;
            this.WindowState = FormWindowState.Minimized;
        }
        private void button5_Click(object sender, EventArgs e)
        {
            on = false;
            button5.Enabled = false;
            button4.Enabled = true;
            sw.Stop();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.TextLength == 0)
            {
                button4.Enabled = false;
            }
            else
            {
                button4.Enabled = true;
            }
        }
        public static void setdgv(DataGridView dt, int num)
        {
            dt.Rows.Clear();
            if (num!=-1) {
                for (int i = 0; i < macroses[num].mousearray.Count; i++)
                {
                    float timeee = macroses[num].mousearray[i].time;
                    timeee = timeee / 100;
                    dt.Rows.Add(i + 1, macroses[num].mousearray[i].position_x, macroses[num].mousearray[i].position_y, macroses[num].mousearray[i].type_click, timeee);

                } 
            }

        }
        public static String typeclick(int i)
        {
            String temp = "";
            switch (i)
            {
                case 1:
                    temp = "ЛКМ нажата";
                    break;
                case 2:
                    temp = "ЛКМ отжата";
                    break;
                case 3:
                    temp = "ПКМ нажата";
                    break;
                case 4:
                    temp = "ПКМ отжата";
                    break;
                default:
                    break;
            }

            return temp;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            setdgv(dataGridView1, listBox4.SelectedIndex);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem != null)
            {
                
                listBox3.Items.Add(macroses[listBox2.SelectedIndex].name+" "+textBox1.Text+" раз/раза ");
                indexcikl.Add(listBox2.SelectedIndex);
                kolcikl.Add(Convert.ToInt32(textBox1.Text));
            }
            
           

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedItem != null)
            {
                indexcikl.RemoveAt(listBox3.SelectedIndex);

                kolcikl.RemoveAt(listBox3.SelectedIndex);
                listBox3.Items.RemoveAt(listBox3.SelectedIndex);
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {

            string createText = SerializeExtension.SerializeToString(macroses);
            File.WriteAllText(path, createText);
            string str=File.ReadAllText(path).Replace("utf-16", "utf-8");
            File.WriteAllText(path, str);
        }
        private void iniMacros(){
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(path);
            XmlElement xRoot = xDoc.DocumentElement;
            int i = 0;
            foreach (XmlNode macros1 in xRoot){  //обход macros
                macroses.Add(new macros());
                foreach (XmlNode elm in macros1.ChildNodes){ // Обход детей макроса
                    if (elm.Name == "name") macroses[i].name = elm.InnerText;
                    if (elm.Name == "mousearray") {
                        foreach (XmlNode mousearray1 in elm.ChildNodes){ //Обход племяников макроса
                            int x = 0, y=0, type = 0, time = 0;
                            foreach (XmlNode MouseDate1 in mousearray1.ChildNodes)
                            {
                                if (MouseDate1.Name == "position_x") x = Convert.ToInt32(MouseDate1.InnerText);
                                if (MouseDate1.Name == "position_y") y = Convert.ToInt32(MouseDate1.InnerText);
                                if (MouseDate1.Name == "type_click") type = Convert.ToInt32(MouseDate1.InnerText);
                                if (MouseDate1.Name == "time") time = Convert.ToInt32(MouseDate1.InnerText);
                            }
                            macroses[i].mousearray.Add(new MouseDate(x, y, type, time));
                        }
                    }
                }
                i++;
            }
            countmacros = macroses.Count-1;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
            try {
                int i = Convert.ToInt32(textBox1.Text);
                strvvodkol = textBox1.Text;
            }
            catch (Exception)
            {
                MessageBox.Show("Вводить только числа");
                textBox1.Text = strvvodkol;
            }

        }

        private void run(){
            for (int i = 0; i <= indexcikl.Count-1; i++){
                Console.WriteLine("Номер цикла="+i+" in "+(indexcikl.Count-1));
                for (int z = 0; z <= kolcikl[i]-1; z++){
                    Console.WriteLine("количество раз=" + z + "z in " + (kolcikl[i] - 1));
                   
                    for (int y = 0; y <= macroses[indexcikl[i]].mousearray.Count-1; y++){
                       
                        Console.WriteLine("действие в цикле=" + y + "y in " + (macroses[indexcikl[i]].mousearray.Count - 1));
                        int xpos = macroses[indexcikl[i]].mousearray[y].position_x;
                        int ypos = macroses[indexcikl[i]].mousearray[y].position_y;
                        int time = macroses[indexcikl[i]].mousearray[y].time;
                        if (macroses[indexcikl[i]].mousearray[y].time * 10 < 100)
                        {
                            Thread.Sleep(100);
                        }
                        else
                            Thread.Sleep(macroses[indexcikl[i]].mousearray[y].time * 10);

                        switch (macroses[indexcikl[i]].mousearray[y].type_click)
                        {
                            case 1: MouseDate.mouseLeftDown(xpos, ypos); break;
                            case 2: MouseDate.mouseLeftUp(xpos, ypos); break;
                            case 3: MouseDate.mouseRightDown(xpos, ypos); break;
                            case 4: MouseDate.mouseRightUp(xpos, ypos); break;
                            default: break;
                        }
                       

                    }
                    

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form f = this;
            this.WindowState = FormWindowState.Minimized;
            run();
            this.WindowState = FormWindowState.Normal;

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.Text = Convert.ToString(label8.Text);
        }

        private void textBox3_focus(object sender, EventArgs e)
        {
            focuskey = true;
            textBox3.Text =Convert.ToString( label8.Text);

        }
     

        private void textBox3_onfocus(object sender, EventArgs e)
        {
            focuskey = false;
        }
        private void textca(object sender, EventArgs e)
        {
            textBox3.Text = Convert.ToString(label8.Text);
            //string str = File.ReadAllText("bind.txt");
            File.WriteAllText("bind.txt", textBox3.Text);
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int temp=0;
            try
            {
                int i = Convert.ToInt32(dataGridView1.CurrentCell.Value);
                temp = Convert.ToInt32(dataGridView1.CurrentCell.Value);
            }
            catch (Exception) {
                MessageBox.Show("Только числа!");
                dataGridView1.CurrentCell.Value =temp;
                    }
          
            }
        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            button7.Enabled = true;
        }

        private void dataGridView1_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            button7.Enabled = false;

        }

        private void button7_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
        }
        public void savemacrosses() {
            macroses[listBox4.SelectedIndex].mousearray.Clear();
            for (int i = 0; i < dataGridView1.Rows.Count-1; i++){
                int x =Convert.ToInt32( dataGridView1[1, i].Value);
                int y = Convert.ToInt32(dataGridView1[2, i].Value);
                int tc = Convert.ToInt32(dataGridView1[3, i].Value);
                double t1 = Convert.ToDouble(dataGridView1[4, i].Value)*100;
                int t = Convert.ToInt32(t1);
                macroses[listBox4.SelectedIndex].mousearray.Add(new MouseDate(x,y,tc,t));
                }
            string createText = SerializeExtension.SerializeToString(macroses);
            File.WriteAllText(path, createText);
            string str = File.ReadAllText(path).Replace("utf-16", "utf-8");
            File.WriteAllText(path, str);

        }

        private void button6_Click(object sender, EventArgs e)
        {
            savemacrosses();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox4.SelectedIndex!=-1) {
                macroses.RemoveAt(listBox4.SelectedIndex);
                countmacros--;
                indexcikl.Clear();
                kolcikl.Clear();
                listBox3.Items.Clear();
                listBox4.Items.RemoveAt(listBox4.SelectedIndex);
                       }
            string createText = SerializeExtension.SerializeToString(macroses);
            File.WriteAllText(path, createText);
            string str = File.ReadAllText(path).Replace("utf-16", "utf-8");
            File.WriteAllText(path, str);


        }
    }
}
    
    


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Memory
{
    public partial class Form1 : Form
    {
        // to map the memory size to fit the Form panel
        int START = 1000000;
        int END = 0;
        int holecount;
        int processcount;
        int N = 0;// not allocated processes
        public LinkedList<process> processes = new LinkedList<process>();
        public LinkedList<hole> holes = new LinkedList<hole>();
        public LinkedList<process> memory = new LinkedList<process>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            holecount++;
            if (holecount < numericUpDown1.Value)
            {
                hole h = new hole();
                h.addhole(Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text));
                textBox1.Text = "";
                textBox2.Text = "";
                h.holenumber = holecount;
                holes.AddLast(h);

                if (h.holeStartAddress >= END)
                    END = h.holeStartAddress + h.size;

                if (h.holeStartAddress < START)
                    START = h.holeStartAddress;
            }

            if (holecount == numericUpDown1.Value)
            {
                hole h = new hole();
                h.addhole(Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text));
                h.holenumber = holecount;
                holes.AddLast(h);

                if (h.holeStartAddress >= END)
                    END = h.holeStartAddress + h.size;

                if (h.holeStartAddress < START)
                    START = h.holeStartAddress;

                label1.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label8.Visible = false;
                numericUpDown1.Visible = false;
                textBox1.Visible = false;
                textBox2.Visible = false;
                button1.Visible = false;

                label2.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                numericUpDown2.Visible = true;
                textBox3.Visible = true;
                textBox4.Visible = true;
                button2.Visible = true;
                label9.Visible = true;
            }
        }
        
        private void drawHoles()
        {
            Graphics H = panel1.CreateGraphics();
            foreach (hole h in holes)
            {
                //start address
                Label startLabel = new Label();
                startLabel.Text = h.holeStartAddress.ToString();
                startLabel.Font = new Font("Monotype Corsiva", 8);
                Point position1 = new Point(0, (220 * (h.holeStartAddress - START) / (END - START)));
                startLabel.Location = position1;
                startLabel.Size = new System.Drawing.Size(50, 10);
                panel2.Controls.Add(startLabel);
                //end address
                Label endLabel = new Label();
                endLabel.Text = (h.holeStartAddress + h.size).ToString();
                endLabel.Font = new Font("Monotype Corsiva", 8);
                Point position2 = new Point(0, ((220 * ((h.holeStartAddress + h.size) - START) / (END - START))));
                endLabel.Location = position2;
                endLabel.Size = new System.Drawing.Size(50, 10);
                panel2.Controls.Add(endLabel);

                SolidBrush brush = new SolidBrush(Color.White);
                H.FillRectangle(brush, 0, (220 * (h.holeStartAddress - START) / (END - START)),
                    92, (220 * h.size / (END - START)));
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            processcount++;
            if (processcount < numericUpDown2.Value)
            {
                process p = new process();
                p.addProcess(textBox4.Text, Convert.ToInt32(textBox3.Text));
                textBox3.Text = "";
                textBox4.Text = "";

                processes.AddLast(p);
            }
            if (processcount == numericUpDown2.Value)
            {
                drawHoles();
                process p = new process();
                p.addProcess(textBox4.Text, Convert.ToInt32(textBox3.Text));               
                processes.AddLast(p);

                label2.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                numericUpDown2.Visible = false;
                textBox3.Visible = false;
                textBox4.Visible = false;
                button2.Visible = false;
                label9.Visible = false;

                label7.Visible = true;
                radioButton1.Visible = true;
                radioButton2.Visible = true;
                radioButton3.Visible = true;
                button3.Visible = true;
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //first fit
            if (radioButton1.Checked)
            {
                foreach (process j in processes)
                {
                    foreach (hole i in holes)
                    {
                        if (j.size < i.size)
                        {
                            //process
                            Graphics F = panel1.CreateGraphics();
                            SolidBrush newbrush = new SolidBrush(Color.Green);
                            F.FillRectangle(newbrush, 0, (220 * (i.holeStartAddress - START) / (END - START)),
                                92, (220 * (j.size) / (END - START)));

                            //process name 
                            Label name = new Label();
                            name.Text = j.name ;
                            name.Font = new Font("Monotype Corsiva", 8);
                            Point position3 = new Point(0, (220 * (i.holeStartAddress + (j.size/2) - START) / (END - START)));
                            name.Location = position3;
                            name.Size = new System.Drawing.Size(50, 10);
                            panel3.Controls.Add(name);

                            //process end 
                            Label end = new Label();
                            end.Text = (i.holeStartAddress + j.size).ToString();
                            end.Font = new Font("Monotype Corsiva", 8);
                            Point position4 = new Point(0, (220 * (i.holeStartAddress + j.size - START) / (END - START)));
                            end.Location = position4;
                            end.Size = new System.Drawing.Size(50, 10);
                            panel3.Controls.Add(end);

                            j.addStartAddress(i.getHoleAddr());
                            i.holeStartAddress = j.processStartAddress + j.getSize();
                            i.size = i.size - j.getSize();

                            MessageBox.Show("First Fit"+"\r\nProcess " + j.name + "\r\nProcess Start Address: " +
                                j.processStartAddress + "\r\nProcess end address: " + (j.processStartAddress+j.getSize()).ToString() +
                                "\r\nHole " + i.holenumber + "\r\nHole Start Address: " + i.holeStartAddress +
                                "\r\nHole Size: " + i.size);
                            break;
                        }
                        else if (j.size == i.size)
                        {
                            //process
                            Graphics F = panel1.CreateGraphics();
                            SolidBrush newbrush = new SolidBrush(Color.Green);
                            F.FillRectangle(newbrush, 0, (220 * (i.holeStartAddress - START) / (END - START)),
                                92, (220 * (j.size) / (END - START)));

                            //process name 
                            Label name = new Label();
                            name.Text = j.name ;
                            name.Font = new Font("Monotype Corsiva", 8);
                            Point position3 = new Point(0, (220 * (i.holeStartAddress + (j.size/2) - START) / (END - START)));
                            name.Location = position3;
                            name.Size = new System.Drawing.Size(50, 10);
                            panel3.Controls.Add(name);

                            //process end 
                            Label end = new Label();
                            end.Text = (i.holeStartAddress + j.size).ToString();
                            end.Font = new Font("Monotype Corsiva", 8);
                            Point position4 = new Point(0, (220 * (i.holeStartAddress + j.size - START) / (END - START)));
                            end.Location = position4;
                            end.Size = new System.Drawing.Size(50, 10);
                            panel3.Controls.Add(end);

                            j.addStartAddress(i.getHoleAddr());

                            MessageBox.Show("First Fit" + "\r\nProcess " + j.name + "\r\nProcess Start Address: " +
                               j.processStartAddress + "\r\nProcess end address: " + (j.processStartAddress + j.getSize()).ToString());

                            break;
                        }
                    }
                }

                //Not Allocated Processes
                foreach (process l in processes)
                {
                    if (l.processStartAddress == -1)
                    {
                        Label process = new Label();
                        process.Text = l.name + " size: "+(l.getSize()).ToString();
                        process.Font = new Font("Monotype Corsiva", 10);
                        Point position = new Point(0,N+20);
                        process.Location = position;
                        process.Size = new System.Drawing.Size(200, 15);
                        panel4.Controls.Add(process);
                        N = N + 20;
                        
                        MessageBox.Show("First Fit" + "\r\n*Not Allocated Process* " + "\r\nProcess " + l.name +
                            "\r\nProcess Size: " + l.getSize());

                    }
                }
            }

            //best fit
            if (radioButton2.Checked)
            {
                var BestFit = holes.OrderBy(item => item.size);
                foreach (process j in processes)
                {
                    foreach (hole i in BestFit)
                    {
                        if (j.size < i.size)
                        {
                            //process
                            Graphics F = panel1.CreateGraphics();
                            SolidBrush newbrush = new SolidBrush(Color.Green);
                            F.FillRectangle(newbrush, 0, (220 * (i.holeStartAddress - START) / (END - START)),
                                92, (220 * (j.size) / (END - START)));

                            //process name 
                            Label name = new Label();
                            name.Text = j.name ;
                            name.Font = new Font("Monotype Corsiva", 8);
                            Point position3 = new Point(0, (220 * (i.holeStartAddress + (j.size/2) - START) / (END - START)));
                            name.Location = position3;
                            name.Size = new System.Drawing.Size(50, 10);
                            panel3.Controls.Add(name);

                            //process end 
                            Label end = new Label();
                            end.Text = (i.holeStartAddress + j.size).ToString();
                            end.Font = new Font("Monotype Corsiva", 8);
                            Point position4 = new Point(0, (220 * (i.holeStartAddress + j.size - START) / (END - START)));
                            end.Location = position4;
                            end.Size = new System.Drawing.Size(50, 10);
                            panel3.Controls.Add(end);

                            j.addStartAddress(i.getHoleAddr());
                            i.holeStartAddress = j.processStartAddress + j.getSize();
                            i.size = i.size - j.getSize();

                            MessageBox.Show("Best Fit" + "\r\nProcess " + j.name + "\r\nProcess Start Address: " +
                                j.processStartAddress + "\r\nProcess end address: " + (j.processStartAddress + j.getSize()).ToString() +
                                "\r\nHole " + i.holenumber + "\r\nHole Start Address: " + i.holeStartAddress +
                                "\r\nHole Size: " + i.size);

                            BestFit = BestFit.OrderBy(item => item.size);

                            break;
                        }
                        else if (j.size == i.size)
                        {
                            //process
                            Graphics F = panel1.CreateGraphics();
                            SolidBrush newbrush = new SolidBrush(Color.Green);
                            F.FillRectangle(newbrush, 0, (220 * (i.holeStartAddress - START) / (END - START)),
                                92, (220 * (j.size) / (END - START)));

                            //process name label
                            Label name = new Label();
                            name.Text = j.name ;
                            name.Font = new Font("Monotype Corsiva", 8);
                            Point position3 = new Point(0, (220 * (i.holeStartAddress + (j.size/2) - START) / (END - START)));
                            name.Location = position3;
                            name.Size = new System.Drawing.Size(50, 10);
                            panel3.Controls.Add(name);

                            //process end 
                            Label end = new Label();
                            end.Text = (i.holeStartAddress + j.size).ToString();
                            end.Font = new Font("Monotype Corsiva", 8);
                            Point position4 = new Point(0, (220 * (i.holeStartAddress + j.size - START) / (END - START)));
                            end.Location = position4;
                            end.Size = new System.Drawing.Size(50, 10);
                            panel3.Controls.Add(end);

                            j.addStartAddress(i.getHoleAddr());
                            i.holeStartAddress = j.processStartAddress + j.getSize();
                            i.size = 0;

                            MessageBox.Show("First Fit" + "\r\nProcess " + j.name + "\r\nProcess Start Address: " +
                               j.processStartAddress + "\r\nProcess end address: " + (j.processStartAddress + j.getSize()).ToString()); 

                            break;
                        }
                    }
                }

                //Not Allocated Processes
                foreach (process l in processes)
                {
                    if (l.processStartAddress == -1)
                    {
                        Label process = new Label();
                        process.Text = l.name + " size: " + (l.getSize()).ToString();
                        process.Font = new Font("Monotype Corsiva", 10);
                        Point position = new Point(0, N + 20);
                        process.Location = position;
                        process.Size = new System.Drawing.Size(200, 15);
                        panel4.Controls.Add(process);
                        N = N + 20;

                        MessageBox.Show("Best Fit" + "\r\n*Not Allocated Process* " + "\r\nProcess " + l.name +
                            "\r\nProcess Size: " + l.getSize());

                    }
                }
                
            }

            //worst fit
            if (radioButton3.Checked)
            {
                var WorstFit = holes.OrderByDescending(item => item.size);
                foreach (process j in processes)
                {
                    foreach (hole i in WorstFit)
                    {
                        if (j.size < i.size)
                        {
                            //process
                            Graphics F = panel1.CreateGraphics();
                            SolidBrush newbrush = new SolidBrush(Color.Green);
                            F.FillRectangle(newbrush, 0, (220 * (i.holeStartAddress - START) / (END - START)),
                                92, (220 * (j.size) / (END - START)));

                            //process name 
                            Label name = new Label();
                            name.Text = j.name ;
                            name.Font = new Font("Monotype Corsiva", 8);
                            Point position3 = new Point(0, (220 * (i.holeStartAddress + (j.size/2) - START) / (END - START)));
                            name.Location = position3;
                            name.Size = new System.Drawing.Size(50, 10);
                            panel3.Controls.Add(name);

                            //process end 
                            Label end = new Label();
                            end.Text = (i.holeStartAddress + j.size).ToString();
                            end.Font = new Font("Monotype Corsiva", 8);
                            Point position4 = new Point(0, (220 * (i.holeStartAddress + j.size - START) / (END - START)));
                            end.Location = position4;
                            end.Size = new System.Drawing.Size(50, 10);
                            panel3.Controls.Add(end);

                            j.addStartAddress(i.getHoleAddr());
                            i.holeStartAddress = j.processStartAddress + j.getSize();
                            i.size = i.size - j.getSize();

                            MessageBox.Show("Worst Fit" + "\r\nProcess " + j.name + "\r\nProcess Start Address: " +
                                j.processStartAddress + "\r\nProcess end address: " + (j.processStartAddress + j.getSize()).ToString() +
                                "\r\nHole " + i.holenumber + "\r\nHole Start Address: " + i.holeStartAddress +
                                "\r\nHole Size: " + i.size);

                            WorstFit= WorstFit.OrderByDescending(item => item.size);
                            
                            break;
                        }
                        else if (j.size == i.size)
                        {
                            //process
                            Graphics F = panel1.CreateGraphics();
                            SolidBrush newbrush = new SolidBrush(Color.Green);
                            F.FillRectangle(newbrush, 0, (220 * (i.holeStartAddress - START) / (END - START)),
                                92, (220 * (j.size) / (END - START)));

                            //process name label
                            Label name = new Label();
                            name.Text = j.name ;
                            name.Font = new Font("Monotype Corsiva", 8);
                            Point position3 = new Point(0, (220 * (i.holeStartAddress + (j.size/2) - START) / (END - START)));
                            name.Location = position3;
                            name.Size = new System.Drawing.Size(50, 10);
                            panel3.Controls.Add(name);

                            //process end 
                            Label end = new Label();
                            end.Text = (i.holeStartAddress + j.size).ToString();
                            end.Font = new Font("Monotype Corsiva", 8);
                            Point position4 = new Point(0, (220 * (i.holeStartAddress + j.size - START) / (END - START)));
                            end.Location = position4;
                            end.Size = new System.Drawing.Size(50, 10);
                            panel3.Controls.Add(end);

                            j.addStartAddress(i.getHoleAddr());
                            i.holeStartAddress = j.processStartAddress + j.getSize();
                            i.size = 0;

                            MessageBox.Show("First Fit" + "\r\nProcess " + j.name + "\r\nProcess Start Address: " +
                               j.processStartAddress + "\r\nProcess end address: " + (j.processStartAddress + j.getSize()).ToString());

                            break;
                        }
                    }
                }

                //Not Allocated Processes
                foreach (process l in processes)
                {
                    if (l.processStartAddress == -1)
                    {
                        Label process = new Label();
                        process.Text = l.name + " size: " + (l.getSize()).ToString();
                        process.Font = new Font("Monotype Corsiva", 10);
                        Point position = new Point(0, N + 20);
                        process.Location = position;
                        process.Size = new System.Drawing.Size(200, 15);
                        panel4.Controls.Add(process);
                        N = N + 20;

                        MessageBox.Show("Worst Fit" + "\r\n*Not Allocated Process* " + "\r\nProcess " + l.name +
                            "\r\nProcess Size: " + l.getSize());

                    }
                }        
            }
        }
    }
}

public class hole
{
    public int holeStartAddress;
    public int size;
    public int holenumber;
    public void addhole(int a, int s)
    { 
        holeStartAddress = a;
        size = s;
    }
    public int getHoleSize()
    { return size; }
    public int getHoleAddr()
    { return holeStartAddress; }
}

public class process
{
    public string name;
    public int size;
    public int processStartAddress;
    public void addProcess(string n, int s)
    {
        processStartAddress = -1;
        name = n;
        size = s;
    }
    public string getProcess()
    { return name; }
    public int getSize()
    { return size; }
    public void addStartAddress(int a)
    { processStartAddress = a; }
}
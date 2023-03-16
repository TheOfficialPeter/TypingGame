/*
    Author: Pieter Malan
    Filename: Form1.cs
    Description: This file includes the main window class and all the methods, events connected to it.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Project_2
{
    public partial class mainWindow : Form
    {
        public float wrong = 0; // wrong is the amount incorrect, but it doesn't reset to 0 unless game ended. Used for average.
        public float counter = 0; // counter is used to end the game when all letters are typed
        public float average = 0; // average gets updated and displayed on screen
        public float wrongCounter = 0; // wrongCounter is the amount incorrect, but it is used to refresh typed letters until the correct one is typed.
        public Hashtable letters = new Hashtable(); // hashtable to store all the letters
        bool started = false; // bolean used to start/stop game
        string[] sentences = { "She wondered what his eyes were saying beneath his mirrored sunglasses.", "The light that burns twice as bright burns half as long." , "He felt that dining on the bridge brought romance to his relationship with his cat.", "It doesn't sound like that will ever be on my travel list.", "Cats are good pets, for they are clean and are not noisy.", "Bill ran from the giraffe toward the dolphin.", "The elderly neighborhood became enraged over the coyotes who had been blamed for the poodle's disappearance.", "Her fragrance of choice was fresh garlic.", "The toy brought back fond memories of being lost in the rain forest.", "Red is greener than purple, for sure." };

        public mainWindow()
        {
            InitializeComponent();
        }
        
        // Close button top-right
        private void close_btn_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Minimize button top-right
        private void minimize_click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // Start game button
        private void start_click(object sender, EventArgs e)
        {
            if (!started)
            {

                for (int v = 0; v < listBox1.Items.Count; v++)
                {
                    listBox1.Items.RemoveAt(v);
                    v--;
                }

                GC.Collect(); // clear previously saved hashtables?
                letters = new Hashtable();

                toTypeLabel.Visible = true;
                whatYouTypeLabel.Visible = true;
                whatYouType.Visible = true;
                toType.Visible = true;
                started = true;
                wrong = 0;
                wrongCounter = 0;
                status.Text = "Playing";
                Random random = new Random();
                toType.Text = this.sentences[random.Next(0,10)];

                average = 100-(wrong/toType.Text.Length*100);
                average = (float)Math.Round(average, 2);
                total.Text = (average.ToString() + "%");

                for (int x = 0; x < toType.Text.Length; x++)
                {
                    Console.WriteLine(toType.Text[x]);
                    try {
                        letters.Add(toType.Text[x], toType.Text[x] + " - 100");
                    }
                    catch
                    {
                        //ignore duplicate values for letters in listbox error
                    }
                }

                foreach (var item in letters.Values)
                {
                    listBox1.Items.Add(item);
                }
            }

        }

        // Stop game button
        private void stop_click(object sender, EventArgs e)
        {
            if (started)
            {
                counter = 0;
                total.Text = "";
                whatYouType.Visible = false;
                toTypeLabel.Visible = false;
                whatYouTypeLabel.Visible = false;
                started = false;
                status.Text = "Not Playing";
                toType.Text = "";
                whatYouType.Text = "";
            }

        }

        // track keyboard input
        private void Check_Key(KeyPressEventArgs e)
        {
            if (started)
            {
                if (counter >= (toType.Text.Length)-1)
                {
                    counter = 0;
                    started = false;
                    whatYouType.Text = "";
                    toType.Text = "";
                    whatYouType.Visible = false;
                    toType.Visible = false;
                    toTypeLabel.Visible = false;
                    whatYouTypeLabel.Visible = false;
                    status.Text = "Not Playing";
                }
                else
                {
                    if (e.KeyChar == toType.Text[((int)counter)])
                    {
                        if (wrongCounter >= 1)
                        {
                            whatYouType.Text = whatYouType.Text.Remove(whatYouType.Text.Length - 1,1);
                            whatYouType.ForeColor = Color.FromArgb(1, 64, 64, 64);
                        }
                        wrongCounter = 0;
                        whatYouType.Text += e.KeyChar;
                        counter += 1;
                        average = 100-(wrong/toType.Text.Length*100);
                        average = (float)Math.Round(average, 2);
                        total.Text = (average.ToString() + "%");

                    }
                    else
                    {
                        wrongCounter++;
                        wrong++;
                        average = 100-(wrong/toType.Text.Length*100);
                        whatYouType.ForeColor = Color.Red;
                        
                        if (average < 0)
                        {
                            average = 0;
                            counter = 0;
                            started = false;
                            whatYouType.Text = "";
                            toType.Text = "";
                            whatYouType.Visible = false;
                            toType.Visible = false;
                            toTypeLabel.Visible = false;
                            whatYouTypeLabel.Visible = false;
                            total.Text = average.ToString() + "%";
                            status.Text = "Not Playing";
                            wrong = 0;
                            wrongCounter = 0;
                            return;
                        }
                        
                        average = (float)Math.Round(average, 2);
                        total.Text = average.ToString() + "%";
                        letters[e.KeyChar] = e.KeyChar + " - " + average.ToString();
                            
                        for (int v = 0; v < listBox1.Items.Count; v++)
                        {
                            listBox1.Items.RemoveAt(v);
                            v--;
                        }

                        foreach (var item in letters.Values)
                        {
                            listBox1.Items.Add(item);
                        }

                        if (wrongCounter == 1)
                        {
                            whatYouType.Text += e.KeyChar;
                        }
                        else if (wrongCounter >= 1)
                        {
                            whatYouType.Text = whatYouType.Text.Remove(whatYouType.Text.Length - 1,1);
                            whatYouType.Text += e.KeyChar;
                        }
                    }
                }
            }
        }

        // Capture key presses and send it to the Check_Key function to do futher evaluations
        private void mainWindow_KeyPress(object sender, KeyPressEventArgs e)
        {
            Check_Key(e);
        }

        // Capture key presses and send it to the Check_Key function to do futher evaluations. ListBox also captures keypresses because the main window isn't focused on at start.
        private void accuracy_KeyPress(object sender, KeyPressEventArgs e)
        {
            Check_Key(e);
        }
    }
}

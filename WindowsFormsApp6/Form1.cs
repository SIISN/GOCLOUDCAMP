using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using WMPLib;

namespace WindowsFormsApp6
{


    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private Thread musicThread;

        private void timer1_Tick(object sender, EventArgs e)
        {
            trackBar2.Maximum = Convert.ToInt32(wmp.currentMedia.duration);
            trackBar2.Value = Convert.ToInt32(wmp.controls.currentPosition);

            if(wmp != null)
            {
                int s = (int)wmp.controls.currentPosition;
                int h = s / 3600;
                int m = (s - (h * 3600)) / 60;
                s = s - (h * 3600 + m * 60);
                label1.Text = String.Format("{0:D}:{1:D2}:{2:D2}", h, m, s);
            }
            else
            {
                label1.Text = "0:00:00";
            }
        }

        WindowsMediaPlayer wmp = new WindowsMediaPlayer();
        //WMPLib.IWMPPlaylist playlist = wmp.playlistCollection.newPlaylist("myplaylist");

        WMPLib.IWMPMedia media;
        bool pausePlay = false;
        private void PlayMusic(string fileName)
        {
            timer1.Enabled = true;
            timer1.Interval = 1000;
            wmp.URL = fileName;
            wmp.controls.play();
            
        }
        DoublyLinkedList<string> linkedList = new DoublyLinkedList<string>();

        public void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "MP3 Files (*.mp3)|*.mp3";
            DoublyLinkedList<string> linkedList1 = new DoublyLinkedList<string>();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    linkedList1.Add(Path.GetFileNameWithoutExtension(fileName));
                    linkedList.Add(fileName);
                }
                listBox1.Items.Clear();
                foreach (var item in linkedList)
                {
                    listBox1.Items.Add(item);
                }
                name = linkedList.ElementAt(index);
            }
        }
        string name;
        int tindex = 0;
        int index = 0;

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (wmp.status == "")
                {
                    pausePlay = true;
                    PlayMusic(linkedList.ElementAt(index));

                }
                else if (index != tindex)
                {
                    pausePlay = true;
                    wmp.controls.play();
                   
                }
                else
                {
                    pausePlay = true;
                    PlayMusic(linkedList.ElementAt(index));

                }
            }
            catch
            {
                MessageBox.Show("Добавьте треки!");
            }

            //wmp.controls.play();

        }

        private void Pause_Click(object sender, EventArgs e)
        {
            if (pausePlay != false)
            {
                pausePlay = false;
                wmp.controls.pause();
            }
             
        }

        private void Volume_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            wmp.settings.volume = trackBar1.Value;
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        public void Delete_Click(object sender, EventArgs e)
        {
            linkedList.Remove(linkedList.ElementAt(index));
            listBox1.Items.Clear();
            foreach (var item in linkedList)
            {
                listBox1.Items.Add(item);
            }
            wmp.controls.stop();
            tindex++;
        }

        private void Next_Click(object sender, EventArgs e)
        {
            tindex = index;
            if(index == linkedList.Count() -1)
            {
                index = 0;
            }
            else
            {
                index++;
            }
            PlayMusic(linkedList.ElementAt(index));
        }

        private void Prev_Click(object sender, EventArgs e)
        {
            tindex = index;
            if(index==0)
            {
                index = linkedList.Count() -1;
            }
            else
            {
                index--;
            }
            PlayMusic(linkedList.ElementAt(index));
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            index = listBox1.SelectedIndex;
        }
    }
    public class DoublyNode<T>
    {
        public DoublyNode(T data)
        {
            Data = data;
        }
        public T Data { get; set; }
        public DoublyNode<T> Previous { get; set; }
        public DoublyNode<T> Next { get; set; }
    }

    public class DoublyLinkedList<T> : IEnumerable<T>  // двусвязный список
    {
        DoublyNode<T> head; // головной/первый элемент
        DoublyNode<T> tail; // последний/хвостовой элемент
        int count;
        public void Add(T data)
        {
            DoublyNode<T> node = new DoublyNode<T>(data);

            if (head == null)
                head = node;
            else
            {
                tail.Next = node;
                node.Previous = tail;
            }
            tail = node;
            count++;
        }

        public bool Remove(T data)
        {
            DoublyNode<T> current = head;

            // поиск удаляемого узла
            while (current != null)
            {
                if (current.Data.Equals(data))
                {
                    break;
                }
                current = current.Next;
            }
            if (current != null)
            {
                // если узел не последний
                if (current.Next != null)
                {
                    current.Next.Previous = current.Previous;
                }
                else
                {
                    // если последний, переустанавливаем tail
                    tail = current.Previous;
                }

                // если узел не первый
                if (current.Previous != null)
                {
                    current.Previous.Next = current.Next;
                }
                else
                {
                    // если первый, переустанавливаем head
                    head = current.Next;
                }
                count--;
                return true;
            }
            return false;
        }

        public int Contains(T data)
        {
            DoublyNode<T> current = head;
            int pos = 0;
            while (current != null)
            {
                if (current.Data.Equals(data))
                {
                    return pos +1;
                }
                pos++;
                current = current.Next;
            }
            return -1;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            DoublyNode<T> current = head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }

        public IEnumerable<T> BackEnumerator()
        {
            DoublyNode<T> current = tail;
            while (current != null)
            {
                yield return current.Data;
                current = current.Previous;
            }
        }
    }
}

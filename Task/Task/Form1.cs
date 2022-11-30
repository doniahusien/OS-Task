using System.ComponentModel;
namespace Task
{
    public partial class Form1 : Form
    {
        BackgroundWorker worker = new BackgroundWorker();
        public Form1()
        {
            InitializeComponent();
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = true;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.DoWork += Worker_DoWork;
        }
        void copyFile(string source,string des)
        {
            FileStream fout = new FileStream(des, FileMode.Create);
            FileStream fin = new FileStream(source, FileMode.Open);
            byte[] bt = new byte[1048756];
            int readByte;
            while ((readByte = fin.Read(bt, 0, bt.Length)) > 0)
            {
                fout.Write(bt, 0, readByte);
                worker.ReportProgress((int)(fin.Position * 100 / fin.Length));

            }
            fin.Close();
            fout.Close();
        }
        
        private void Worker_DoWork(object sender,DoWorkEventArgs e)
        {

            copyFile(textBox1.Text, textBox2.Text);
        }
        
        private void Worker_ProgressChanged(object sender,ProgressChangedEventArgs e)
        {
            progressBar1.Value= e.ProgressPercentage;
            label1.Text= progressBar1.Value.ToString()+"%";

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog o = new OpenFileDialog();
            if(o.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = o.FileName;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(fbd.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text= Path.Combine(fbd.SelectedPath,Path.GetFileName(textBox1.Text));
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            worker.RunWorkerAsync(); 
        }
    }
}
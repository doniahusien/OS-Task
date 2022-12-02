using System.ComponentModel;
namespace Task
{
    public partial class Form1 : Form
    {
        //gives you the ability to execute time-consuming operations asynchronously ("in the background")
        //يمنحك القدرة على تنفيذ عمليات تستغرق وقتا طويلا بشكل غير متزامن ("في الخلفية") ،
        BackgroundWorker worker = new BackgroundWorker();
        public Form1()
        {
            InitializeComponent();
            //Indicates if a BackgroundWorker supports asynchronous غير متزامنcancellation
            //هنا شغلت ان ينفع بعد كدا اكنسل اللي شغال
            worker.WorkerSupportsCancellation = true;
            //report progress updates.
            //هنا شغلت ان ينفع يبلغني ب اي تحديث يحصل 
            worker.WorkerReportsProgress = true;
            //بتحصل لما يجي ليها تقرير من اللي فوقها report Progress
            //event to report the progress 
            worker.ProgressChanged += Worker_ProgressChanged;
            //s raised when you call the RunWorkerAsync method بيشتغل لما استدعي  
            worker.DoWork += Worker_DoWork; 
        }
        //بتاخد حاجه تعملها كوبي و مكان يتعمل فيه الكوبي
        void copyFile(string source,string des)
        {   //بعمل الفايل اللي هعمل كوبي فيه
            FileStream fout = new FileStream(des, FileMode.Create);
            //بفتح الفايل اللي هعمله كوبي عشان اخد منه الداتا 
            FileStream fin = new FileStream(source, FileMode.Open);            
            byte[] bt = new byte[1048756];//ارري ب عدد معين من البايت
            int readByte;
            //بمشي اقرأ بلوك واحد من الفايل واشوفه اكبر من الصفر يعني مش فاضي و اخزنه في المتغير readbyte
            while ((readByte = fin.Read(bt/*first bit in array as buffer*/, 0 /*offset*/, bt.Length/* array size as count*/)) > 0)
            {
                //ببدأ اكتب بلوك اللي قرأته فوق 
                //بكتب في الفايل الجديد
                fout.Write(bt, 0, readByte);
                worker.ReportProgress((int)(fin.Position * 100 / fin.Length));//بتبعت المفروض التغيير اللي حصل مش فاكراها اوي

            }//هقفل الفايلات 
            fin.Close();
            fout.Close();
        }
        //This event is fired when the RunWorkerAsync method is called
        //RunWorkerAsync بيشتغل لما استدعي
        //دي لما تشتغل ب تبدأ ب الفانكشن بتاعت الكوبي
        private void Worker_DoWork(object sender,DoWorkEventArgs e)
        {
            copyFile(textBox1.Text, textBox2.Text);
        }
        //occurs when ReportProgress is called بيشتغل لما يجي تقرير ف ينفذه 
        //event to report the progress  of an asynchronous operation to the use
        private void Worker_ProgressChanged(object sender,ProgressChangedEventArgs e)
        {
            //بياخد النسبه بتاعت العمليه ويحطه في قيمه البار
            // This event handler updates the progress bar.
            progressBar1.Value= e.ProgressPercentage;
            //عشان يظهر الرقم كام في الميه ع حسب البار وصل ل كام
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
            OpenFileDialog o = new OpenFileDialog();//عشان افتح واشوف الفايلات اللي عندي
            if(o.ShowDialog()/*دي بتخليني اعرف اختار */== DialogResult.OK/*هنا ان ااختارت حاجه */)
            {
                textBox1.Text = o.FileName;//بيحط اسم الفايل اللي اختارته في اول تكست بوكس عشان دا اللي هعمله كوبي
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();//هنا بقي عشان هختار فولدر ك مكان اعين فيه مش فايل
            if(fbd.ShowDialog() == DialogResult.OK)//نفس اللي فوق
            {
                //بياخد مسار الفولدر اللي اختارته واسم الفايل اللي ف البوكس الاول عشان يعمل فايل نسخه منه ب نفس الاسم في المسار اللي لسه مختاره
                textBox2.Text= Path.Combine(fbd.SelectedPath,Path.GetFileName(textBox1.Text));

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            worker.RunWorkerAsync(); //starts the thread
                                     // Start BackgroundWorker
        }
    }
}
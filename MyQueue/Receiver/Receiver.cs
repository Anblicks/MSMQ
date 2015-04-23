using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Messaging;
using System.Threading;
using System.Diagnostics;

namespace Receiver
{    
    public partial class Receiver : Form
    {
        public System.Messaging.MessageQueue mq;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Receiver());
        }
        public Receiver()
        {
            InitializeComponent();

            if (MessageQueue.Exists(@".\Private$\MyQueue"))
            {
                mq = new System.Messaging.MessageQueue(@".\Private$\MyQueue");
                mq.ReceiveCompleted += new ReceiveCompletedEventHandler(mq_ReceiveCompleted);
            }
            else
            {
                mq.ReceiveCompleted += new ReceiveCompletedEventHandler(mq_ReceiveCompleted);
                MsgBox1.Items.Clear();
                MsgBox1.Items.Add("There is no messge queue, please check you queue once.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {            
            try
            {
                mq.BeginReceive(new TimeSpan(0, 0, 3));
                button1.Visible = false;
            }
            catch
            {
                
            }
            MsgBox1.Items.Add("Message receiving started.");
        }


        private void mq_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            //if (mq.GetAllMessages().Length > 0)
            {
                System.Messaging.Message msg;
                string m;
                try
                {
                    msg = mq.EndReceive(e.AsyncResult);
                    msg.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });
                    m = msg.Body.ToString();
                    //MsgBox1.Items.Add(m);
                    this.Invoke(new MethodInvoker(delegate()
                    {
                        MsgBox1.Items.Add(m);
                    }));
                    // Receive next message                    
                }
                catch (Exception ex)
                {

                }
            }
            mq.BeginReceive(new TimeSpan(0, 0, 3));
        }
    }
}

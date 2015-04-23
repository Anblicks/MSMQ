using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Messaging;

namespace QueueingObjects
{
    public partial class _Default : Page
    {
        public static System.Messaging.MessageQueue mq;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (MessageQueue.Exists(@".\Private$\MyQueue"))
                    mq = new System.Messaging.MessageQueue(@".\Private$\MyQueue");
                else
                    mq = MessageQueue.Create(@".\Private$\MyQueue");
            }
        }

        protected void sendmessage_Click(object sender, EventArgs e)
        {            
            System.Messaging.Message mm = new System.Messaging.Message();
            mm.Body = txt.Text;
            mm.Label = "Msg" + Guid.NewGuid();
            mq.Send(mm);
        }
    }
}
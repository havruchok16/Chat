using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Chat
{
    class ServerUser
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public OperationContext Operation { get; set; }
    }
}

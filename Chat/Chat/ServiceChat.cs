using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Chat
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "ServiceChat" в коде и файле конфигурации.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)] // клиенты будут работать с одним сервисом
    public class ServiceChat : IServiceChat
    {
        List<ServerUser> users = new List<ServerUser>();
        int nextId = 1;
        public int Connect(string userName)
        {
            ServerUser user = new ServerUser()
            {
                ID = nextId,
                UserName = userName,
                Operation = OperationContext.Current
            };
            nextId++;
            SendMessage(" " + user.UserName + " connected to the chat.", 0);
            users.Add(user);
            return user.ID;
        }

        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.ID == id);
            if (user != null)
            {
                users.Remove(user);
                SendMessage(" " + user.UserName + " left the chat.", 0);
            }
        }

        public void SendMessage(string message, int id)
        {
            foreach (var u in users)
            {
                string answer = DateTime.Now.ToShortTimeString();
                var user = users.FirstOrDefault(i => i.ID == id);
                if (user != null)
                {
                    answer += " " + user.UserName + ": ";
                }
                answer += message;
                u.Operation.GetCallbackChannel<IServiceChatCallBack>().CallBackMessage(answer);
            }
        }
    }
}

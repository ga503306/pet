using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using pet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace pet.Hubs
{
    [HubName("defaultHub")]
    public class DefaultHub : Hub
    {
        private Model1 db = new Model1();
        public void Get()
        {
            var result = new
            {
                data = "call"
            };
            Clients.All.Get(result);
        }
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var item = db.Signalr.Where(x => x.connectid == Context.ConnectionId).ToList();
            if (item != null)
            {
                db.Signalr.RemoveRange(item);
                db.SaveChanges();
            }

            return base.OnDisconnected(stopCalled);
        }
    }
}
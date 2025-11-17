using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace MedicalApp.Shared.Hubs
{
    public class RecordHub : Hub
    {
        public async Task ClinicModify(int id, string action)
        {
            // action can be: added, updated, deleted
            await Clients.All.SendAsync("ClinicModified", id, action);
        }
    }
}

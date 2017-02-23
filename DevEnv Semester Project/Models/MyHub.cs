using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace DevEnv_Semester_Project.Models
{
    public class MyHub : Hub
    {
        public void newEmployee()
        {
            string username = Context.User.Identity.Name;
            Clients.All.newEmployee(username);
        }
    }
}
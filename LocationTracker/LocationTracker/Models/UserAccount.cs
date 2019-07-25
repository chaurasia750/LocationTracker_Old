using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationTracker.Models
{
    public class UserAccount
    {
        public Guid ID { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Designation { get; set; }
        public string ECode { get; set; }
        public string Section { get; set; }
        public string FullDepartmentInfo { get; set; }
        public string MobileNumber { get; set; }

        public bool ReqPwdChange { get; set; }
        public string Journeys { get; set; }
        public string Tours { get; set; } = JsonConvert.SerializeObject(new List<Guid>());
        public string ParentIDs { get; set; }
        public string ChildIDs { get; set; }
    }
}

using System.Collections.Generic;
using Newtonsoft.Json;

namespace ServerApplication.Common.Models
{
    public class TeamLead : Worker
    {
        [JsonIgnore] private List<Report> _allReports = new List<Report>();

        public TeamLead(string name, bool isLeader = true) : base(name, isLeader)
        {
        }

        public List<Report> AllReports
        {
            get
            {
                if (IsTeamLead())
                {
                    return _allReports;
                }

                return null;
            }
            set
            {
                if (IsTeamLead())
                {
                    _allReports = value;
                }
            }
        }

        public override bool IsTeamLead()
        {
            return true;
        }
    }
}
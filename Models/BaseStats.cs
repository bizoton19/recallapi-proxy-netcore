using System;
using System.Collections;
using System.Collections.Generic;
using Opendata.Recalls.Commands;

namespace Opendata.Recalls.Stats
{
    public class BaseStats{
        public string Browser { get; set; }
        public string OS { get; set; }
        public string Device { get; set; }
        public DateTime Date { get; set; }
    }
    public class InstallInfo: BaseStats
    {
        
        public string InstallStatus {get;set;}
    }

    public class ErrorInfo: BaseStats{
        string Error {get;set;}
    }

     enum InstallStatus{Canceled, Success, Error, Unknown}

}
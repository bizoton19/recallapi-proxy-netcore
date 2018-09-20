using System;
using System.Net;
using System.Collections.Generic;


namespace Opendata.Recalls.Models{
    public class RecallComparer : IEqualityComparer<Recall>
    {
        public bool Equals(Recall recall1, Recall recall2)
        {
            if(recall1 == recall2)
                return true;
            if( recall1 == null || recall2 == null)
                return false;
            return recall1.RecallID == recall2.RecallID;
        }

        public int GetHashCode(Recall obj)
        {
            return obj != null ? obj.RecallID: 0;
        }
    }
}
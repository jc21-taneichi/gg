using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    class VEvent
    {
        public string DTSTART { get; set; }
        public string CODE { get; set; }
        public string SUMMARY { get; set; }
        public string ADDRESS { get; set; }
        public string LOCATION { get; set; }

        public static async Task<List<VEvent>> GetStringAsync(string uri)
        {
            using (var httpClient = new HttpClient())
            {


                string response =  await httpClient.GetStringAsync(uri);
                //Console.Out.WriteLine(response); //iCloud全データ表示


                List<VEvent> vEvents = SplitVEvent(response);
                List<VEvent> vEvents2 = new List<VEvent>();
                foreach (VEvent ve in vEvents)
                {
                    string date = ve.DTSTART;
                    string day = date.Substring(0, 8);

                    DateTime dtNow = DateTime.Now;

                    for (int i=0; i<=6; i++)
                    {
                        if (day == dtNow.AddDays(i).ToString("yyyyMMdd"))
                        {
                            vEvents2.Add(ve);
                        }
                    }
                }
                return vEvents2;
            }
        }

        private static  List<VEvent> SplitVEvent(string response)
        {
            string[] responses = response.Split('\r', '\n');
            List<VEvent> list = new List<VEvent>();
            int i = 0;
            while (true)
            {
                while (responses[i].Equals("BEGIN:VEVENT") == false)
                {
                    i++;
                    if (i >= responses.Length)
                    {
                        return list;
                    }
                }
                VEvent vEvent = new VEvent();
                while (responses[i].Equals("END:VEVENT") == false)
                {
                    string r = responses[i];
                    string s = Check(r, "DTSTART;TZID=Asia/Tokyo");
                    //string day = s.Substring(1,8);
                    if (s != null)
                    {
                        String inpStr = s;
                        String outStr = "";
                        int p = inpStr.IndexOf("T");
                        if (p < 0) p = inpStr.Length;
                        outStr = inpStr.Substring(0, p);
                        vEvent.DTSTART = outStr;
                    }


                    s = Check(r, "SUMMARY");
                    if (s != null)
                    {
                        vEvent.SUMMARY = s;
                    }


                    s = Check(r, "X-APPLE-STRUCTURED-LOCATION;VALUE=URI;X-ADDRESS=\"");
                    if (s != null)
                    {
                        vEvent.CODE = s.Substring(0, 8);
                    }



                    s = Check2(r, "LOCATION:");
                    if (s != null)
                    {
                        String inpStr = s;
                        String outStr = "";
                        int p = inpStr.IndexOf("\\");
                        if (p < 0) p = inpStr.Length;
                        outStr = inpStr.Substring(0, p);
                        vEvent.ADDRESS = outStr;
                    }


                    i++;
                    if (i >= responses.Length)
                    {
                        return list;
                    }
                }
                list.Add(vEvent);
            }
        }
        private static string Check(string r, string s)
        {
            if (r.IndexOf(s) >= 0)
            {
                return r.Substring(s.Length + 1);
            }
            return null;
        }
        private static  string Check2(string r, string s)
        {
            if (r.IndexOf(s) >= 0)
            {
                return r.Substring(s.Length + 0);
            }
            return null;
        }
    }
    
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Data.Html;


namespace TopTastic.Model
{
    public class BBCTop40PlaylistSource : IPlaylistSource
    {
        const string requestUri = @"http://www.bbc.co.uk/radio1/chart/singles/print";
        private HttpClient client;

        public BBCTop40PlaylistSource()
        {
            this.client = new HttpClient();
            this.client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)");
        }

        public async Task<PlaylistData> GetPlaylistAsync()
        {
            var response = await client.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            var html = await response.Content.ReadAsStringAsync();
            var playlistData = ExtractPlaylistData(html);
          
            return playlistData;
        }

        public static PlaylistData ExtractPlaylistData(string html)
        {
            var playlistData = new PlaylistData();
            playlistData.Items = new List<PlaylistDataItem>();
            playlistData.Description = "UK Top 40 singles: http://www.bbc.co.uk/radio1/chart/singles";
            playlistData.Title = string.Format("UK Top 40 {0:D} ", ExtractDate(html));
            playlistData.SearchKeys = new List<string>();

            var regexChartItem = new Regex(@"<td>(?<Position>.*?)</td>.*?<td>(?<Status>.*?)</td>.*?<td>(?<Previous>.*?)</td>.*?<td>(?<Weeks>.*?)</td>.*?<td>(?<Artist>.*?)</td>.*?<td>(?<Title>.*?)</td>", RegexOptions.Singleline);
            var matches = regexChartItem.Matches(html);

            foreach (Match m in matches)
            {
                var item = new PlaylistDataItem()
                {
                    Position = HtmlUtilities.ConvertToText(m.Groups["Position"].Value).ToInt(),
                    Status = HtmlUtilities.ConvertToText(m.Groups["Status"].Value),
                    Weeks = HtmlUtilities.ConvertToText(m.Groups["Weeks"].Value).ToInt(),
                    Previous = HtmlUtilities.ConvertToText(m.Groups["Previous"].Value).ToInt(),
                    Artist = HtmlUtilities.ConvertToText(m.Groups["Artist"].Value),
                    Title = HtmlUtilities.ConvertToText(m.Groups["Title"].Value)
                };

                playlistData.Items.Add(item);

                var searchKey = string.Format("{0} {1}", item.Artist, item.Title);
                playlistData.SearchKeys.Add(searchKey);
            }

            return playlistData;
        }


        public static IList<string> ExtractSearchKeys(string html)
        {
            var searchKeys = new List<string>();

            /*
               <th>Position</th>
               <th>Status</th>
               <th>Previous</th>
               <th>Weeks</th>
               <th>Artist</th>
               <th>Title</th>
            */
            var regexChartItem = new Regex(@"<td>(?<Position>.*?)</td>.*?<td>(?<Status>.*?)</td>.*?<td>(?<Previous>.*?)</td>.*?<td>(?<Weeks>.*?).*?<td>(?<Artist>.*?)</td>.*?<td>(?<Title>.*?)</td>", RegexOptions.Singleline);
            var matches = regexChartItem.Matches(html);

            foreach (Match m in matches)
            {
                string title = HtmlUtilities.ConvertToText(m.Groups["Title"].Value);
                string artist = HtmlUtilities.ConvertToText(m.Groups["Artist"].Value);

                var searchKey = string.Format("{0} {1}", artist, title);
                searchKeys.Add(searchKey);
            }

            return searchKeys;
        }

        public static DateTime ExtractDate(string html)
        {
            // <title>The Official UK Top 40 Singles Chart - 3rd November 2013</title>
            Regex regexDate = new Regex("<title>.+-(?<Date>.+?)</title>", RegexOptions.Singleline); 
            Match m = regexDate.Match(html);
       
            if (m.Success)
            {
                string dateValue = RemoveOrdinalsFromDateString(m.Groups["Date"].Value);

                DateTime date;
                if (DateTime.TryParse(dateValue, out date))
                {
                    return date;
                }
            }

            // Unable to extract date. Default to today
            return DateTime.Today;
        }

        public static string RemoveOrdinalsFromDateString(string dateString)
        {
            // 14th August 2015 
            Regex regexDateOrdinals = new Regex( "(.*)([0-9]+)(st|nd|rd|th)(.*)");

            string result = dateString;
            string replacement = "$1$2$4";
            
            Match m = regexDateOrdinals.Match(dateString);
            while (m.Success)
            {
                result = m.Result(replacement);
                m = regexDateOrdinals.Match(result);
            }

            return result;
        }
    }
}

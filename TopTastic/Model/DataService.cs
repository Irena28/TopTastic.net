﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopTastic.Model
{
    public class DataService : IDataService
    {
        public async void GetPlaylistData(Action<BBCTop40PlaylistData, Exception> callback)
        {
            var playlistSource = new BBCTop40PlaylistSource();
            BBCTop40PlaylistData playlistData = null;
            Exception err = null;

            try
            {
                playlistData = await playlistSource.GetPlaylistAsync();
            }
            catch(Exception ex)
            {
                err = ex;
            }

            callback(playlistData, err);
        }

        public void GetThumnails(Action<IList<string>, Exception> callback)
        {
            var testurl = @"https://yt3.ggpht.com/-sxaZFRBWPHU/AAAAAAAAAAI/AAAAAAAAAAA/XvrEJtXxRbQ/s88-c-k-no/photo.jpg";
            var testThumbs = new List<string>();
            for(int i=0; i<40; i++)
            {
                testThumbs.Add(testurl);
            }
            callback(testThumbs, null);
        }

    }
}

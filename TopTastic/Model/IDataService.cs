﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopTastic.Model
{
    public interface IDataService
    {
        void GetPlaylistData(Action<BBCTop40PlaylistData, Exception> callback);
        void GetVideoInfo(IPlaylistData playlistData, Action<IList<VideoInfo>, Exception> callback);
        void GetArtistInfo(string artistQuery, Action<string, Exception> callback);
        void GetYoutubeVideoUri(string videoId, Action<Uri, Exception> callback);
        void CreatePlaylist(IPlaylistData playlistData, Action<string, Exception> callback);
        void DownloadMedia(Uri videoUri, string artist, string title, bool extractAudio, Action <string, Exception> callback);
        void SearchYouTube(string searchString, Action<string, Exception> callback);
    }
}

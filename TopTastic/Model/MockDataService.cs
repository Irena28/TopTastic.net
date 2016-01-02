﻿using System;
using System.Collections.Generic;
using Windows.Storage;
using EchoNest;

namespace TopTastic.Model
{
    public class MockDataService : IDataService
    {
        public void CreatePlaylist(IPlaylistData playlistData, Action<string, Exception> callback)
        {
            throw new NotImplementedException();
        }

        public void DownloadMedia(Uri videoUri, string artist, string title, bool extractAudio, Action<string, Exception> callback)
        {
            throw new NotImplementedException();
        }

        public void GetArtistInfo(string artistQuery, Action<string, Exception> callback)
        {

            Exception ex = null;
            string artistInfo = null;

            try
            {
                var mockEchoNest = new Mock();
                artistInfo =  mockEchoNest.GetArtistInfo();
            }
            catch (Exception e)
            {
                ex = e;
            }

            callback(artistInfo, ex);
        }

        public async void GetPlaylistData(Action<BBCTop40PlaylistData, Exception> callback)
        {
            var testFileUri = new Uri("ms-appx:///Assets/TestChart.html");
            BBCTop40PlaylistData playlistData = null;
            Exception err = null;
            try
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(testFileUri);
                var html = await FileIO.ReadTextAsync(file);
                playlistData = BBCTop40PlaylistSource.ExtractPlaylistData(html);
            }
            catch(Exception ex)
            {
                err = ex;
            }
            callback(playlistData, err);
        }

        public void GetVideoInfo(IPlaylistData playlistData, Action<IList<VideoInfo>, Exception> callback)
        {
 
            var videoList = new List<VideoInfo>();
            Exception ex = null;

            try
            {
                int index = 0;
                foreach (var searchKey in playlistData.SearchKeys)
                {
                    videoList.Add(new VideoInfo(index++, "ms-appx:///Assets/p030kf95.jpg", "DK_0jXPuIr0"));
                }
            }
            catch (Exception e)
            {
                ex = e;
            }
            callback(videoList, ex);
        }

        public async void GetYoutubeVideoUri(string videoId, Action<Uri, Exception> callback)
        {
            Exception ex = null;
            Uri videoUri = null;

            try
            {
                videoUri = await YoutubeExtractor.DownloadUrlResolver.GetVideoUriAsync(videoId);
                //var youTubeUri = await YouTube.GetVideoUriAsync(videoId, YouTubeQuality.Quality720P);
                //videoUri = youTubeUri.Uri;
            }
            catch (Exception e)
            {
                ex = e;
            }

            callback(videoUri, ex);
        }
    }
}

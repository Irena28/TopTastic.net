﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace EchoNest.Song
{
    public static class SongBucketExtensions
    {
        public static IEnumerable<SongBucket> GetBuckets(this SongBucket bucket)
        {
            var buckets = bucket.ToString().Split(',');

            foreach (var s in buckets)
            {
                SongBucket parsed;
                if (Enum.TryParse(s.Trim(), out parsed))
                {
                    yield return parsed;
                }
            }
        }

        public static IEnumerable<string> GetBucketDescriptions(this SongBucket bucket)
        {
            var buckets = bucket.GetBuckets();

            foreach (var b in buckets)
            {
                yield return GetDescription(b);
            }
        }

        private static string GetDescription(Enum value)
        {
            return EnumHelpers.GetDescription(value);
        }
    }
}
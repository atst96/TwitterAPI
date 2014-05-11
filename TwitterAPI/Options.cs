using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterAPI
{
    public class StatusUpdateOptions
    {
        /// <summary>返信先のStatusIdを指定します</summary>
        [Parameters("in_reply_to_status_id")]
        public decimal InReplyToStatusId { get; set; }

        /// <summary>緯度</summary>
        [Parameters("lat")]
        public double Lat { get; set; }

        /// <summary>経度</summary>
        [Parameters("long")]
        public double? Long { get; set; }

        [Parameters("place_id")]
        public string PlaceId { get; set; }

        [Parameters("display_cordinates")]
        public bool? DisplayCoordinates { get; set; }
    }

    public class DirectMessageOptions : ParameterClass
    {
        [Parameters("since_id")]
        public decimal? SinceId { get; set; }

        [Parameters("max_id")]
        public decimal? MaxId { get; set; }

        [Parameters("count")]
        public int? Count { get; set; }

        [Parameters("include_entities")]
        public bool? IncludeEntities { get; set; }

        [Parameters("skip_status")]
        public bool? SkipStatus { get; set; }
    }
}

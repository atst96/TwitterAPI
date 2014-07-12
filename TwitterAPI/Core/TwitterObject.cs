using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.Serialization;

namespace TwitterAPI
{
    public enum RepliesType
    {
        Default,
        All
    }

    public enum WithType
    {
        User,
        Followings
    }

	public class StringArray : List<string>
	{
		public override string ToString()
		{
			return string.Join(",", this);
		}
	}

	public class CursorOption : ParameterClass
	{
		[Parameters("cursor")]
		public int? Cursor { get; set; }
	}

	[DataContract]
	public class UserIds
	{
		[DataMember(Name = "previous_cursor")]
		public int PreviousCursor { get; set; }

		[DataMember(Name = "ids")]
		public List<decimal> Ids { get; set; }

		[DataMember(Name = "previous_cursor_str")]
		public string StringPreviousCursor { get; set; }

		[DataMember(Name = "next_cursor")]
		public int NextCursor { get; set; }

		[DataMember(Name = "next_cursor_str")]
		public string StringNextCursor { get; set; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.Serialization;
using System.Reflection;
using System.Diagnostics;

namespace TwitterAPI
{
	[DataContract]
	public class TwitterError
	{
		[DataMember(Name = "message")]
		public string Message { get; set; }

		[DataMember(Name = "code")]
		public long ErrorCode { get; set; }
	}
}
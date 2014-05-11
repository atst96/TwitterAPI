using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace TwitterAPI.Core
{
    class ITwitterObject
    {
        
    }

	public abstract class Config
	{
		public static IWebProxy Proxy = WebRequest.GetSystemWebProxy();
	}
}

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
}

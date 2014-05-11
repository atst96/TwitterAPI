using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterAPI
{
    public abstract partial class TwitterAccount
    {
        /*----------/  URLs  /----------*/
        private static string Url_Verify_Credentials = "https://api.twitter.com/1.1/account/verify_credentials.json";

		private static string Url_Users_Show = "https://api.twitter.com/1.1/users/show.json";
	}
}

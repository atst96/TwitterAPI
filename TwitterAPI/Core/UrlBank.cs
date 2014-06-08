using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwitterAPI
{
    public abstract class UrlBank
    {
        //   Timelines

		/// <summary>
		/// GET statuses/mentions_timeline 
		/// </summary>
        public static readonly string HomeTimeline = "https://api.twitter.com/1.1/statuses/home_timeline.json";

		/// <summary>
		/// GET statuses/user_timeline 
		/// </summary>
		public static readonly string UserTimeline = "https://api.twitter.com/1.1/statuses/user_timeline.json";

		/// <summary>
		/// GET statuses/home_timeline 
		/// </summary>
		public static readonly string MentionsTimeline = "https://api.twitter.com/1.1/statuses/mentions_timeline.json";
		
		/// <summary>
		/// GET statuses/retweets_of_me 
		/// </summary>
		public static readonly string RetweetsOfMe = "https://api.twitter.com/1.1/statuses/retweets_of_me.json";






		// Tweets

		/// <summary>
		/// GET statuses/retweets/:id 
		/// </summary>
		public static readonly string StatusesRetweets = "https://api.twitter.com/1.1/statuses/retweets/{0}.json";

		/// <summary>
		/// GET statuses/show/:id 
		/// </summary>
		public static readonly string StatusesShow = "https://api.twitter.com/1.1/statuses/show.json?id={0}";

		/// <summary>
		/// POST statuses/destroy/:id 
		/// </summary>
		public static readonly string StatusesDestroy = "https://api.twitter.com/1.1/statuses/destroy/{0}.json";

		/// <summary>
		/// POST statuses/update 
		/// </summary>
		public static readonly string StatusesUpdate = "https://api.twitter.com/1.1/statuses/update.json";

		/// <summary>
		/// POST statuses/retweet/:id 
		/// </summary>
		public static readonly string StatusesRetweet = "https://api.twitter.com/1.1/statuses/retweet/{0}.json";

		/// <summary>
		/// POST statuses/update_with_media 
		/// </summary>
		public static readonly string StatusesUpdateWithMedia = "https://api.twitter.com/1.1/statuses/update_with_media.json";
		
		/// <summary>
		/// GET statuses/oembed 
		/// </summary>
		public static readonly string StatusesOembed = "https://api.twitter.com/1.1/statuses/oembed.json";
		
		/// <summary>
		/// GET statuses/retweeters/ids 
		/// </summary>
		public static readonly string StatusesRetweeters = "https://api.twitter.com/1.1/statuses/retweeters/ids.json";






		// Search

		/// <summary>
		/// GET search/tweets 
		/// </summary>
		public static readonly string SearchTweets = "https://api.twitter.com/1.1/search/tweets.json";






		// Streaming

		/// <summary>
		/// POST statuses/filter 
		/// </summary>
		public static readonly string StatusesFilter = "https://stream.twitter.com/1.1/statuses/filter.json";
		
		/// <summary>
		/// GET statuses/sample 
		/// </summary>
		public static readonly string StatusesSample = "https://stream.twitter.com/1.1/statuses/sample.json";
		
		/// <summary>
		/// GET statuses/firehose 
		/// </summary>
		public static readonly string StatusesFirehouse = "https://stream.twitter.com/1.1/statuses/firehose.json";
		
		/// <summary>
		/// GET user 
		/// </summary>
		public static readonly string UserStream = "https://userstream.twitter.com/1.1/user.json";
		
		/// <summary>
		/// GET site 
		/// </summary>
		public static readonly string SiteStream = "https://sitestream.twitter.com/1.1/site.json";






		// DirectMessage

		/// <summary>
		/// GET direct_messages 
		/// </summary>
        public static readonly string DirectMessages = "https://api.twitter.com/1.1/direct_messages.json";
		
		/// <summary>
		/// GET direct_messages/sent 
		/// </summary>
		public static readonly string DirectMessagesSent = "https://api.twitter.com/1.1/direct_messages/sent.json";
		
		/// <summary>
		/// GET direct_messages/show
		/// </summary>
		public static readonly string DirectMessageShow = "https://api.twitter.com/1.1/direct_messages/show.json?id={0}";
		
		/// <summary>
		/// POST direct_messages/destroy 
		/// </summary>
		public static readonly string DirectMessagesDestroy = "https://api.twitter.com/1.1/direct_messages/destroy.json";
		
		/// <summary>
		/// POST direct_messages/new 
		/// </summary>
		public static readonly string DirectMessagesNew = "https://api.twitter.com/1.1/direct_messages/new.json";






		// Friends & Followers

		/// <summary>
		/// GET friendships/no_retweets/ids 
		/// </summary>
		public static readonly string FriendshipsNoRetweetsIds = "https://api.twitter.com/1.1/friendships/no_retweets/ids.json";
		
		/// <summary>
		/// GET friends/ids 
		/// </summary>
		public static readonly string FriendsIds = "https://api.twitter.com/1.1/friends/ids.json";
		
		/// <summary>
		/// GET followers/ids 
		/// </summary>
		public static readonly string FollowersIds = "https://api.twitter.com/1.1/followers/ids.json";
		
		/// <summary>
		/// GET friendships/incoming 
		/// </summary>
		public static readonly string FriendshipsIncoming = "https://api.twitter.com/1.1/friendships/incoming.json";
		
		/// <summary>
		/// GET friendships/outgoing 
		/// </summary>
		public static readonly string FriendshiptsOutgoing = "https://api.twitter.com/1.1/friendships/outgoing.format";
		
		/// <summary>
		/// POST friendships/create 
		/// </summary>
		public static readonly string FriendshipsCreate = "https://api.twitter.com/1.1/friendships/create.json";
		
		/// <summary>
		/// POST friendships/destroy 
		/// </summary>
		public static readonly string FriendshipsDestroy = "https://api.twitter.com/1.1/friendships/destroy.json";
		
		/// <summary>
		/// POST friendships/update 
		/// </summary>
		public static readonly string FriendshipsUpdate = "https://api.twitter.com/1.1/friendships/update.json";
		
		/// <summary>
		/// GET friendships/show 
		/// </summary>
		public static readonly string FriendshipsShow = "https://api.twitter.com/1.1/friendships/show.json";
		
		/// <summary>
		/// GET friends/list 
		/// </summary>
		public static readonly string FriendsList = "https://api.twitter.com/1.1/friends/list.json";
		
		/// <summary>
		/// GET followers/list 
		/// </summary>
		public static readonly string FollowersList = "https://api.twitter.com/1.1/followers/list.json";
		
		/// <summary>
		/// GET friendships/lookup 
		/// </summary>
		public static readonly string FriendshipsLookup = "https://api.twitter.com/1.1/friendships/lookup.json";






		// Users

		/// <summary>
		/// GET account/settings 
		/// POST account/settings 
		/// </summary>
		public static readonly string AccountSettings = "https://api.twitter.com/1.1/account/settings.json";
		
		/// <summary>
		/// GET account/verify_credentials 
		/// </summary>
		public static readonly string AccountVerifyCredentails = "https://api.twitter.com/1.1/account/verify_credentials.json";
		
		/// <summary>
		/// POST account/update_delivery_device 
		/// </summary>
		public static readonly string AccountUpdateDeliveryDevice = "https://api.twitter.com/1.1/account/update_delivery_device.json";
		
		/// <summary>
		/// POST account/update_profile 
		/// </summary>
		public static readonly string AccountUpdateProfile = "https://api.twitter.com/1.1/account/update_profile.json";
		
		/// <summary>
		/// POST account/update_profile_background_image 
		/// </summary>
		public static readonly string AccountUpdateProfileBackgroundImage = "https://api.twitter.com/1.1/account/update_profile_background_image.json";
		
		/// <summary>
		/// POST account/update_profile_colors 
		/// </summary>
		public static readonly string AccountUpdateProfileColors = "https://api.twitter.com/1.1/account/update_profile_colors.json";
		
		/// <summary>
		/// POST account/update_profile_image 
		/// </summary>
		public static readonly string AccountUpdateProfileImage = "https://api.twitter.com/1.1/account/update_profile_image.json";
		
		/// <summary>
		/// GET blocks/list 
		/// </summary>
		public static readonly string BlocksList = "https://api.twitter.com/1.1/blocks/list.json";
		
		/// <summary>
		/// GET blocks/ids 
		/// </summary>
		public static readonly string BlocksIds = "https://api.twitter.com/1.1/blocks/ids.json";
		
		/// <summary>
		/// POST blocks/create 
		/// </summary>
		public static readonly string BlocksCreate = "https://api.twitter.com/1.1/blocks/create.json";
		
		/// <summary>
		/// POST blocks/destroy 
		/// </summary>
		public static readonly string BlocksDestroy = "https://api.twitter.com/1.1/blocks/destroy.json";
		
		/// <summary>
		/// GET users/lookup 
		/// </summary>
		public static readonly string UsersLookup = "https://api.twitter.com/1.1/users/lookup.json";
		
		/// <summary>
		/// GET users/show 
		/// </summary>
		public static readonly string UsersShow = "https://api.twitter.com/1.1/users/show.json";
		
		/// <summary>
		/// GET users/search 
		/// </summary>
		public static readonly string UsersSearch = "https://api.twitter.com/1.1/users/search.json";
		
		/// <summary>
		/// GET users/contributees 
		/// </summary>
		public static readonly string UsersContributees = "https://api.twitter.com/1.1/users/contributees.json";
		
		/// <summary>
		/// GET users/contributors 
		/// </summary>
		public static readonly string UsersContributors = "https://api.twitter.com/1.1/users/contributors.json";
		
		/// <summary>
		/// POST account/remove_profile_banner 
		/// </summary>
		public static readonly string AccountRemoveProfileBanner = "https://api.twitter.com/1.1/account/remove_profile_banner.json";
		
		/// <summary>
		/// POST account/update_profile_banner 
		/// </summary>
		public static readonly string AccountUpdateProfileBanner = "https://api.twitter.com/1.1/account/update_profile_banner.json";
		
		/// <summary>
		/// GET users/profile_banner 
		/// </summary>
		public static readonly string UsersProfileBanner = "https://api.twitter.com/1.1/users/profile_banner.json";
		
		/// <summary>
		/// POST mutes/users/create 
		/// </summary>
		public static readonly string MutesUsersCreate = "https://api.twitter.com/1.1/mutes/users/create.json";
		
		/// <summary>
		/// POST mutes/users/destroy 
		/// </summary>
		public static readonly string MutesUsersDestroy = "https://api.twitter.com/1.1/mutes/users/destroy.json";
		
		/// <summary>
		/// GET mutes/users/ids 
		/// </summary>
		public static readonly string MutesUsersIds = "https://api.twitter.com/1.1/mutes/users/ids.json";
		
		/// <summary>
		/// GET mutes/users/list 
		/// </summary>
		public static readonly string MutesUsersList = "https://api.twitter.com/1.1/mutes/users/list.json";






		// Suggested Users

		/// <summary>
		/// GET users/suggestions/:slug 
		/// </summary>
		public static readonly string UsersSuggestionsSlug = "https://api.twitter.com/1.1/users/suggestions/{0}.json";
		
		/// <summary>
		/// GET users/suggestions 
		/// </summary>
		public static readonly string UsersSuggestions = "https://api.twitter.com/1.1/users/suggestions.format";
		
		/// <summary>
		/// GET users/suggestions/:slug/members 
		/// </summary>
		public static readonly string UsersSuggestionsSlugMembers = "https://api.twitter.com/1.1/users/suggestions/{0}/members.json";






		// Favorites

		/// <summary>
		/// GET favorites/list 
		/// </summary>
		public static readonly string FavoritesList = "https://api.twitter.com/1.1/favorites/list.json";
		
		/// <summary>
		/// POST favorites/destroy 
		/// </summary>
		public static readonly string FavoritesDestroy = "https://api.twitter.com/1.1/favorites/destroy.json";
		
		/// <summary>
		/// POST favorites/create 
		/// </summary>
		public static readonly string FavoritesCreate = "https://api.twitter.com/1.1/favorites/create.json";






		// Lists

		/// <summary>
		/// GET lists/list 
		/// </summary>
		public static readonly string ListsList = "https://api.twitter.com/1.1/lists/list.json";
		
		/// <summary>
		/// GET lists/statuses 
		/// </summary>
		public static readonly string ListsStatuses = "https://api.twitter.com/1.1/lists/statuses.json";
		
		/// <summary>
		/// POST lists/members/destroy 
		/// </summary>
		public static readonly string ListsMembersDestroy = "https://api.twitter.com/1.1/lists/members/destroy.json";
		
		/// <summary>
		/// GET lists/memberships 
		/// </summary>
		public static readonly string ListsMemberships = "https://api.twitter.com/1.1/lists/memberships.json";
		
		/// <summary>
		/// GET lists/subscribers 
		/// </summary>
		public static readonly string ListsSubscrivers = "https://api.twitter.com/1.1/lists/subscribers.json";
		
		/// <summary>
		/// POST lists/subscribers/create 
		/// </summary>
		public static readonly string ListsSubscriversCreate = "https://api.twitter.com/1.1/lists/subscribers/create.json";
		
		/// <summary>
		/// GET lists/subscribers/show 
		/// </summary>
		public static readonly string ListsSubscriversShow = "https://api.twitter.com/1.1/lists/subscribers/show.json";
		
		/// <summary>
		/// POST lists/subscribers/destroy 
		/// </summary>
		public static readonly string ListsSubscriversDestroy = "https://api.twitter.com/1.1/lists/subscribers/destroy.json";
		
		/// <summary>
		/// POST lists/members/create_all 
		/// </summary>
		public static readonly string ListsMembersCreateAll = "https://api.twitter.com/1.1/lists/members/create_all.json";
		
		/// <summary>
		/// GET lists/members/show 
		/// </summary>
		public static readonly string ListsMembersShow = "https://api.twitter.com/1.1/lists/members/show.json";
		
		/// <summary>
		/// GET lists/members 
		/// </summary>
		public static readonly string ListsMembers = "https://api.twitter.com/1.1/lists/members.json";
		
		/// <summary>
		/// POST lists/members/create 
		/// </summary>
		public static readonly string ListsMembersCreate = "https://api.twitter.com/1.1/lists/members/create.json";
		
		/// <summary>
		/// POST lists/destroy 
		/// </summary>
		public static readonly string ListsDestroy = "https://api.twitter.com/1.1/lists/destroy.json";
		
		/// <summary>
		/// POST lists/update 
		/// </summary>
		public static readonly string ListsUpdate = "https://api.twitter.com/1.1/lists/update.json";
		
		/// <summary>
		/// POST lists/create 
		/// </summary>
		public static readonly string ListsCreate = "https://api.twitter.com/1.1/lists/create.json";
		
		/// <summary>
		/// GET lists/show 
		/// </summary>
		public static readonly string ListsShow = "https://api.twitter.com/1.1/lists/show.json";
		
		/// <summary>
		/// GET lists/subscriptions 
		/// </summary>
		public static readonly string ListsSubscriptions = "https://api.twitter.com/1.1/lists/subscriptions.json";
		
		/// <summary>
		/// POST lists/members/destroy_all 
		/// </summary>
		public static readonly string ListsMembersDestroyAll = "https://api.twitter.com/1.1/lists/members/destroy_all.json";
		
		/// <summary>
		/// GET lists/ownerships 
		/// </summary>
		public static readonly string ListsOwnerships = "https://api.twitter.com/1.1/lists/ownerships.json";






		// Saved Searches

		/// <summary>
		/// GET saved_searches/list 
		/// </summary>
		public static readonly string SavedSearchesList = "https://api.twitter.com/1.1/saved_searches/list.json";
		
		/// <summary>
		/// GET saved_searches/show/:id 
		/// </summary>
		public static readonly string SavedSearchesShow = "https://api.twitter.com/1.1/saved_searches/show/{0}.json";
		
		/// <summary>
		/// POST saved_searches/create 
		/// </summary>
		public static readonly string SavedSearchesCreate = "https://api.twitter.com/1.1/saved_searches/create.json";
		
		/// <summary>
		/// POST saved_searches/destroy/:id 
		/// </summary>
		public static readonly string SavedSearchesDestroy = "https://api.twitter.com/1.1/saved_searches/destroy/{0}.json";






		// Places & Geo

		/// <summary>
		/// GET geo/id/:place_id 
		/// </summary>
		public static readonly string GeoId = "https://api.twitter.com/1.1/geo/id/{0}.json";
		
		/// <summary>
		/// GET geo/reverse_geocode 
		/// </summary>
		public static readonly string GeoReverseGeoCode = "https://api.twitter.com/1.1/geo/reverse_geocode.json";
		
		/// <summary>
		/// GET geo/search 
		/// </summary>
		public static readonly string GeoSearch = "https://api.twitter.com/1.1/geo/search.json";
		
		/// <summary>
		/// GET geo/similar_places 
		/// </summary>
		public static readonly string GeoSimilarPlaces = "https://api.twitter.com/1.1/geo/similar_places.json";
		
		/// <summary>
		/// POST geo/place 
		/// </summary>
		public static readonly string GeoPlace = "https://api.twitter.com/1.1/geo/place.json";






		// Trends

		/// <summary>
		/// GET trends/place 
		/// </summary>
		public static readonly string TrendsPlace = "https://api.twitter.com/1.1/trends/place.json";
		
		/// <summary>
		/// GET trends/available 
		/// </summary>
		public static readonly string TrendsAvailable = "https://api.twitter.com/1.1/trends/available.json";
		
		/// <summary>
		/// GET trends/closest 
		/// </summary>
		public static readonly string TrendsClosest = "https://api.twitter.com/1.1/trends/closest.json";






		// Spam Reporting

		/// <summary>
		/// POST users/report_spam 
		/// </summary>
		public static readonly string UsersReprotSpam = "https://api.twitter.com/1.1/users/report_spam.json";






		// OAuth

		/// <summary>
		/// GET oauth/authenticate 
		/// </summary>
		public static readonly string OAuthAuthenticate = "https://api.twitter.com/oauth/authenticate";

		/// <summary>
		/// GET oauth/authorize 
		/// </summary>
		public static readonly string OAuthAuthorize = "https://api.twitter.com/oauth/authorize";

		/// <summary>
		/// POST oauth/access_token 
		/// </summary>
		public static readonly string OAuthAccessToken = "https://api.twitter.com/oauth/access_token";

		/// <summary>
		/// POST oauth/request_token 
		/// </summary>
		public static readonly string OAuthRequestToken = "https://api.twitter.com/oauth/request_token";

		/// <summary>
		/// POST oauth2/token 
		/// </summary>
		public static readonly string OAuth2Token = "https://api.twitter.com/oauth2/token";

		/// <summary>
		/// POST oauth2/invalidate_token 
		/// </summary>
		public static readonly string OAuth2InvalidateToken = "https://api.twitter.com/oauth2/invalidate_token";






		// Help
		
		/// <summary>
		/// GET help/configuration 
		/// </summary>
		public static readonly string HelpConfiguration = "https://api.twitter.com/1.1/help/configuration.json";
		
		/// <summary>
		/// GET help/languages 
		/// </summary>
		public static readonly string HelpLanguages = "https://api.twitter.com/1.1/help/languages.json";
		
		/// <summary>
		/// GET help/privacy 
		/// </summary>
		public static readonly string HelpPrivacy = "https://api.twitter.com/1.1/help/privacy.json";
		
		/// <summmary>
		/// GET help/tos 
		/// </summary>
		public static readonly string HelpTos = "https://api.twitter.com/1.1/help/tos.json";
		
		/// <summary>
		/// GET application/rate_limit_status 
		/// </summary>
		public static readonly string ApplicationRateLimitStatus = "https://api.twitter.com/1.1/application/rate_limit_status.json";






		// Tweets

		/// <summary>
		/// GET statuses/lookup 
		/// </summary>
		public static readonly string StatusesLookup = "https://api.twitter.com/1.1/statuses/lookup.json";

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToTwitter;
using SociOLRoom.Analytics.Core;


namespace TwitterListener
{
    public class TwitterService
    {
        private TwitterContext _twitterCtx;
        public event EventHandler<GenericEventArgs<Status>> OnNewMessage;

        static IAuthorizer DoApplicationOnlyAuth()
        {
            var auth = new SingleUserAuthorizer
            {
                CredentialStore = new SingleUserInMemoryCredentialStore
                {
                    ConsumerKey = Config.Twitter.ConsumerKey,
                    ConsumerSecret = Config.Twitter.ConsumerSecret,
                    AccessToken = Config.Twitter.AccessToken,
                    AccessTokenSecret = Config.Twitter.AccessTokenSecret
                }
            };

            return auth;
        }

        public async Task Authenticate()
        {
            var auth = DoApplicationOnlyAuth();

            await auth.AuthorizeAsync();
            _twitterCtx = new TwitterContext(auth);
        }

        public async Task GetStreamTweets(string tag)
        {


            await
                (from strm in _twitterCtx.Streaming
                 where strm.Type == StreamingType.Filter &&
                           strm.Track == tag
                 select strm)
                    .StartAsync(strm =>
                    {
                        if (strm.EntityType == StreamEntityType.Status)
                        {
                            var status = strm.Entity as Status;
                                if (status.Entities != null)
                            {
                                if (status.Entities.MediaEntities != null && status.Entities.MediaEntities.Count > 0 && status.RetweetedStatus.CreatedAt == DateTime.MinValue)
                                {
                                    OnNewMessage?.Invoke(this, new GenericEventArgs<Status> { Data = status });
                                }
                            }
                        }
                        return Task.FromResult(0);
                    });

        }

        public async Task<List<Status>> GetTweets(string tag, ulong sinceId = 0)
        {
            var auth = DoApplicationOnlyAuth();

            await auth.AuthorizeAsync();


            List<Status> tweets = new List<Status>();

            using (var twitterCtx = new TwitterContext(auth))
            {

                Search searchResponse = await (twitterCtx.Search.Where(search =>
                    search.Type == SearchType.Search && search.Query == "#" + tag + " -filter:retweets"
                    && search.ResultType == ResultType.Recent && search.SinceID == sinceId
                    && search.IncludeEntities == true)).SingleOrDefaultAsync();

                if (searchResponse != null && searchResponse.Statuses != null)
                //searchResponse.Statuses.ForEach(tweet => tweet.Entities.
                {
                    foreach (var tweet in searchResponse.Statuses)
                    {
                        if (tweet.Entities != null)
                        {
                            if (tweet.Entities.MediaEntities != null && tweet.Entities.MediaEntities.Count > 0 && tweet.RetweetedStatus.CreatedAt == DateTime.MinValue)
                            {
                                tweets.Add(tweet);
                            }
                        }
                    }
                }


            }
            return tweets;
        }
    }
}
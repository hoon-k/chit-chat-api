namespace ChitChatAPI.DiscussionAPI.DataModel
{
    public class PostRequest
    {
        public string Body { get; set; }

        public string AuthorID { get; set; }

        public string TopicTitle { get; set; }

        public string TopicID { get; set; }
    }
}
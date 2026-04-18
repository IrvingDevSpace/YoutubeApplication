namespace YoutubeApplication.Enums
{
    public enum RatingTag
    {
        None,
        Like,
        Dislike
    }

    public static class RatingTagExtensions
    {
        public static string ToApiString(this RatingTag rating)
        {
            return rating.ToString().ToLower();
        }

        public static RatingTag ToRatingTag(this string? rating)
        {
            return rating?.ToLower() switch
            {
                "like" => RatingTag.Like,
                "dislike" => RatingTag.Dislike,
                _ => RatingTag.None
            };
        }
    }
}

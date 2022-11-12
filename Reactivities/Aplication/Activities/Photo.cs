namespace Reactivities.Aplication.Activities
{
    public class Photo
    {
        //This [Id] will be the [Public] [Id] that will [get back] from [Cloudinary]
        public string Id { get; set; }
        public string Url { get; set; }
        //The [IsMain] is to [Check] [if it's] the [User] [Main Photo]
        public bool IsMain { get; set; }
    }
}

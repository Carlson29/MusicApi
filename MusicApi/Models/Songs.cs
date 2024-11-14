namespace MusicApi.Models
{
    public class Songs
    {
        public int Song_Id { get; set; }
        public string Title {  get; set; }
        public string Genre { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime ReleaseDate { get; set; }

    }
}

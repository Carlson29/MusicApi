﻿namespace MusicApi.Models
{
    public class SongsDto
    {
        public string Title { get; set; }
    
        public string Genre { get; set; }
        public TimeSpan Duration { get; set; }

        public DateTime? ReleaseDate { get; set; }
    }
}

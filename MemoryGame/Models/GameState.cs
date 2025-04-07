using System;
using System.Collections.Generic;

namespace MemoryGame.Models
{
    public class GameState
    {
        public string PlayerName { get; set; }
        public int Category { get; set; }

        public int Rows { get; set; }

        public int Columns { get; set; }

        public int TotalTime { get; set; }

        public int RemainingTime { get; set; }
        public DateTime SavedAt { get; set; }


        [System.Text.Json.Serialization.JsonIgnore]
        public string FilePath { get; set; }

        public bool IsCompleted { get; set; }
        public List<CardState> Cards { get; set; } = new List<CardState>();

        [System.Text.Json.Serialization.JsonIgnore]
        public string GameMode
        {
            get
            {
                return (Rows == 4 && Columns == 4) ? "Standard" : "Custom";
            }
        }
    }

    public class CardState
    {
        public int Id { get; set; }

        public int Value { get; set; }

        public string ImagePath { get; set; }

        public bool IsFlipped { get; set; } 
        public bool IsMatched { get; set; }

        public int Position { get; set; }
    }
}
﻿using System;

namespace Blazor20Questions.Shared
{
    public class GameResponse
    {
        /// <summary>
        /// Unique ID of the Game
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Whether the Guesser(s) won the game
        /// </summary>
        public bool Won { get; set; }

        /// <summary>
        /// Whether the game has finished
        /// </summary>
        public bool Complete { get; set; }

        /// <summary>
        /// Time that the game expires
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Number of questions that can still be asked
        /// </summary>
        public int QuestionsRemaining { get; set; }

        /// <summary>
        /// Whether guesses count as questions
        /// </summary>
        public bool GuessesCountAsQuestions { get; set; }

        /// <summary>
        /// Can ask more than one question at a time
        /// </summary>
        public bool AllowConcurrentQuestions { get; set; }

        /// <summary>
        /// What the whole game is about!
        /// </summary>
        /// <remarks>
        /// Only populated if game is over
        /// </remarks>
        public string Subject { get; set; }
    }
}

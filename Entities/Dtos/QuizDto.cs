﻿using Entities.Models;

namespace Entities.Dtos
{
    public class QuizDto 
    {
        public string Title { get; set; }
        public int QuizId { get; set; }
        public ICollection<QuestionDto> Questions { get; set; }
        public DateTime CreatedDate { get; set; }
        public int QuestionCount { get; set; }
        public bool ShowCase { get; set; }
    }
}
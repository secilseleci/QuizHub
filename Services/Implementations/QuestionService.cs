using AutoMapper;
using Entities.Dtos;
using Entities.Exeptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services.Implemantations
{
    public class QuestionService : IQuestionService
    {

        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;

        public QuestionService(IRepositoryManager manager,
        IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }

        public async Task<ResultGeneric<Question>> CreateOneQuestion(QuestionDto questionDto)
        {
            var question = _mapper.Map<Question>(questionDto);
            await _manager.Question.CreateAsync(question);
            var saveResult = await _manager.SaveAsync();
            if (!saveResult)
            {
                return ResultGeneric<Question>.Fail("Soru kaydedilemedi.", "Bir hata oluştu. Lütfen tekrar deneyin.");
            }
            return ResultGeneric<Question>.Ok(question);
        }

        public async Task<Result> DeleteOneQuestion(int id)
        {
            var question = await _manager.Question.GetOneQuestionAsync(id, trackChanges: false);
            if (question == null)
            {
                return Result.Fail("Soru bulunamadı.", "Silmek istediğiniz soru mevcut değil.");
            }

            await _manager.Question.DeleteOneQuestionAsync(question);
            var saveResult = await _manager.SaveAsync();
            if (!saveResult)
            {
                return Result.Fail("Soru silinemedi.", "Bir hata oluştu. Lütfen tekrar deneyin.");
            }

            return Result.Ok();
        }

        public async Task<ResultGeneric<IEnumerable<Question>>> GetAllQuestions(bool trackChanges)
        {
            var questions = await _manager.Question.GetAllQuestionsAsync(trackChanges);
            if (questions == null || !questions.Any())
            {
                return ResultGeneric<IEnumerable<Question>>.Fail("Soru bulunamadı.", "Henüz kayıtlı soru yok.");
            }
            return ResultGeneric<IEnumerable<Question>>.Ok(questions);
        }


        public async Task<ResultGeneric<Question>> GetOneQuestion(int id, bool trackChanges)
        {
            var question = await _manager.Question.GetOneQuestionAsync(id, trackChanges);
            if (question == null)
            {
                return ResultGeneric<Question>.Fail("Soru bulunamadı.", "Aradığınız soru mevcut değil.");
            }
            return ResultGeneric<Question>.Ok(question);
        }
        public async Task<ResultGeneric<Question>> UpdateOneQuestion(QuestionDto questionDto)
        {
            var question = await _manager.Question.GetOneQuestionAsync(questionDto.QuestionId, trackChanges: true);
            if (question == null)
            {
                return ResultGeneric<Question>.Fail("Soru bulunamadı.", "Güncellemek istediğiniz soru mevcut değil.");
            }
            _mapper.Map(questionDto, question);

            var saveResult = await _manager.SaveAsync();
            if (!saveResult)
            {
                return ResultGeneric<Question>.Fail("Soru güncellenemedi.", "Bir hata oluştu. Lütfen tekrar deneyin.");
            }

            return ResultGeneric<Question>.Ok(question);
        }

        public async Task<ResultGeneric<IEnumerable<Question>>> GetQuestionsByQuizId(int quizId, bool trackChanges)
        {
            var questions = await _manager.Question.GetQuestionsByQuizIdAsync(quizId, trackChanges);
            if (questions == null || !questions.Any())
            {
                return ResultGeneric<IEnumerable<Question>>.Fail("Quiz sorusu bulunamadı.", "Bu quiz ile ilişkili soru mevcut değil.");
            }
            return ResultGeneric<IEnumerable<Question>>.Ok(questions);
        }

        public async Task<ResultGeneric<Question>> GetOneQuestionWithOptions(int id, bool trackChanges)
        {
            var question = await _manager.Question.GetOneQuestionWithOptionsAsync(id, trackChanges);
            if (question == null)
            {
                return ResultGeneric<Question>.Fail("Soru ve seçenekleri bulunamadı.", "Aradığınız soru mevcut değil.");
            }
            return ResultGeneric<Question>.Ok(question);
        }

        public async Task<ResultGeneric<QuestionDto>> GetNextQuestion(int quizId, int currentQuestionOrder, int selectedOptionId)
        {
            var quizResult = await _manager.Quiz.GetQuizWithDetailsAsync(quizId, trackChanges: false);
            if (quizResult == null)
            {
                return ResultGeneric<QuestionDto>.Fail("Quiz bulunamadı.");
            }

            int totalQuestions = quizResult.Questions.Count;

            var nextQuestion = await _manager.Question.GetNextQuestionByQuizIdAsync(quizId, currentQuestionOrder, trackChanges: false);
            bool isLastQuestion = nextQuestion == null;

            var questionToShow = isLastQuestion
                ? await _manager.Question.GetLastQuestionByQuizIdAsync(quizId, trackChanges: false)
                : nextQuestion;

            var questionDto = _mapper.Map<QuestionDto>(questionToShow);

            if (isLastQuestion)
            {
                foreach (var option in questionDto.Options)
                {
                    option.IsSelected = option.OptionId == selectedOptionId;
                    option.IsDisabled = true;
                }
            }
            questionDto.QuestionCount = totalQuestions;
            questionDto.IsLastQuestion = isLastQuestion;

            return ResultGeneric<QuestionDto>.Ok(questionDto);
        }


    }
}


using AutoMapper;
using Entities.Exeptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services.Implemantations
{
    public class UserQuizInfoService : IUserQuizInfoService
    {
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;
        private readonly IUserAnswerService _userAnswerService;
        private readonly IUserQuizInfoTempService _userQuizInfoTempService;

        public UserQuizInfoService(
            IRepositoryManager manager,
            IMapper mapper,
            IUserAnswerService userAnswerService,
            IUserQuizInfoTempService userQuizInfoTempService)
        {
            _manager = manager;
            _mapper = mapper;
            _userAnswerService = userAnswerService;
            _userQuizInfoTempService = userQuizInfoTempService;
        }

        public async Task<ResultGeneric<UserQuizInfo>> GetUserQuizInfoByQuizIdAndUserId(int quizId, string userId, bool trackChanges)
        {
            var userQuizInfo = await _manager.UserQuizInfo.GetUserQuizInfoByQuizIdAndUserIdAsync(quizId, userId, trackChanges);
            if (userQuizInfo == null)
            {
                return ResultGeneric<UserQuizInfo>.Fail("Quiz bilgisi bulunamadı.", "Kullanıcı ve quiz ID'si ile ilgili veri bulunamadı.");
            }
            return ResultGeneric<UserQuizInfo>.Ok(userQuizInfo);
        }

        public async Task<ResultGeneric<IEnumerable<UserQuizInfo>>> GetUserQuizInfoByUserId(string userId, bool trackChanges)
        {
            var userQuizInfos = await _manager.UserQuizInfo.GetUserQuizInfoByUserIdAsync(userId, trackChanges);
            return userQuizInfos.Any()
                ? ResultGeneric<IEnumerable<UserQuizInfo>>.Ok(userQuizInfos)
                : ResultGeneric<IEnumerable<UserQuizInfo>>.Fail("Kullanıcı quiz bilgisi bulunamadı.", "Kullanıcı ID'si ile ilgili veri bulunamadı.");
        }

        public async Task<Result> AssignQuizToUsers(int quizId, List<string> userIds)
        {
            foreach (var userId in userIds)
            {
                var existingAssignment = await _manager.UserQuizInfo.GetUserQuizInfoByQuizIdAndUserIdAsync(quizId, userId, false);

                if (existingAssignment == null)
                {
                    var assignment = new UserQuizInfo
                    {
                        QuizId = quizId,
                        UserId = userId,
                        IsCompleted = false,
                        Score = 0,
                        CorrectAnswer = 0,
                        FalseAnswer = 0,
                    };
                    await _manager.UserQuizInfo.CreateOneUserQuizInfoAsync(assignment);
                }
            }

            var saveResult = await _manager.SaveAsync();
            return saveResult ? Result.Ok() : Result.Fail("Quiz kullanıcıya atanamadı.", "Bir hata oluştu. Lütfen tekrar deneyin.");
        }


        public async Task<ResultGeneric<UserQuizInfo>> CreateOneUserQuizInfo(UserQuizInfo userQuizInfo)
        {
            await _manager.UserQuizInfo.CreateOneUserQuizInfoAsync(userQuizInfo);
            var saveResult = await _manager.SaveAsync();

            return saveResult
                ? ResultGeneric<UserQuizInfo>.Ok(userQuizInfo)
                : ResultGeneric<UserQuizInfo>.Fail("Kullanıcı quiz bilgisi kaydedilemedi.", "Bir hata oluştu. Lütfen tekrar deneyin.");
        }

        public async Task<ResultGeneric<UserQuizInfo>> UpdateOneUserQuizInfo(UserQuizInfo userQuizInfo)
        {
            await _manager.UserQuizInfo.UpdateOneUserQuizInfoAsync(userQuizInfo);
            var saveResult = await _manager.SaveAsync();

            return saveResult
                ? ResultGeneric<UserQuizInfo>.Ok(userQuizInfo)
                : ResultGeneric<UserQuizInfo>.Fail("Kullanıcı quiz bilgisi güncellenemedi.", "Bir hata oluştu. Lütfen tekrar deneyin.");
        }
        public async Task<ResultGeneric<UserQuizInfo>> GetUserQuizInfoById(int id, bool trackChanges)
        {
            var userQuizInfo = await _manager.UserQuizInfo.GetUserQuizInfoByIdAsync(id, trackChanges);
            return userQuizInfo != null
                ? ResultGeneric<UserQuizInfo>.Ok(userQuizInfo)
                : ResultGeneric<UserQuizInfo>.Fail("Quiz bilgisi bulunamadı.", "Belirtilen ID ile ilgili veri mevcut değil.");
        }

        public async Task<ResultGeneric<UserQuizInfo>> ProcessQuiz(int quizId, string userId)
        {
            var userQuizInfoTemp = await _manager.UserQuizInfoTemp.GetOneTempInfoByUserIdAsync(userId, false);
            if (userQuizInfoTemp == null)
            {
                return ResultGeneric<UserQuizInfo>.Fail("Geçici quiz bilgisi bulunamadı.", "Geçici quiz kaydı bulunamadı.");
            }

            var userAnswersTemp = await _manager.UserAnswerTemp.GetTempAnswersByTempInfoIdAsync(userQuizInfoTemp.UserQuizInfoTempId, false);
            var corrects = userAnswersTemp.Count(a => a.IsCorrect);
            var falses = userAnswersTemp.Count(a => !a.IsCorrect);
            var score = (corrects * 100) / (corrects + falses);

            userQuizInfoTemp.CorrectAnswer = corrects;
            userQuizInfoTemp.FalseAnswer = falses;
            userQuizInfoTemp.Score = score;
            userQuizInfoTemp.IsCompleted = true;
            await _userQuizInfoTempService.UpdateTempInfoAsync(userQuizInfoTemp);

            var existingUserQuizInfo = await _manager.UserQuizInfo.GetUserQuizInfoByQuizIdAndUserIdAsync(quizId, userId, false);

            if (existingUserQuizInfo != null)
            {
                var updatedQuiz = await UpdateQuiz(existingUserQuizInfo, userAnswersTemp, userQuizInfoTemp);

                 await _userQuizInfoTempService.RemoveTempInfoAsync(userQuizInfoTemp);
                return ResultGeneric<UserQuizInfo>.Ok(updatedQuiz.Data);
            }
            else
            {
                var newInfo = await SaveQuiz(userQuizInfoTemp, userAnswersTemp);
                await _userQuizInfoTempService.RemoveTempInfoAsync(userQuizInfoTemp);
                return ResultGeneric<UserQuizInfo>.Ok(newInfo.Data);
            }
        }

        public async Task<ResultGeneric<UserQuizInfo>> SaveQuiz(UserQuizInfoTemp userQuizInfoTemp, IEnumerable<UserAnswerTemp> userAnswersTemp)
        {
            var userQuizInfo = new UserQuizInfo
            {
                UserId = userQuizInfoTemp.UserId,
                QuizId = userQuizInfoTemp.QuizId,
                IsCompleted = userQuizInfoTemp.IsCompleted,
                CorrectAnswer = userQuizInfoTemp.CorrectAnswer,
                FalseAnswer = userQuizInfoTemp.FalseAnswer,
                Score = userQuizInfoTemp.Score,
                CompletedAt = DateTime.Now,
                IsSuccessful = userQuizInfoTemp.Score >= 60,
            };

            await CreateOneUserQuizInfo(userQuizInfo);

            foreach (var answer in userAnswersTemp)
            {
                var userAnswer = new UserAnswer
                {
                    UserQuizInfoId = userQuizInfo.UserQuizInfoId,
                    QuestionId = answer.QuestionId,
                    SelectedOptionId = answer.SelectedOptionId,
                    IsCorrect = answer.IsCorrect
                };
                await _userAnswerService.CreateUserAnswer(userAnswer);
            }

            return ResultGeneric<UserQuizInfo>.Ok(userQuizInfo);
        }

        public async Task<ResultGeneric<UserQuizInfo>> UpdateQuiz(UserQuizInfo existingQuizInfo, IEnumerable<UserAnswerTemp> userAnswersTemp, UserQuizInfoTemp userQuizInfoTemp)
        {
            if (userQuizInfoTemp.Score > existingQuizInfo.Score)
            {
                foreach (var tempAnswer in userAnswersTemp)
                {
                    var existingAnswer = await _manager.UserAnswer.GetUserAnswerAsync(existingQuizInfo.UserQuizInfoId, tempAnswer.QuestionId, true);
                    if (existingAnswer != null)
                    {
                        existingAnswer.SelectedOptionId = tempAnswer.SelectedOptionId;
                        existingAnswer.IsCorrect = tempAnswer.IsCorrect;
                        await _manager.UserAnswer.UpdateUserAnswerAsync(existingAnswer);
                    }
                }

                existingQuizInfo.Score = userQuizInfoTemp.Score;
                existingQuizInfo.IsSuccessful = userQuizInfoTemp.Score >= 60;
                existingQuizInfo.CorrectAnswer = userQuizInfoTemp.CorrectAnswer;
                existingQuizInfo.FalseAnswer = userQuizInfoTemp.FalseAnswer;
                existingQuizInfo.CompletedAt = DateTime.Now;

                await UpdateOneUserQuizInfo(existingQuizInfo);
            }
            else
            {
                _mapper.Map(userQuizInfoTemp, existingQuizInfo);
            }
             return ResultGeneric<UserQuizInfo>.Ok(existingQuizInfo);
        }

        public async Task<ResultGeneric<IEnumerable<UserQuizInfo>>> GetRetakeableQuizzesByUserId(string userId, bool trackChanges)
        {
            var retakeableQuizzes = await _manager.UserQuizInfo.GetRetakeableQuizzesByUserIdAsync(userId, trackChanges);
            return retakeableQuizzes.Any()
                ? ResultGeneric<IEnumerable<UserQuizInfo>>.Ok(retakeableQuizzes)
                : ResultGeneric<IEnumerable<UserQuizInfo>>.Fail("Tekrar alınabilir quiz bulunamadı.", "Bu kullanıcı için tekrar alınabilir quiz yok.");
        }

        public async Task<ResultGeneric<IEnumerable<UserQuizInfo>>> GetCompletedQuizzesByUserId(string userId, bool trackChanges)
        {
            var completedQuizzes = await _manager.UserQuizInfo.GetCompletedQuizzesByUserIdAsync(userId, trackChanges);
            return completedQuizzes.Any()
                ? ResultGeneric<IEnumerable<UserQuizInfo>>.Ok(completedQuizzes)
                : ResultGeneric<IEnumerable<UserQuizInfo>>.Fail("Tamamlanmış quiz bulunamadı.", "Bu kullanıcı için tamamlanmış quiz yok.");
        }

       
    }
}

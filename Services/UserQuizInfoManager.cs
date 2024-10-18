using AutoMapper;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class UserQuizInfoManager : IUserQuizInfoService
    {
 
        private readonly IRepositoryManager _manager; 
        private readonly IMapper _mapper;
        private readonly IUserAnswerService _userAnswerService;  // Gerekli servis
        private readonly IUserQuizInfoTempService _userQuizInfoTempService;
        public UserQuizInfoManager(IRepositoryManager manager, 
            IMapper mapper,
            IUserAnswerService userAnswerService,
            IUserQuizInfoTempService userQuizInfoTempService)
        {
            _manager = manager;
            _mapper = mapper;
            _userAnswerService = userAnswerService;
            _userQuizInfoTempService = userQuizInfoTempService;
             
        }

        public UserQuizInfo? GetUserQuizInfoByQuizIdAndUserId(int quizId, string userId, bool trackChanges)
        {
            return _manager.UserQuizInfo.GetUserQuizInfoByQuizIdAndUserId(quizId, userId, trackChanges);
        }

        public IEnumerable<UserQuizInfo> GetUserQuizInfoByUserId(string userId, bool trackChanges)
        {
          return _manager.UserQuizInfo.GetUserQuizInfoByUserId(userId, trackChanges);
         
        }

        public void AssignQuizToUsers(int quizId, List<string> userIds)
        {
            foreach (var userId in userIds)
            {
                // Aynı quiz zaten atanmışsa tekrar ekleme
                var existingAssignment = _manager.UserQuizInfo
                    .GetUserQuizInfoByQuizIdAndUserId(quizId, userId, false);

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

                    _manager.UserQuizInfo.CreateOneUserQuizInfo(assignment);
                }
            }

            _manager.Save();
        }

        public void CreateOneUserQuizInfo(UserQuizInfo userQuizInfo)
        {
            _manager.UserQuizInfo.CreateOneUserQuizInfo(userQuizInfo);
            _manager.Save();
        }

        public void UpdateOneUserQuizInfo(UserQuizInfo userQuizInfo)
        {
            _manager.UserQuizInfo.Update(userQuizInfo);
            _manager.Save();  
        }

        public UserQuizInfo GetUserQuizInfoById(int id, bool trackChanges)
        {
            var userQuizInfo=_manager.UserQuizInfo.GetUserQuizInfoById(id,false);     
            return userQuizInfo;
        }

        public UserQuizInfo ProcessQuiz(int quizId, string userId)
        {

            // 1. Geçici quiz verilerini al
            var userQuizInfoTemp = _manager.UserQuizInfoTemp.GetOneTempInfoByUserId(userId, trackChanges: false);
            if (userQuizInfoTemp == null)
            {
                throw new Exception("Geçici quiz bilgisi bulunamadı.");
            }
            // 2. Geçici Cevapları al ve doğru/yanlış hesapla
            var userAnswersTemp = _manager.UserAnswerTemp.GetTempAnswersByTempInfoId(userQuizInfoTemp.UserQuizInfoTempId, trackChanges: false);

            var corrects = userAnswersTemp.Count(a => a.IsCorrect);
            var falses = userAnswersTemp.Count(a => !a.IsCorrect);
            var score = (corrects * 100) / (corrects + falses);

            // 3. doğru/yanlış verilerini tempinfoda güncelle.
            userQuizInfoTemp.CorrectAnswer = corrects;
            userQuizInfoTemp.FalseAnswer = falses;
            userQuizInfoTemp.Score = score;
            userQuizInfoTemp.IsCompleted = true;
            _userQuizInfoTempService.UpdateTempInfo(userQuizInfoTemp);

            // 4. databasede bu quizid ve userid ile kayıtlı bir userquizinfo var mı diye sor
            var existingUserQuizInfo = _manager.UserQuizInfo.GetUserQuizInfoByQuizIdAndUserId(quizId, userId, trackChanges: false);

            // 5. Eğer eski bir kayıt varsa onu güncelleyerek döndür
            if (existingUserQuizInfo != null)
            {
              var existingInfo=  UpdateQuiz(existingUserQuizInfo, userAnswersTemp, userQuizInfoTemp);
                _userQuizInfoTempService.RemoveTempInfo(userQuizInfoTemp);
                return existingInfo;
            }
            // Yoksa temp tablelarını kalıcı hale getirmesi için Save metoduna yolla ve yaratılan kaydı döndür 
            else
            {
                var newInfo = SaveQuiz(userQuizInfoTemp, userAnswersTemp);
                _userQuizInfoTempService.RemoveTempInfo(userQuizInfoTemp);
                return newInfo;
            }
        }

        public UserQuizInfo SaveQuiz(UserQuizInfoTemp userQuizInfoTemp, IEnumerable<UserAnswerTemp> userAnswersTemp)
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
                IsSuccessful= userQuizInfoTemp.Score >= 60,
            };

            CreateOneUserQuizInfo(userQuizInfo); 

            foreach (var answer in userAnswersTemp)
            {
                var userAnswer = new UserAnswer
                {
                    UserQuizInfoId = userQuizInfo.UserQuizInfoId,
                    QuestionId = answer.QuestionId,
                    SelectedOptionId = answer.SelectedOptionId,
                    IsCorrect = answer.IsCorrect
                };
                _userAnswerService.CreateUserAnswer(userAnswer);
            }

            return userQuizInfo;

        }
        public UserQuizInfo UpdateQuiz(UserQuizInfo existingQuizInfo, IEnumerable<UserAnswerTemp> userAnswersTemp, UserQuizInfoTemp userQuizInfoTemp)
        {
            // Eğer yeni score daha yüksekse güncelleme yap
            if (userQuizInfoTemp.Score > existingQuizInfo.Score)
            {
              foreach (var tempAnswer in userAnswersTemp)
                { 
                   var existingAnswer = _manager.UserAnswer.GetUserAnswer(existingQuizInfo.UserQuizInfoId, tempAnswer.QuestionId, true);
                    if (existingAnswer != null) 
                        {
                        existingAnswer.SelectedOptionId = tempAnswer.SelectedOptionId;
                        existingAnswer.IsCorrect = tempAnswer.IsCorrect;
                        _manager.UserAnswer.UpdateUserAnswer(existingAnswer);
                        };
                }
            existingQuizInfo.Score = userQuizInfoTemp.Score;
            existingQuizInfo.IsSuccessful = userQuizInfoTemp.Score>=60;
            existingQuizInfo.CorrectAnswer = userQuizInfoTemp.CorrectAnswer;
            existingQuizInfo.FalseAnswer = userQuizInfoTemp.FalseAnswer;
            existingQuizInfo.CompletedAt = DateTime.Now;

            UpdateOneUserQuizInfo(existingQuizInfo);
            }

            return existingQuizInfo;
        }

        public IEnumerable<UserQuizInfo> GetRetakeableQuizzesByUserId(string userId, bool trackChanges)
        {
            return _manager.UserQuizInfo.GetRetakeableQuizzesByUserId(userId, trackChanges);
        }

        public IEnumerable<UserQuizInfo> GetCompletedQuizzesByUserId(string userId, bool trackChanges)
        {
            return _manager.UserQuizInfo.GetCompletedQuizzesByUserId(userId, trackChanges);
        }
    }
}
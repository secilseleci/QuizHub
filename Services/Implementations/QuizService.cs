using AutoMapper;
using ClosedXML.Excel;
using Entities.Dtos;
using Entities.Exeptions;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Contracts;
using Services.Contracts;



namespace Services.Implemantations
{
    public class QuizService : IQuizService
    {
        private readonly IRepositoryManager _manager;
        private readonly IDepartmentService _departmentService;
        private readonly IQuestionService _questionService;
        private readonly IUserAnswerTempService _userAnswerTempService;
        private readonly IUserQuizInfoTempService _userQuizInfoTempService;
        private readonly IMapper _mapper;
        private readonly RepositoryContext _context;

        public QuizService(
            IRepositoryManager manager, 
            IMapper mapper, 
            RepositoryContext context,
            IDepartmentService departmentService,
            IQuestionService questionService,
            IUserAnswerTempService userAnswerTempService,
            IUserQuizInfoTempService userQuizInfoTempService)
        {
            _manager = manager;
            _mapper = mapper;
            _context = context;
            _departmentService = departmentService;
            _questionService = questionService;
            _userAnswerTempService = userAnswerTempService;
            _userQuizInfoTempService= userQuizInfoTempService;
        }

        public async Task<ResultGeneric<Quiz>> CreateOneQuiz(QuizDtoForInsertion quizDto)
        {
            var quiz = _mapper.Map<Quiz>(quizDto);

            foreach (var question in quiz.Questions)
            {
                var correctOption = question.Options.ElementAtOrDefault(question.CorrectOptionId);
                if (correctOption != null)
                {
                    correctOption.IsCorrect = true;
                }
            }
            quiz.QuestionCount = quiz.Questions.Count;

            await _manager.Quiz.CreateOneQuizAsync(quiz);

            var saveResult = await _manager.SaveAsync();
            if (!saveResult)
            {
                return ResultGeneric<Quiz>.Fail("Quiz kaydedilemedi.", "Bir hata oluştu. Lütfen tekrar deneyin.");
            }

            foreach (var question in quiz.Questions)
            {
                var correctOption = question.Options.FirstOrDefault(o => o.IsCorrect);
                if (correctOption != null)
                {
                    question.CorrectOptionId = correctOption.OptionId;
                }
            }

            await _manager.SaveAsync(); 
            return ResultGeneric<Quiz>.Ok(quiz);
        }

        public async Task<Result> DeleteOneQuiz(int id)
        {
            Quiz quiz = await _manager.Quiz.GetOneQuizAsync(id, false);
            if (quiz == null)
            {
                return Result.Fail("Quiz bulunamadı veya zaten silindi.", "Silmek istediğiniz quiz bulunamadı.");
            }
            await _manager.Quiz.DeleteOneQuizAsync(quiz);
            var saveResult = await _manager.SaveAsync();
            if (!saveResult)
            {
                return Result.Fail("Quiz veritabanından silinemedi.", "Bir şeyler ters gitti. Lütfen tekrar deneyin.");
            }

            return Result.Ok();
        }
        public async Task<ResultGeneric<QuizDtoForUpdate>> GetOneQuizForUpdate(int id, bool trackChanges)
        {
            var quiz = await _manager.Quiz.GetQuizWithDetailsAsync(id, trackChanges);

            if (quiz == null)
                return ResultGeneric<QuizDtoForUpdate>.Fail("Quiz bulunamadı.", "Güncellemek istediğiniz quiz bulunamadı.");

            var quizDto = _mapper.Map<QuizDtoForUpdate>(quiz);
            return ResultGeneric<QuizDtoForUpdate>.Ok(quizDto);
        }
        public async Task<ResultGeneric<Quiz>> UpdateOneQuiz(QuizDtoForUpdate quizDto)
        {
            var existingQuiz = await _manager.Quiz.GetQuizWithDetailsAsync(quizDto.QuizId, trackChanges: true);
            if (existingQuiz == null)
            {
                return ResultGeneric<Quiz>.Fail("Quiz bulunamadı.", "Güncellemek istediğiniz quiz bulunamadı.");
            }

            _mapper.Map(quizDto, existingQuiz);

            foreach (var question in quizDto.Questions)
            {
                var existingQuestion = existingQuiz.Questions.FirstOrDefault(q => q.QuestionId == question.QuestionId);
                if (existingQuestion != null)
                {
                 
                    // Tüm seçeneklerin doğruluğunu sıfırlıyoruz
                    foreach (var option in existingQuestion.Options)
                    {
                        option.IsCorrect = false;
                    }

                    // Doğru seçenek işaretleniyor
                    var correctOption = existingQuestion.Options.FirstOrDefault(o => o.OptionId == question.CorrectOptionId);
                    if (correctOption != null)
                    {
                        correctOption.IsCorrect = true;
                    }
                }
            }


            // 5. Değişiklikleri veritabanına kaydetme
            var saveResult = await _manager.SaveAsync();
            if (!saveResult)
            {
                return ResultGeneric<Quiz>.Fail("Quiz güncellenemedi.", "Bir şeyler ters gitti. Lütfen tekrar deneyin.");
            }

            return ResultGeneric<Quiz>.Ok(existingQuiz);
        }

        public async Task<ResultGeneric<IEnumerable<Quiz>>> GetAllQuizzes(bool trackChanges)
        {
            var quizzes = await _manager.Quiz.GetAllQuizzesAsync(trackChanges);
            if (quizzes == null || !quizzes.Any())
            {
                return ResultGeneric<IEnumerable<Quiz>>.Fail("Quiz bulunamadı.", "Henüz kayıtlı bir quiz yok.");
            }
            return ResultGeneric<IEnumerable<Quiz>>.Ok(quizzes);

        }
        public async Task<ResultGeneric<Quiz>> GetOneQuiz(int id, bool trackChanges)
        {
            var quiz = await _manager.Quiz.GetOneQuizAsync(id, trackChanges);
            if (quiz == null)
            {
                return ResultGeneric<Quiz>.Fail("Quiz bulunamadı.", "Aradığınız quiz mevcut değil.");
            }

            return ResultGeneric<Quiz>.Ok(quiz);
        }
        
        public async Task<ResultGeneric<Quiz>> GetQuizWithDetails(int quizId, bool trackChanges)
        {
            var quiz=await _manager.Quiz.GetQuizWithDetailsAsync(quizId, trackChanges);
            if (quiz == null)
            {
                return ResultGeneric<Quiz>.Fail("Quiz bulunamadı.", "Aradığınız quiz mevcut değil.");
            }
            return ResultGeneric<Quiz>.Ok(quiz);

        }
      

        public async Task<ResultGeneric<IEnumerable<Quiz>>> GetShowCaseQuizzes(bool trackChanges)
        {
            var quizzes = await _manager.Quiz.GetShowCaseQuizzesAsync(trackChanges);
            if (quizzes == null || !quizzes.Any())
            {
                return ResultGeneric<IEnumerable<Quiz>>.Fail("Quiz bulunamadı.", "Henüz kayıtlı bir quiz yok.");
            }
            return ResultGeneric<IEnumerable<Quiz>>.Ok(quizzes);

        }

        public async Task<ResultGeneric<Quiz>> GetQuizWithDepartments(int quizId,bool trackChanges) 
        {    var quiz = await _manager.Quiz.GetQuizWithDepartmentsAsync(quizId, trackChanges);
            if (quiz == null)
            {
                return ResultGeneric<Quiz>.Fail("Quiz bulunamadı.", "İlgili quiz mevcut değil.");
            }
            return ResultGeneric<Quiz>.Ok(quiz);

        }
        public async Task<ResultGeneric<IEnumerable<Quiz>>> GetQuizzesByDepartmentId(int departmentId, bool trackChanges)
        {
            var quizzes = await _manager.Quiz.GetQuizzesByDepartmentIdAsync(departmentId, trackChanges);

            if (quizzes == null || !quizzes.Any())
            {
                return ResultGeneric<IEnumerable<Quiz>>.Fail("Quiz bulunamadı.", "Bu departmanla ilişkili bir quiz bulunamadı.");
            }

            return ResultGeneric<IEnumerable<Quiz>>.Ok(quizzes);
        }

        public async Task<Result> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return Result.Fail("Excel dosyası yüklenmedi.", "Lütfen geçerli bir dosya yükleyin.");
            }

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var workbook = new XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);
                    var rows = worksheet.RowsUsed();

                    var quizTitle = rows.Skip(1).First().Cell(1).Value.ToString();

                    var quiz = new Quiz
                    {ShowCase = true,
                        Title = quizTitle,
                        CreatedDate = DateTime.Now,
                        Questions = new List<Question>()
                    };
                    int questionOrder = 1;
                    foreach (var row in rows.Skip(1))
                    {
                        var questionText = row.Cell(2).Value.ToString();
                        var option1 = row.Cell(3).IsEmpty() ? null : row.Cell(3).Value.ToString();
                        var option2 = row.Cell(4).IsEmpty() ? null : row.Cell(4).Value.ToString();
                        var option3 = row.Cell(5).IsEmpty() ? null : row.Cell(5).Value.ToString();
                        var option4 = row.Cell(6).IsEmpty() ? null : row.Cell(6).Value.ToString();
                        var option5 = row.Cell(7).IsEmpty() ? null : row.Cell(7).Value.ToString();

                        var correctAnswer = row.Cell(8).Value.ToString();

                        var optionList = new List<Option>
                {
                    new Option { OptionText = option1, IsCorrect = option1 == correctAnswer },
                    new Option { OptionText = option2, IsCorrect = option2 == correctAnswer },
                    new Option { OptionText = option3, IsCorrect = option3 == correctAnswer },
                    new Option { OptionText = option4, IsCorrect = option4 == correctAnswer },
                    new Option { OptionText = option5, IsCorrect = option5 == correctAnswer }
                }.Where(o => !string.IsNullOrWhiteSpace(o.OptionText)).ToList();

                        if (optionList.Count < 2)
                        {
                            continue;
                        }

                        var question = new Question
                        {
                            QuestionText = questionText,
                            Quiz = quiz,
                            Options = optionList,
                            Order = questionOrder++
                        };

                        var correctOption = optionList.FirstOrDefault(o => o.IsCorrect);
                        if (correctOption != null)
                        {
                            question.CorrectOptionId = correctOption.OptionId;
                        }

                        quiz.Questions.Add(question);
                    }
                    quiz.QuestionCount = quiz.Questions.Count;
                    await _manager.Quiz.CreateOneQuizAsync(quiz);

                    var saveResult = await _manager.SaveAsync();
                    if (!saveResult)
                    {
                        return Result.Fail("Quiz veritabanına kaydedilemedi.", "Bir şeyler ters gitti. Lütfen tekrar deneyin.");
                    }


                    foreach (var question in quiz.Questions)
                    {
                        var correctOption = question.Options.FirstOrDefault(o => o.IsCorrect);
                        if (correctOption != null)
                        {
                            question.CorrectOptionId = correctOption.OptionId;
                        }
                    }

                    await _manager.SaveAsync();
                }
            }

            return Result.Ok();
        }


        public async Task<Result> AssignQuizToDepartments(int quizId, List<int> selectedDepartmentIds)
        {
            // 1. Quiz'i departmanlarıyla birlikte getiriyoruz
            var quiz = await _manager.Quiz.GetQuizWithDepartmentsAsync(quizId, true);
            if (quiz == null)
            {
                return Result.Fail("Quiz bulunamadı.", "Atamak istediğiniz quiz mevcut değil.");
            }

            // 2. Mevcut atanmış departman ID'lerini alıyoruz
            var currentDepartmentIds = quiz.Departments?.Select(d => d.DepartmentId).ToList() ?? new List<int>();

            // 3. Kaldırılması gereken departmanları ve eklenmesi gereken departmanları belirliyoruz
            var departmentsToRemove = currentDepartmentIds.Except(selectedDepartmentIds).ToList();
            var departmentsToAdd = selectedDepartmentIds.Except(currentDepartmentIds).ToList();

            // 4. Kaldırılacak departmanları quiz'den çıkarıyoruz
            foreach (var departmentId in departmentsToRemove)
            {
                var department = quiz.Departments.FirstOrDefault(d => d.DepartmentId == departmentId);
                if (department != null)
                {
                    quiz.Departments.Remove(department);
                }
            }

            // 5. Eklenmesi gereken departmanları quiz'e ekliyoruz
            foreach (var departmentId in departmentsToAdd)
            {
                var departmentResult = await _departmentService.GetOneDepartment(departmentId, trackChanges: false);
                if (!departmentResult.IsSuccess)
                {
                    return Result.Fail($"Department bulunamadı: ID {departmentId}", "Geçersiz departman ID'si.");
                }
                quiz.Departments.Add(departmentResult.Data);
            }

            // 6. Değişiklikleri kaydet
            var saveResult = await _manager.SaveAsync();
            if (!saveResult)
            {
                return Result.Fail("Departman atamaları kaydedilemedi.", "Bir şeyler ters gitti. Lütfen tekrar deneyin.");
            }

            return Result.Ok();
        }

        public async Task<ResultGeneric<IEnumerable<Quiz>>> GetPendingQuizzesForUser(string userId, bool trackChanges)
        {   
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return ResultGeneric<IEnumerable<Quiz>>.Fail("Kullanıcı bulunamadı.", "Belirtilen ID'ye sahip kullanıcı mevcut değil.");
            }

            var departmentId = user.DepartmentId;
            if (departmentId == null)
            {
                return ResultGeneric<IEnumerable<Quiz>>.Fail("Kullanıcı departmanı bulunamadı.", "Kullanıcıya ait departman bilgisi mevcut değil.");
            }
            var allQuizzes = await _manager.Quiz.GetQuizzesByDepartmentIdAsync(departmentId, trackChanges);

            var solvedQuizIds = (await _manager.UserQuizInfo.GetUserQuizInfoByUserIdAsync(userId, trackChanges))
                        .Select(uqi => uqi.QuizId)
                        .ToList();

            var inProgressQuizIds = (await _manager.UserQuizInfoTemp.GetIncompleteQuizzesByUserIdAsync(userId, trackChanges))
                        .Select(uqi => uqi.QuizId)
                        .ToList();

            var pendingQuizzes = allQuizzes
                       .Where(q => !solvedQuizIds.Contains(q.QuizId) && !inProgressQuizIds.Contains(q.QuizId))
                       .ToList();

            return pendingQuizzes.Any()
              ? ResultGeneric<IEnumerable<Quiz>>.Ok(pendingQuizzes)
              : ResultGeneric<IEnumerable<Quiz>>.Fail("Bekleyen quiz bulunamadı.", "Bu kullanıcı için tamamlanmamış quiz yok.");
        }
 
        public async Task<ResultGeneric<QuizDtoForUser>> StartQuiz(int quizId, string userId)
        {
             var quizResult = await GetQuizWithDetails(quizId, trackChanges: false);
            
            if (!quizResult.IsSuccess)
            {
                return ResultGeneric<QuizDtoForUser>.Fail("Quiz bulunamadı.");
            }

            var userQuizInfoTemp = new UserQuizInfoTemp
            {
                UserId = userId,
                QuizId = quizId,
                IsCompleted = false,
                CorrectAnswer = 0,
                FalseAnswer = 0,
                StartedAt = DateTime.Now,
            };
            await _manager.UserQuizInfoTemp.CreateTempInfoAsync(userQuizInfoTemp);
            await _manager.SaveAsync();

            var firstQuestion = quizResult.Data.Questions.OrderBy(q => q.Order).FirstOrDefault();
            if (firstQuestion == null)
            {
                return ResultGeneric<QuizDtoForUser>.Fail("Bu quiz için sorular bulunamadı.");
            }
 
            var quizDto = _mapper.Map<QuizDtoForUser>(quizResult.Data);
            quizDto.QuestionCount = quizResult.Data.Questions.Count;
            quizDto.Questions = new List<QuestionDto> { _mapper.Map<QuestionDto>(firstQuestion) };  

            return ResultGeneric<QuizDtoForUser>.Ok(quizDto);
        }
        public async Task<Result> SaveAnswer(int quizId, int questionId, int selectedOptionId, string userId)
        {
            var userQuizInfoTempResult = await _userQuizInfoTempService.GetTempInfoByQuizIdAndUserIdAsync(quizId, userId, trackChanges: false);
            if (!userQuizInfoTempResult.IsSuccess)
            {
                return Result.Fail("Quiz bilgisi bulunamadı. Quiz'e başlamamış olabilirsiniz.");
            }

            var questionResult = await _questionService.GetOneQuestionWithOptions(questionId, trackChanges: false);
            if (!questionResult.IsSuccess)
            {
                return Result.Fail("Geçersiz soru ID'si.");
            }

            bool isCorrect = selectedOptionId == questionResult.Data.CorrectOptionId;

            var newAnswer = new UserAnswerTemp
            {
                UserQuizInfoTempId = userQuizInfoTempResult.Data.UserQuizInfoTempId,
                QuestionId = questionId,
                SelectedOptionId = selectedOptionId,
                IsCorrect = isCorrect
            };
            await _userAnswerTempService.CreateTempAnswer(newAnswer);
            await _manager.SaveAsync();

            return Result.Ok();
        }

        public async Task<ResultGeneric<QuizDtoForUser>> ContinueQuiz(int quizId, string userId)
        {
            
            var userQuizInfoTemp = await _manager.UserQuizInfoTemp.GetTempInfoByQuizIdAndUserIdAsync(quizId, userId, trackChanges: false);
            if (userQuizInfoTemp == null)
            {
                return ResultGeneric<QuizDtoForUser>.Fail("Quiz bulunamadı!", "Quiz devam bilgisi mevcut değil.");
            }

            var lastUserAnswerTemp = (await _manager.UserAnswerTemp.GetTempAnswersByTempInfoIdAsync(userQuizInfoTemp.UserQuizInfoTempId, trackChanges: false))
                .OrderByDescending(a => a.QuestionId)
                .FirstOrDefault();

            int currentQuestionOrder = lastUserAnswerTemp == null ? 0 : (await _manager.Question.GetOneQuestionAsync(lastUserAnswerTemp.QuestionId, trackChanges: false))?.Order ?? 0;

            var quizResult = await _manager.Quiz.GetQuizWithDetailsAsync(quizId, trackChanges: false);
            if (quizResult == null)
            {
                return ResultGeneric<QuizDtoForUser>.Fail("Quiz bulunamadı!", "Quiz mevcut değil.");
            }

            bool isLastQuestion = currentQuestionOrder >= quizResult.Questions.Count;

            var questionToShow = isLastQuestion
                ? quizResult.Questions.Last()  // Son soru
                : quizResult.Questions.FirstOrDefault(q => q.Order == currentQuestionOrder + 1) ?? quizResult.Questions.Last();

            var questionDto = _mapper.Map<QuestionDto>(questionToShow);

            if (isLastQuestion && lastUserAnswerTemp != null)
            {
                foreach (var option in questionDto.Options)
                {
                    option.IsSelected = option.OptionId == lastUserAnswerTemp.SelectedOptionId;
                    option.IsDisabled = true;
                }
            }

            var quizDto = _mapper.Map<QuizDtoForUser>(quizResult);
            quizDto.QuestionCount = quizResult.Questions.Count;
            quizDto.Questions = new List<QuestionDto> { questionDto };
            quizDto.ShowFinishButton = isLastQuestion;

            return ResultGeneric<QuizDtoForUser>.Ok(quizDto);
        }

        public async Task<ResultGeneric<IEnumerable<Quiz>>> GetQuizzesWithDepartmentsAsync(bool trackChanges)
        {
            var quizzes = await _manager.Quiz.GetQuizzesWithDepartmentsAsync(trackChanges);
            return quizzes != null
                ? ResultGeneric<IEnumerable<Quiz>>.Ok(quizzes)
                : ResultGeneric<IEnumerable<Quiz>>.Fail("No quizzes found.", "Quiz verisi bulunamadı.");
        }
    }
}
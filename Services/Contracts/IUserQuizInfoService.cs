using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
      
        public interface IUserQuizInfoService
        {
            // Kullanıcının belirli bir quize ait sonucunu alır
            UserQuizInfo? GetUserQuizInfoByQuizIdAndUserId(int quizId, string userId, bool trackChanges);

            // Kullanıcının tüm quiz sonuçlarını alır (yeni ekleme)
            IEnumerable<UserQuizInfo> GetUserQuizInfoByUserId(string userId, bool trackChanges);
        }
    }
 

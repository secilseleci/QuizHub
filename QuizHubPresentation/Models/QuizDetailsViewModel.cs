public class QuizDetailsViewModel
{
    public string Title { get; set; }            // Quiz başlığı
    public int QuestionCount { get; set; }       // Soru sayısı
    public int EstimatedTime { get; set; }       // Tahmini süre (dakika cinsinden, soru sayısına göre)
    public bool IsCompleted { get; set; }        // Quiz tamamlandı mı?
}
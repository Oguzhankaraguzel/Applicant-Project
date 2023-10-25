namespace CAWA.Application.MethodAnswers
{
    public record BaseMethodAnswer
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public string ExceptionMessage { get; set; } = "";
    }
}

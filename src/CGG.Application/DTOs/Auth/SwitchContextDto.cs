namespace CGG.Application.DTOs.Auth
{
    public enum ContextType
    {
        System,
        Family,
        School
    }

    public class UserTokenContext
    {
        public ContextType Type { get; set; } = ContextType.System;
        public Guid? ContextId { get; set; }
        public string? ContextRole { get; set; }   // "parent", "child", "teacher", "school-admin"
        public string? ContextName { get; set; }   // "Сім'я Іванових", "Школа №1"
    }

    public class SwitchContextRequestDto
    {
        public required ContextType ContextType { get; set; }
        public Guid? ContextId { get; set; }
    }

    public class SwitchContextResponseDto
    {
        public required string Token { get; set; }
        public required UserTokenContext ActiveContext { get; set; }
    }
}

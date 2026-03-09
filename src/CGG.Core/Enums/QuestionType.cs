namespace CGG.Core.Enums
{
    /// <summary>
    /// Defines the UI rendering type for a survey question.
    /// Maps from legacy string values used in JSON seeds and the old Next.js frontend.
    /// </summary>
    public enum QuestionType
    {
        /// <summary>Radio / button list – only one option allowed. Legacy: "select", "radio", "single-choice"</summary>
        SingleChoice,

        /// <summary>Checkbox list – multiple options allowed. Legacy: "multiselect", "multiple-choice"</summary>
        MultipleChoice,

        /// <summary>Short single-line text input. Legacy: "input", "text"</summary>
        Text,

        /// <summary>Multi-line text area. Legacy: "textarea"</summary>
        Textarea,

        /// <summary>Numeric range buttons (e.g. 1-10). Legacy: "number-buttons", "scale"</summary>
        Scale,

        /// <summary>Star / emoji rating. Legacy: "rating"</summary>
        Rating,

        /// <summary>Free-form feedback step (not sent to AI analysis). Legacy: "feedback"</summary>
        Feedback,

        /// <summary>Email address input. Legacy: "email"</summary>
        Email,

        /// <summary>Plain numeric input. Legacy: "number"</summary>
        Number
    }
}

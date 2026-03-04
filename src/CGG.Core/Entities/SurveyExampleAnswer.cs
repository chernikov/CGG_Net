namespace CGG.Core.Entities;

public class SurveyExampleAnswer
{
    public Guid Id { get; set; }
    public Guid ProfileId { get; set; }
    public required string Purpose { get; set; }
    public required string ValueJson { get; set; } // raw JSON: string, number, or array

    public SurveyExampleProfile? Profile { get; set; }
}

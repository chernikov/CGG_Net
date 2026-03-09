namespace CGG.Application.Specifications.Survey;

public class SurveyByTypeSpecification : BaseSpecification<CGG.Core.Entities.Survey>
{
    public SurveyByTypeSpecification(string surveyType)
        : base(s => s.SurveyType == surveyType) { }
}

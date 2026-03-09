using CGG.Core.Entities;

namespace CGG.Application.Specifications.SurveyExample;

public class ActiveExampleProfilesSpecification : BaseSpecification<SurveyExampleProfile>
{
    public ActiveExampleProfilesSpecification(string surveyType)
        : base(p => p.SurveyType == surveyType && p.IsActive)
    {
        ApplyOrderBy(p => p.SortOrder);
        AddInclude(p => p.Answers);
    }
}

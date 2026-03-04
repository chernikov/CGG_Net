using System;
using System.Linq.Expressions;
using CGG.Core.Entities;

namespace CGG.Application.Specifications.Survey;

public class SurveyWithDetailsSpecification : BaseSpecification<CGG.Core.Entities.Survey>
{
    public SurveyWithDetailsSpecification(Guid id)
        : base(s => s.Id == id)
    {
        AddIncludes();
    }

    public SurveyWithDetailsSpecification(string name)
        : base(s => s.SurveyType == name)
    {
        AddIncludes();
    }

    private void AddIncludes()
    {
        UseSplitQuery();
        AddInclude("Steps");
        AddInclude("Steps.Questions");
        AddInclude("Steps.Questions.Translations");
        AddInclude("Steps.Questions.Translations.Language");
        AddInclude("Steps.Questions.Options");
        AddInclude("Steps.Questions.Options.Translations");
        AddInclude("Steps.Questions.Options.Translations.Language");
    }
}

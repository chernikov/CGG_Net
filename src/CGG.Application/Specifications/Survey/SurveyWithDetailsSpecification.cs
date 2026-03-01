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
        AddInclude("Steps");
        AddInclude("Steps.Question");
        AddInclude("Steps.Question.Translations");
        AddInclude("Steps.Question.Translations.Language");
        AddInclude("Steps.Question.Options");
        AddInclude("Steps.Question.Options.Translations");
        AddInclude("Steps.Question.Options.Translations.Language");
    }
}

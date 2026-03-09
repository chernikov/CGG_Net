using System.Linq.Expressions;
using CGG.Core.Entities;

namespace CGG.Application.Specifications.Survey;

public class SurveySummarySpecification : BaseSpecification<CGG.Core.Entities.Survey>
{
    public SurveySummarySpecification() : base()
    {
        AddInclude("Steps");
    }
}

using CGG.Application.DTOs.SurveyExample;
using MediatR;

namespace CGG.Application.Features.SurveyExample.Queries.GetSurveyExampleProfiles;

public record GetSurveyExampleProfilesQuery(string SurveyType)
    : IRequest<List<SurveyExampleProfileDto>>;

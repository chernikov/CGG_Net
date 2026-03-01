using CGG.Application.DTOs.Survey;
using MediatR;

namespace CGG.Application.Features.Survey.Queries.GetSurveyByName;

public record GetSurveyByNameQuery(string Name) : IRequest<SurveyDetailDto?>;

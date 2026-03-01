using System.Collections.Generic;
using CGG.Application.DTOs.Survey;
using MediatR;

namespace CGG.Application.Features.Survey.Queries.GetAllSurveys;

public record GetAllSurveysQuery : IRequest<List<SurveySummaryDto>>;

using System;
using CGG.Application.DTOs.Survey;
using MediatR;

namespace CGG.Application.Features.Survey.Queries.GetSurveyById;

public record GetSurveyByIdQuery(Guid Id) : IRequest<SurveyDetailDto?>;

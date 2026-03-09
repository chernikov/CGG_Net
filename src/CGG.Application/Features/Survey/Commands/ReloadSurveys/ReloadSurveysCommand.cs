using MediatR;

namespace CGG.Application.Features.Survey.Commands.ReloadSurveys;

public record ReloadSurveysCommand() : IRequest<bool>;

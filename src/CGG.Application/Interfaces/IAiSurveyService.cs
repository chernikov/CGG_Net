using System.Threading;
using System.Threading.Tasks;
using CGG.Application.DTOs.Survey;

namespace CGG.Application.Interfaces;

/// <summary>
/// Orchestrates the AI analysis of a single survey step:
/// loads the prompt template, builds the prompt, calls the AI provider,
/// and returns the structured result.
/// </summary>
public interface IAiSurveyService
{
    /// <summary>
    /// Analyse a completed survey step and return a JSON-formatted AI result.
    /// </summary>
    Task<AiAnalysisResponseDto> AnalyzeStepAsync(
        SubmitSurveyStepDto dto,
        CancellationToken cancellationToken = default);
}

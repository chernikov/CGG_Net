using System.Threading;
using System.Threading.Tasks;
using CGG.Core.Entities;

namespace CGG.Application.Interfaces;

/// <summary>
/// Loads AI prompt templates from the data store.
/// Templates are stored in the AiPromptTemplates table with category = "survey".
/// Lookup order: (surveyType + stepNumber) → (stepNumber only) → null.
/// </summary>
public interface IAiPromptLoaderService
{
    /// <summary>
    /// Returns the best-matching active prompt template for the given survey type and step.
    /// Returns null when no template exists (caller should use a hard-coded fallback).
    /// </summary>
    Task<AiPromptTemplate?> LoadPromptForStepAsync(
        string surveyType,
        int stepNumber,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the JSON output-format schema string for a given step.
    /// "short" format for intermediate steps, "full" format for the last step.
    /// </summary>
    string GetOutputFormatJson(string surveyType, int stepNumber, int totalSteps);
}

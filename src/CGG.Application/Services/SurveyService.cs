using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CGG.Application.DTOs.Survey;
using CGG.Application.Interfaces;
using CGG.Application.Specifications.Survey;
using CGG.Core.Entities;
using CGG.Core.Interfaces;

namespace CGG.Application.Services;

public class SurveyService : ISurveyService
{
    private readonly ISurveyRepository _surveyRepository;
    private readonly IMapper _mapper;

    public SurveyService(ISurveyRepository surveyRepository, IMapper mapper)
    {
        _surveyRepository = surveyRepository;
        _mapper = mapper;
    }

    public async Task<List<SurveySummaryDto>> GetAllSurveysAsync(CancellationToken cancellationToken = default)
    {
        var spec = new SurveySummarySpecification();
        var surveys = await _surveyRepository.ListAsync(spec, cancellationToken);
        
        return _mapper.Map<List<SurveySummaryDto>>(surveys);
    }

    public async Task<SurveyDetailDto?> GetSurveyByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var spec = new SurveyWithDetailsSpecification(id);
        var survey = await _surveyRepository.FirstOrDefaultAsync(spec, cancellationToken);
        
        if (survey == null)
            return null;
            
        return MapAndSortSurvey(survey);
    }

    public async Task<SurveyDetailDto?> GetSurveyByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var spec = new SurveyWithDetailsSpecification(name);
        var survey = await _surveyRepository.FirstOrDefaultAsync(spec, cancellationToken);
        
        if (survey == null)
            return null;
            
        return MapAndSortSurvey(survey);
    }

    private SurveyDetailDto MapAndSortSurvey(CGG.Core.Entities.Survey survey)
    {
        var dto = _mapper.Map<SurveyDetailDto>(survey);
        
        // Sorting exactly as in the original controller:
        // Steps.OrderBy(step => step.StepNumber)
        // Options.OrderBy(o => o.SortOrder)
        
        dto.Steps = dto.Steps.OrderBy(s => s.StepNumber).ToList();
        
        foreach (var step in dto.Steps)
        {
            if (step.Question != null && step.Question.Options != null)
            {
                step.Question.Options = step.Question.Options.OrderBy(o => o.SortOrder).ToList();
            }
        }
        
        return dto;
    }
}

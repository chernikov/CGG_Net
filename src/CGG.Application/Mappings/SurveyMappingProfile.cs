using System.Linq;
using AutoMapper;
using CGG.Application.DTOs.Survey;
using CGG.Core.Entities;

namespace CGG.Application.Mappings;

public class SurveyMappingProfile : Profile
{
    public SurveyMappingProfile()
    {
        CreateMap<CGG.Core.Entities.Survey, SurveySummaryDto>()
            .ForMember(dest => dest.StepCount, opt => opt.MapFrom(src => src.Steps.Count));
            
        CreateMap<CGG.Core.Entities.Survey, SurveyDetailDto>();
        
        CreateMap<SurveyStep, SurveyStepDto>();
        
        CreateMap<SurveyQuestion, SurveyQuestionDto>();
        
        CreateMap<SurveyQuestionOption, SurveyQuestionOptionDto>();
        
        CreateMap<SurveyQuestionTranslation, SurveyTranslationDto>()
            .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.Language != null ? src.Language.Code : string.Empty));
            
        CreateMap<SurveyQuestionOptionTranslation, SurveyTranslationDto>()
            .ForMember(dest => dest.LanguageCode, opt => opt.MapFrom(src => src.Language != null ? src.Language.Code : string.Empty));
    }
}

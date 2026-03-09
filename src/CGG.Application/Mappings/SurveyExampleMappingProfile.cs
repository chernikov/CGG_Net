using System.Text.Json;
using AutoMapper;
using CGG.Application.DTOs.SurveyExample;
using CGG.Core.Entities;

namespace CGG.Application.Mappings;

public class SurveyExampleMappingProfile : Profile
{
    public SurveyExampleMappingProfile()
    {
        CreateMap<SurveyExampleProfile, SurveyExampleProfileDto>()
            .ForMember(dest => dest.Answers, opt => opt.MapFrom<AnswersDictionaryResolver>());
    }
}

public class AnswersDictionaryResolver
    : IValueResolver<SurveyExampleProfile, SurveyExampleProfileDto, Dictionary<string, JsonElement>>
{
    public Dictionary<string, JsonElement> Resolve(
        SurveyExampleProfile source,
        SurveyExampleProfileDto destination,
        Dictionary<string, JsonElement> destMember,
        ResolutionContext context)
        => source.Answers.ToDictionary(
            a => a.Purpose,
            a => JsonSerializer.Deserialize<JsonElement>(a.ValueJson));
}

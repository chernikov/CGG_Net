using AutoMapper;
using CGG.Application.DTOs.SurveyExample;
using CGG.Application.Specifications.SurveyExample;
using CGG.Core.Entities;
using CGG.Core.Interfaces;
using MediatR;

namespace CGG.Application.Features.SurveyExample.Queries.GetSurveyExampleProfiles;

public class GetSurveyExampleProfilesQueryHandler
    : IRequestHandler<GetSurveyExampleProfilesQuery, List<SurveyExampleProfileDto>>
{
    private readonly IReadRepository<SurveyExampleProfile> _repository;
    private readonly IMapper _mapper;

    public GetSurveyExampleProfilesQueryHandler(
        IReadRepository<SurveyExampleProfile> repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<SurveyExampleProfileDto>> Handle(
        GetSurveyExampleProfilesQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new ActiveExampleProfilesSpecification(request.SurveyType);
        var profiles = await _repository.ListAsync(spec, cancellationToken);
        return _mapper.Map<List<SurveyExampleProfileDto>>(profiles);
    }
}

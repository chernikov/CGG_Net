using System;
using System.Threading;
using System.Threading.Tasks;
using CGG.Core.Entities;
using CGG.Core.Interfaces;
using CGG.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CGG.Infrastructure.Repositories;

public class SurveyRepository : Repository<Survey>, ISurveyRepository
{
    public SurveyRepository(ApplicationDbContext context) : base(context) { }
}

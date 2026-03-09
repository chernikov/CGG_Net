using System.Threading;
using System.Threading.Tasks;

namespace CGG.Core.Interfaces;

public interface ISurveyManagementService
{
    Task<bool> ReloadSurveysAsync(CancellationToken cancellationToken = default);
}

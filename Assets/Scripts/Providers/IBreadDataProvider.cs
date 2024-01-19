using Cysharp.Threading.Tasks;
using PirateSoftwareGameJam.Client.Data;

namespace PirateSoftwareGameJam.Client.Providers
{
    public interface IBreadDataProvider
    {
        UniTask<BreadData> GetBreadData(BreadType breadType);
    }
}

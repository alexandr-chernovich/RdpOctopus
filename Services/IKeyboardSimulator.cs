using System;
using System.Threading;
using System.Threading.Tasks;

namespace RdpOctopus.Services
{
    public interface IKeyboardSimulator
    {
        Task Type(string text, IProgress<int> progress, CancellationToken cancellationToken = default);
    }
}

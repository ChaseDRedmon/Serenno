using System;
using System.Threading;
using System.Threading.Tasks;
using LazyCache;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serenno.Domain;
using Serenno.Services.Users;

namespace Serenno.Services.Accounts
{
    public sealed record AllycodeExistsRequest(uint allycode) : IRequest<bool>;
    public sealed record InvalidateAllycodeExistsRequest(uint allycode) : IRequest { }
    
    public class DoesAllycodeExist
    {
        private readonly SerennoContext _serennoContext;
        private readonly IAppCache _appCache;

        public DoesAllycodeExist(SerennoContext serennoContext, IAppCache appCache)
        {
            _serennoContext = serennoContext;
            _appCache = appCache;
        }

        public async Task<bool> Handle(AllycodeExistsRequest request, CancellationToken cancellationToken)
        {
            return await _appCache.GetOrAddAsync(GetCacheKey(request.allycode),
                () => _serennoContext.Accounts.AnyAsync(o => o.Allycode == request.allycode, cancellationToken: cancellationToken),
                DateTimeOffset.Now.AddMonths(1));
        }
        
        private static string GetCacheKey(uint allycode)
        {
            return $"{nameof(AllycodeExistsRequest)}/{allycode}";
        }
    }
}
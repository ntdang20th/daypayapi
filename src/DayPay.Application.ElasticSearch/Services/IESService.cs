using DayPay.Dtos.ESDto;
using Elastic.Clients.Elasticsearch.QueryDsl;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DayPay.Services;

public interface IESService<T> where T : DayPayESDto
{
    Task CreateIndexIfNotExists();

    Task<bool> AddOrUpdate(T document);

    Task<bool> AddOrUpdateBulk(IEnumerable<T> documents);

    Task<T> Get(string key);

    Task<IEnumerable<T>> GetAll();

    Task<List<T>> Query(Query predicate);

    Task<bool> Remove(string key);

    Task<bool> RemoveAll();
}
